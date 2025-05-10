using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 将 TextMeshProUGUI 转换为普通 Text 组件的工具
/// </summary>
public class TMProToTextConverter : EditorWindow
{
    private Font defaultFont;
    private bool convertLocalizedComponents = true;
    private Vector2 scrollPosition;
    private List<GameObject> prefabsToConvert = new List<GameObject>();
    
    [MenuItem("Tools/Convert TMPro To Text")]
    public static void ShowWindow()
    {
        GetWindow<TMProToTextConverter>("TMPro 转换工具");
    }

    [MenuItem("Tools/Convert Scene TMPro To Text")]
    public static void ConvertCurrentScene()
    {
        var window = GetWindow<TMProToTextConverter>();
        window.ConvertCurrentSceneTexts();
    }

    private void ConvertCurrentSceneTexts()
    {
        // 确保有默认字体
        if (defaultFont == null)
        {
            // 尝试加载 Arial 字体
            defaultFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
        }

        // 获取当前场景中的所有 TextMeshProUGUI
        var tmpTexts = GameObject.FindObjectsOfType<TextMeshProUGUI>(true);
        int count = 0;

        // 记录要转换的对象
        var objectsToConvert = new List<GameObject>();
        foreach (var tmp in tmpTexts)
        {
            if (!objectsToConvert.Contains(tmp.gameObject))
            {
                objectsToConvert.Add(tmp.gameObject);
            }
        }

        // 开始转换
        Undo.SetCurrentGroupName("Convert TMPro To Text");
        int group = Undo.GetCurrentGroup();

        foreach (var go in objectsToConvert)
        {
            var tmpText = go.GetComponent<TextMeshProUGUI>();
            if (tmpText != null)
            {
                Undo.RecordObject(go, "Convert TMPro");
                ConvertToText(tmpText);
                count++;
            }

            // 转换本地化组件
            var localizedText = go.GetComponent<LocalizedTextMeshPro>();
            if (localizedText != null)
            {
                Undo.RecordObject(go, "Convert Localized TMPro");
                ConvertLocalizedComponent(localizedText);
            }

            // 移除字体切换器
            var fontSwitcher = go.GetComponent<LocalizedFontSwitcher>();
            if (fontSwitcher != null)
            {
                Undo.DestroyObjectImmediate(fontSwitcher);
            }
        }

        Undo.CollapseUndoOperations(group);
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log($"已转换场景中的 {count} 个 TextMeshPro 组件");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("TextMeshPro 转换工具", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // 字体设置
        EditorGUILayout.LabelField("字体设置", EditorStyles.boldLabel);
        defaultFont = (Font)EditorGUILayout.ObjectField("默认字体", defaultFont, typeof(Font), false);

        EditorGUILayout.Space();

        // 转换选项
        EditorGUILayout.LabelField("转换选项", EditorStyles.boldLabel);
        convertLocalizedComponents = EditorGUILayout.Toggle("转换本地化组件", convertLocalizedComponents);

        EditorGUILayout.Space();

        // 查找预制体按钮
        if (GUILayout.Button("查找所有 TMPro UI 预制体"))
        {
            FindTMProPrefabs();
        }

        EditorGUILayout.Space();

        // 显示找到的预制体
        EditorGUILayout.LabelField("待转换预制体", EditorStyles.boldLabel);
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        for (int i = prefabsToConvert.Count - 1; i >= 0; i--)
        {
            EditorGUILayout.BeginHorizontal();
            prefabsToConvert[i] = (GameObject)EditorGUILayout.ObjectField(prefabsToConvert[i], typeof(GameObject), false);
            if (GUILayout.Button("移除", GUILayout.Width(60)))
            {
                prefabsToConvert.RemoveAt(i);
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();

        // 转换按钮
        GUI.enabled = prefabsToConvert.Count > 0 && defaultFont != null;
        if (GUILayout.Button("开始转换"))
        {
            ConvertPrefabs();
        }
        GUI.enabled = true;
    }

    private void FindTMProPrefabs()
    {
        prefabsToConvert.Clear();
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Resources/UI" });
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            
            // 检查预制体是否包含 TextMeshProUGUI 组件
            if (prefab.GetComponentInChildren<TextMeshProUGUI>(true) != null)
            {
                prefabsToConvert.Add(prefab);
            }
        }
    }

    private void ConvertPrefabs()
    {
        foreach (var prefab in prefabsToConvert)
        {
            if (prefab == null) continue;

            string prefabPath = AssetDatabase.GetAssetPath(prefab);
            GameObject instance = PrefabUtility.LoadPrefabContents(prefabPath);

            try
            {
                // 转换所有 TextMeshProUGUI 组件
                var tmpTexts = instance.GetComponentsInChildren<TextMeshProUGUI>(true);
                foreach (var tmpText in tmpTexts)
                {
                    ConvertToText(tmpText);
                }

                // 转换本地化组件
                if (convertLocalizedComponents)
                {
                    var localizedTexts = instance.GetComponentsInChildren<LocalizedTextMeshPro>(true);
                    foreach (var localizedText in localizedTexts)
                    {
                        ConvertLocalizedComponent(localizedText);
                    }

                    // 移除字体切换器
                    var fontSwitchers = instance.GetComponentsInChildren<LocalizedFontSwitcher>(true);
                    foreach (var switcher in fontSwitchers)
                    {
                        DestroyImmediate(switcher);
                    }
                }

                // 保存更改
                PrefabUtility.SaveAsPrefabAsset(instance, prefabPath);
                Debug.Log($"已转换预制体: {prefab.name}");
            }
            finally
            {
                // 清理
                PrefabUtility.UnloadPrefabContents(instance);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("所有预制体转换完成！");
    }

    private void ConvertToText(TextMeshProUGUI tmpText)
    {
        if (tmpText == null) return;

        GameObject go = tmpText.gameObject;
        RectTransform rt = tmpText.rectTransform;

        // 保存原始值
        string text = tmpText.text;
        Color color = tmpText.color;
        TextAlignmentOptions alignment = tmpText.alignment;
        bool raycastTarget = tmpText.raycastTarget;
        float fontSize = tmpText.fontSize;
        FontStyles fontStyle = tmpText.fontStyle;
        float lineSpacing = tmpText.lineSpacing;
        bool enableWordWrapping = tmpText.enableWordWrapping;

        // 移除 TMPro 组件
        DestroyImmediate(tmpText);

        // 添加普通 Text 组件
        Text newText = go.AddComponent<Text>();
        newText.text = text;
        newText.color = color;
        newText.raycastTarget = raycastTarget;
        newText.fontSize = Mathf.RoundToInt(fontSize);
        newText.lineSpacing = lineSpacing;
        
        // 设置字体
        newText.font = defaultFont;

        // 转换对齐方式
        newText.alignment = ConvertAlignment(alignment);

        // 转换字体样式
        newText.fontStyle = ConvertFontStyle(fontStyle);

        // 设置自动换行
        newText.horizontalOverflow = enableWordWrapping ? HorizontalWrapMode.Wrap : HorizontalWrapMode.Overflow;
    }

    private void ConvertLocalizedComponent(LocalizedTextMeshPro localizedText)
    {
        if (localizedText == null) return;

        GameObject go = localizedText.gameObject;
        string termKey = localizedText.GetType().GetField("termKey", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(localizedText) as string;

        // 移除原组件
        DestroyImmediate(localizedText);

        // 添加新的本地化组件
        var localizeStringEvent = go.GetComponent<UnityEngine.Localization.Components.LocalizeStringEvent>();
        if (localizeStringEvent != null && !string.IsNullOrEmpty(termKey))
        {
            // 设置本地化引用
            localizeStringEvent.StringReference.TableReference = "GameStrings";
            localizeStringEvent.StringReference.TableEntryReference = termKey;

            // 获取 Text 组件并设置监听
            Text textComponent = go.GetComponent<Text>();
            if (textComponent != null)
            {
                localizeStringEvent.OnUpdateString.RemoveAllListeners();
                localizeStringEvent.OnUpdateString.AddListener((string text) => textComponent.text = text);
            }
        }
    }

    private TextAnchor ConvertAlignment(TextAlignmentOptions tmpAlignment)
    {
        // 水平对齐
        bool left = (tmpAlignment & TextAlignmentOptions.Left) != 0;
        bool right = (tmpAlignment & TextAlignmentOptions.Right) != 0;
        bool center = (tmpAlignment & TextAlignmentOptions.Center) != 0;
        
        // 垂直对齐
        bool top = (tmpAlignment & TextAlignmentOptions.Top) != 0;
        bool bottom = (tmpAlignment & TextAlignmentOptions.Bottom) != 0;
        bool middle = (tmpAlignment & TextAlignmentOptions.Midline) != 0;

        if (top)
        {
            if (left) return TextAnchor.UpperLeft;
            if (right) return TextAnchor.UpperRight;
            return TextAnchor.UpperCenter;
        }
        else if (bottom)
        {
            if (left) return TextAnchor.LowerLeft;
            if (right) return TextAnchor.LowerRight;
            return TextAnchor.LowerCenter;
        }
        else
        {
            if (left) return TextAnchor.MiddleLeft;
            if (right) return TextAnchor.MiddleRight;
            return TextAnchor.MiddleCenter;
        }
    }

    private FontStyle ConvertFontStyle(FontStyles tmpStyle)
    {
        if ((tmpStyle & FontStyles.Bold) != 0 && (tmpStyle & FontStyles.Italic) != 0)
            return FontStyle.BoldAndItalic;
        if ((tmpStyle & FontStyles.Bold) != 0)
            return FontStyle.Bold;
        if ((tmpStyle & FontStyles.Italic) != 0)
            return FontStyle.Italic;
        return FontStyle.Normal;
    }
}
