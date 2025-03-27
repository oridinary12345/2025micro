using UnityEngine;

public class UIQuitAppPopup : UIMenuPopup
{
	[SerializeField]
	private UIGameButton _closeButton;

	[SerializeField]
	private UIGameButton _quitButton;

	protected override void Awake()
	{
		base.Awake();
		_closeButton.OnClick(base.Hide);
		_closeButton.ActivateOnBackKey();
		_quitButton.OnClick(OnQuitButtonClicked);
	}

	private void OnQuitButtonClicked()
	{
		Application.Quit();
	}
}