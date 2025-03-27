public class RewardWeapon : Reward
{
	public override RewardType RewardType => RewardType.weapon;

	public string WeaponId
	{
		get;
		private set;
	}

	public int CardCount
	{
		get;
		private set;
	}

	public RewardWeapon(string weaponId, int cardCount, float percentage)
		: base(percentage, true)
	{
		WeaponId = weaponId;
		CardCount = cardCount;
	}

	public override bool Merge(Reward reward)
	{
		RewardWeapon rewardWeapon = reward as RewardWeapon;
		if (rewardWeapon != null && WeaponId == rewardWeapon.WeaponId)
		{
			CardCount += rewardWeapon.CardCount;
			return true;
		}
		return false;
	}
}