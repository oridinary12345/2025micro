public class RewardFreeCoins : Reward
{
	private RewardType _rewardType;

	public override RewardType RewardType => _rewardType;

	public int CoinsAmount
	{
		get;
		private set;
	}

	public RewardFreeCoins(RewardType rewardType, int coinsAmount)
		: base(1f)
	{
		_rewardType = rewardType;
		CoinsAmount = coinsAmount;
	}

	public override bool Merge(Reward reward)
	{
		RewardFreeCoins rewardFreeCoins = reward as RewardFreeCoins;
		if (rewardFreeCoins != null && _rewardType == rewardFreeCoins.RewardType)
		{
			CoinsAmount += rewardFreeCoins.CoinsAmount;
			return true;
		}
		return false;
	}

	public override string ToString()
	{
		return "+" + CoinsAmount.ToString("### ### ###").Trim() + InlineSprites.GetLootInlineSprite("lootCoin");
	}
}