public class RewardHero : Reward
{
	public override RewardType RewardType => RewardType.hero;

	public string HeroId
	{
		get;
		private set;
	}

	public int CardCount
	{
		get;
		private set;
	}

	public RewardHero(string heroId, int cardCount, float percentage)
		: base(percentage, true)
	{
		HeroId = heroId;
		CardCount = cardCount;
	}

	public override bool Merge(Reward reward)
	{
		RewardHero rewardHero = reward as RewardHero;
		if (rewardHero != null && HeroId == rewardHero.HeroId)
		{
			CardCount += rewardHero.CardCount;
			return true;
		}
		return false;
	}
}