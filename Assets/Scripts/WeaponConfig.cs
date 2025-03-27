using UnityEngine;

public class WeaponConfig
{
	public string Id;

	public string Title;

	public string WorldId;

	public int HpMaxBase;

	public int HpMaxLevelRatio;

	public int UpgradePriceBase;

	public int DamageMin;

	public int DamageMax;

	public int DamageLevelRatio;

	public Element ElementWeak;

	public Element ElementCritical;

	public Element ElementMiss;

	public WeaponType WeaponType;

	public WeaponRangeType RangeType;

	public int PushForce;

	public float CriticalChances;

	public bool CanCounterGhost;

	public int GetHPMax(int level)
	{
		return HpMaxBase + HpMaxLevelRatio * (level - 1);
	}

	public int GetLevelUpPriceAmount(int level)
	{
		if (level == 1)
		{
			return UpgradePriceBase;
		}
		int num = UpgradePriceBase + (level - 1) * 200;
		return GetLevelUpPriceAmount(level - 1) + num;
	}

	public int GetCurrentCardsCount(int currentLevel, int cardCountTotal)
	{
		int num = cardCountTotal;
		for (int i = 1; i < currentLevel; i++)
		{
			num -= GetCardAmountNeeded(i);
		}
		return Mathf.Max(0, num);
	}

	public int GetCardAmountNeeded(int level)
	{
		return level + 1;
	}

	public int GetRepairPriceAmount(int level, int hp)
	{
		float num = (float)GetHPMax(level) - (float)hp;
		return Mathf.CeilToInt(num / 5f);
	}

	public override bool Equals(object obj)
	{
		HeroConfig heroConfig = obj as HeroConfig;
		return heroConfig.Id == Id;
	}

	public override int GetHashCode()
	{
		return $"{Id}".GetHashCode();
	}

	public override string ToString()
	{
		return $"[WeaponConfig] Id = {Id}, DamageMin = {DamageMin}, DamageMax = {DamageMax}";
	}
}