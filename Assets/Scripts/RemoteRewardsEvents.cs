using System;

public class RemoteRewardsEvents
{
	public event Action<Reward> RemoteRewardReceivedEvent;

	public void OnRemoteRewardReceived(Reward reward)
	{
		if (this.RemoteRewardReceivedEvent != null)
		{
			this.RemoteRewardReceivedEvent(reward);
		}
	}
}