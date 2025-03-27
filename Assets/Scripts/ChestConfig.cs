using System;
using System.Collections.Generic;

[Serializable]
public class ChestConfig
{
	public string Id;

	public string Name;

	public ChestType ChestType;

	public int WaitingTimeHour;

	public int Max;

	public List<string> RewardIds;

	public string PrefabSkinName;

	public string PriceLootId;

	public int PriceLootAmout;

	public bool IsContinuousTimer;

	public static ChestConfig Invalid = new ChestConfig
	{
		Id = "chestInvalid"
	};

	public bool IsValid()
	{
		return Id != Invalid.Id;
	}
}