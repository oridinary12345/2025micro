public class LootData
{
	private readonly LootConfig _config;

	private readonly LootProfile _profile;

	public string Id => _config.Id;

	public string Title => _config.Title;

	public int Amount => _profile.Amount;

	public int AmountMax => _config.MaxCapacity;

	public LootData(LootConfig config, LootProfile profile)
	{
		_config = config;
		_profile = profile;
	}
}