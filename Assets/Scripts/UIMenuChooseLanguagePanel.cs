using UnityEngine;

public class UIMenuChooseLanguagePanel : UIMenuPopup
{
	[SerializeField]
	private UIGameButton _buttonClose;

	[SerializeField]
	private UIGameButton _englishButton;

	[SerializeField]
	private UIGameButton _frenchButton;

	[SerializeField]
	private UIGameButton _chineseButton;

	protected override void Awake()
	{
		base.Awake();
		_buttonClose.OnClick(OnCloseButtonClicked);
		_buttonClose.ActivateOnBackKey();
		_englishButton.OnClick(delegate
		{
			OnLanguageSelected(SystemLanguage.English, "English");
		});
		_frenchButton.OnClick(delegate
		{
			OnLanguageSelected(SystemLanguage.French, "French");
		});

		if (_chineseButton != null)
		{
			_chineseButton.OnClick(delegate
			{
				OnLanguageSelected(SystemLanguage.ChineseSimplified, "Chinese (Simplified)");
			});
		}
	}

	private void OnLanguageSelected(SystemLanguage language, string unityLocalizationLanguage)
	{
		// 保存到玩家设置
		App.Instance.Player.SettingsManager.Language = language.ToString();

		// 设置Unity本地化系统的语言
		if (LocalizationHelper.HasLanguage(unityLocalizationLanguage))
		{
			LocalizationHelper.CurrentLanguage = unityLocalizationLanguage;
		}

		OnCloseButtonClicked();
	}

	private void OnCloseButtonClicked()
	{
		if (MonoSingleton<UIMenuStack>.Instance.Peek() == this)
		{
			Hide();
		}
	}
}