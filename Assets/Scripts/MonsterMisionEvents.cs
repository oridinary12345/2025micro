using System;
using System.Collections.Generic;

public class MonsterMisionEvents
{
	public event Action<MonsterMissionData, List<Reward>> MonsterRewardClaimedEvent;

	public event Action MonsterMissionRefreshedEvent;

	public event Action<string> MonsterMissionCompletedEvent;

	public void OnMonsterRewardClaimed(MonsterMissionData data, List<Reward> rewards)
	{
		if (this.MonsterRewardClaimedEvent != null)
		{
			this.MonsterRewardClaimedEvent(data, rewards);
		}
	}

	public void OnMonsterMissionRefreshed()
	{
		if (this.MonsterMissionRefreshedEvent != null)
		{
			this.MonsterMissionRefreshedEvent();
		}
	}

	public void OnMonsterMissionCompleted(string monsterId)
	{
		if (this.MonsterMissionCompletedEvent != null)
		{
			this.MonsterMissionCompletedEvent(monsterId);
		}
	}
}