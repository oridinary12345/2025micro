using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SimpleLocalizedText : MonoBehaviour
{
    private Text _text;
    private string _originalText;

    private void Awake()
    {
        _text = GetComponent<Text>();
        _originalText = _text.text;
        UpdateText();
    }

    private void OnEnable()
    {
        // 订阅语言变化事件
        LocalizationHelper.OnLanguageChanged += UpdateText;
        UpdateText();
    }

    private void OnDisable()
    {
        // 取消订阅语言变化事件
        LocalizationHelper.OnLanguageChanged -= UpdateText;
    }

    private void UpdateText()
    {
        if (_text != null && !string.IsNullOrEmpty(_originalText))
        {
            // 获取翻译文本，LocalizationHelper.GetTranslation已经确保在找不到翻译时返回原文本
            string translatedText = LocalizationHelper.GetTranslation(_originalText);
            
            // 无论如何都更新文本，确保显示内容
            _text.text = translatedText;
        }
    }
}
