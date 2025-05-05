using System.Collections;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

/// <summary>
/// 本地化测试脚本，用于在控制台中显示当前语言和一些本地化文本
/// </summary>
public class LocalizationTester : MonoBehaviour
{
    [Header("测试设置")]
    [SerializeField] private bool testOnStart = true;
    [SerializeField] private bool testOnUpdate = false;
    [SerializeField] private float updateInterval = 5f;

    [Header("测试术语")]
    [SerializeField] private List<string> testTerms = new List<string>() { "HELLO", "WELCOME", "SETTINGS", "PLAY", "QUIT" };

    private float lastUpdateTime;

    private void Start()
    {
        // 等待本地化系统初始化
        StartCoroutine(WaitForLocalizationToInitialize());
    }

    private System.Collections.IEnumerator WaitForLocalizationToInitialize()
    {
        // 等待本地化系统初始化
        yield return LocalizationSettings.InitializationOperation;

        // 初始化完成后进行测试
        if (testOnStart)
        {
            TestLocalization();
        }
    }

    private void Update()
    {
        if (testOnUpdate && Time.time - lastUpdateTime >= updateInterval)
        {
            TestLocalization();
            lastUpdateTime = Time.time;
        }
    }

    /// <summary>
    /// 测试本地化功能
    /// </summary>
    public void TestLocalization()
    {
        // 获取当前语言
        var currentLocale = LocalizationSettings.SelectedLocale;
        if (currentLocale != null)
        {
            Debug.Log($"当前语言: {currentLocale.Identifier.CultureInfo?.NativeName ?? currentLocale.Identifier.Code} ({currentLocale.Identifier.Code})");
        }
        else
        {
            Debug.Log("当前语言: 未设置");
            return;
        }

        // 测试本地化文本
        Debug.Log("测试本地化文本:");
        foreach (string term in testTerms)
        {
            string translation = GetTranslation(term);
            Debug.Log($"  {term}: {translation}");
        }
    }

    /// <summary>
    /// 切换到下一个可用的语言
    /// </summary>
    public void SwitchToNextLanguage()
    {
        var availableLocales = LocalizationSettings.AvailableLocales.Locales;
        if (availableLocales.Count == 0)
        {
            Debug.LogWarning("没有可用的语言");
            return;
        }

        var currentLocale = LocalizationSettings.SelectedLocale;
        int currentIndex = availableLocales.IndexOf(currentLocale);
        int nextIndex = (currentIndex + 1) % availableLocales.Count;

        LocalizationSettings.SelectedLocale = availableLocales[nextIndex];

        Debug.Log($"已切换语言为: {LocalizationSettings.SelectedLocale.Identifier.CultureInfo?.NativeName ?? LocalizationSettings.SelectedLocale.Identifier.Code} ({LocalizationSettings.SelectedLocale.Identifier.Code})");

        // 测试新语言
        TestLocalization();
    }

    /// <summary>
    /// 获取术语的翻译
    /// </summary>
    private string GetTranslation(string term)
    {
        // 直接使用LocalizationHelper获取翻译
        return LocalizationHelper.GetTranslation(term);
    }
}
