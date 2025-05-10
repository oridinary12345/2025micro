using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMenuHudPanel : MonoBehaviour
{
	[SerializeField]
	private UILootBar _coinsBar;

	[SerializeField]
	private UILootBar _rubyBar;

	[SerializeField]
	private UIGameButton _languageButton;

	[SerializeField]
	private Text _languageText;

	private void Awake()
	{
		UnityCanvasContainer componentInChildren = base.transform.GetComponentInChildren<UnityCanvasContainer>();
		if (componentInChildren != null)
		{
			componentInChildren.Init();
		}

		if (_coinsBar != null)
		{
			_coinsBar.Init("lootCoin");
		}

		if (_rubyBar != null)
		{
			_rubyBar.Init("lootRuby");
		}

		// 初始化语言按钮
		if (_languageButton != null)
		{
			_languageButton.OnClick(OnLanguageButtonClicked);
			UpdateLanguageButtonText();
		}
	}

	/// <summary>
	/// 点击语言切换按钮
	/// </summary>
	private void OnLanguageButtonClicked()
	{
		// 显示语言选择面板
		var languagePanel = FindObjectOfType<UIMenuChooseLanguagePanel>();
		if (languagePanel != null && UIMenuStack.Instance != null)
		{
			UIMenuStack.Instance.Push(languagePanel);
		}
		else
		{
			Debug.LogWarning("Language panel or UIMenuStack.Instance is null");
		}
	}

	/// <summary>
	/// 更新语言按钮文本
	/// </summary>
	private void UpdateLanguageButtonText()
	{
		if (_languageText != null)
		{
			// 显示当前语言
			string currentLanguage = LocalizationHelper.CurrentLanguage;
			if (!string.IsNullOrEmpty(currentLanguage))
			{
				_languageText.text = currentLanguage;
			}
			else
			{
				_languageText.text = "Unknown";
				Debug.LogWarning("Current language is null or empty");
			}
		}
	}
}