public class RewardLoot : Reward
{
	private readonly LootProfile _profile;

	public override RewardType RewardType => RewardType.loot;

	public string LootId => _profile.LootId;

	public int LootAmount => _profile.Amount;

	public RewardLoot(LootProfile profile, float percentage, bool isCard)
		: base(percentage, isCard)
	{
		_profile = profile;
	}

	public override bool Merge(Reward reward)
	{
		RewardLoot rewardLoot = reward as RewardLoot;
		if (rewardLoot != null && LootId == rewardLoot.LootId)
		{
			_profile.Amount += rewardLoot.LootAmount;
			return true;
		}
		return false;
	}

	public override string ToString()
	{
		return "+" + LootAmount.ToString("### ### ###").Trim() + InlineSprites.GetLootInlineSprite(LootId);
	}
}