using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

/// <summary>
/// 语言切换测试脚本，用于在运行时切换语言并查看效果
/// </summary>
public class LanguageSwitcher : MonoBehaviour
{
    [Header("UI 引用")]
    [SerializeField] public TMP_Dropdown languageDropdown;
    [SerializeField] public Button applyButton;
    [SerializeField] public TextMeshProUGUI statusText;

    [Header("测试文本")]
    [SerializeField] public List<TextMeshProUGUI> testTexts = new List<TextMeshProUGUI>();

    [Header("测试设置")]
    [SerializeField] private bool initializeOnStart = true;
    [SerializeField] private bool refreshOnLanguageChange = true;

    // 可用的语言列表
    private List<Locale> availableLocales = new List<Locale>();

    private void Start()
    {
        if (initializeOnStart)
        {
            Initialize();
        }
    }

    /// <summary>
    /// 初始化语言切换器
    /// </summary>
    public void Initialize()
    {
        // 等待本地化系统初始化完成
        StartCoroutine(WaitForLocalizationToInitialize());
    }

    private System.Collections.IEnumerator WaitForLocalizationToInitialize()
    {
        // 等待本地化系统初始化
        yield return LocalizationSettings.InitializationOperation;

        // 初始化完成后设置UI
        SetupUI();
    }

    /// <summary>
    /// 设置UI
    /// </summary>
    private void SetupUI()
    {
        // 获取可用的语言
        availableLocales = LocalizationSettings.AvailableLocales.Locales;

        // 清空下拉菜单
        languageDropdown.ClearOptions();

        // 添加语言选项
        List<string> options = new List<string>();
        int currentIndex = 0;
        int selectedIndex = 0;

        foreach (var locale in availableLocales)
        {
            // 获取语言显示名称
            string displayName = GetLanguageDisplayName(locale);
            options.Add(displayName);

            // 检查是否为当前选中的语言
            if (LocalizationSettings.SelectedLocale == locale)
            {
                selectedIndex = currentIndex;
            }

            currentIndex++;
        }

        // 设置下拉菜单选项
        languageDropdown.AddOptions(options);
        languageDropdown.value = selectedIndex;

        // 添加事件监听
        languageDropdown.onValueChanged.AddListener(OnLanguageSelected);

        if (applyButton != null)
        {
            applyButton.onClick.AddListener(ApplySelectedLanguage);
        }

        // 更新状态文本
        UpdateStatusText();
    }

    /// <summary>
    /// 获取语言的显示名称
    /// </summary>
    private string GetLanguageDisplayName(Locale locale)
    {
        // 根据语言代码返回友好的显示名称
        switch (locale.Identifier.Code)
        {
            case "en":
                return "English";
            case "fr":
                return "Français";
            case "zh-CN":
                return "简体中文";
            default:
                // 使用语言的本地化名称
                return locale.Identifier.CultureInfo?.NativeName ?? locale.Identifier.Code;
        }
    }

    /// <summary>
    /// 当选择语言时调用
    /// </summary>
    private void OnLanguageSelected(int index)
    {
        if (refreshOnLanguageChange)
        {
            ApplySelectedLanguage();
        }
    }

    /// <summary>
    /// 应用选中的语言
    /// </summary>
    public void ApplySelectedLanguage()
    {
        if (languageDropdown.value >= 0 && languageDropdown.value < availableLocales.Count)
        {
            // 获取选中的语言
            Locale selectedLocale = availableLocales[languageDropdown.value];

            // 设置当前语言
            LocalizationSettings.SelectedLocale = selectedLocale;

            // 更新状态文本
            UpdateStatusText();

            // 打印调试信息
            Debug.Log($"已切换语言为: {GetLanguageDisplayName(selectedLocale)} ({selectedLocale.Identifier.Code})");
        }
    }

    /// <summary>
    /// 更新状态文本
    /// </summary>
    private void UpdateStatusText()
    {
        if (statusText != null)
        {
            Locale currentLocale = LocalizationSettings.SelectedLocale;
            if (currentLocale != null)
            {
                statusText.text = $"当前语言: {GetLanguageDisplayName(currentLocale)} ({currentLocale.Identifier.Code})";
            }
            else
            {
                statusText.text = "当前语言: 未设置";
            }
        }
    }

    /// <summary>
    /// 刷新所有测试文本
    /// </summary>
    public void RefreshAllTestTexts()
    {
        foreach (var textComponent in testTexts)
        {
            if (textComponent != null)
            {
                // 获取LocalizeStringEvent组件并刷新
                var localizeEvent = textComponent.GetComponent<UnityEngine.Localization.Components.LocalizeStringEvent>();
                if (localizeEvent != null)
                {
                    localizeEvent.RefreshString();
                }
            }
        }
    }
}
