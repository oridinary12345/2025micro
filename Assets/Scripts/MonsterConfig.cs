using System.Collections.Generic;

public class MonsterConfig
{
	public string Id;

	public string Name;

	public int DamageMin;

	public int DamageMax;

	public int HP;

	public MonsterType MonsterType;

	public List<string> RewardIdHit;

	public List<string> RewardIdDead;

	public Element Element;

	public string MovePatternForward;

	public string MovePatternBackward;

	public int TokenValue;

	public WeaponType WeaponType;

	public float MissRate;

	public int AttackEachRoundCount;

	public int LifeDurationRoundCount;

	public int AttackAttemptCountMax;

	public bool IsChest()
	{
		return Id == "chest" || Id == "chestSmall" || Id == "chestMedium" || Id == "chestBig" || Id == "chestLocked";
	}

	public bool IsLockedChest()
	{
		return Id == "chestLocked";
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
		return $"[MonsterConfig Id = {Id}, DamageMin = {DamageMin}, DamageMax = {DamageMax}, HP = {HP}]";
	}
}