using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// 资源显示管理器，用于替换富文本中的sprite标签
/// </summary>
public class ResourceDisplayManager : MonoBehaviour
{
    private static ResourceDisplayManager _instance;
    public static ResourceDisplayManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ResourceDisplayManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("ResourceDisplayManager");
                    _instance = go.AddComponent<ResourceDisplayManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    [Tooltip("资源显示预制体")]
    [SerializeField] private GameObject resourceDisplayPrefab;

    [Tooltip("资源显示的父物体（用于对象池）")]
    [SerializeField] private Transform resourceDisplayParent;

    // 对象池
    private Queue<ResourceDisplay> resourceDisplayPool = new Queue<ResourceDisplay>();
    
    // 正则表达式，用于匹配<sprite=X>Y格式
    private static readonly Regex spritePattern = new Regex(@"<sprite=(\d+)>(\d+)", RegexOptions.Compiled);

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        // 确保有父物体用于对象池
        if (resourceDisplayParent == null)
        {
            GameObject parent = new GameObject("ResourceDisplayPool");
            parent.transform.SetParent(transform);
            resourceDisplayParent = parent.transform;
        }
    }

    /// <summary>
    /// 从对象池获取一个ResourceDisplay实例
    /// </summary>
    private ResourceDisplay GetResourceDisplay()
    {
        ResourceDisplay display;
        
        if (resourceDisplayPool.Count > 0)
        {
            display = resourceDisplayPool.Dequeue();
            display.gameObject.SetActive(true);
        }
        else
        {
            if (resourceDisplayPrefab == null)
            {
                Debug.LogError("ResourceDisplayPrefab未设置！");
                return null;
            }
            
            GameObject newDisplay = Instantiate(resourceDisplayPrefab, resourceDisplayParent);
            display = newDisplay.GetComponent<ResourceDisplay>();
            
            if (display == null)
            {
                Debug.LogError("ResourceDisplayPrefab上没有ResourceDisplay组件！");
                return null;
            }
        }
        
        return display;
    }

    /// <summary>
    /// 将ResourceDisplay返回对象池
    /// </summary>
    public void ReleaseResourceDisplay(ResourceDisplay display)
    {
        if (display != null)
        {
            display.gameObject.SetActive(false);
            resourceDisplayPool.Enqueue(display);
        }
    }

    /// <summary>
    /// 替换TextMeshPro中的sprite标签
    /// </summary>
    /// <param name="textComponent">TextMeshPro组件</param>
    /// <param name="container">用于放置ResourceDisplay的容器</param>
    public void ReplaceSpriteTags(TextMeshProUGUI textComponent, Transform container)
    {
        if (textComponent == null || container == null)
            return;

        // 清除容器中的所有子物体
        foreach (Transform child in container)
        {
            if (child.GetComponent<ResourceDisplay>() != null)
            {
                ReleaseResourceDisplay(child.GetComponent<ResourceDisplay>());
            }
            else
            {
                Destroy(child.gameObject);
            }
        }

        string originalText = textComponent.text;
        
        // 使用正则表达式查找所有sprite标签
        MatchCollection matches = spritePattern.Matches(originalText);
        
        if (matches.Count == 0)
            return;

        // 隐藏原始文本
        textComponent.gameObject.SetActive(false);
        
        // 创建一个新的文本，不包含sprite标签
        string plainText = spritePattern.Replace(originalText, "");
        
        // 创建一个新的TextMeshPro显示纯文本
        GameObject plainTextObj = new GameObject("PlainText");
        plainTextObj.transform.SetParent(container, false);
        TextMeshProUGUI plainTextComponent = plainTextObj.AddComponent<TextMeshProUGUI>();
        
        // 复制原始文本组件的属性
        plainTextComponent.font = textComponent.font;
        plainTextComponent.fontSize = textComponent.fontSize;
        plainTextComponent.color = textComponent.color;
        plainTextComponent.alignment = textComponent.alignment;
        plainTextComponent.text = plainText;
        
        // 设置RectTransform
        RectTransform plainRectTransform = plainTextObj.GetComponent<RectTransform>();
        RectTransform originalRectTransform = textComponent.GetComponent<RectTransform>();
        plainRectTransform.anchorMin = originalRectTransform.anchorMin;
        plainRectTransform.anchorMax = originalRectTransform.anchorMax;
        plainRectTransform.pivot = originalRectTransform.pivot;
        plainRectTransform.sizeDelta = originalRectTransform.sizeDelta;
        plainRectTransform.anchoredPosition = originalRectTransform.anchoredPosition;
        
        // 处理每个sprite标签
        foreach (Match match in matches)
        {
            int spriteIndex = int.Parse(match.Groups[1].Value);
            int value = int.Parse(match.Groups[2].Value);
            
            // 获取sprite
            Sprite sprite = ResourceManager.Instance.GetResourceIconByIndex(spriteIndex);
            
            // 创建ResourceDisplay
            ResourceDisplay resourceDisplay = GetResourceDisplay();
            if (resourceDisplay != null)
            {
                resourceDisplay.transform.SetParent(container, false);
                resourceDisplay.SetValue(sprite, value);
                
                // 计算位置（这里需要根据实际情况调整）
                // 这部分可能需要更复杂的布局计算，取决于你的UI设计
                // 这里只是一个简单的示例
                int matchIndex = 0;
                for (int i = 0; i < matches.Count; i++)
                {
                    if (matches[i] == match)
                    {
                        matchIndex = i;
                        break;
                    }
                }
                
                resourceDisplay.transform.position = new Vector3(
                    plainTextObj.transform.position.x + 100 * matchIndex,
                    plainTextObj.transform.position.y,
                    plainTextObj.transform.position.z
                );
            }
        }
    }

    /// <summary>
    /// 替换Text中的sprite标签
    /// </summary>
    /// <param name="textComponent">Text组件</param>
    /// <param name="container">用于放置ResourceDisplay的容器</param>
    public void ReplaceSpriteTags(Text textComponent, Transform container)
    {
        if (textComponent == null || container == null)
            return;

        // 清除容器中的所有子物体
        foreach (Transform child in container)
        {
            if (child.GetComponent<ResourceDisplay>() != null)
            {
                ReleaseResourceDisplay(child.GetComponent<ResourceDisplay>());
            }
            else
            {
                Destroy(child.gameObject);
            }
        }

        string originalText = textComponent.text;
        
        // 使用正则表达式查找所有sprite标签
        MatchCollection matches = spritePattern.Matches(originalText);
        
        if (matches.Count == 0)
            return;

        // 隐藏原始文本
        textComponent.gameObject.SetActive(false);
        
        // 创建一个新的文本，不包含sprite标签
        string plainText = spritePattern.Replace(originalText, "");
        
        // 创建一个新的Text显示纯文本
        GameObject plainTextObj = new GameObject("PlainText");
        plainTextObj.transform.SetParent(container, false);
        Text plainTextComponent = plainTextObj.AddComponent<Text>();
        
        // 复制原始文本组件的属性
        plainTextComponent.font = textComponent.font;
        plainTextComponent.fontSize = textComponent.fontSize;
        plainTextComponent.color = textComponent.color;
        plainTextComponent.alignment = textComponent.alignment;
        plainTextComponent.text = plainText;
        
        // 设置RectTransform
        RectTransform plainRectTransform = plainTextObj.GetComponent<RectTransform>();
        RectTransform originalRectTransform = textComponent.GetComponent<RectTransform>();
        plainRectTransform.anchorMin = originalRectTransform.anchorMin;
        plainRectTransform.anchorMax = originalRectTransform.anchorMax;
        plainRectTransform.pivot = originalRectTransform.pivot;
        plainRectTransform.sizeDelta = originalRectTransform.sizeDelta;
        plainRectTransform.anchoredPosition = originalRectTransform.anchoredPosition;
        
        // 处理每个sprite标签
        foreach (Match match in matches)
        {
            int spriteIndex = int.Parse(match.Groups[1].Value);
            int value = int.Parse(match.Groups[2].Value);
            
            // 获取sprite
            Sprite sprite = ResourceManager.Instance.GetResourceIconByIndex(spriteIndex);
            
            // 创建ResourceDisplay
            ResourceDisplay resourceDisplay = GetResourceDisplay();
            if (resourceDisplay != null)
            {
                resourceDisplay.transform.SetParent(container, false);
                resourceDisplay.SetValue(sprite, value);
                
                // 计算位置（这里需要根据实际情况调整）
                int matchIndex = 0;
                for (int i = 0; i < matches.Count; i++)
                {
                    if (matches[i] == match)
                    {
                        matchIndex = i;
                        break;
                    }
                }
                
                resourceDisplay.transform.position = new Vector3(
                    plainTextObj.transform.position.x + 100 * matchIndex,
                    plainTextObj.transform.position.y,
                    plainTextObj.transform.position.z
                );
            }
        }
    }
}
