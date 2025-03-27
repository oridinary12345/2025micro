using System.Collections.Generic;

public class WorldConfigs : Configs<WorldConfigs>
{
	public const string DefaultWorldId = "w00";

	private readonly Dictionary<string, WorldConfig> _configs = new Dictionary<string, WorldConfig>();

	public override ConfigType ConfigType => ConfigType.World;

	protected override void Init()
	{
		base.Init();
		_configs["w00"] = new WorldConfig
		{
			Id = "w00",
			Title = "ENCHANTED FOREST",
			Index = 0,
			PriceToPlay = LootProfile.Create("lootCoin", 1000),
			PricePlayIncrement = 100,
			PriceAmountMax = 20000,
			PriceToUnlock = LootProfile.Create("lootCoin")
		};
		_configs["w01"] = new WorldConfig
		{
			Id = "w01",
			Title = "THE DUNGEON",
			Index = 1,
			PriceToPlay = LootProfile.Create("lootCoin", 2000),
			PricePlayIncrement = 200,
			PriceAmountMax = 40000,
			PriceToUnlock = LootProfile.Create("lootCoin", 5000),
			MissionIdsToUnlock = new List<string>
			{
				"missionUnlockWorld01.01n",
				"missionUnlockWorld01.02n",
				"missionUnlockWorld01.03n"
			}
		};
		_configs["w02"] = new WorldConfig
		{
			Id = "w02",
			Title = "THE SWAMP",
			Index = 2,
			PriceToPlay = LootProfile.Create("lootCoin", 4000),
			PricePlayIncrement = 400,
			PriceAmountMax = 80000,
			PriceToUnlock = LootProfile.Create("lootCoin", 25000),
			MissionIdsToUnlock = new List<string>
			{
				"missionUnlockWorld02.01",
				"missionUnlockWorld02.02",
				"missionUnlockWorld02.03"
			}
		};
		MissionConfigs instance = MonoSingleton<MissionConfigs>.Instance;
		instance.AddConfig(new MissionConfig
		{
			Id = "missionUnlockWorld01.01n",
			MissionType = MissionType.MonsterKill,
			Objective = 75f,
			ParamStr1 = string.Empty,
			ParamStr2 = string.Empty,
			ParamInt1 = 0
		});
		instance.AddConfig(new MissionConfig
		{
			Id = "missionUnlockWorld01.02n",
			MissionType = MissionType.MonsterKill,
			Objective = 15f,
			ParamStr1 = "mushroom",
			ParamStr2 = "weapon01",
			ParamInt1 = 0
		});
		instance.AddConfig(new MissionConfig
		{
			Id = "missionUnlockWorld01.03n",
			MissionType = MissionType.MonsterKill,
			Objective = 10f,
			ParamStr1 = "mushroom",
			ParamStr2 = "weapon02",
			ParamInt1 = 0
		});
		instance.AddConfig(new MissionConfig
		{
			Id = "missionUnlockWorld02.01",
			MissionType = MissionType.MonsterKill,
			Objective = 500f,
			ParamStr1 = string.Empty,
			ParamStr2 = string.Empty,
			ParamInt1 = 0
		});
		instance.AddConfig(new MissionConfig
		{
			Id = "missionUnlockWorld02.02",
			MissionType = MissionType.MonsterKill,
			Objective = 10f,
			ParamStr1 = "ghost",
			ParamStr2 = "weapon05",
			ParamInt1 = 0
		});
		instance.AddConfig(new MissionConfig
		{
			Id = "missionUnlockWorld02.03",
			MissionType = MissionType.MonsterKill,
			Objective = 20f,
			ParamStr1 = "spider",
			ParamStr2 = "weapon06",
			ParamInt1 = 0
		});
	}

	public WorldConfig GetConfig(string id)
	{
 WorldConfig value;		if (_configs.TryGetValue(id, out value))
		{
			return value;
		}
		return WorldConfig.Invalid;
	}

	public WorldConfig GetNextWorld(string id)
	{
		int num = GetConfig(id).Index + 1;
		foreach (WorldConfig value in _configs.Values)
		{
			if (value.Index == num)
			{
				return value;
			}
		}
		return WorldConfig.Invalid;
	}

	public List<WorldConfig> GetAllConfigs()
	{
		return new List<WorldConfig>(_configs.Values);
	}
}