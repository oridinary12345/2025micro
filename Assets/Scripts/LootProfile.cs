using Newtonsoft.Json;
using System;

[Serializable]
public class LootProfile
{
	[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public string LootId;

	[JsonProperty(PropertyName = "q", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public int Amount;

	public static LootProfile Create(string lootId, int amount = 0)
	{
		LootProfile lootProfile = new LootProfile();
		lootProfile.LootId = lootId;
		lootProfile.Amount = amount;
		return lootProfile;
	}

	public bool IsValid()
	{
		return LootId != LootConfig.Invalid.Id;
	}

	public override string ToString()
	{
		return Amount.ToString("### ### ###").Trim() + InlineSprites.GetLootInlineSprite(LootId);
	}
}