using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// 本地化辅助类，用于替代I2.Loc.LocalizationManager
/// 提供与I2本地化系统类似的API，简化迁移过程
/// </summary>
public static class LocalizationHelper
{
    private static bool isInitialized = false;
    private static Dictionary<string, string> cachedTranslations = new Dictionary<string, string>();

    /// <summary>
    /// 语言变化事件
    /// </summary>
    public static Action OnLanguageChanged;

    /// <summary>
    /// 当前语言
    /// </summary>
    public static string CurrentLanguage
    {
        get
        {
            if (LocalizationSettings.SelectedLocale != null)
            {
                return LocalizationSettings.SelectedLocale.Identifier.CultureInfo.DisplayName;
            }
            return "English";
        }
        set
        {
            SetLanguage(value);
        }
    }

    /// <summary>
    /// 当前语言代码
    /// </summary>
    public static string CurrentLanguageCode
    {
        get
        {
            if (LocalizationSettings.SelectedLocale != null)
            {
                return LocalizationSettings.SelectedLocale.Identifier.Code;
            }
            return "en";
        }
    }

    /// <summary>
    /// 初始化本地化系统
    /// </summary>
    public static void Initialize()
    {
        if (isInitialized)
            return;

        // 确保本地化设置已加载
        var initOperation = LocalizationSettings.InitializationOperation;
        if (initOperation.IsDone)
        {
            OnLocalizationInitialized(initOperation);
        }
        else
        {
            initOperation.Completed += OnLocalizationInitialized;
        }
    }

    private static void OnLocalizationInitialized(AsyncOperationHandle<LocalizationSettings> obj)
    {
        isInitialized = true;

        // 从玩家设置中获取语言设置
        if (App.Instance != null && App.Instance.Player != null && App.Instance.Player.SettingsManager != null)
        {
            string savedLanguage = App.Instance.Player.SettingsManager.Language;
            
            // 如果没有保存的语言设置，或者保存的是英语，则使用中文
            if (string.IsNullOrEmpty(savedLanguage) || savedLanguage == "English")
            {
                if (HasLanguage("Chinese (Simplified)"))
                {
                    SetLanguage("Chinese (Simplified)");
                    if (App.Instance.Player.SettingsManager != null)
                    {
                        App.Instance.Player.SettingsManager.Language = "Chinese (Simplified)";
                    }
                    return;
                }
            }
            else if (!string.IsNullOrEmpty(savedLanguage))
            {
                // 将SystemLanguage转换为Unity本地化系统的语言名称
                string unityLocalizationLanguage = ConvertToUnityLocalizationLanguage(savedLanguage);
                
                // 设置语言
                if (HasLanguage(unityLocalizationLanguage))
                {
                    SetLanguage(unityLocalizationLanguage);
                    return;
                }
            }
        }

        // 如果没有设置语言，默认使用中文
        if (HasLanguage("Chinese (Simplified)"))
        {
            SetLanguage("Chinese (Simplified)");
            if (App.Instance?.Player?.SettingsManager != null)
            {
                App.Instance.Player.SettingsManager.Language = "Chinese (Simplified)";
            }
        }

        Debug.Log($"本地化系统初始化完成，当前语言：{CurrentLanguage}");
    }

    private static string ConvertToUnityLocalizationLanguage(string systemLanguage)
    {
        // 将SystemLanguage转换为Unity本地化系统的语言名称
        switch (systemLanguage)
        {
            case "ChineseSimplified":
                return "Chinese (Simplified)";
            case "English":
                return "English";
            case "French":
                return "French";
            default:
                return systemLanguage;
        }
    }

    /// <summary>
    /// 获取翻译文本
    /// </summary>
    /// <param name="term">要翻译的术语</param>
    /// <param name="fixForRTL">是否修复RTL文本</param>
    /// <param name="maxLineLengthForRTL">RTL文本最大行长度</param>
    /// <param name="ignoreRTLnumbers">是否忽略RTL数字</param>
    /// <param name="applyParameters">是否应用参数</param>
    /// <param name="localParametersRoot">本地参数根对象</param>
    /// <param name="overrideLanguage">覆盖语言</param>
    /// <param name="tableName">字符串表名称，默认为GameStrings</param>
    /// <returns>翻译后的文本</returns>
    public static string GetTranslation(string term, bool fixForRTL = true, int maxLineLengthForRTL = 0,
        bool ignoreRTLnumbers = true, bool applyParameters = false, GameObject localParametersRoot = null,
        string overrideLanguage = null, string tableName = "GameStrings")
    {
        if (string.IsNullOrEmpty(term))
            return string.Empty;

        // 创建缓存键（表名+术语）
        string cacheKey = $"{tableName}:{term}";
        
        // 检查缓存
        if (cachedTranslations.TryGetValue(cacheKey, out string cachedTranslation))
            return cachedTranslation;

        // 从指定字符串表获取翻译
        try
        {
            // 使用StringDatabase直接获取本地化字符串
            var localizedString = new LocalizedString(tableName, term);
            string translation = localizedString.GetLocalizedString();

            if (!string.IsNullOrEmpty(translation))
            {
                cachedTranslations[cacheKey] = translation;
                return translation;
            }
        }
        catch (System.Exception)
        {
            // 静默忽略异常，不输出警告
        }

        // 如果找不到翻译，直接返回原始术语
        // 不输出任何警告日志
        
        return term;
    }

    /// <summary>
    /// 尝试获取翻译
    /// </summary>
    /// <param name="term">要翻译的术语</param>
    /// <param name="translation">输出翻译</param>
    /// <param name="fixForRTL">是否修复RTL文本</param>
    /// <param name="maxLineLengthForRTL">RTL文本最大行长度</param>
    /// <param name="ignoreRTLnumbers">是否忽略RTL数字</param>
    /// <param name="applyParameters">是否应用参数</param>
    /// <param name="localParametersRoot">本地参数根对象</param>
    /// <param name="overrideLanguage">覆盖语言</param>
    /// <param name="tableName">字符串表名称，默认为GameStrings</param>
    /// <returns>是否成功获取翻译</returns>
    public static bool TryGetTranslation(string term, out string translation, bool fixForRTL = true,
        int maxLineLengthForRTL = 0, bool ignoreRTLnumbers = true, bool applyParameters = false,
        GameObject localParametersRoot = null, string overrideLanguage = null, string tableName = "GameStrings")
    {
        translation = GetTranslation(term, fixForRTL, maxLineLengthForRTL, ignoreRTLnumbers, applyParameters,
            localParametersRoot, overrideLanguage, tableName);

        // 如果翻译结果与输入的术语相同，则认为未找到翻译
        return translation != term;
    }

    /// <summary>
    /// 设置语言
    /// </summary>
    /// <param name="languageName">语言名称</param>
    public static void SetLanguage(string languageName)
    {
        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            if (locale.Identifier.CultureInfo.DisplayName == languageName)
            {
                LocalizationSettings.SelectedLocale = locale;
                ClearCache();
                Debug.Log($"语言已切换为: {languageName}");
                
                // 触发语言变化事件
                OnLanguageChanged?.Invoke();
                return;
            }
        }

        Debug.LogWarning($"未找到语言: {languageName}");
    }

    /// <summary>
    /// 检查是否支持指定语言
    /// </summary>
    /// <param name="languageName">语言名称</param>
    /// <returns>是否支持该语言</returns>
    public static bool HasLanguage(string languageName)
    {
        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            if (locale.Identifier.CultureInfo.DisplayName == languageName)
                return true;
        }
        return false;
    }

    /// <summary>
    /// 获取所有可用语言
    /// </summary>
    /// <returns>语言列表</returns>
    public static List<string> GetAllLanguages()
    {
        List<string> languages = new List<string>();
        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            languages.Add(locale.Identifier.CultureInfo.DisplayName);
        }
        return languages;
    }

    /// <summary>
    /// 清除翻译缓存
    /// </summary>
    public static void ClearCache()
    {
        cachedTranslations.Clear();
    }
}
