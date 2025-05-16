using System;
using UnityEngine;
using UnityEngine.UI;

public class UIAdsPanel : UIMenuPopup
{
	private const string AdsWatchedKey = "AdsWatchedKey";

	[SerializeField]
	private UIGameButton _closeButton;

	[SerializeField]
	private UIGameButton _watchAdsButton;

	[SerializeField]
	private Text _reward1Text;

	[SerializeField]
	private Text _reward2Text;
	
	[SerializeField]
	private ResourceDisplay _reward1Display; // 金币显示
	
	[SerializeField]
	private ResourceDisplay _reward2Display; // 宝石显示

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
		// 使用ResourceDisplay显示金币和宝石
		if (_reward1Display != null)
		{
			Sprite coinIcon = null;
			
			// 尝试从SpriteAssetManager获取图标
			if (SpriteAssetManager.Instance != null)
			{
				coinIcon = SpriteAssetManager.Instance.GetSprite(3); // 原来的sprite=3
			}
			// 如果SpriteAssetManager不可用，尝试使用ResourceManager
			else if (ResourceManager.Instance != null)
			{
				coinIcon = ResourceManager.Instance.GetResourceIconBySpriteIndex(3);
			}
			
			_reward1Display.SetValue(coinIcon, coins);
		}
		// 兼容旧版本
		else if (_reward1Text != null)
		{
			_reward1Text.text = $"{coins.ToString()} <sprite=3>";
		}
		
		if (_reward2Display != null)
		{
			Sprite gemIcon = null;
			
			// 尝试从SpriteAssetManager获取图标
			if (SpriteAssetManager.Instance != null)
			{
				gemIcon = SpriteAssetManager.Instance.GetSprite(2); // 原来的sprite=2
			}
			// 如果SpriteAssetManager不可用，尝试使用ResourceManager
			else if (ResourceManager.Instance != null)
			{
				gemIcon = ResourceManager.Instance.GetResourceIconBySpriteIndex(2);
			}
			
			_reward2Display.SetValue(gemIcon, rubies);
		}
		// 兼容旧版本
		else if (_reward2Text != null)
		{
			_reward2Text.text = $"{rubies.ToString()} <sprite=2>";
		}
		
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