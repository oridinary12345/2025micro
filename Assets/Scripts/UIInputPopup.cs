using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInputPopup : UIMenuPopup
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

	[SerializeField]
	private TMP_InputField _inputText;

	protected override void Awake()
	{
		base.Awake();
		_closeButton.OnClick(base.Hide);
		_closeButton.ActivateOnBackKey();
	}

	public void Init(string title, string confirmationMessage, string confirmationButtonLabel, Action<string> onContinue, bool isNegativeAction, bool isdigitsOnly)
	{
		_titleText.text = title;
		_messageText.text = confirmationMessage;
		_buttonLabelText.text = confirmationButtonLabel;
		_confirmationButton.ClearOnClickAction();
		_confirmationButton.OnClick(delegate
		{
			if (!string.IsNullOrEmpty(_inputText.text))
			{
				onContinue(_inputText.text);
			}
		});
		_confirmationBackgroundSprite.sprite = Resources.Load<Sprite>((!isNegativeAction) ? "UI/btn_selection" : "UI/btn_red");
		_inputText.text = string.Empty;
		_inputText.contentType = ((!isdigitsOnly) ? TMP_InputField.ContentType.Alphanumeric : TMP_InputField.ContentType.IntegerNumber);
	}

	public override void OnPush()
	{
		_inputText.ActivateInputField();
		base.OnPush();
	}
}