using System;
using System.Collections.Generic;
using UnityEngine;

public class UIChestsPanel : MonoBehaviour
{
	[SerializeField]
	private UIChestBox _chestAdsBox;

	private ChestData _chestAdsBoxData;

	private UINoAdsAvailablePanel _noAdsPopup;

	private UIAdsPanel _adsPanel;

	private UIRewardsController _menuRewardController;

	private PlayerChestManager _chestManager;

	public void Init(PlayerChestManager chestManager)
	{
		_chestManager = chestManager;
		_noAdsPopup = UINoAdsAvailablePanel.Create();
		_menuRewardController = UIRewardsController.Create();
		_adsPanel = UIAdsPanel.Create();
		_chestAdsBoxData = chestManager.GetChestData("chestAds");
		_chestAdsBox.Init(_chestAdsBoxData);
		UpdateChestVisibility();
		if (!MonoSingleton<GameAdsController>.IsCreated())
		{
			MonoSingleton<GameAdsController>.Instance.Create();
		}
		OnVideoAvailabilityChanged(MonoSingleton<GameAdsController>.Instance.AreRewardedVideoReady());
		_chestAdsBox.Button.OnClick(OnAdsButtonClicked);
		if (_chestAdsBoxData != null)
		{
			_chestAdsBoxData.Events.ChestRedeemedEvent += OnChestRedeemed;
			_chestAdsBoxData.ChestUpdatedEvent += OnChestUpdated;
		}
	}

	private void OnDestroy()
	{
		if (_chestAdsBoxData != null)
		{
			_chestAdsBoxData.Events.ChestRedeemedEvent -= OnChestRedeemed;
			_chestAdsBoxData.ChestUpdatedEvent -= OnChestUpdated;
		}
	}

	private void UpdateChestVisibility()
	{
		if (_chestAdsBoxData.IsSleeping())
		{
			_chestAdsBox.gameObject.SetActive( false);
		}
	}

	private void OnEnable()
	{
		RegisterEvents();
	}

	private void OnDisable()
	{
		UnRegisterEvents();
	}

	private void RegisterEvents()
	{
		UnRegisterEvents();
		MonoSingleton<GameAdsController>.Instance.Events.VideoPlayFailedEvent += OnVideoPlayFailed;
		MonoSingleton<GameAdsController>.Instance.Events.VideoRewardCompletedEvent += OnVideoRewardCompleted;
		MonoSingleton<GameAdsController>.Instance.Events.VideoAvailabilityChangedEvent += OnVideoAvailabilityChanged;
	}

	private void UnRegisterEvents()
	{
		if (MonoSingleton<GameAdsController>.IsCreated())
		{
			MonoSingleton<GameAdsController>.Instance.Events.VideoPlayFailedEvent -= OnVideoPlayFailed;
			MonoSingleton<GameAdsController>.Instance.Events.VideoRewardCompletedEvent -= OnVideoRewardCompleted;
			MonoSingleton<GameAdsController>.Instance.Events.VideoAvailabilityChangedEvent -= OnVideoAvailabilityChanged;
		}
	}

	private void OnChestRedeemed(string chestId)
	{
		if (_chestAdsBoxData.Config.Id == chestId)
		{
			UpdateChestVisibility();
		}
	}

	private void OnChestUpdated()
	{
		UpdateChestVisibility();
	}

	private void OnAdsButtonClicked()
	{
		if (!_chestAdsBoxData.HasRedeemedAll())
		{
			int amountPerMinuteMax = App.Instance.Player.MuseumManager.GetAmountPerMinuteMax();
			int b = Mathf.RoundToInt((float)amountPerMinuteMax * 3f);
			int coins = Mathf.Max(250, b);
			int rubies = 2;
			Action watchAd = delegate
			{
				MonoSingleton<GameAdsController>.Instance.PlayRewardedVideo(GameAdsPlacement.FreeCoins);
			};
			_adsPanel.ShowAds(watchAd, coins, rubies);
		}
	}

	private void MaybeHideFreeCoinsButton()
	{
		_chestAdsBox.gameObject.SetActive(MonoSingleton<GameAdsController>.Instance.AreRewardedVideoReady() && !_chestAdsBoxData.IsSleeping());
	}

	private void OnVideoAvailabilityChanged(bool isAvailable)
	{
		if (_chestAdsBox != null && _chestAdsBox.gameObject != null && _chestAdsBoxData != null)
		{
			_chestAdsBox.gameObject.SetActive(isAvailable && !_chestAdsBoxData.IsSleeping());
		}
	}

	private void OnVideoPlayFailed()
	{
		_noAdsPopup.Show();
	}

	private void OnVideoRewardCompleted(string placementId, List<Reward> rewards)
	{
		if (placementId == GameAdsPlacement.FreeCoins.ToString())
		{
			_chestManager.RedeemChest(_chestAdsBoxData);
			_menuRewardController.Show(rewards);
		}
	}
}