using System.Collections.Generic;

public class WorldConfig
{
	public string Id;

	public string Title;

	public int Index;

	public LootProfile PriceToUnlock;

	public LootProfile PriceToPlay;

	public int PricePlayIncrement;

	public int PriceAmountMax;

	public List<string> MissionIdsToUnlock;

	public static WorldConfig Invalid = new WorldConfig
	{
		Id = "worldInvalid"
	};

	public override bool Equals(object obj)
	{
		WorldConfig worldConfig = obj as WorldConfig;
		return worldConfig != null && worldConfig.Id == Id;
	}

	public override int GetHashCode()
	{
		return $"{Id}".GetHashCode();
	}

	public bool IsValid()
	{
		return Id != Invalid.Id;
	}
}