public class RewardFreeHeroHeal : Reward
{
	public override RewardType RewardType => RewardType.freeHeroHeal;

	public RewardFreeHeroHeal()
		: base(1f)
	{
	}

	public override bool Merge(Reward reward)
	{
		return true;
	}
}