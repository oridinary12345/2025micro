public class RewardLife : Reward
{
	public override RewardType RewardType => RewardType.life;

	public int HealingAmount
	{
		get;
		private set;
	}

	public RewardLife(int healingAmount, float percentage)
		: base(percentage)
	{
		HealingAmount = healingAmount;
	}

	public override bool Merge(Reward reward)
	{
		RewardLife rewardLife = reward as RewardLife;
		if (rewardLife != null)
		{
			HealingAmount += rewardLife.HealingAmount;
			return true;
		}
		return false;
	}
}