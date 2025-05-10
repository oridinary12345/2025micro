using UnityEngine;
using UnityEditor;
using UnityEngine.Localization.Settings;

public class LanguageMenu
{
    [MenuItem("Language/English")]
    private static void SwitchToEnglish()
    {
        LocalizationHelper.CurrentLanguage = "English";
        // 标记本地化设置为已修改
        EditorUtility.SetDirty(LocalizationSettings.Instance);
    }

    [MenuItem("Language/Chinese (Simplified)")]
    private static void SwitchToChinese()
    {
        LocalizationHelper.CurrentLanguage = "Chinese (Simplified)";
        // 标记本地化设置为已修改
        EditorUtility.SetDirty(LocalizationSettings.Instance);
    }

    [MenuItem("Language/French")]
    private static void SwitchToFrench()
    {
        LocalizationHelper.CurrentLanguage = "French";
        // 标记本地化设置为已修改
        EditorUtility.SetDirty(LocalizationSettings.Instance);
    }
}
