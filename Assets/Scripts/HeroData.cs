using System.Collections.Generic;
using UnityEngine;

public class HeroData
{
	public HeroConfig HeroConfig;

	public HeroProfile Profile;

	public List<WeaponData> Weapons;

	public HeroEvents Events;

	public int CardCollectedCount => HeroConfig.GetCurrentCardsCount(Level, Profile.CardCollectedCount);

	public int CardAmountNeededBase => HeroConfig.GetCardAmountNeeded(Level);

	public float NextLevelObjective01 => Mathf.Clamp01((float)CardCollectedCount / (float)CardAmountNeededBase);

	public bool CardObjectiveReached => CardCollectedCount >= CardAmountNeededBase;

	public string NextLevelProgressString => CardCollectedCount + "/" + CardAmountNeededBase;

	public int Level => Profile.Level;

	public bool Unlocked => Profile.CardCollectedCount > 0;

	public bool HasReachMaxLevel => Level >= 20;

	public HeroData(HeroConfig heroConfig, HeroProfile profile, List<WeaponData> weapons, HeroEvents events)
	{
		HeroConfig = heroConfig;
		Profile = profile;
		Weapons = weapons;
		Events = events;
	}

	public bool CanHeal()
	{
		return Profile.HP < GetMaxHP();
	}

	public int GetMaxHP()
	{
		return HeroConfig.GetHPMax(Profile.Level);
	}

	public int GetLevelUpPriceAmount()
	{
		return HeroConfig.GetLevelUpPriceAmount(Profile.Level);
	}

	public int GetNextLevelHPBonus()
	{
		return GetHPMax(Profile.Level + 1) - GetMaxHP();
	}

	private int GetHPMax(int level)
	{
		return HeroConfig.GetHPMax(level);
	}
}