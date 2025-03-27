using System.Collections.Generic;
using UnityEngine;
using Utils;

public class RewardConfigs : Configs<RewardConfigs>
{
	private readonly Dictionary<string, RewardConfig> _configs = new Dictionary<string, RewardConfig>();

	public override ConfigType ConfigType => ConfigType.Reward;

	protected override void Init()
	{
		base.Init();
		MonoSingleton<MonsterConfigs>.Instance.Create();
		AddConfig(new RewardConfig
		{
			Id = "reward" + GameAdsPlacement.FreeRubies,
			RewardType = RewardType.loot,
			RewardTypeId = "lootRuby",
			AmountMin = 10,
			AmountMax = 10,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0
		});
		AddConfig(new RewardConfig
		{
			Id = "reward" + GameAdsPlacement.FreeHeroUpgrade,
			RewardType = RewardType.freeHeroUpgrade,
			RewardTypeId = string.Empty,
			AmountMin = 1,
			AmountMax = 1,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0
		});
		AddConfig(new RewardConfig
		{
			Id = "reward" + GameAdsPlacement.FreeWeaponUpgrade,
			RewardType = RewardType.freeWeaponUpgrade,
			RewardTypeId = string.Empty,
			AmountMin = 1,
			AmountMax = 1,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0
		});
		AddConfig(new RewardConfig
		{
			Id = "reward" + GameAdsPlacement.FreeHeroHeal,
			RewardType = RewardType.freeHeroHeal,
			RewardTypeId = string.Empty,
			AmountMin = 1,
			AmountMax = 1,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0
		});
		AddConfig(new RewardConfig
		{
			Id = "reward" + GameAdsPlacement.FreeWeaponRepair,
			RewardType = RewardType.freeWeaponRepair,
			RewardTypeId = string.Empty,
			AmountMin = 1,
			AmountMax = 1,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0
		});
		foreach (ShopProductConfig productConfig in ShopProductCatalog.GetProductConfigs(ShopProductType.Rubies))
		{
			AddConfig(new RewardConfig
			{
				Id = productConfig.RewardId,
				RewardType = RewardType.loot,
				RewardTypeId = "lootRuby",
				AmountMin = productConfig.Amount,
				AmountMax = productConfig.Amount,
				Weight = 0,
				Percentage = 1f,
				ParamInt1 = 0
			});
		}
		foreach (ShopProductConfig productConfig2 in ShopProductCatalog.GetProductConfigs(ShopProductType.Coins))
		{
			AddConfig(new RewardConfig
			{
				Id = productConfig2.RewardId,
				RewardType = RewardType.loot,
				RewardTypeId = "lootCoin",
				AmountMin = productConfig2.Amount,
				AmountMax = productConfig2.Amount,
				Weight = 0,
				Percentage = 1f,
				ParamInt1 = 0
			});
		}
	}

	public RewardConfig GetConfig(string id)
	{
 RewardConfig value;		if (!string.IsNullOrEmpty(id) && _configs.TryGetValue(id, out value))
		{
			return value;
		}
		return null;
	}

	public void AddConfig(RewardConfig config)
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
				RewardConfig rewardConfig = _configs[@string];
				rewardConfig.RewardType = Enum.TryParse(file.GetString(i, "RewardType"), RewardType.loot);
				rewardConfig.RewardTypeId = file.GetString(i, "RewardTypeId", string.Empty);
				rewardConfig.AmountMin = file.GetInt(i, "AmountMin");
				rewardConfig.AmountMax = file.GetInt(i, "AmountMax");
				rewardConfig.Weight = file.GetInt(i, "Weight");
				rewardConfig.Percentage = file.GetFloat(i, "Percentage");
				rewardConfig.ParamInt1 = file.GetInt(i, "ParamInt1");
			}
			else
			{
				UnityEngine.Debug.LogWarning("[" + ConfigType + "] failed to overwrite " + @string);
			}
		}
	}
}