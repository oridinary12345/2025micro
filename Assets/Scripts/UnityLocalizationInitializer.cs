using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

/// <summary>
/// Unity本地化系统初始化器
/// </summary>
public class UnityLocalizationInitializer : MonoBehaviour
{
    [SerializeField]
    private string defaultLanguage = "en";
    
    [SerializeField]
    private bool initializeOnAwake = true;
    
    private void Awake()
    {
        if (initializeOnAwake)
        {
            Initialize();
        }
    }
    
    public void Initialize()
    {
        // 初始化本地化系统
        StartCoroutine(InitializeLocalization());
    }
    
    private IEnumerator InitializeLocalization()
    {
        // 等待本地化系统初始化
        yield return LocalizationSettings.InitializationOperation;
        
        // 从玩家设置中获取语言设置
        string savedLanguage = GetSavedLanguage();
        
        // 设置语言
        if (!string.IsNullOrEmpty(savedLanguage))
        {
            SetLanguage(savedLanguage);
        }
        else
        {
            // 使用默认语言
            SetLanguage(defaultLanguage);
        }
        
        // 打印调试信息
        Debug.Log($"本地化系统初始化完成，当前语言：{LocalizationSettings.SelectedLocale?.Identifier.Code}");
    }
    
    private string GetSavedLanguage()
    {
        // 从玩家设置中获取语言设置
        if (App.Instance != null && App.Instance.Player != null && App.Instance.Player.SettingsManager != null)
        {
            string systemLanguage = App.Instance.Player.SettingsManager.Language;
            return ConvertSystemLanguageToLocaleCode(systemLanguage);
        }
        
        return null;
    }
    
    private string ConvertSystemLanguageToLocaleCode(string systemLanguage)
    {
        // 将SystemLanguage转换为Locale代码
        switch (systemLanguage)
        {
            case "English":
                return "en";
            case "French":
                return "fr";
            case "ChineseSimplified":
            case "Chinese":
                return "zh-CN";
            default:
                return "en"; // 默认为英语
        }
    }
    
    public void SetLanguage(string localeCode)
    {
        // 查找对应的Locale
        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            if (locale.Identifier.Code == localeCode)
            {
                // 设置选中的Locale
                LocalizationSettings.SelectedLocale = locale;
                
                // 保存到玩家设置
                SaveLanguageToPlayerSettings(localeCode);
                
                return;
            }
        }
        
        Debug.LogWarning($"未找到Locale: {localeCode}");
    }
    
    private void SaveLanguageToPlayerSettings(string localeCode)
    {
        // 将Locale代码转换为SystemLanguage
        string systemLanguage = ConvertLocaleCodeToSystemLanguage(localeCode);
        
        // 保存到玩家设置
        if (App.Instance != null && App.Instance.Player != null && App.Instance.Player.SettingsManager != null)
        {
            App.Instance.Player.SettingsManager.Language = systemLanguage;
        }
    }
    
    private string ConvertLocaleCodeToSystemLanguage(string localeCode)
    {
        // 将Locale代码转换为SystemLanguage
        switch (localeCode)
        {
            case "en":
                return "English";
            case "fr":
                return "French";
            case "zh-CN":
                return "Chinese";
            default:
                return "English"; // 默认为英语
        }
    }
}
