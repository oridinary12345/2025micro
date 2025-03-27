using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIConfirmationPopup : UIMenuPopup
{
	[SerializeField]
	private UIGameButton _closeButton;

	[SerializeField]
	private UIGameButton _confirmationButton;

	[SerializeField]
	private Image _confirmationBackgroundSprite;

	[SerializeField]
	private TextMeshProUGUI _titleText;

	[SerializeField]
	private TextMeshProUGUI _messageText;

	[SerializeField]
	private TextMeshProUGUI _buttonLabelText;

	protected override void Awake()
	{
		base.Awake();
		_closeButton.OnClick(base.Hide);
		_closeButton.ActivateOnBackKey();
	}

	public void Init(string title, string confirmationMessage, string confirmationButtonLabel, Action onContinue, bool isNegative)
	{
		_titleText.text = title;
		_messageText.text = confirmationMessage;
		_buttonLabelText.text = confirmationButtonLabel;
		_confirmationButton.OnClick(delegate
		{
			onContinue();
		});
		_confirmationBackgroundSprite.sprite = Resources.Load<Sprite>((!isNegative) ? "UI/btn_selection" : "UI/btn_red");
	}
}