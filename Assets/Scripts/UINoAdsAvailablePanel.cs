using UnityEngine;

public class UINoAdsAvailablePanel : UIMenuPopup
{
	[SerializeField]
	private UIGameButton _closeButton;

	public static UINoAdsAvailablePanel Create()
	{
		UINoAdsAvailablePanel uINoAdsAvailablePanel = Object.Instantiate(Resources.Load<UINoAdsAvailablePanel>("UI/NoAdsAvailablePopup"));
		uINoAdsAvailablePanel.GetComponent<RectTransform>().SetParent(Object.FindObjectOfType<UIAppCanvas>().transform, false);
		return uINoAdsAvailablePanel;
	}

	protected override void Awake()
	{
		base.Awake();
		_closeButton.OnClick(base.Hide);
		_closeButton.ActivateOnBackKey();
	}
}