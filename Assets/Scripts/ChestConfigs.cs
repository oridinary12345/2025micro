using System.Collections.Generic;
using UnityEngine;
using Utils;

public class ChestConfigs : Configs<ChestConfigs>
{
	public const int ChestAdsFreeRubies = 2;

	public const string CHEST_ADS_ID = "chestAds";

	public const string CHEST_TIMEDFREE_ID = "chestTimedFree";

	public const string CHEST_PREMIUM_ID1 = "chestPremium1";

	public const string CHEST_PREMIUM_ID2 = "chestPremium2";

	public static string REWARD_CHEST_ADS_ID1 = "reward" + GameAdsPlacement.FreeCoins;

	public static string REWARD_CHEST_ADS_ID2 = REWARD_CHEST_ADS_ID1 + "2";

	public static string REWARD_CHEST_TIMEDFREE_ID1 = "reward" + "chestTimedFree".ToUpperFirst();

	public static string REWARD_CHEST_TIMEDFREE_ID2 = REWARD_CHEST_TIMEDFREE_ID1 + "2";

	public static string REWARD_CHEST_TIMEDFREE_ID3 = REWARD_CHEST_TIMEDFREE_ID1 + "3";

	public static string REWARD_CHEST_PREMIUM1_ID1 = "reward" + "chestPremium1".ToUpperFirst() + "1";

	public static string REWARD_CHEST_PREMIUM1_ID2A = "reward" + "chestPremium1".ToUpperFirst() + "2A";

	public static string REWARD_CHEST_PREMIUM1_ID2B = "reward" + "chestPremium1".ToUpperFirst() + "2B";

	public static string REWARD_CHEST_PREMIUM1_ID2C = "reward" + "chestPremium1".ToUpperFirst() + "2C";

	public static string REWARD_CHEST_PREMIUM1_ID3 = "reward" + "chestPremium1".ToUpperFirst() + "3";

	public static string REWARD_CHEST_PREMIUM2_ID1 = "reward" + "chestPremium2".ToUpperFirst() + "1";

	public static string REWARD_CHEST_PREMIUM2_ID2A = "reward" + "chestPremium2".ToUpperFirst() + "2A";

	public static string REWARD_CHEST_PREMIUM2_ID2B = "reward" + "chestPremium2".ToUpperFirst() + "2B";

	public static string REWARD_CHEST_PREMIUM2_ID2C = "reward" + "chestPremium2".ToUpperFirst() + "2C";

	public static string REWARD_CHEST_PREMIUM2_ID3 = "reward" + "chestPremium2".ToUpperFirst() + "3";

	private readonly Dictionary<string, ChestConfig> _configs = new Dictionary<string, ChestConfig>();

	public override ConfigType ConfigType => ConfigType.Chest;

	public ChestConfig GetConfig(string id)
	{
 ChestConfig value;		if (!string.IsNullOrEmpty(id) && _configs.TryGetValue(id, out value))
		{
			return value;
		}
		return ChestConfig.Invalid;
	}

	protected override void Init()
	{
		base.Init();
		AddConfig(new ChestConfig
		{
			Id = "chestAds",
			Name = "ADS CHEST",
			ChestType = ChestType.Ads,
			WaitingTimeHour = 4,
			Max = 3,
			RewardIds = new List<string>
			{
				REWARD_CHEST_ADS_ID1,
				REWARD_CHEST_ADS_ID2
			},
			PrefabSkinName = "ChestAds",
			IsContinuousTimer = true
		});
		AddConfig(new ChestConfig
		{
			Id = "chestTimedFree",
			Name = "FREE CHEST",
			ChestType = ChestType.TimedFree,
			WaitingTimeHour = 4,
			Max = 1,
			RewardIds = new List<string>
			{
				REWARD_CHEST_TIMEDFREE_ID1,
				REWARD_CHEST_TIMEDFREE_ID2,
				REWARD_CHEST_TIMEDFREE_ID3
			},
			PrefabSkinName = "ChestMedium"
		});
		AddConfig(new ChestConfig
		{
			Id = "chestPremium1",
			Name = "PREMIUM CHEST",
			ChestType = ChestType.Big,
			WaitingTimeHour = 0,
			Max = int.MaxValue,
			RewardIds = new List<string>
			{
				REWARD_CHEST_PREMIUM1_ID1,
				REWARD_CHEST_PREMIUM1_ID2A,
				REWARD_CHEST_PREMIUM1_ID2B,
				REWARD_CHEST_PREMIUM1_ID2C,
				REWARD_CHEST_PREMIUM1_ID3
			},
			PrefabSkinName = "ChestBig",
			PriceLootId = "lootRuby",
			PriceLootAmout = 750
		});
		AddConfig(new ChestConfig
		{
			Id = "chestPremium2",
			Name = "PREMIUM CHEST",
			ChestType = ChestType.Big,
			WaitingTimeHour = 0,
			Max = int.MaxValue,
			RewardIds = new List<string>
			{
				REWARD_CHEST_PREMIUM2_ID1,
				REWARD_CHEST_PREMIUM2_ID2A,
				REWARD_CHEST_PREMIUM2_ID2B,
				REWARD_CHEST_PREMIUM2_ID2C,
				REWARD_CHEST_PREMIUM2_ID3
			},
			PrefabSkinName = "ChestBig",
			PriceLootId = "lootRuby",
			PriceLootAmout = 2500
		});
		RewardConfigs instance = MonoSingleton<RewardConfigs>.Instance;
		instance.AddConfig(new RewardConfig
		{
			Id = REWARD_CHEST_ADS_ID1,
			RewardType = RewardType.freeCoinsAds,
			RewardTypeId = string.Empty,
			AmountMin = 1,
			AmountMax = 1,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0
		});
		instance.AddConfig(new RewardConfig
		{
			Id = REWARD_CHEST_ADS_ID2,
			RewardType = RewardType.loot,
			RewardTypeId = "lootRuby",
			AmountMin = 2,
			AmountMax = 2,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0
		});
		instance.AddConfig(new RewardConfig
		{
			Id = REWARD_CHEST_TIMEDFREE_ID1,
			RewardType = RewardType.chestTimed,
			AmountMin = 1,
			AmountMax = 1,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0
		});
		instance.AddConfig(new RewardConfig
		{
			Id = REWARD_CHEST_TIMEDFREE_ID2,
			RewardType = RewardType.loot,
			RewardTypeId = "lootRuby",
			AmountMin = 10,
			AmountMax = 10,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0
		});
		instance.AddConfig(new RewardConfig
		{
			Id = REWARD_CHEST_TIMEDFREE_ID3,
			RewardType = RewardType.weapon,
			RewardTypeId = string.Empty,
			AmountMin = 1,
			AmountMax = 1,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0,
			IsCard = true
		});
		instance.AddConfig(new RewardConfig
		{
			Id = REWARD_CHEST_PREMIUM1_ID1,
			RewardType = RewardType.chestPremium1,
			AmountMin = 1,
			AmountMax = 1,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0
		});
		instance.AddConfig(new RewardConfig
		{
			Id = REWARD_CHEST_PREMIUM1_ID2A,
			RewardType = RewardType.weapon,
			RewardTypeId = string.Empty,
			RewardSubTypeId = WeaponRangeType.Short.ToString(),
			AmountMin = 3,
			AmountMax = 3,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0,
			IsCard = true
		});
		instance.AddConfig(new RewardConfig
		{
			Id = REWARD_CHEST_PREMIUM1_ID2B,
			RewardType = RewardType.weapon,
			RewardTypeId = string.Empty,
			RewardSubTypeId = WeaponRangeType.Medium.ToString(),
			AmountMin = 3,
			AmountMax = 3,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0,
			IsCard = true
		});
		instance.AddConfig(new RewardConfig
		{
			Id = REWARD_CHEST_PREMIUM1_ID2C,
			RewardType = RewardType.weapon,
			RewardTypeId = string.Empty,
			RewardSubTypeId = WeaponRangeType.Long.ToString(),
			AmountMin = 3,
			AmountMax = 3,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0,
			IsCard = true
		});
		instance.AddConfig(new RewardConfig
		{
			Id = REWARD_CHEST_PREMIUM1_ID3,
			RewardType = RewardType.hero,
			RewardTypeId = string.Empty,
			AmountMin = 3,
			AmountMax = 3,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0,
			IsCard = true
		});
		instance.AddConfig(new RewardConfig
		{
			Id = REWARD_CHEST_PREMIUM2_ID1,
			RewardType = RewardType.chestPremium2,
			AmountMin = 1,
			AmountMax = 1,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0
		});
		instance.AddConfig(new RewardConfig
		{
			Id = REWARD_CHEST_PREMIUM2_ID2A,
			RewardType = RewardType.weapon,
			RewardTypeId = string.Empty,
			RewardSubTypeId = WeaponRangeType.Short.ToString(),
			AmountMin = 15,
			AmountMax = 15,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0,
			IsCard = true
		});
		instance.AddConfig(new RewardConfig
		{
			Id = REWARD_CHEST_PREMIUM2_ID2B,
			RewardType = RewardType.weapon,
			RewardTypeId = string.Empty,
			RewardSubTypeId = WeaponRangeType.Medium.ToString(),
			AmountMin = 15,
			AmountMax = 15,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0,
			IsCard = true
		});
		instance.AddConfig(new RewardConfig
		{
			Id = REWARD_CHEST_PREMIUM2_ID2C,
			RewardType = RewardType.weapon,
			RewardTypeId = string.Empty,
			RewardSubTypeId = WeaponRangeType.Long.ToString(),
			AmountMin = 15,
			AmountMax = 15,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0,
			IsCard = true
		});
		instance.AddConfig(new RewardConfig
		{
			Id = REWARD_CHEST_PREMIUM2_ID3,
			RewardType = RewardType.hero,
			RewardTypeId = string.Empty,
			AmountMin = 15,
			AmountMax = 15,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0,
			IsCard = true
		});
	}

	public void AddConfig(ChestConfig config)
	{
		_configs[config.Id] = config;
	}

	public override void LoadFromCSV(CSVFile file)
	{
		for (int i = 0; i < file.EntriesCount; i++)
		{
			string @string = file.GetString(i, "Id");
			if (_configs.ContainsKey(@string))
			{
				ChestConfig chestConfig = _configs[@string];
				chestConfig.Id = file.GetString(i, "Id");
				chestConfig.ChestType = Enum.TryParse(file.GetString(i, "ChestType"), ChestType.Invalid);
			}
			else
			{
				UnityEngine.Debug.LogWarning("[" + ConfigType + "] failed to overwrite " + @string);
			}
		}
	}
}