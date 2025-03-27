using System.Collections.Generic;

public class RewardList : Reward
{
	public override RewardType RewardType => RewardType.list;

	public List<Reward> Rewards
	{
		get;
		private set;
	}

	public RewardList()
		: base(1f)
	{
		Rewards = new List<Reward>();
	}

	public void Add(Reward reward)
	{
		Rewards.Add(reward);
	}

	public void AddRange(List<Reward> rewards)
	{
		Rewards.AddRange(rewards);
	}

	public override bool Merge(Reward reward)
	{
		RewardList rewardList = reward as RewardList;
		if (rewardList != null)
		{
			AddRange(rewardList.Rewards);
			return true;
		}
		return false;
	}
}