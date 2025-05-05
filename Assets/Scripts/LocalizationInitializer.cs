using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// 本地化初始化器，用于初始化Unity本地化系统
/// </summary>
public class LocalizationInitializer : MonoBehaviour
{
    [SerializeField]
    private string defaultLanguage = "Chinese (Simplified)";

    private void Awake()
    {
        // 初始化本地化辅助类
        LocalizationHelper.Initialize();
        
        // 等待本地化系统初始化完成
        StartCoroutine(InitializeLocalization());
    }

    private IEnumerator InitializeLocalization()
    {
        // 等待本地化系统初始化
        yield return LocalizationSettings.InitializationOperation;
        
        // 设置默认语言
        SetDefaultLanguage();
        
        // 打印调试信息
        PrintDebugInfo();
    }

    private void SetDefaultLanguage()
    {
        // 从玩家设置中获取语言设置
        string savedLanguage = null;
        if (App.Instance != null && App.Instance.Player != null && App.Instance.Player.SettingsManager != null)
        {
            savedLanguage = App.Instance.Player.SettingsManager.Language;
        }

        // 如果有保存的语言设置，使用它
        if (!string.IsNullOrEmpty(savedLanguage))
        {
            SetLanguageBySystemLanguage(savedLanguage);
        }
        // 否则使用默认语言
        else if (LocalizationHelper.HasLanguage(defaultLanguage))
        {
            LocalizationHelper.CurrentLanguage = defaultLanguage;
        }
    }

    private void SetLanguageBySystemLanguage(string systemLanguage)
    {
        // 将SystemLanguage转换为Locale标识符
        switch (systemLanguage)
        {
            case "English":
                LocalizationHelper.CurrentLanguage = "English";
                break;
            case "French":
                LocalizationHelper.CurrentLanguage = "French";
                break;
            case "ChineseSimplified":
            case "Chinese":
                LocalizationHelper.CurrentLanguage = "Chinese (Simplified)";
                break;
            default:
                LocalizationHelper.CurrentLanguage = "English"; // 默认为英语
                break;
        }
    }

    private void PrintDebugInfo()
    {
        // 打印当前语言设置信息（调试用）
        Debug.Log($"当前语言: {LocalizationHelper.CurrentLanguage}");
        Debug.Log($"当前语言代码: {LocalizationHelper.CurrentLanguageCode}");
        
        // 列出所有可用的语言
        List<string> languages = LocalizationHelper.GetAllLanguages();
        Debug.Log($"可用语言: {string.Join(", ", languages)}");
    }
}
