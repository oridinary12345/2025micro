using System;
using System.Collections.Generic;

public class GameAdsEvents
{
	public event Action<string, List<Reward>> VideoRewardCompletedEvent;

	public event Action VideoPlayFailedEvent;

	public event Action<bool> VideoAvailabilityChangedEvent;

	public event Action VideoOpenedEvent;

	public event Action VideoClosedEvent;

	public void OnVideoRewardCompleted(string placementId, List<Reward> rewards)
	{
		if (this.VideoRewardCompletedEvent != null)
		{
			this.VideoRewardCompletedEvent(placementId, rewards);
		}
	}

	public void OnVideoPlayFailed()
	{
		if (this.VideoPlayFailedEvent != null)
		{
			this.VideoPlayFailedEvent();
		}
	}

	public void OnVideoAvailabilityChanged(bool available)
	{
		if (this.VideoAvailabilityChangedEvent != null)
		{
			this.VideoAvailabilityChangedEvent(available);
		}
	}

	public void OnVideoOpened()
	{
		if (this.VideoOpenedEvent != null)
		{
			this.VideoOpenedEvent();
		}
	}

	public void OnVideoClosed()
	{
		if (this.VideoClosedEvent != null)
		{
			this.VideoClosedEvent();
		}
	}
}