using UnityEngine;

public class UIBetaAnnouncementPanel : UIMenuPopup
{
	[SerializeField]
	private UIGameButton _closeButton;

	protected override void Awake()
	{
		base.Awake();
		_closeButton.OnClick(base.Hide);
		_closeButton.ActivateOnBackKey();
	}

	public override void OnFocusGained()
	{
		App.Instance.Player.SetBetaWarningSeen();
	}
}