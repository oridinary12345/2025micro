public class GameIronSource : MonoSingleton<GameIronSource>
{
    private GameAdsController _controller;

    public GameIronSource Setup(GameAdsController controller)
    {
        base.transform.parent = controller.transform;
        _controller = controller;
        return this;
    }

    protected virtual void OnDestroy()
    {
    }

    public bool AreRewardedVideoReady()
    {
        return true;
    }


    public void ShowRewardedVideo(string placementId)
    {
        RewardedVideoAdRewardedEvent(placementId);
    }

    private void RewardedVideoAdOpenedEvent()
    {
        _controller.OnVideoOpened();
    }

    private void RewardedVideoAdClosedEvent()
    {
        _controller.OnVideoClosed();
    }

    private void RewardedVideoAvailabilityChangedEvent(bool available)
    {
        bool isAvailable = available;
        _controller.OnVideoAvailabilityChanged(isAvailable);
    }

    private void RewardedVideoAdStartedEvent()
    {
    }

    private void RewardedVideoAdEndedEvent()
    {
    }

    private void RewardedVideoAdRewardedEvent(string placementId)
    {
        this._controller.OnVideoRewardCompleted(placementId);
    }
    private void RewardedVideoAdShowFailedEvent()
    {
        _controller.OnVideoPlayFailed();
    }

    private void OnApplicationPause(bool isPaused)
    {
    }
}