using System;
using TMPro;
using UnityEngine;

public class UIAdsPanel : UIMenuPopup
{
	private const string AdsWatchedKey = "AdsWatchedKey";

	[SerializeField]
	private UIGameButton _closeButton;

	[SerializeField]
	private UIGameButton _watchAdsButton;

	[SerializeField]
	private TextMeshProUGUI _reward1Text;

	[SerializeField]
	private TextMeshProUGUI _reward2Text;

	private Action _watchAd;

	public static UIAdsPanel Create()
	{
		UIAdsPanel uIAdsPanel = UnityEngine.Object.Instantiate(Resources.Load<UIAdsPanel>("UI/AdsPanel"));
		uIAdsPanel.GetComponent<RectTransform>().SetParent(UnityEngine.Object.FindObjectOfType<UIAppCanvas>().transform, false);
		return uIAdsPanel;
	}

	protected override void Awake()
	{
		base.Awake();
		_closeButton.OnClick(base.Hide);
		_closeButton.ActivateOnBackKey();
		_watchAdsButton.OnClick(OnWatchAdButtonClicked);
	}

	public void ShowAds(Action watchAd, int coins, int rubies)
	{
		_reward1Text.text = $"{coins.ToString()} <sprite=3>";
		_reward2Text.text = $"{rubies.ToString()} <sprite=2>";
		if (PlayerPrefs.GetInt("AdsWatchedKey", 0) == 0)
		{
			_watchAd = watchAd;
			Show();
		}
		else
		{
			watchAd();
		}
	}

	private void OnWatchAdButtonClicked()
	{
		Hide();
		PlayerPrefs.SetInt("AdsWatchedKey", 1);
		_watchAd();
	}
}