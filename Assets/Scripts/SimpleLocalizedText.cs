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
            string translatedText = LocalizationHelper.GetTranslation(_originalText);
            if (!string.IsNullOrEmpty(translatedText))
            {
                _text.text = translatedText;
            }
        }
    }
}
