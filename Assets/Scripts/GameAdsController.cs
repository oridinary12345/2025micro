using System.Collections.Generic;
using UnityEngine;
using Utils;

public class GameAdsController : MonoSingleton<GameAdsController>
{
    public GameAdsEvents Events;

    private GameIronSource _adProvider;

    private GameAdsPlacement _fullyWatchedPlacement;

    private bool _isVideoClosed = true;

    private RewardContext _rewardContext;

    protected override void Init()
    {
        base.Init();
        Events = new GameAdsEvents();
        _adProvider = MonoSingleton<GameIronSource>.Instance;
        _adProvider.Setup(this);
    }

    public bool PlayRewardedVideo(GameAdsPlacement placement, RewardContext rewardContext = null)
    {
        if (!_adProvider.AreRewardedVideoReady())
        {
            UnityEngine.Debug.Log("AreRewardedVideoReady = false !!!");
            return false;
        }
        _rewardContext = rewardContext;
        _adProvider.ShowRewardedVideo(placement.ToString());
        return true;
    }

    public bool AreRewardedVideoReady()
    {
        return _adProvider.AreRewardedVideoReady();
    }

    public bool IsPlacementCapped(GameAdsPlacement placement)
    {
        return true;
    }

    public void OnVideoPlayFailed()
    {
        _fullyWatchedPlacement = GameAdsPlacement.None;
        Events.OnVideoPlayFailed();
    }

    public void OnVideoRewardCompleted(string placementId)
    {
        _fullyWatchedPlacement = Enum.TryParse(placementId, GameAdsPlacement.None);
        if (_isVideoClosed)
        {
            GiveRewards();
        }
    }

    public void OnVideoOpened()
    {
        _isVideoClosed = false;
        _fullyWatchedPlacement = GameAdsPlacement.None;
        MonoSingleton<GameMusicManager>.Instance.PauseMusic();
        Events.OnVideoOpened();
    }

    public void OnVideoClosed()
    {
        _isVideoClosed = true;
        MonoSingleton<GameMusicManager>.Instance.ResumeMusic();
        Events.OnVideoClosed();
        if (_fullyWatchedPlacement != 0)
        {
            GiveRewards();
        }
    }

    private void GiveRewards()
    {
        List<Reward> list = new List<Reward>();
        Reward reward = null;
        int num = 1;
        do
        {
            string rewardId = "reward" + _fullyWatchedPlacement + ((num <= 1) ? string.Empty : num.ToString());
            reward = App.Instance.RewardFactory.Create(rewardId, _rewardContext);
            if (reward != null)
            {
                CurrencyReason currencyReason = Enum.TryParse("adsReward" + _fullyWatchedPlacement, CurrencyReason.unknown);
                if (currencyReason == CurrencyReason.unknown && _fullyWatchedPlacement == GameAdsPlacement.FreeCoins)
                {
                    currencyReason = CurrencyReason.adsRewardFreeChest;
                }
                App.Instance.Player.RewardManager.Redeem(reward, currencyReason);
                list.Add(reward);
            }
            num++;
        }
        while (reward != null);
        Events.OnVideoRewardCompleted(_fullyWatchedPlacement.ToString(), list);
        _fullyWatchedPlacement = GameAdsPlacement.None;
    }

    public void OnVideoAvailabilityChanged(bool isAvailable)
    {
        Events.OnVideoAvailabilityChanged(isAvailable);
    }
}