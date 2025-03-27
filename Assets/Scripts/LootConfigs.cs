using System.Collections.Generic;
using Utils;

public class LootConfigs : Configs<LootConfigs>
{
	public const string LOOT_COIN = "lootCoin";

	public const string LOOT_RUBY = "lootRuby";

	public const string LOOT_SAPPHIRE = "lootSapphire";

	public const string LOOT_LIFE = "lootLife";

	public const string LOOT_WEAPON = "lootWeapon";

	public const string LOOT_HERO = "lootHero";

	public const string LOOT_KEY = "lootKey";

	public const string LOOT_ITEM_BOMB = "lootItemBomb";

	private readonly Dictionary<string, LootConfig> _configs = new Dictionary<string, LootConfig>();

	public override ConfigType ConfigType => ConfigType.Loot;

	protected override void Init()
	{
		base.Init();
		_configs["lootCoin"] = new LootConfig
		{
			Id = "lootCoin",
			Title = "Coins",
			LootType = LootType.currencySoft,
			MaxCapacity = 999999
		};
		_configs["lootRuby"] = new LootConfig
		{
			Id = "lootRuby",
			Title = "Rubies",
			LootType = LootType.currencyHard,
			MaxCapacity = 999999
		};
		_configs["lootLife"] = new LootConfig
		{
			Id = "lootLife",
			Title = "Life",
			LootType = LootType.currencySoft,
			MaxCapacity = 9999
		};
		_configs["lootKey"] = new LootConfig
		{
			Id = "lootKey",
			Title = "Key",
			LootType = LootType.currencySoft,
			MaxCapacity = 999
		};
		_configs["lootItemBomb"] = new LootConfig
		{
			Id = "lootItemBomb",
			Title = "Bomb",
			LootType = LootType.currentItem,
			MaxCapacity = 10
		};
	}

	public LootConfig GetConfig(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			return null;
		}
 LootConfig value;		if (_configs.TryGetValue(id, out value))
		{
			return value;
		}
		return null;
	}

	public override void LoadFromCSV(CSVFile file)
	{
		for (int i = 0; i < file.EntriesCount; i++)
		{
			string @string = file.GetString(i, "Id");
			if (_configs.ContainsKey(@string))
			{
				LootConfig lootConfig = _configs[@string];
				lootConfig.Title = file.GetString(i, "Title");
				lootConfig.LootType = Enum.TryParse(file.GetString(i, "LootType"), LootType.currencySoft);
				lootConfig.MaxCapacity = file.GetInt(i, "MaxCapacity");
			}
		}
	}
}