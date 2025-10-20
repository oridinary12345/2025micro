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
			OnLanguageSelected(SystemLanguage.English);
		});
		_frenchButton.OnClick(delegate
		{
			OnLanguageSelected(SystemLanguage.French);
		});
		if (_chineseButton != null)
		{
			_chineseButton.OnClick(delegate
			{
				OnLanguageSelected(SystemLanguage.Chinese);
			});
		}
	}

	private void OnLanguageSelected(SystemLanguage language)
	{
		App.Instance.Player.SettingsManager.Language = language.ToString();
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
