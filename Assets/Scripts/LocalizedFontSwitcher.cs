using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

/// <summary>
/// 本地化字体切换器，根据当前语言自动切换字体
/// </summary>
public class LocalizedFontSwitcher : MonoBehaviour
{
    [System.Serializable]
    public class LocalizedFont
    {
        public string localeCode;
        public TMP_FontAsset font;
        public Material material;  // 可选的自定义材质
        
        public Material GetMaterial()
        {
            return material != null ? material : (font != null ? font.material : null);
        }
    }

    [Header("字体设置")]
    [SerializeField] private TMP_FontAsset defaultFont;
    [SerializeField] private List<LocalizedFont> localizedFonts = new List<LocalizedFont>();

    [Header("目标文本")]
    [SerializeField] private List<TextMeshProUGUI> targetTexts = new List<TextMeshProUGUI>();
    [SerializeField] public bool findAllTextComponentsInChildren = true;

    [Header("设置")]
    [SerializeField] private bool switchOnStart = true;
    [SerializeField] private bool switchOnLocaleChanged = true;

    private void Start()
    {
        if (switchOnStart)
        {
            // 等待本地化系统初始化
            StartCoroutine(WaitForLocalizationToInitialize());
        }
    }

    private System.Collections.IEnumerator WaitForLocalizationToInitialize()
    {
        // 等待本地化系统初始化
        yield return LocalizationSettings.InitializationOperation;

        // 初始化
        Initialize();
    }

    private void Initialize()
    {
        // 如果需要查找所有子对象中的文本组件
        if (findAllTextComponentsInChildren)
        {
            targetTexts.Clear();
            targetTexts.AddRange(GetComponentsInChildren<TextMeshProUGUI>(true));
        }

        // 注册语言变更事件
        if (switchOnLocaleChanged)
        {
            LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
        }

        // 立即应用当前语言的字体
        ApplyFontForCurrentLocale();
    }

    private void OnDestroy()
    {
        // 取消注册事件
        if (switchOnLocaleChanged)
        {
            LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
        }
    }

    private void OnLocaleChanged(Locale locale)
    {
        ApplyFontForCurrentLocale();
    }

    /// <summary>
    /// 应用当前语言的字体
    /// </summary>
    public void ApplyFontForCurrentLocale()
    {
        if (LocalizationSettings.SelectedLocale == null)
            return;

        string currentLocaleCode = LocalizationSettings.SelectedLocale.Identifier.Code;
        TMP_FontAsset fontToApply = GetFontForLocale(currentLocaleCode);

        ApplyFontToTargets(fontToApply);

        Debug.Log($"已应用字体: {fontToApply.name} 用于语言: {currentLocaleCode}");
    }

    /// <summary>
    /// 获取指定语言的字体
    /// </summary>
    private TMP_FontAsset GetFontForLocale(string localeCode)
    {
        // 查找匹配的字体
        foreach (var localizedFont in localizedFonts)
        {
            if (localizedFont.localeCode == localeCode && localizedFont.font != null)
            {
                return localizedFont.font;
            }
        }

        // 如果没有找到匹配的字体，返回默认字体
        return defaultFont;
    }

    /// <summary>
    /// 应用字体到目标文本
    /// </summary>
    private void ApplyFontToTargets(TMP_FontAsset font)
    {
        if (font == null)
        {
            Debug.LogWarning("[LocalizedFontSwitcher] 尝试应用空字体资源");
            return;
        }

        // 获取当前语言的字体配置
        string currentLocaleCode = LocalizationSettings.SelectedLocale?.Identifier.Code;
        LocalizedFont localizedFont = localizedFonts.Find(f => f.localeCode == currentLocaleCode);
        Material material = localizedFont?.GetMaterial() ?? font.material;

        foreach (var text in targetTexts)
        {
            if (text != null)
            {
                try
                {
                    // 应用字体资源
                    text.font = font;
                    
                    // 应用材质（如果有自定义材质）
                    if (material != null && material != text.fontMaterial)
                    {
                        text.fontMaterial = material;
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[LocalizedFontSwitcher] 应用字体时出错: {e.Message}");
                }
            }
        }
    }

    /// <summary>
    /// 添加目标文本
    /// </summary>
    public void AddTargetText(TextMeshProUGUI text)
    {
        if (text != null && !targetTexts.Contains(text))
        {
            targetTexts.Add(text);

            // 立即应用字体
            if (LocalizationSettings.SelectedLocale != null)
            {
                string currentLocaleCode = LocalizationSettings.SelectedLocale.Identifier.Code;
                TMP_FontAsset fontToApply = GetFontForLocale(currentLocaleCode);
                text.font = fontToApply;
            }
        }
    }

    /// <summary>
    /// 移除目标文本
    /// </summary>
    public void RemoveTargetText(TextMeshProUGUI text)
    {
        if (text != null)
        {
            targetTexts.Remove(text);
        }
    }
}
