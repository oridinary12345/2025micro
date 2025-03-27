public class LootConfig
{
	public string Id;

	public string Title;

	public LootType LootType;

	public int MaxCapacity;

	public static LootConfig Invalid = new LootConfig
	{
		Id = "lootInvalid",
		Title = string.Empty,
		LootType = LootType.currencySoft,
		MaxCapacity = 0
	};

	public override bool Equals(object obj)
	{
		HeroConfig heroConfig = obj as HeroConfig;
		return heroConfig.Id == Id;
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