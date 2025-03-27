using UnityEngine;

public abstract class Reward
{
	private readonly float _percentage01;

	public bool HasUnlockedSomething;

	public abstract RewardType RewardType
	{
		get;
	}

	public bool IsCard
	{
		get;
		private set;
	}

	protected Reward(float percentage01, bool isCard = false)
	{
		_percentage01 = percentage01;
		IsCard = isCard;
	}

	public bool CheckOdds()
	{
		if (_percentage01 == 0f || _percentage01 == 1f)
		{
			return true;
		}
		return Random.value < _percentage01;
	}

	public abstract bool Merge(Reward reward);
}