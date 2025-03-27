public class RewardFreeWeaponRepair : Reward
{
	public override RewardType RewardType => RewardType.freeWeaponRepair;

	public string WeaponId
	{
		get;
		private set;
	}

	public RewardFreeWeaponRepair(string weaponId)
		: base(1f)
	{
		WeaponId = weaponId;
	}

	public override bool Merge(Reward reward)
	{
		RewardFreeWeaponRepair rewardFreeWeaponRepair = reward as RewardFreeWeaponRepair;
		if (rewardFreeWeaponRepair != null)
		{
			return WeaponId == rewardFreeWeaponRepair.WeaponId;
		}
		return false;
	}
}