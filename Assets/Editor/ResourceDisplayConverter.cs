using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// 资源显示转换器，用于将富文本sprite标签转换为ResourceDisplay
/// </summary>
public class ResourceDisplayConverter : EditorWindow
{
    private GameObject targetObject;
    private GameObject resourceDisplayPrefab;
    private bool processChildren = true;
    private bool processTextMeshPro = true;
    private bool processLegacyText = true;
    
    // 正则表达式，用于匹配<sprite=X>Y格式
    private static readonly Regex spritePattern = new Regex(@"<sprite=(\d+)>(\d+)", RegexOptions.Compiled);

    [MenuItem("工具/资源显示转换器")]
    public static void ShowWindow()
    {
        GetWindow<ResourceDisplayConverter>("资源显示转换器");
    }

    private void OnGUI()
    {
        GUILayout.Label("将富文本sprite标签转换为ResourceDisplay", EditorStyles.boldLabel);
        
        EditorGUILayout.Space();
        
        targetObject = EditorGUILayout.ObjectField("目标对象", targetObject, typeof(GameObject), true) as GameObject;
        resourceDisplayPrefab = EditorGUILayout.ObjectField("ResourceDisplay预制体", resourceDisplayPrefab, typeof(GameObject), false) as GameObject;
        
        EditorGUILayout.Space();
        
        processChildren = EditorGUILayout.Toggle("处理子对象", processChildren);
        processTextMeshPro = EditorGUILayout.Toggle("处理TextMeshPro", processTextMeshPro);
        processLegacyText = EditorGUILayout.Toggle("处理Legacy Text", processLegacyText);
        
        EditorGUILayout.Space();
        
        EditorGUI.BeginDisabledGroup(targetObject == null || resourceDisplayPrefab == null);
        if (GUILayout.Button("转换"))
        {
            ConvertSpriteTexts();
        }
        EditorGUI.EndDisabledGroup();
    }

    private void ConvertSpriteTexts()
    {
        if (targetObject == null || resourceDisplayPrefab == null)
            return;
            
        // 检查预制体是否包含ResourceDisplay组件
        if (resourceDisplayPrefab.GetComponent<ResourceDisplay>() == null)
        {
            EditorUtility.DisplayDialog("错误", "ResourceDisplay预制体必须包含ResourceDisplay组件！", "确定");
            return;
        }
        
        int convertedCount = 0;
        
        // 处理TextMeshPro
        if (processTextMeshPro)
        {
            TextMeshProUGUI[] tmpTexts;
            
            if (processChildren)
                tmpTexts = targetObject.GetComponentsInChildren<TextMeshProUGUI>(true);
            else
                tmpTexts = targetObject.GetComponents<TextMeshProUGUI>();
                
            foreach (var text in tmpTexts)
            {
                if (ConvertTextMeshPro(text))
                    convertedCount++;
            }
        }
        
        // 处理Legacy Text
        if (processLegacyText)
        {
            Text[] legacyTexts;
            
            if (processChildren)
                legacyTexts = targetObject.GetComponentsInChildren<Text>(true);
            else
                legacyTexts = targetObject.GetComponents<Text>();
                
            foreach (var text in legacyTexts)
            {
                if (ConvertLegacyText(text))
                    convertedCount++;
            }
        }
        
        if (convertedCount > 0)
            EditorUtility.DisplayDialog("完成", $"已转换 {convertedCount} 个文本组件。", "确定");
        else
            EditorUtility.DisplayDialog("完成", "没有找到需要转换的文本组件。", "确定");
    }

    private bool ConvertTextMeshPro(TextMeshProUGUI text)
    {
        if (text == null)
            return false;
            
        string originalText = text.text;
        
        // 检查是否包含sprite标签
        if (!spritePattern.IsMatch(originalText))
            return false;
            
        // 创建一个容器用于放置ResourceDisplay
        GameObject container = new GameObject(text.gameObject.name + "_Container");
        container.transform.SetParent(text.transform.parent);
        container.transform.localPosition = text.transform.localPosition;
        container.transform.localRotation = text.transform.localRotation;
        container.transform.localScale = text.transform.localScale;
        
        // 添加RectTransform组件
        RectTransform containerRect = container.AddComponent<RectTransform>();
        RectTransform textRect = text.GetComponent<RectTransform>();
        containerRect.anchorMin = textRect.anchorMin;
        containerRect.anchorMax = textRect.anchorMax;
        containerRect.pivot = textRect.pivot;
        containerRect.sizeDelta = textRect.sizeDelta;
        containerRect.anchoredPosition = textRect.anchoredPosition;
        
        // 创建纯文本
        string plainText = spritePattern.Replace(originalText, "");
        GameObject plainTextObj = new GameObject("PlainText");
        plainTextObj.transform.SetParent(container.transform, false);
        TextMeshProUGUI plainTextComponent = plainTextObj.AddComponent<TextMeshProUGUI>();
        
        // 复制原始文本组件的属性
        plainTextComponent.font = text.font;
        plainTextComponent.fontSize = text.fontSize;
        plainTextComponent.color = text.color;
        plainTextComponent.alignment = text.alignment;
        plainTextComponent.text = plainText;
        
        // 设置RectTransform
        RectTransform plainRectTransform = plainTextObj.GetComponent<RectTransform>();
        plainRectTransform.anchorMin = new Vector2(0, 0);
        plainRectTransform.anchorMax = new Vector2(1, 1);
        plainRectTransform.pivot = new Vector2(0.5f, 0.5f);
        plainRectTransform.sizeDelta = Vector2.zero;
        plainRectTransform.anchoredPosition = Vector2.zero;
        
        // 处理每个sprite标签
        MatchCollection matches = spritePattern.Matches(originalText);
        foreach (Match match in matches)
        {
            int spriteIndex = int.Parse(match.Groups[1].Value);
            int value = int.Parse(match.Groups[2].Value);
            
            // 创建ResourceDisplay
            GameObject resourceDisplayObj = PrefabUtility.InstantiatePrefab(resourceDisplayPrefab) as GameObject;
            resourceDisplayObj.transform.SetParent(container.transform, false);
            ResourceDisplay resourceDisplay = resourceDisplayObj.GetComponent<ResourceDisplay>();
            
            // 设置位置（这里需要根据实际情况调整）
            // 在实际应用中，你可能需要更复杂的布局计算
            int matchIndex = 0;
            for (int i = 0; i < matches.Count; i++)
            {
                if (matches[i] == match)
                {
                    matchIndex = i;
                    break;
                }
            }
            float xOffset = 100 * matchIndex;
            resourceDisplayObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(xOffset, 0);
            
            // 设置值（这里需要从ResourceManager获取sprite，但在编辑器中可能无法访问）
            // 在实际运行时，你需要通过ResourceManager.Instance.GetResourceIconByIndex(spriteIndex)获取sprite
            EditorGUIUtility.PingObject(resourceDisplayObj);
        }
        
        // 禁用原始文本
        text.gameObject.SetActive(false);
        
        return true;
    }

    private bool ConvertLegacyText(Text text)
    {
        if (text == null)
            return false;
            
        string originalText = text.text;
        
        // 检查是否包含sprite标签
        if (!spritePattern.IsMatch(originalText))
            return false;
            
        // 创建一个容器用于放置ResourceDisplay
        GameObject container = new GameObject(text.gameObject.name + "_Container");
        container.transform.SetParent(text.transform.parent);
        container.transform.localPosition = text.transform.localPosition;
        container.transform.localRotation = text.transform.localRotation;
        container.transform.localScale = text.transform.localScale;
        
        // 添加RectTransform组件
        RectTransform containerRect = container.AddComponent<RectTransform>();
        RectTransform textRect = text.GetComponent<RectTransform>();
        containerRect.anchorMin = textRect.anchorMin;
        containerRect.anchorMax = textRect.anchorMax;
        containerRect.pivot = textRect.pivot;
        containerRect.sizeDelta = textRect.sizeDelta;
        containerRect.anchoredPosition = textRect.anchoredPosition;
        
        // 创建纯文本
        string plainText = spritePattern.Replace(originalText, "");
        GameObject plainTextObj = new GameObject("PlainText");
        plainTextObj.transform.SetParent(container.transform, false);
        Text plainTextComponent = plainTextObj.AddComponent<Text>();
        
        // 复制原始文本组件的属性
        plainTextComponent.font = text.font;
        plainTextComponent.fontSize = text.fontSize;
        plainTextComponent.color = text.color;
        plainTextComponent.alignment = text.alignment;
        plainTextComponent.text = plainText;
        
        // 设置RectTransform
        RectTransform plainRectTransform = plainTextObj.GetComponent<RectTransform>();
        plainRectTransform.anchorMin = new Vector2(0, 0);
        plainRectTransform.anchorMax = new Vector2(1, 1);
        plainRectTransform.pivot = new Vector2(0.5f, 0.5f);
        plainRectTransform.sizeDelta = Vector2.zero;
        plainRectTransform.anchoredPosition = Vector2.zero;
        
        // 处理每个sprite标签
        MatchCollection matches = spritePattern.Matches(originalText);
        foreach (Match match in matches)
        {
            int spriteIndex = int.Parse(match.Groups[1].Value);
            int value = int.Parse(match.Groups[2].Value);
            
            // 创建ResourceDisplay
            GameObject resourceDisplayObj = PrefabUtility.InstantiatePrefab(resourceDisplayPrefab) as GameObject;
            resourceDisplayObj.transform.SetParent(container.transform, false);
            ResourceDisplay resourceDisplay = resourceDisplayObj.GetComponent<ResourceDisplay>();
            
            // 设置位置（这里需要根据实际情况调整）
            int matchIndex = 0;
            for (int i = 0; i < matches.Count; i++)
            {
                if (matches[i] == match)
                {
                    matchIndex = i;
                    break;
                }
            }
            float xOffset = 100 * matchIndex;
            resourceDisplayObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(xOffset, 0);
            
            // 设置值（这里需要从ResourceManager获取sprite，但在编辑器中可能无法访问）
            EditorGUIUtility.PingObject(resourceDisplayObj);
        }
        
        // 禁用原始文本
        text.gameObject.SetActive(false);
        
        return true;
    }
}
