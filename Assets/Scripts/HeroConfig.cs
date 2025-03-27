using System;
using UnityEngine;
using Utils;

public class HeroConfig
{
	public string Id;

	public string Name;

	public int HpMaxBase;

	public int HpLevelUpPriceVariant;

	public float MissRate;

	public override bool Equals(object obj)
	{
		HeroConfig heroConfig = obj as HeroConfig;
		return heroConfig.Id == Id;
	}

	public int GetHPMax(int level)
	{
		if (level == 1)
		{
			return HpMaxBase;
		}
		float num = (level % 10 != 0) ? 1.1f : 1.5f;
		return Convert.ToInt32(Utils.Math.ROUND((float)GetHPMax(level - 1) * num + (float)level, -1));
	}

	public int GetLevelUpPriceAmount(int level)
	{
		if (level == 1)
		{
			return HpLevelUpPriceVariant;
		}
		int num = HpLevelUpPriceVariant + (level - 1) * 200;
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

	public override int GetHashCode()
	{
		return $"{Id}".GetHashCode();
	}

	public override string ToString()
	{
		return $"[HeroConfig] Id = {Id}, Name = {Name}, HpMaxBase = {HpMaxBase}, HpLevelUpPriceVariant = {HpLevelUpPriceVariant}, MissRate = {MissRate}, ";
	}
}