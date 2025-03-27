using System.Collections.Generic;
using UnityEngine;

public class AdventureLevelConfigs : Configs<AdventureLevelConfigs>
{
	private readonly Dictionary<string, AdventureLevelConfig> _configs = new Dictionary<string, AdventureLevelConfig>();

	public override ConfigType ConfigType => ConfigType.Level;

	public List<string> GetMonsterIds(string worldId)
	{
		List<string> list = new List<string>();
		foreach (AdventureLevelConfig value in _configs.Values)
		{
			if (!(value.WorldId != worldId))
			{
				foreach (WaveConfig wave in value.Waves)
				{
					WaveConfig current2 = wave;
					foreach (LevelMonster monster in current2.Monsters)
					{
						if (!list.Contains(monster.MonsterId))
						{
							list.Add(monster.MonsterId);
						}
					}
				}
			}
		}
		return list;
	}

	protected override void Init()
	{
		base.Init();
		_configs["w00L00"] = new AdventureLevelConfig
		{
			Id = "w00L00",
			WorldId = "w00",
			Title = "Level 1",
			Index = 0
		};
		_configs["w00L01"] = new AdventureLevelConfig
		{
			Id = "w00L01",
			WorldId = "w00",
			Title = "Level 2",
			Index = 1
		};
		_configs["w00L02"] = new AdventureLevelConfig
		{
			Id = "w00L02",
			WorldId = "w00",
			Title = "Level 3",
			Index = 2
		};
		_configs["w00L03"] = new AdventureLevelConfig
		{
			Id = "w00L03",
			WorldId = "w00",
			Title = "Level 4",
			Index = 3
		};
		_configs["w00L04"] = new AdventureLevelConfig
		{
			Id = "w00L04",
			WorldId = "w00",
			Title = "Level 5",
			Index = 4
		};
		_configs["w00L05"] = new AdventureLevelConfig
		{
			Id = "w00L05",
			WorldId = "w00",
			Title = "Level 6",
			Index = 5
		};
		_configs["w00L06"] = new AdventureLevelConfig
		{
			Id = "w00L06",
			WorldId = "w00",
			Title = "Level 7",
			Index = 6
		};
		_configs["w00L07"] = new AdventureLevelConfig
		{
			Id = "w00L07",
			WorldId = "w00",
			Title = "Level 8",
			Index = 7
		};
		_configs["w00L08"] = new AdventureLevelConfig
		{
			Id = "w00L08",
			WorldId = "w00",
			Title = "Level 9",
			Index = 8
		};
		_configs["w00L09"] = new AdventureLevelConfig
		{
			Id = "w00L09",
			WorldId = "w00",
			Title = "Level 10",
			Index = 9
		};
		_configs["w00L10"] = new AdventureLevelConfig
		{
			Id = "w00L10",
			WorldId = "w00",
			Title = "Level 11",
			Index = 10
		};
		_configs["w00L11"] = new AdventureLevelConfig
		{
			Id = "w00L11",
			WorldId = "w00",
			Title = "Level 12",
			Index = 11
		};
		_configs["w00L12"] = new AdventureLevelConfig
		{
			Id = "w00L12",
			WorldId = "w00",
			Title = "Level 13",
			Index = 12
		};
		_configs["w00L13"] = new AdventureLevelConfig
		{
			Id = "w00L13",
			WorldId = "w00",
			Title = "Level 14",
			Index = 13
		};
		_configs["w00L14"] = new AdventureLevelConfig
		{
			Id = "w00L14",
			WorldId = "w00",
			Title = "Level 15",
			Index = 14
		};
		_configs["w00L00"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 2,
				MonsterPerWaveMax = 3,
				TokenObjective = 90,
				SpawnLayerMin = 2,
				SpawnLayerMax = 2,
				RoundCountBetweenWave = 5,
				MaxMonster = 3,
				MonsterBoost = 1f,
				DamageToWeapon = 1
			}
		};
		WaveConfig waveConfig = _configs["w00L00"].Waves[0];
		waveConfig.Monsters.Add(new LevelMonster("mushroom"), 100);
		_configs["w00L01"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 2,
				MonsterPerWaveMax = 3,
				TokenObjective = 120,
				SpawnLayerMin = 3,
				SpawnLayerMax = 4,
				RoundCountBetweenWave = 5,
				MaxMonster = 3,
				MonsterBoost = 1.1f,
				DamageToWeapon = 1
			}
		};
		WaveConfig waveConfig2 = _configs["w00L01"].Waves[0];
		waveConfig2.Monsters.Add(new LevelMonster("mushroom"), 75);
		WaveConfig waveConfig3 = _configs["w00L01"].Waves[0];
		waveConfig3.Monsters.Add(new LevelMonster("woodlog"), 25);
		_configs["w00L02"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 3,
				MonsterPerWaveMax = 3,
				TokenObjective = 150,
				SpawnLayerMin = 5,
				SpawnLayerMax = 5,
				RoundCountBetweenWave = 5,
				MaxMonster = 4,
				MonsterBoost = 1.2f,
				DamageToWeapon = 1
			}
		};
		WaveConfig waveConfig4 = _configs["w00L02"].Waves[0];
		waveConfig4.Monsters.Add(new LevelMonster("mushroom"), 40);
		WaveConfig waveConfig5 = _configs["w00L02"].Waves[0];
		waveConfig5.Monsters.Add(new LevelMonster("woodlog"), 30);
		WaveConfig waveConfig6 = _configs["w00L02"].Waves[0];
		waveConfig6.Monsters.Add(new LevelMonster("acorn"), 30);
		_configs["w00L03"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 3,
				MonsterPerWaveMax = 3,
				TokenObjective = 180,
				SpawnLayerMin = 2,
				SpawnLayerMax = 5,
				RoundCountBetweenWave = 5,
				MaxMonster = 4,
				MonsterBoost = 1.3f,
				DamageToWeapon = 1
			}
		};
		WaveConfig waveConfig7 = _configs["w00L03"].Waves[0];
		waveConfig7.Monsters.Add(new LevelMonster("mushroom"), 50);
		WaveConfig waveConfig8 = _configs["w00L03"].Waves[0];
		waveConfig8.Monsters.Add(new LevelMonster("woodlog"), 30);
		WaveConfig waveConfig9 = _configs["w00L03"].Waves[0];
		waveConfig9.Monsters.Add(new LevelMonster("acorn"), 20);
		_configs["w00L04"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 3,
				MonsterPerWaveMax = 5,
				TokenObjective = 210,
				SpawnLayerMin = 2,
				SpawnLayerMax = 5,
				RoundCountBetweenWave = 4,
				MaxMonster = 4,
				MonsterBoost = 1.9f,
				DamageToWeapon = 1
			}
		};
		WaveConfig waveConfig10 = _configs["w00L04"].Waves[0];
		waveConfig10.Monsters.Add(new LevelMonster("mushroom"), 10);
		WaveConfig waveConfig11 = _configs["w00L04"].Waves[0];
		waveConfig11.Monsters.Add(new LevelMonster("woodlog"), 20);
		WaveConfig waveConfig12 = _configs["w00L04"].Waves[0];
		waveConfig12.Monsters.Add(new LevelMonster("acorn"), 70);
		_configs["w00L05"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 5,
				MonsterPerWaveMax = 6,
				TokenObjective = 240,
				SpawnLayerMin = 1,
				SpawnLayerMax = 3,
				RoundCountBetweenWave = 5,
				MaxMonster = 4,
				MonsterBoost = 1.4f,
				DamageToWeapon = 2
			}
		};
		WaveConfig waveConfig13 = _configs["w00L05"].Waves[0];
		waveConfig13.Monsters.Add(new LevelMonster("mushroom"), 80);
		WaveConfig waveConfig14 = _configs["w00L05"].Waves[0];
		waveConfig14.Monsters.Add(new LevelMonster("woodlog"), 10);
		WaveConfig waveConfig15 = _configs["w00L05"].Waves[0];
		waveConfig15.Monsters.Add(new LevelMonster("acorn"), 0);
		WaveConfig waveConfig16 = _configs["w00L05"].Waves[0];
		waveConfig16.Monsters.Add(new LevelMonster("dragon"), 10);
		_configs["w00L06"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 2,
				MonsterPerWaveMax = 3,
				TokenObjective = 270,
				SpawnLayerMin = 2,
				SpawnLayerMax = 4,
				RoundCountBetweenWave = 4,
				MaxMonster = 4,
				MonsterBoost = 1.5f,
				DamageToWeapon = 2
			}
		};
		WaveConfig waveConfig17 = _configs["w00L06"].Waves[0];
		waveConfig17.Monsters.Add(new LevelMonster("mushroom"), 30);
		WaveConfig waveConfig18 = _configs["w00L06"].Waves[0];
		waveConfig18.Monsters.Add(new LevelMonster("woodlog"), 10);
		WaveConfig waveConfig19 = _configs["w00L06"].Waves[0];
		waveConfig19.Monsters.Add(new LevelMonster("acorn"), 0);
		WaveConfig waveConfig20 = _configs["w00L06"].Waves[0];
		waveConfig20.Monsters.Add(new LevelMonster("dragon"), 50);
		WaveConfig waveConfig21 = _configs["w00L06"].Waves[0];
		waveConfig21.Monsters.Add(new LevelMonster("strawberry"), 10);
		_configs["w00L07"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 3,
				MonsterPerWaveMax = 4,
				TokenObjective = 300,
				SpawnLayerMin = 1,
				SpawnLayerMax = 3,
				RoundCountBetweenWave = 4,
				MaxMonster = 5,
				MonsterBoost = 1.6f,
				DamageToWeapon = 2
			}
		};
		WaveConfig waveConfig22 = _configs["w00L07"].Waves[0];
		waveConfig22.Monsters.Add(new LevelMonster("mushroom"), 0);
		WaveConfig waveConfig23 = _configs["w00L07"].Waves[0];
		waveConfig23.Monsters.Add(new LevelMonster("woodlog"), 50);
		WaveConfig waveConfig24 = _configs["w00L07"].Waves[0];
		waveConfig24.Monsters.Add(new LevelMonster("acorn"), 20);
		WaveConfig waveConfig25 = _configs["w00L07"].Waves[0];
		waveConfig25.Monsters.Add(new LevelMonster("dragon"), 10);
		WaveConfig waveConfig26 = _configs["w00L07"].Waves[0];
		waveConfig26.Monsters.Add(new LevelMonster("strawberry"), 20);
		_configs["w00L08"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 3,
				MonsterPerWaveMax = 4,
				TokenObjective = 330,
				SpawnLayerMin = 1,
				SpawnLayerMax = 3,
				RoundCountBetweenWave = 3,
				MaxMonster = 5,
				MonsterBoost = 1.7f,
				DamageToWeapon = 2
			}
		};
		WaveConfig waveConfig27 = _configs["w00L08"].Waves[0];
		waveConfig27.Monsters.Add(new LevelMonster("mushroom"), 10);
		WaveConfig waveConfig28 = _configs["w00L08"].Waves[0];
		waveConfig28.Monsters.Add(new LevelMonster("woodlog"), 15);
		WaveConfig waveConfig29 = _configs["w00L08"].Waves[0];
		waveConfig29.Monsters.Add(new LevelMonster("acorn"), 60);
		WaveConfig waveConfig30 = _configs["w00L08"].Waves[0];
		waveConfig30.Monsters.Add(new LevelMonster("dragon"), 15);
		_configs["w00L09"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 4,
				MonsterPerWaveMax = 5,
				TokenObjective = 360,
				SpawnLayerMin = 1,
				SpawnLayerMax = 4,
				RoundCountBetweenWave = 3,
				MaxMonster = 5,
				MonsterBoost = 2.4f,
				DamageToWeapon = 2
			}
		};
		WaveConfig waveConfig31 = _configs["w00L09"].Waves[0];
		waveConfig31.Monsters.Add(new LevelMonster("mushroom"), 30);
		WaveConfig waveConfig32 = _configs["w00L09"].Waves[0];
		waveConfig32.Monsters.Add(new LevelMonster("woodlog"), 25);
		WaveConfig waveConfig33 = _configs["w00L09"].Waves[0];
		waveConfig33.Monsters.Add(new LevelMonster("acorn"), 30);
		WaveConfig waveConfig34 = _configs["w00L09"].Waves[0];
		waveConfig34.Monsters.Add(new LevelMonster("dragon"), 15);
		_configs["w00L10"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 3,
				MonsterPerWaveMax = 5,
				TokenObjective = 390,
				SpawnLayerMin = 5,
				SpawnLayerMax = 5,
				RoundCountBetweenWave = 4,
				MaxMonster = 5,
				MonsterBoost = 1.8f,
				DamageToWeapon = 3
			}
		};
		WaveConfig waveConfig35 = _configs["w00L10"].Waves[0];
		waveConfig35.Monsters.Add(new LevelMonster("mushroom"), 90);
		WaveConfig waveConfig36 = _configs["w00L10"].Waves[0];
		waveConfig36.Monsters.Add(new LevelMonster("strawberry"), 10);
		_configs["w00L11"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 3,
				MonsterPerWaveMax = 5,
				TokenObjective = 420,
				SpawnLayerMin = 2,
				SpawnLayerMax = 3,
				RoundCountBetweenWave = 3,
				MaxMonster = 5,
				MonsterBoost = 1.9f,
				DamageToWeapon = 3
			}
		};
		WaveConfig waveConfig37 = _configs["w00L11"].Waves[0];
		waveConfig37.Monsters.Add(new LevelMonster("mushroom"), 35);
		WaveConfig waveConfig38 = _configs["w00L11"].Waves[0];
		waveConfig38.Monsters.Add(new LevelMonster("woodlog"), 35);
		WaveConfig waveConfig39 = _configs["w00L11"].Waves[0];
		waveConfig39.Monsters.Add(new LevelMonster("acorn"), 10);
		WaveConfig waveConfig40 = _configs["w00L11"].Waves[0];
		waveConfig40.Monsters.Add(new LevelMonster("strawberry"), 20);
		_configs["w00L12"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 4,
				MonsterPerWaveMax = 5,
				TokenObjective = 450,
				SpawnLayerMin = 2,
				SpawnLayerMax = 3,
				RoundCountBetweenWave = 3,
				MaxMonster = 6,
				MonsterBoost = 2f,
				DamageToWeapon = 3
			}
		};
		WaveConfig waveConfig41 = _configs["w00L12"].Waves[0];
		waveConfig41.Monsters.Add(new LevelMonster("mushroom"), 0);
		WaveConfig waveConfig42 = _configs["w00L12"].Waves[0];
		waveConfig42.Monsters.Add(new LevelMonster("woodlog"), 0);
		WaveConfig waveConfig43 = _configs["w00L12"].Waves[0];
		waveConfig43.Monsters.Add(new LevelMonster("acorn"), 80);
		WaveConfig waveConfig44 = _configs["w00L12"].Waves[0];
		waveConfig44.Monsters.Add(new LevelMonster("dragon"), 10);
		WaveConfig waveConfig45 = _configs["w00L12"].Waves[0];
		waveConfig45.Monsters.Add(new LevelMonster("strawberry"), 10);
		_configs["w00L13"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 3,
				MonsterPerWaveMax = 6,
				TokenObjective = 480,
				SpawnLayerMin = 2,
				SpawnLayerMax = 2,
				RoundCountBetweenWave = 2,
				MaxMonster = 7,
				MonsterBoost = 2.1f,
				DamageToWeapon = 3
			}
		};
		WaveConfig waveConfig46 = _configs["w00L13"].Waves[0];
		waveConfig46.Monsters.Add(new LevelMonster("mushroom"), 80);
		WaveConfig waveConfig47 = _configs["w00L13"].Waves[0];
		waveConfig47.Monsters.Add(new LevelMonster("woodlog"), 5);
		WaveConfig waveConfig48 = _configs["w00L13"].Waves[0];
		waveConfig48.Monsters.Add(new LevelMonster("acorn"), 5);
		WaveConfig waveConfig49 = _configs["w00L13"].Waves[0];
		waveConfig49.Monsters.Add(new LevelMonster("dragon"), 5);
		WaveConfig waveConfig50 = _configs["w00L13"].Waves[0];
		waveConfig50.Monsters.Add(new LevelMonster("strawberry"), 5);
		_configs["w00L14"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 6,
				MonsterPerWaveMax = 6,
				TokenObjective = 510,
				SpawnLayerMin = 4,
				SpawnLayerMax = 4,
				RoundCountBetweenWave = 9,
				MaxMonster = 8,
				MonsterBoost = 2.9f,
				DamageToWeapon = 3
			}
		};
		WaveConfig waveConfig51 = _configs["w00L14"].Waves[0];
		waveConfig51.Monsters.Add(new LevelMonster("mushroom"), 15);
		WaveConfig waveConfig52 = _configs["w00L14"].Waves[0];
		waveConfig52.Monsters.Add(new LevelMonster("woodlog"), 15);
		WaveConfig waveConfig53 = _configs["w00L14"].Waves[0];
		waveConfig53.Monsters.Add(new LevelMonster("acorn"), 25);
		WaveConfig waveConfig54 = _configs["w00L14"].Waves[0];
		waveConfig54.Monsters.Add(new LevelMonster("dragon"), 10);
		WaveConfig waveConfig55 = _configs["w00L14"].Waves[0];
		waveConfig55.Monsters.Add(new LevelMonster("strawberry"), 35);
		_configs["w01L00"] = new AdventureLevelConfig
		{
			Id = "w01L00",
			WorldId = "w01",
			Title = "Level 1",
			Index = 0
		};
		_configs["w01L01"] = new AdventureLevelConfig
		{
			Id = "w01L01",
			WorldId = "w01",
			Title = "Level 2",
			Index = 1
		};
		_configs["w01L02"] = new AdventureLevelConfig
		{
			Id = "w01L02",
			WorldId = "w01",
			Title = "Level 3",
			Index = 2
		};
		_configs["w01L03"] = new AdventureLevelConfig
		{
			Id = "w01L03",
			WorldId = "w01",
			Title = "Level 4",
			Index = 3
		};
		_configs["w01L04"] = new AdventureLevelConfig
		{
			Id = "w01L04",
			WorldId = "w01",
			Title = "Level 5",
			Index = 4
		};
		_configs["w01L05"] = new AdventureLevelConfig
		{
			Id = "w01L05",
			WorldId = "w01",
			Title = "Level 6",
			Index = 5
		};
		_configs["w01L06"] = new AdventureLevelConfig
		{
			Id = "w01L06",
			WorldId = "w01",
			Title = "Level 7",
			Index = 6
		};
		_configs["w01L07"] = new AdventureLevelConfig
		{
			Id = "w01L07",
			WorldId = "w01",
			Title = "Level 8",
			Index = 7
		};
		_configs["w01L08"] = new AdventureLevelConfig
		{
			Id = "w01L08",
			WorldId = "w01",
			Title = "Level 9",
			Index = 8
		};
		_configs["w01L09"] = new AdventureLevelConfig
		{
			Id = "w01L09",
			WorldId = "w01",
			Title = "Level 10",
			Index = 9
		};
		_configs["w01L10"] = new AdventureLevelConfig
		{
			Id = "w01L10",
			WorldId = "w01",
			Title = "Level 11",
			Index = 10
		};
		_configs["w01L11"] = new AdventureLevelConfig
		{
			Id = "w01L11",
			WorldId = "w01",
			Title = "Level 12",
			Index = 11
		};
		_configs["w01L12"] = new AdventureLevelConfig
		{
			Id = "w01L12",
			WorldId = "w01",
			Title = "Level 13",
			Index = 12
		};
		_configs["w01L13"] = new AdventureLevelConfig
		{
			Id = "w01L13",
			WorldId = "w01",
			Title = "Level 14",
			Index = 13
		};
		_configs["w01L14"] = new AdventureLevelConfig
		{
			Id = "w01L14",
			WorldId = "w01",
			Title = "Level 15",
			Index = 14
		};
		_configs["w01L00"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 2,
				MonsterPerWaveMax = 3,
				TokenObjective = 90,
				SpawnLayerMin = 1,
				SpawnLayerMax = 2,
				RoundCountBetweenWave = 5,
				MaxMonster = 4,
				MonsterBoost = 1f,
				DamageToWeapon = 1
			}
		};
		WaveConfig waveConfig56 = _configs["w01L00"].Waves[0];
		waveConfig56.Monsters.Add(new LevelMonster("skeleton"), 100);
		WaveConfig waveConfig57 = _configs["w01L00"].Waves[0];
		waveConfig57.Monsters.Add(new LevelMonster("spider"), 0);
		WaveConfig waveConfig58 = _configs["w01L00"].Waves[0];
		waveConfig58.Monsters.Add(new LevelMonster("ghost"), 0);
		WaveConfig waveConfig59 = _configs["w01L00"].Waves[0];
		waveConfig59.Monsters.Add(new LevelMonster("snake"), 0);
		WaveConfig waveConfig60 = _configs["w01L00"].Waves[0];
		waveConfig60.Monsters.Add(new LevelMonster("eye"), 0);
		_configs["w01L01"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 2,
				MonsterPerWaveMax = 3,
				TokenObjective = 120,
				SpawnLayerMin = 3,
				SpawnLayerMax = 4,
				RoundCountBetweenWave = 5,
				MaxMonster = 4,
				MonsterBoost = 1.1f,
				DamageToWeapon = 1
			}
		};
		WaveConfig waveConfig61 = _configs["w01L01"].Waves[0];
		waveConfig61.Monsters.Add(new LevelMonster("skeleton"), 60);
		WaveConfig waveConfig62 = _configs["w01L01"].Waves[0];
		waveConfig62.Monsters.Add(new LevelMonster("spider"), 0);
		WaveConfig waveConfig63 = _configs["w01L01"].Waves[0];
		waveConfig63.Monsters.Add(new LevelMonster("ghost"), 40);
		WaveConfig waveConfig64 = _configs["w01L01"].Waves[0];
		waveConfig64.Monsters.Add(new LevelMonster("snake"), 0);
		WaveConfig waveConfig65 = _configs["w01L01"].Waves[0];
		waveConfig65.Monsters.Add(new LevelMonster("eye"), 0);
		_configs["w01L02"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 3,
				MonsterPerWaveMax = 3,
				TokenObjective = 150,
				SpawnLayerMin = 3,
				SpawnLayerMax = 5,
				RoundCountBetweenWave = 5,
				MaxMonster = 5,
				MonsterBoost = 1.2f,
				DamageToWeapon = 1
			}
		};
		WaveConfig waveConfig66 = _configs["w01L02"].Waves[0];
		waveConfig66.Monsters.Add(new LevelMonster("skeleton"), 75);
		WaveConfig waveConfig67 = _configs["w01L02"].Waves[0];
		waveConfig67.Monsters.Add(new LevelMonster("spider"), 25);
		WaveConfig waveConfig68 = _configs["w01L02"].Waves[0];
		waveConfig68.Monsters.Add(new LevelMonster("ghost"), 0);
		WaveConfig waveConfig69 = _configs["w01L02"].Waves[0];
		waveConfig69.Monsters.Add(new LevelMonster("snake"), 0);
		WaveConfig waveConfig70 = _configs["w01L02"].Waves[0];
		waveConfig70.Monsters.Add(new LevelMonster("eye"), 0);
		_configs["w01L03"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 3,
				MonsterPerWaveMax = 3,
				TokenObjective = 180,
				SpawnLayerMin = 2,
				SpawnLayerMax = 4,
				RoundCountBetweenWave = 5,
				MaxMonster = 4,
				MonsterBoost = 1.3f,
				DamageToWeapon = 1
			}
		};
		WaveConfig waveConfig71 = _configs["w01L03"].Waves[0];
		waveConfig71.Monsters.Add(new LevelMonster("skeleton"), 0);
		WaveConfig waveConfig72 = _configs["w01L03"].Waves[0];
		waveConfig72.Monsters.Add(new LevelMonster("spider"), 70);
		WaveConfig waveConfig73 = _configs["w01L03"].Waves[0];
		waveConfig73.Monsters.Add(new LevelMonster("ghost"), 20);
		WaveConfig waveConfig74 = _configs["w01L03"].Waves[0];
		waveConfig74.Monsters.Add(new LevelMonster("snake"), 10);
		WaveConfig waveConfig75 = _configs["w01L03"].Waves[0];
		waveConfig75.Monsters.Add(new LevelMonster("eye"), 0);
		_configs["w01L04"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 3,
				MonsterPerWaveMax = 5,
				TokenObjective = 210,
				SpawnLayerMin = 2,
				SpawnLayerMax = 3,
				RoundCountBetweenWave = 4,
				MaxMonster = 5,
				MonsterBoost = 1.9f,
				DamageToWeapon = 1
			}
		};
		WaveConfig waveConfig76 = _configs["w01L04"].Waves[0];
		waveConfig76.Monsters.Add(new LevelMonster("skeleton"), 30);
		WaveConfig waveConfig77 = _configs["w01L04"].Waves[0];
		waveConfig77.Monsters.Add(new LevelMonster("spider"), 0);
		WaveConfig waveConfig78 = _configs["w01L04"].Waves[0];
		waveConfig78.Monsters.Add(new LevelMonster("ghost"), 20);
		WaveConfig waveConfig79 = _configs["w01L04"].Waves[0];
		waveConfig79.Monsters.Add(new LevelMonster("snake"), 50);
		WaveConfig waveConfig80 = _configs["w01L04"].Waves[0];
		waveConfig80.Monsters.Add(new LevelMonster("eye"), 0);
		_configs["w01L05"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 5,
				MonsterPerWaveMax = 6,
				TokenObjective = 240,
				SpawnLayerMin = 1,
				SpawnLayerMax = 3,
				RoundCountBetweenWave = 5,
				MaxMonster = 6,
				MonsterBoost = 1.4f,
				DamageToWeapon = 2
			}
		};
		WaveConfig waveConfig81 = _configs["w01L05"].Waves[0];
		waveConfig81.Monsters.Add(new LevelMonster("skeleton"), 65);
		WaveConfig waveConfig82 = _configs["w01L05"].Waves[0];
		waveConfig82.Monsters.Add(new LevelMonster("spider"), 0);
		WaveConfig waveConfig83 = _configs["w01L05"].Waves[0];
		waveConfig83.Monsters.Add(new LevelMonster("ghost"), 0);
		WaveConfig waveConfig84 = _configs["w01L05"].Waves[0];
		waveConfig84.Monsters.Add(new LevelMonster("snake"), 10);
		WaveConfig waveConfig85 = _configs["w01L05"].Waves[0];
		waveConfig85.Monsters.Add(new LevelMonster("eye"), 25);
		_configs["w01L06"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 2,
				MonsterPerWaveMax = 3,
				TokenObjective = 270,
				SpawnLayerMin = 4,
				SpawnLayerMax = 5,
				RoundCountBetweenWave = 4,
				MaxMonster = 6,
				MonsterBoost = 1.5f,
				DamageToWeapon = 2
			}
		};
		WaveConfig waveConfig86 = _configs["w01L06"].Waves[0];
		waveConfig86.Monsters.Add(new LevelMonster("skeleton"), 40);
		WaveConfig waveConfig87 = _configs["w01L06"].Waves[0];
		waveConfig87.Monsters.Add(new LevelMonster("spider"), 20);
		WaveConfig waveConfig88 = _configs["w01L06"].Waves[0];
		waveConfig88.Monsters.Add(new LevelMonster("ghost"), 20);
		WaveConfig waveConfig89 = _configs["w01L06"].Waves[0];
		waveConfig89.Monsters.Add(new LevelMonster("snake"), 20);
		WaveConfig waveConfig90 = _configs["w01L06"].Waves[0];
		waveConfig90.Monsters.Add(new LevelMonster("eye"), 0);
		_configs["w01L07"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 3,
				MonsterPerWaveMax = 4,
				TokenObjective = 300,
				SpawnLayerMin = 1,
				SpawnLayerMax = 2,
				RoundCountBetweenWave = 4,
				MaxMonster = 7,
				MonsterBoost = 1.6f,
				DamageToWeapon = 2
			}
		};
		WaveConfig waveConfig91 = _configs["w01L07"].Waves[0];
		waveConfig91.Monsters.Add(new LevelMonster("skeleton"), 20);
		WaveConfig waveConfig92 = _configs["w01L07"].Waves[0];
		waveConfig92.Monsters.Add(new LevelMonster("spider"), 50);
		WaveConfig waveConfig93 = _configs["w01L07"].Waves[0];
		waveConfig93.Monsters.Add(new LevelMonster("ghost"), 0);
		WaveConfig waveConfig94 = _configs["w01L07"].Waves[0];
		waveConfig94.Monsters.Add(new LevelMonster("snake"), 20);
		WaveConfig waveConfig95 = _configs["w01L07"].Waves[0];
		waveConfig95.Monsters.Add(new LevelMonster("eye"), 10);
		_configs["w01L08"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 3,
				MonsterPerWaveMax = 4,
				TokenObjective = 330,
				SpawnLayerMin = 1,
				SpawnLayerMax = 3,
				RoundCountBetweenWave = 3,
				MaxMonster = 7,
				MonsterBoost = 1.7f,
				DamageToWeapon = 2
			}
		};
		WaveConfig waveConfig96 = _configs["w01L08"].Waves[0];
		waveConfig96.Monsters.Add(new LevelMonster("skeleton"), 0);
		WaveConfig waveConfig97 = _configs["w01L08"].Waves[0];
		waveConfig97.Monsters.Add(new LevelMonster("spider"), 0);
		WaveConfig waveConfig98 = _configs["w01L08"].Waves[0];
		waveConfig98.Monsters.Add(new LevelMonster("ghost"), 35);
		WaveConfig waveConfig99 = _configs["w01L08"].Waves[0];
		waveConfig99.Monsters.Add(new LevelMonster("snake"), 45);
		WaveConfig waveConfig100 = _configs["w01L08"].Waves[0];
		waveConfig100.Monsters.Add(new LevelMonster("eye"), 20);
		_configs["w01L09"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 4,
				MonsterPerWaveMax = 5,
				TokenObjective = 360,
				SpawnLayerMin = 1,
				SpawnLayerMax = 4,
				RoundCountBetweenWave = 3,
				MaxMonster = 7,
				MonsterBoost = 2.4f,
				DamageToWeapon = 2
			}
		};
		WaveConfig waveConfig101 = _configs["w01L09"].Waves[0];
		waveConfig101.Monsters.Add(new LevelMonster("skeleton"), 10);
		WaveConfig waveConfig102 = _configs["w01L09"].Waves[0];
		waveConfig102.Monsters.Add(new LevelMonster("spider"), 0);
		WaveConfig waveConfig103 = _configs["w01L09"].Waves[0];
		waveConfig103.Monsters.Add(new LevelMonster("ghost"), 50);
		WaveConfig waveConfig104 = _configs["w01L09"].Waves[0];
		waveConfig104.Monsters.Add(new LevelMonster("snake"), 10);
		WaveConfig waveConfig105 = _configs["w01L09"].Waves[0];
		waveConfig105.Monsters.Add(new LevelMonster("eye"), 30);
		_configs["w01L10"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 3,
				MonsterPerWaveMax = 5,
				TokenObjective = 390,
				SpawnLayerMin = 5,
				SpawnLayerMax = 5,
				RoundCountBetweenWave = 4,
				MaxMonster = 7,
				MonsterBoost = 1.8f,
				DamageToWeapon = 3
			}
		};
		WaveConfig waveConfig106 = _configs["w01L10"].Waves[0];
		waveConfig106.Monsters.Add(new LevelMonster("skeleton"), 0);
		WaveConfig waveConfig107 = _configs["w01L10"].Waves[0];
		waveConfig107.Monsters.Add(new LevelMonster("spider"), 15);
		WaveConfig waveConfig108 = _configs["w01L10"].Waves[0];
		waveConfig108.Monsters.Add(new LevelMonster("ghost"), 0);
		WaveConfig waveConfig109 = _configs["w01L10"].Waves[0];
		waveConfig109.Monsters.Add(new LevelMonster("snake"), 75);
		WaveConfig waveConfig110 = _configs["w01L10"].Waves[0];
		waveConfig110.Monsters.Add(new LevelMonster("eye"), 10);
		_configs["w01L11"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 3,
				MonsterPerWaveMax = 5,
				TokenObjective = 420,
				SpawnLayerMin = 2,
				SpawnLayerMax = 3,
				RoundCountBetweenWave = 3,
				MaxMonster = 7,
				MonsterBoost = 1.9f,
				DamageToWeapon = 3
			}
		};
		WaveConfig waveConfig111 = _configs["w01L11"].Waves[0];
		waveConfig111.Monsters.Add(new LevelMonster("skeleton"), 35);
		WaveConfig waveConfig112 = _configs["w01L11"].Waves[0];
		waveConfig112.Monsters.Add(new LevelMonster("spider"), 35);
		WaveConfig waveConfig113 = _configs["w01L11"].Waves[0];
		waveConfig113.Monsters.Add(new LevelMonster("ghost"), 10);
		WaveConfig waveConfig114 = _configs["w01L11"].Waves[0];
		waveConfig114.Monsters.Add(new LevelMonster("snake"), 20);
		WaveConfig waveConfig115 = _configs["w01L11"].Waves[0];
		waveConfig115.Monsters.Add(new LevelMonster("eye"), 0);
		_configs["w01L12"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 4,
				MonsterPerWaveMax = 5,
				TokenObjective = 450,
				SpawnLayerMin = 2,
				SpawnLayerMax = 3,
				RoundCountBetweenWave = 3,
				MaxMonster = 8,
				MonsterBoost = 2f,
				DamageToWeapon = 3
			}
		};
		WaveConfig waveConfig116 = _configs["w01L12"].Waves[0];
		waveConfig116.Monsters.Add(new LevelMonster("skeleton"), 0);
		WaveConfig waveConfig117 = _configs["w01L12"].Waves[0];
		waveConfig117.Monsters.Add(new LevelMonster("spider"), 0);
		WaveConfig waveConfig118 = _configs["w01L12"].Waves[0];
		waveConfig118.Monsters.Add(new LevelMonster("ghost"), 80);
		WaveConfig waveConfig119 = _configs["w01L12"].Waves[0];
		waveConfig119.Monsters.Add(new LevelMonster("snake"), 10);
		WaveConfig waveConfig120 = _configs["w01L12"].Waves[0];
		waveConfig120.Monsters.Add(new LevelMonster("eye"), 10);
		_configs["w01L13"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 3,
				MonsterPerWaveMax = 6,
				TokenObjective = 480,
				SpawnLayerMin = 2,
				SpawnLayerMax = 2,
				RoundCountBetweenWave = 2,
				MaxMonster = 9,
				MonsterBoost = 2.1f,
				DamageToWeapon = 3
			}
		};
		WaveConfig waveConfig121 = _configs["w01L13"].Waves[0];
		waveConfig121.Monsters.Add(new LevelMonster("skeleton"), 80);
		WaveConfig waveConfig122 = _configs["w01L13"].Waves[0];
		waveConfig122.Monsters.Add(new LevelMonster("spider"), 5);
		WaveConfig waveConfig123 = _configs["w01L13"].Waves[0];
		waveConfig123.Monsters.Add(new LevelMonster("ghost"), 5);
		WaveConfig waveConfig124 = _configs["w01L13"].Waves[0];
		waveConfig124.Monsters.Add(new LevelMonster("snake"), 5);
		WaveConfig waveConfig125 = _configs["w01L13"].Waves[0];
		waveConfig125.Monsters.Add(new LevelMonster("eye"), 5);
		_configs["w01L14"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 6,
				MonsterPerWaveMax = 6,
				TokenObjective = 510,
				SpawnLayerMin = 4,
				SpawnLayerMax = 4,
				RoundCountBetweenWave = 9,
				MaxMonster = 10,
				MonsterBoost = 2.9f,
				DamageToWeapon = 3
			}
		};
		WaveConfig waveConfig126 = _configs["w01L14"].Waves[0];
		waveConfig126.Monsters.Add(new LevelMonster("skeleton"), 15);
		WaveConfig waveConfig127 = _configs["w01L14"].Waves[0];
		waveConfig127.Monsters.Add(new LevelMonster("spider"), 15);
		WaveConfig waveConfig128 = _configs["w01L14"].Waves[0];
		waveConfig128.Monsters.Add(new LevelMonster("ghost"), 25);
		WaveConfig waveConfig129 = _configs["w01L14"].Waves[0];
		waveConfig129.Monsters.Add(new LevelMonster("snake"), 35);
		WaveConfig waveConfig130 = _configs["w01L14"].Waves[0];
		waveConfig130.Monsters.Add(new LevelMonster("eye"), 10);
		_configs["w02L00"] = new AdventureLevelConfig
		{
			Id = "w02L00",
			WorldId = "w02",
			Title = "Level 1",
			Index = 0
		};
		_configs["w02L01"] = new AdventureLevelConfig
		{
			Id = "w02L01",
			WorldId = "w02",
			Title = "Level 2",
			Index = 1
		};
		_configs["w02L02"] = new AdventureLevelConfig
		{
			Id = "w02L02",
			WorldId = "w02",
			Title = "Level 3",
			Index = 2
		};
		_configs["w02L03"] = new AdventureLevelConfig
		{
			Id = "w02L03",
			WorldId = "w02",
			Title = "Level 4",
			Index = 3
		};
		_configs["w02L04"] = new AdventureLevelConfig
		{
			Id = "w02L04",
			WorldId = "w02",
			Title = "Level 5",
			Index = 4
		};
		_configs["w02L05"] = new AdventureLevelConfig
		{
			Id = "w02L05",
			WorldId = "w02",
			Title = "Level 6",
			Index = 5
		};
		_configs["w02L06"] = new AdventureLevelConfig
		{
			Id = "w02L06",
			WorldId = "w02",
			Title = "Level 7",
			Index = 6
		};
		_configs["w02L07"] = new AdventureLevelConfig
		{
			Id = "w02L07",
			WorldId = "w02",
			Title = "Level 8",
			Index = 7
		};
		_configs["w02L08"] = new AdventureLevelConfig
		{
			Id = "w02L08",
			WorldId = "w02",
			Title = "Level 9",
			Index = 8
		};
		_configs["w02L09"] = new AdventureLevelConfig
		{
			Id = "w02L09",
			WorldId = "w02",
			Title = "Level 10",
			Index = 9
		};
		_configs["w02L10"] = new AdventureLevelConfig
		{
			Id = "w02L10",
			WorldId = "w02",
			Title = "Level 11",
			Index = 10
		};
		_configs["w02L11"] = new AdventureLevelConfig
		{
			Id = "w02L11",
			WorldId = "w02",
			Title = "Level 12",
			Index = 11
		};
		_configs["w02L12"] = new AdventureLevelConfig
		{
			Id = "w02L12",
			WorldId = "w02",
			Title = "Level 13",
			Index = 12
		};
		_configs["w02L13"] = new AdventureLevelConfig
		{
			Id = "w02L13",
			WorldId = "w02",
			Title = "Level 14",
			Index = 13
		};
		_configs["w02L14"] = new AdventureLevelConfig
		{
			Id = "w02L14",
			WorldId = "w02",
			Title = "Level 15",
			Index = 14
		};
		_configs["w02L00"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 2,
				MonsterPerWaveMax = 3,
				TokenObjective = 90,
				SpawnLayerMin = 1,
				SpawnLayerMax = 2,
				RoundCountBetweenWave = 5,
				MaxMonster = 4,
				MonsterBoost = 1f,
				DamageToWeapon = 1
			}
		};
		WaveConfig waveConfig131 = _configs["w02L00"].Waves[0];
		waveConfig131.Monsters.Add(new LevelMonster("fishman"), 0);
		WaveConfig waveConfig132 = _configs["w02L00"].Waves[0];
		waveConfig132.Monsters.Add(new LevelMonster("leech"), 0);
		WaveConfig waveConfig133 = _configs["w02L00"].Waves[0];
		waveConfig133.Monsters.Add(new LevelMonster("salamander"), 100);
		WaveConfig waveConfig134 = _configs["w02L00"].Waves[0];
		waveConfig134.Monsters.Add(new LevelMonster("mushroomToxic"), 0);
		WaveConfig waveConfig135 = _configs["w02L00"].Waves[0];
		waveConfig135.Monsters.Add(new LevelMonster("frog"), 0);
		_configs["w02L01"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 2,
				MonsterPerWaveMax = 3,
				TokenObjective = 120,
				SpawnLayerMin = 3,
				SpawnLayerMax = 4,
				RoundCountBetweenWave = 5,
				MaxMonster = 4,
				MonsterBoost = 1.1f,
				DamageToWeapon = 1
			}
		};
		WaveConfig waveConfig136 = _configs["w02L01"].Waves[0];
		waveConfig136.Monsters.Add(new LevelMonster("fishman"), 60);
		WaveConfig waveConfig137 = _configs["w02L01"].Waves[0];
		waveConfig137.Monsters.Add(new LevelMonster("leech"), 0);
		WaveConfig waveConfig138 = _configs["w02L01"].Waves[0];
		waveConfig138.Monsters.Add(new LevelMonster("salamander"), 40);
		WaveConfig waveConfig139 = _configs["w02L01"].Waves[0];
		waveConfig139.Monsters.Add(new LevelMonster("mushroomToxic"), 0);
		WaveConfig waveConfig140 = _configs["w02L01"].Waves[0];
		waveConfig140.Monsters.Add(new LevelMonster("frog"), 0);
		_configs["w02L02"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 3,
				MonsterPerWaveMax = 3,
				TokenObjective = 150,
				SpawnLayerMin = 3,
				SpawnLayerMax = 5,
				RoundCountBetweenWave = 5,
				MaxMonster = 5,
				MonsterBoost = 1.2f,
				DamageToWeapon = 1
			}
		};
		WaveConfig waveConfig141 = _configs["w02L02"].Waves[0];
		waveConfig141.Monsters.Add(new LevelMonster("fishman"), 25);
		WaveConfig waveConfig142 = _configs["w02L02"].Waves[0];
		waveConfig142.Monsters.Add(new LevelMonster("leech"), 50);
		WaveConfig waveConfig143 = _configs["w02L02"].Waves[0];
		waveConfig143.Monsters.Add(new LevelMonster("salamander"), 25);
		WaveConfig waveConfig144 = _configs["w02L02"].Waves[0];
		waveConfig144.Monsters.Add(new LevelMonster("mushroomToxic"), 0);
		WaveConfig waveConfig145 = _configs["w02L02"].Waves[0];
		waveConfig145.Monsters.Add(new LevelMonster("frog"), 0);
		_configs["w02L03"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 3,
				MonsterPerWaveMax = 3,
				TokenObjective = 180,
				SpawnLayerMin = 2,
				SpawnLayerMax = 4,
				RoundCountBetweenWave = 5,
				MaxMonster = 5,
				MonsterBoost = 1.3f,
				DamageToWeapon = 1
			}
		};
		WaveConfig waveConfig146 = _configs["w02L03"].Waves[0];
		waveConfig146.Monsters.Add(new LevelMonster("fishman"), 80);
		WaveConfig waveConfig147 = _configs["w02L03"].Waves[0];
		waveConfig147.Monsters.Add(new LevelMonster("leech"), 10);
		WaveConfig waveConfig148 = _configs["w02L03"].Waves[0];
		waveConfig148.Monsters.Add(new LevelMonster("salamander"), 20);
		WaveConfig waveConfig149 = _configs["w02L03"].Waves[0];
		waveConfig149.Monsters.Add(new LevelMonster("mushroomToxic"), 0);
		WaveConfig waveConfig150 = _configs["w02L03"].Waves[0];
		waveConfig150.Monsters.Add(new LevelMonster("frog"), 0);
		_configs["w02L04"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 3,
				MonsterPerWaveMax = 5,
				TokenObjective = 210,
				SpawnLayerMin = 2,
				SpawnLayerMax = 2,
				RoundCountBetweenWave = 4,
				MaxMonster = 5,
				MonsterBoost = 1.9f,
				DamageToWeapon = 1
			}
		};
		WaveConfig waveConfig151 = _configs["w02L04"].Waves[0];
		waveConfig151.Monsters.Add(new LevelMonster("fishman"), 30);
		WaveConfig waveConfig152 = _configs["w02L04"].Waves[0];
		waveConfig152.Monsters.Add(new LevelMonster("leech"), 70);
		WaveConfig waveConfig153 = _configs["w02L04"].Waves[0];
		waveConfig153.Monsters.Add(new LevelMonster("salamander"), 0);
		WaveConfig waveConfig154 = _configs["w02L04"].Waves[0];
		waveConfig154.Monsters.Add(new LevelMonster("mushroomToxic"), 0);
		WaveConfig waveConfig155 = _configs["w02L04"].Waves[0];
		waveConfig155.Monsters.Add(new LevelMonster("frog"), 0);
		_configs["w02L05"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 5,
				MonsterPerWaveMax = 6,
				TokenObjective = 240,
				SpawnLayerMin = 1,
				SpawnLayerMax = 3,
				RoundCountBetweenWave = 5,
				MaxMonster = 6,
				MonsterBoost = 1.4f,
				DamageToWeapon = 2
			}
		};
		WaveConfig waveConfig156 = _configs["w02L05"].Waves[0];
		waveConfig156.Monsters.Add(new LevelMonster("fishman"), 25);
		WaveConfig waveConfig157 = _configs["w02L05"].Waves[0];
		waveConfig157.Monsters.Add(new LevelMonster("leech"), 20);
		WaveConfig waveConfig158 = _configs["w02L05"].Waves[0];
		waveConfig158.Monsters.Add(new LevelMonster("salamander"), 20);
		WaveConfig waveConfig159 = _configs["w02L05"].Waves[0];
		waveConfig159.Monsters.Add(new LevelMonster("mushroomToxic"), 35);
		WaveConfig waveConfig160 = _configs["w02L05"].Waves[0];
		waveConfig160.Monsters.Add(new LevelMonster("frog"), 0);
		_configs["w02L06"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 2,
				MonsterPerWaveMax = 3,
				TokenObjective = 270,
				SpawnLayerMin = 4,
				SpawnLayerMax = 5,
				RoundCountBetweenWave = 4,
				MaxMonster = 6,
				MonsterBoost = 1.5f,
				DamageToWeapon = 2
			}
		};
		WaveConfig waveConfig161 = _configs["w02L06"].Waves[0];
		waveConfig161.Monsters.Add(new LevelMonster("fishman"), 0);
		WaveConfig waveConfig162 = _configs["w02L06"].Waves[0];
		waveConfig162.Monsters.Add(new LevelMonster("leech"), 0);
		WaveConfig waveConfig163 = _configs["w02L06"].Waves[0];
		waveConfig163.Monsters.Add(new LevelMonster("salamander"), 0);
		WaveConfig waveConfig164 = _configs["w02L06"].Waves[0];
		waveConfig164.Monsters.Add(new LevelMonster("mushroomToxic"), 30);
		WaveConfig waveConfig165 = _configs["w02L06"].Waves[0];
		waveConfig165.Monsters.Add(new LevelMonster("frog"), 70);
		_configs["w02L07"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 3,
				MonsterPerWaveMax = 4,
				TokenObjective = 300,
				SpawnLayerMin = 1,
				SpawnLayerMax = 3,
				RoundCountBetweenWave = 4,
				MaxMonster = 7,
				MonsterBoost = 1.6f,
				DamageToWeapon = 2
			}
		};
		WaveConfig waveConfig166 = _configs["w02L07"].Waves[0];
		waveConfig166.Monsters.Add(new LevelMonster("fishman"), 30);
		WaveConfig waveConfig167 = _configs["w02L07"].Waves[0];
		waveConfig167.Monsters.Add(new LevelMonster("leech"), 50);
		WaveConfig waveConfig168 = _configs["w02L07"].Waves[0];
		waveConfig168.Monsters.Add(new LevelMonster("salamander"), 0);
		WaveConfig waveConfig169 = _configs["w02L07"].Waves[0];
		waveConfig169.Monsters.Add(new LevelMonster("mushroomToxic"), 20);
		WaveConfig waveConfig170 = _configs["w02L07"].Waves[0];
		waveConfig170.Monsters.Add(new LevelMonster("frog"), 0);
		_configs["w02L08"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 3,
				MonsterPerWaveMax = 4,
				TokenObjective = 330,
				SpawnLayerMin = 1,
				SpawnLayerMax = 3,
				RoundCountBetweenWave = 3,
				MaxMonster = 7,
				MonsterBoost = 1.7f,
				DamageToWeapon = 2
			}
		};
		WaveConfig waveConfig171 = _configs["w02L08"].Waves[0];
		waveConfig171.Monsters.Add(new LevelMonster("fishman"), 20);
		WaveConfig waveConfig172 = _configs["w02L08"].Waves[0];
		waveConfig172.Monsters.Add(new LevelMonster("leech"), 0);
		WaveConfig waveConfig173 = _configs["w02L08"].Waves[0];
		waveConfig173.Monsters.Add(new LevelMonster("salamander"), 50);
		WaveConfig waveConfig174 = _configs["w02L08"].Waves[0];
		waveConfig174.Monsters.Add(new LevelMonster("mushroomToxic"), 20);
		WaveConfig waveConfig175 = _configs["w02L08"].Waves[0];
		waveConfig175.Monsters.Add(new LevelMonster("frog"), 10);
		_configs["w02L09"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 4,
				MonsterPerWaveMax = 5,
				TokenObjective = 360,
				SpawnLayerMin = 1,
				SpawnLayerMax = 4,
				RoundCountBetweenWave = 3,
				MaxMonster = 7,
				MonsterBoost = 2.4f,
				DamageToWeapon = 2
			}
		};
		WaveConfig waveConfig176 = _configs["w02L09"].Waves[0];
		waveConfig176.Monsters.Add(new LevelMonster("fishman"), 50);
		WaveConfig waveConfig177 = _configs["w02L09"].Waves[0];
		waveConfig177.Monsters.Add(new LevelMonster("leech"), 10);
		WaveConfig waveConfig178 = _configs["w02L09"].Waves[0];
		waveConfig178.Monsters.Add(new LevelMonster("salamander"), 30);
		WaveConfig waveConfig179 = _configs["w02L09"].Waves[0];
		waveConfig179.Monsters.Add(new LevelMonster("mushroomToxic"), 0);
		WaveConfig waveConfig180 = _configs["w02L09"].Waves[0];
		waveConfig180.Monsters.Add(new LevelMonster("frog"), 10);
		_configs["w02L10"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 3,
				MonsterPerWaveMax = 5,
				TokenObjective = 390,
				SpawnLayerMin = 5,
				SpawnLayerMax = 5,
				RoundCountBetweenWave = 4,
				MaxMonster = 7,
				MonsterBoost = 1.8f,
				DamageToWeapon = 3
			}
		};
		WaveConfig waveConfig181 = _configs["w02L10"].Waves[0];
		waveConfig181.Monsters.Add(new LevelMonster("fishman"), 0);
		WaveConfig waveConfig182 = _configs["w02L10"].Waves[0];
		waveConfig182.Monsters.Add(new LevelMonster("leech"), 0);
		WaveConfig waveConfig183 = _configs["w02L10"].Waves[0];
		waveConfig183.Monsters.Add(new LevelMonster("salamander"), 15);
		WaveConfig waveConfig184 = _configs["w02L10"].Waves[0];
		waveConfig184.Monsters.Add(new LevelMonster("mushroomToxic"), 75);
		WaveConfig waveConfig185 = _configs["w02L10"].Waves[0];
		waveConfig185.Monsters.Add(new LevelMonster("frog"), 10);
		_configs["w02L11"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 3,
				MonsterPerWaveMax = 5,
				TokenObjective = 420,
				SpawnLayerMin = 2,
				SpawnLayerMax = 3,
				RoundCountBetweenWave = 3,
				MaxMonster = 7,
				MonsterBoost = 1.9f,
				DamageToWeapon = 3
			}
		};
		WaveConfig waveConfig186 = _configs["w02L11"].Waves[0];
		waveConfig186.Monsters.Add(new LevelMonster("fishman"), 0);
		WaveConfig waveConfig187 = _configs["w02L11"].Waves[0];
		waveConfig187.Monsters.Add(new LevelMonster("leech"), 35);
		WaveConfig waveConfig188 = _configs["w02L11"].Waves[0];
		waveConfig188.Monsters.Add(new LevelMonster("salamander"), 10);
		WaveConfig waveConfig189 = _configs["w02L11"].Waves[0];
		waveConfig189.Monsters.Add(new LevelMonster("mushroomToxic"), 20);
		WaveConfig waveConfig190 = _configs["w02L11"].Waves[0];
		waveConfig190.Monsters.Add(new LevelMonster("frog"), 35);
		_configs["w02L12"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 4,
				MonsterPerWaveMax = 5,
				TokenObjective = 450,
				SpawnLayerMin = 2,
				SpawnLayerMax = 3,
				RoundCountBetweenWave = 3,
				MaxMonster = 8,
				MonsterBoost = 2f,
				DamageToWeapon = 3
			}
		};
		WaveConfig waveConfig191 = _configs["w02L12"].Waves[0];
		waveConfig191.Monsters.Add(new LevelMonster("fishman"), 80);
		WaveConfig waveConfig192 = _configs["w02L12"].Waves[0];
		waveConfig192.Monsters.Add(new LevelMonster("leech"), 10);
		WaveConfig waveConfig193 = _configs["w02L12"].Waves[0];
		waveConfig193.Monsters.Add(new LevelMonster("salamander"), 0);
		WaveConfig waveConfig194 = _configs["w02L12"].Waves[0];
		waveConfig194.Monsters.Add(new LevelMonster("mushroomToxic"), 10);
		WaveConfig waveConfig195 = _configs["w02L12"].Waves[0];
		waveConfig195.Monsters.Add(new LevelMonster("frog"), 0);
		_configs["w02L13"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 3,
				MonsterPerWaveMax = 6,
				TokenObjective = 480,
				SpawnLayerMin = 2,
				SpawnLayerMax = 2,
				RoundCountBetweenWave = 2,
				MaxMonster = 9,
				MonsterBoost = 2.1f,
				DamageToWeapon = 3
			}
		};
		WaveConfig waveConfig196 = _configs["w02L13"].Waves[0];
		waveConfig196.Monsters.Add(new LevelMonster("fishman"), 80);
		WaveConfig waveConfig197 = _configs["w02L13"].Waves[0];
		waveConfig197.Monsters.Add(new LevelMonster("leech"), 5);
		WaveConfig waveConfig198 = _configs["w02L13"].Waves[0];
		waveConfig198.Monsters.Add(new LevelMonster("salamander"), 5);
		WaveConfig waveConfig199 = _configs["w02L13"].Waves[0];
		waveConfig199.Monsters.Add(new LevelMonster("mushroomToxic"), 5);
		WaveConfig waveConfig200 = _configs["w02L13"].Waves[0];
		waveConfig200.Monsters.Add(new LevelMonster("frog"), 5);
		_configs["w02L14"].Waves = new List<WaveConfig>
		{
			new WaveConfig
			{
				Type = WaveType.Monsters,
				Monsters = new WeightedList<LevelMonster>(),
				MonsterPerWaveMin = 6,
				MonsterPerWaveMax = 6,
				TokenObjective = 510,
				SpawnLayerMin = 4,
				SpawnLayerMax = 4,
				RoundCountBetweenWave = 9,
				MaxMonster = 10,
				MonsterBoost = 2.9f,
				DamageToWeapon = 3
			}
		};
		WaveConfig waveConfig201 = _configs["w02L14"].Waves[0];
		waveConfig201.Monsters.Add(new LevelMonster("fishman"), 15);
		WaveConfig waveConfig202 = _configs["w02L14"].Waves[0];
		waveConfig202.Monsters.Add(new LevelMonster("leech"), 15);
		WaveConfig waveConfig203 = _configs["w02L14"].Waves[0];
		waveConfig203.Monsters.Add(new LevelMonster("salamander"), 25);
		WaveConfig waveConfig204 = _configs["w02L14"].Waves[0];
		waveConfig204.Monsters.Add(new LevelMonster("mushroomToxic"), 35);
		WaveConfig waveConfig205 = _configs["w02L14"].Waves[0];
		waveConfig205.Monsters.Add(new LevelMonster("frog"), 10);
	}

	public AdventureLevelConfig GetConfig(string id)
	{
 AdventureLevelConfig value;		if (_configs.TryGetValue(id, out value))
		{
			return value;
		}
		return null;
	}

	public AdventureLevelConfig GetConfigByIndex(string worldId, int levelIndex)
	{
		List<AdventureLevelConfig> list = new List<AdventureLevelConfig>(_configs.Values);
		int index = Mathf.Min(levelIndex, list.Count - 1);
		return list.Find((AdventureLevelConfig config) => config.WorldId == worldId && config.Index == index);
	}

	public List<float> GetMonsterBoosts(string worldId, int levelIndexMax)
	{
		List<AdventureLevelConfig> list = new List<AdventureLevelConfig>(_configs.Values).FindAll((AdventureLevelConfig config) => config.WorldId == worldId && config.Index <= levelIndexMax);
		return list.ConvertAll(delegate(AdventureLevelConfig config)
		{
			WaveConfig waveConfig = config.Waves[0];
			return waveConfig.MonsterBoost;
		});
	}

	public List<AdventureLevelConfig> GetConfigs(string worldId)
	{
		return new List<AdventureLevelConfig>(_configs.Values).FindAll((AdventureLevelConfig config) => config.WorldId.Equals(worldId));
	}

	public AdventureLevelConfig GetDefaultConfig()
	{
		return GetConfig("w00L00");
	}

	public override void LoadFromCSV(CSVFile file)
	{
		for (int i = 0; i < file.EntriesCount; i++)
		{
			string @string = file.GetString(i, "Id");
			if (_configs.ContainsKey(@string))
			{
				AdventureLevelConfig arg = _configs[@string];
				UnityEngine.Debug.Log("Override completed. " + arg);
			}
			else
			{
				UnityEngine.Debug.LogWarning("[" + ConfigType + "] failed to overwrite " + @string);
			}
		}
	}
}