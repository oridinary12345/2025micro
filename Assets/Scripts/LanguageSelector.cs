using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 语言选择器，用于替代I2.Loc.SetLanguage
/// </summary>
[AddComponentMenu("Localization/Language Selector Button")]
public class LanguageSelector : MonoBehaviour
{
    [SerializeField]
    private string languageName;
    
    [SerializeField]
    private Button button;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();
            
        if (button != null)
            button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        ApplyLanguage();
    }

    public void ApplyLanguage()
    {
        if (LocalizationHelper.HasLanguage(languageName))
        {
            LocalizationHelper.CurrentLanguage = languageName;
            
            // 如果有玩家设置管理器，保存语言设置
            if (App.Instance != null && App.Instance.Player != null && App.Instance.Player.SettingsManager != null)
            {
                // 将Unity本地化系统的语言名称转换为SystemLanguage
                string systemLanguage = ConvertToSystemLanguage(languageName);
                App.Instance.Player.SettingsManager.Language = systemLanguage;
            }
        }
        else
        {
            Debug.LogWarning($"不支持的语言: {languageName}");
        }
    }
    
    private string ConvertToSystemLanguage(string unityLocalizationLanguage)
    {
        // 将Unity本地化系统的语言名称转换为SystemLanguage
        switch (unityLocalizationLanguage)
        {
            case "English":
                return "English";
            case "French":
                return "French";
            case "Chinese (Simplified)":
                return "Chinese";
            default:
                return "English"; // 默认为英语
        }
    }
}
