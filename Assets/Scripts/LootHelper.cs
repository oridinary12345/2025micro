public static class LootHelper
{
	public static void Add(LootProfiles profiles, string lootId, int lootAmount)
	{
 LootProfile value;		if (!profiles.Loots.TryGetValue(lootId, out value))
		{
			value = LootProfile.Create(lootId);
			profiles.Loots.Add(lootId, value);
		}
		value.Amount += lootAmount;
	}
}