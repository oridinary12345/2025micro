using System.Collections.Generic;
using UnityEngine;
using Utils;

public class MonsterConfigs : Configs<MonsterConfigs>
{
	public const string MONSTER_CHEST = "chest";

	public const string MONSTER_CHEST_LOCKED = "chestLocked";

	public const string MONSTER_CHEST_SMALL = "chestSmall";

	public const string MONSTER_CHEST_MEDIUM = "chestMedium";

	public const string MONSTER_CHEST_BIG = "chestBig";

	public const string MONSTER_CHEST_PUB = "chestPub";

	public const string MONSTER_MUSHROOM = "mushroom";

	public const string MONSTER_WOODLOG = "woodlog";

	public const string MONSTER_ACORN = "acorn";

	public const string MONSTER_STRAWBERRY = "strawberry";

	public const string MONSTER_DRAGON = "dragon";

	public const string MONSTER_SKELETON = "skeleton";

	public const string MONSTER_SNAKE = "snake";

	public const string MONSTER_GHOST = "ghost";

	public const string MONSTER_SPIDER = "spider";

	public const string MONSTER_EYE = "eye";

	public const string MONSTER_FISHMAN = "fishman";

	public const string MONSTER_LEECH = "leech";

	public const string MONSTER_MUSHROOM_TOXIC = "mushroomToxic";

	public const string MONSTER_SALAMANDER = "salamander";

	public const string MONSTER_FROG = "frog";

	public const string MONSTER_CHEST_REWARD1 = "rewardChestDead1";

	public const string MONSTER_CHEST_REWARD2 = "rewardChestDead2";

	public const string MONSTER_CHEST_REWARD3 = "rewardChestDead3";

	public const string MONSTER_CHEST_SMALL_REWARD = "rewardChestSmallDead";

	public const string MONSTER_CHEST_MEDIUM_REWARD = "rewardChestMediumDead";

	public const string MONSTER_CHEST_BIG_REWARD = "rewardChestBigDead";

	private readonly Dictionary<string, MonsterConfig> _configs = new Dictionary<string, MonsterConfig>();

	public override ConfigType ConfigType => ConfigType.Monster;

	protected override void Init()
	{
		base.Init();
		_configs["mushroom"] = new MonsterConfig
		{
			Id = "mushroom",
			Name = "MAGIC MUSHROOM",
			DamageMin = 10,
			DamageMax = 16,
			HP = 20,
			MonsterType = MonsterType.monsterNormal,
			RewardIdHit = new List<string>
			{
				"rewardMushroomHit1",
				"rewardMushroomHit2"
			},
			RewardIdDead = new List<string>(),
			Element = Element.A,
			MovePatternForward = "Center",
			MovePatternBackward = "Back",
			AttackAttemptCountMax = 3,
			TokenValue = 10,
			WeaponType = WeaponType.Melee,
			MissRate = 0f,
			LifeDurationRoundCount = 10
		};
		_configs["woodlog"] = new MonsterConfig
		{
			Id = "woodlog",
			Name = "WOODLOG",
			DamageMin = 14,
			DamageMax = 22,
			HP = 35,
			MonsterType = MonsterType.monsterNormal,
			RewardIdHit = new List<string>
			{
				"rewardWoodlogHit1",
				"rewardWoodlogHit2"
			},
			RewardIdDead = new List<string>(),
			Element = Element.A,
			MovePatternForward = "Right,Right,Right,Skip,Center,Center",
			MovePatternBackward = "Back,Back",
			AttackAttemptCountMax = 3,
			TokenValue = 20,
			WeaponType = WeaponType.Melee,
			MissRate = 0f,
			LifeDurationRoundCount = 10
		};
		_configs["acorn"] = new MonsterConfig
		{
			Id = "acorn",
			Name = "ACORN",
			DamageMin = 10,
			DamageMax = 28,
			HP = 52,
			MonsterType = MonsterType.monsterNormal,
			RewardIdHit = new List<string>
			{
				"rewardAcornHit1",
				"rewardAcornHit2"
			},
			RewardIdDead = new List<string>(),
			Element = Element.A,
			MovePatternForward = "Left,Skip,Center,Center,Center",
			MovePatternBackward = "Back,Back",
			AttackAttemptCountMax = 2,
			TokenValue = 40,
			WeaponType = WeaponType.Melee,
			MissRate = 0f,
			LifeDurationRoundCount = 9
		};
		_configs["chest"] = new MonsterConfig
		{
			Id = "chest",
			Name = "CHEST",
			DamageMin = 0,
			DamageMax = 0,
			HP = 15,
			MonsterType = MonsterType.monsterStatic,
			RewardIdHit = new List<string>(),
			RewardIdDead = new List<string>
			{
				"rewardChestDead1",
				"rewardChestDead2",
				"rewardChestDead3"
			},
			Element = Element.None,
			MovePatternForward = "None",
			MovePatternBackward = "None",
			AttackAttemptCountMax = 0,
			TokenValue = 0,
			WeaponType = WeaponType.None,
			MissRate = 0f,
			LifeDurationRoundCount = 7
		};
		_configs["chestLocked"] = new MonsterConfig
		{
			Id = "chestLocked",
			Name = "CHEST",
			DamageMin = 0,
			DamageMax = 0,
			HP = 1,
			MonsterType = MonsterType.monsterStatic,
			RewardIdHit = new List<string>(),
			RewardIdDead = new List<string>(),
			Element = Element.None,
			MovePatternForward = "None",
			MovePatternBackward = "None",
			AttackAttemptCountMax = 0,
			TokenValue = 0,
			WeaponType = WeaponType.None,
			MissRate = 0f,
			LifeDurationRoundCount = 7
		};
		_configs["chestPub"] = new MonsterConfig
		{
			Id = "chestPub",
			Name = "CHEST",
			DamageMin = 0,
			DamageMax = 0,
			HP = 1,
			MonsterType = MonsterType.monsterStatic,
			RewardIdHit = new List<string>(),
			RewardIdDead = new List<string>
			{
				"rewardChestSmallDead"
			},
			Element = Element.None,
			MovePatternForward = "None",
			MovePatternBackward = "None",
			AttackAttemptCountMax = 0,
			TokenValue = 0,
			WeaponType = WeaponType.None,
			MissRate = 0f
		};
		_configs["chestSmall"] = new MonsterConfig
		{
			Id = "chestSmall",
			Name = "CHEST",
			DamageMin = 0,
			DamageMax = 0,
			HP = 30,
			MonsterType = MonsterType.monsterStatic,
			RewardIdHit = new List<string>(),
			RewardIdDead = new List<string>
			{
				"rewardChestSmallDead"
			},
			Element = Element.None,
			MovePatternForward = "None",
			MovePatternBackward = "None",
			AttackAttemptCountMax = 0,
			TokenValue = 0,
			WeaponType = WeaponType.None,
			MissRate = 0f,
			LifeDurationRoundCount = 4
		};
		_configs["chestMedium"] = new MonsterConfig
		{
			Id = "chestMedium",
			Name = "CHEST",
			DamageMin = 0,
			DamageMax = 0,
			HP = 50,
			MonsterType = MonsterType.monsterStatic,
			RewardIdHit = new List<string>(),
			RewardIdDead = new List<string>
			{
				"rewardChestMediumDead"
			},
			Element = Element.None,
			MovePatternForward = "None",
			MovePatternBackward = "None",
			AttackAttemptCountMax = 0,
			TokenValue = 0,
			WeaponType = WeaponType.None,
			MissRate = 0f,
			LifeDurationRoundCount = 4
		};
		_configs["chestBig"] = new MonsterConfig
		{
			Id = "chestBig",
			Name = "CHEST",
			DamageMin = 0,
			DamageMax = 0,
			HP = 90,
			MonsterType = MonsterType.monsterStatic,
			RewardIdHit = new List<string>(),
			RewardIdDead = new List<string>
			{
				"rewardChestBigDead"
			},
			Element = Element.None,
			MovePatternForward = "None",
			MovePatternBackward = "None",
			AttackAttemptCountMax = 0,
			TokenValue = 0,
			WeaponType = WeaponType.None,
			MissRate = 0f,
			LifeDurationRoundCount = 3
		};
		_configs["strawberry"] = new MonsterConfig
		{
			Id = "strawberry",
			Name = "STRAWBERRY",
			DamageMin = 16,
			DamageMax = 18,
			HP = 65,
			MonsterType = MonsterType.monsterNormal,
			RewardIdHit = new List<string>
			{
				"rewardStrawberryHit1",
				"rewardStrawberryHit2"
			},
			RewardIdDead = new List<string>(),
			Element = Element.D,
			MovePatternForward = "Center,Center,Left,Skip,Right,Center,Center",
			MovePatternBackward = "Back,Back,Right,Skip,Back,Back,Left",
			AttackAttemptCountMax = 2,
			TokenValue = 30,
			WeaponType = WeaponType.Melee,
			MissRate = 0f,
			LifeDurationRoundCount = 8
		};
		_configs["dragon"] = new MonsterConfig
		{
			Id = "dragon",
			Name = "DRAGON",
			DamageMin = 16,
			DamageMax = 44,
			HP = 86,
			MonsterType = MonsterType.monsterNormal,
			RewardIdHit = new List<string>
			{
				"rewardDragonHit1",
				"rewardDragonHit2"
			},
			RewardIdDead = new List<string>(),
			Element = Element.B,
			MovePatternForward = "Left,Skip,Center,Skip,Left",
			MovePatternBackward = "Back,Back",
			AttackAttemptCountMax = 1,
			TokenValue = 50,
			WeaponType = WeaponType.Fireball,
			MissRate = 0.05f,
			AttackEachRoundCount = 3,
			LifeDurationRoundCount = 7
		};
		_configs["skeleton"] = new MonsterConfig
		{
			Id = "skeleton",
			Name = "SKELETON",
			DamageMin = 15,
			DamageMax = 24,
			HP = 30,
			MonsterType = MonsterType.monsterNormal,
			RewardIdHit = new List<string>
			{
				"rewardSkeletonHit1",
				"rewardSkeletonHit2"
			},
			RewardIdDead = new List<string>(),
			Element = Element.B,
			MovePatternForward = "center,center",
			MovePatternBackward = "back,back",
			AttackAttemptCountMax = 3,
			TokenValue = 30,
			WeaponType = WeaponType.Melee,
			MissRate = 0f,
			LifeDurationRoundCount = 10
		};
		_configs["snake"] = new MonsterConfig
		{
			Id = "snake",
			Name = "SNAKE",
			DamageMin = 24,
			DamageMax = 66,
			HP = 129,
			MonsterType = MonsterType.monsterNormal,
			RewardIdHit = new List<string>
			{
				"rewardSnakeHit1",
				"rewardSnakeHit2"
			},
			RewardIdDead = new List<string>(),
			Element = Element.D,
			MovePatternForward = "right,center,right,center,right",
			MovePatternBackward = "right,back,right,back,right",
			AttackAttemptCountMax = 1,
			TokenValue = 30,
			WeaponType = WeaponType.Melee,
			MissRate = 0f,
			LifeDurationRoundCount = 10
		};
		_configs["spider"] = new MonsterConfig
		{
			Id = "spider",
			Name = "SPIDER",
			DamageMin = 21,
			DamageMax = 33,
			HP = 52,
			MonsterType = MonsterType.monsterNormal,
			RewardIdHit = new List<string>
			{
				"rewardSpiderHit1",
				"rewardSpiderHit2"
			},
			RewardIdDead = new List<string>(),
			Element = Element.C,
			MovePatternForward = "left,left,left,skip,left,left,center,center",
			MovePatternBackward = "back,skip,back",
			AttackAttemptCountMax = 1,
			TokenValue = 30,
			WeaponType = WeaponType.Ranged,
			MissRate = 0f,
			AttackEachRoundCount = 4,
			LifeDurationRoundCount = 9
		};
		_configs["ghost"] = new MonsterConfig
		{
			Id = "ghost",
			Name = "GHOST",
			DamageMin = 15,
			DamageMax = 42,
			HP = 78,
			MonsterType = MonsterType.monsterNormal,
			RewardIdHit = new List<string>
			{
				"rewardGhostHit1",
				"rewardGhostHit2"
			},
			RewardIdDead = new List<string>(),
			Element = Element.C,
			MovePatternForward = "right,right,skip,center,center,center",
			MovePatternBackward = "back,back,left,back",
			AttackAttemptCountMax = 2,
			TokenValue = 30,
			WeaponType = WeaponType.Melee,
			MissRate = 0f,
			LifeDurationRoundCount = 8
		};
		_configs["eye"] = new MonsterConfig
		{
			Id = "eye",
			Name = "EYE",
			DamageMin = 24,
			DamageMax = 27,
			HP = 98,
			MonsterType = MonsterType.monsterNormal,
			RewardIdHit = new List<string>
			{
				"rewardEyeHit1",
				"rewardEyeHit2"
			},
			RewardIdDead = new List<string>(),
			Element = Element.D,
			MovePatternForward = "center,left,left",
			MovePatternBackward = "back",
			AttackAttemptCountMax = 1,
			TokenValue = 30,
			WeaponType = WeaponType.Ranged,
			MissRate = 0f,
			AttackEachRoundCount = 3,
			LifeDurationRoundCount = 7
		};
		_configs["fishman"] = new MonsterConfig
		{
			Id = "fishman",
			Name = "FISHMAN",
			DamageMin = 23,
			DamageMax = 36,
			HP = 45,
			MonsterType = MonsterType.monsterNormal,
			RewardIdHit = new List<string>
			{
				"rewardFishmanHit1",
				"rewardFishmanHit2"
			},
			RewardIdDead = new List<string>(),
			Element = Element.C,
			MovePatternForward = "center,center,center",
			MovePatternBackward = "back",
			AttackAttemptCountMax = 2,
			TokenValue = 30,
			WeaponType = WeaponType.Melee,
			MissRate = 0f,
			LifeDurationRoundCount = 10
		};
		_configs["leech"] = new MonsterConfig
		{
			Id = "leech",
			Name = "LEECH",
			DamageMin = 32,
			DamageMax = 50,
			HP = 79,
			MonsterType = MonsterType.monsterNormal,
			RewardIdHit = new List<string>
			{
				"rewardLeechHit1",
				"rewardLeechHit2"
			},
			RewardIdDead = new List<string>(),
			Element = Element.E,
			MovePatternForward = "left,skip,center,center,right,skip,center,center",
			MovePatternBackward = "back,right,back,skip,back,left,back",
			AttackAttemptCountMax = 3,
			TokenValue = 30,
			WeaponType = WeaponType.Melee,
			MissRate = 0f,
			LifeDurationRoundCount = 10
		};
		_configs["mushroomToxic"] = new MonsterConfig
		{
			Id = "mushroomToxic",
			Name = "TOXIC MUSHROOM",
			DamageMin = 36,
			DamageMax = 55,
			HP = 132,
			MonsterType = MonsterType.monsterNormal,
			RewardIdHit = new List<string>
			{
				"rewardMushroomToxicHit1",
				"rewardMushroomToxicHit2"
			},
			RewardIdDead = new List<string>(),
			Element = Element.E,
			MovePatternForward = "right,right,right,right,right",
			MovePatternBackward = "back,skip,back",
			AttackAttemptCountMax = 1,
			TokenValue = 30,
			WeaponType = WeaponType.Ranged,
			MissRate = 0f,
			AttackEachRoundCount = 5,
			LifeDurationRoundCount = 9
		};
		_configs["salamander"] = new MonsterConfig
		{
			Id = "salamander",
			Name = "SALAMANDER",
			DamageMin = 23,
			DamageMax = 63,
			HP = 117,
			MonsterType = MonsterType.monsterNormal,
			RewardIdHit = new List<string>
			{
				"rewardSalamanderHit1",
				"rewardSalamanderHit2"
			},
			RewardIdDead = new List<string>(),
			Element = Element.F,
			MovePatternForward = "left,skip,center,center,center",
			MovePatternBackward = "back,back",
			AttackAttemptCountMax = 2,
			TokenValue = 30,
			WeaponType = WeaponType.Melee,
			MissRate = 0f,
			LifeDurationRoundCount = 8
		};
		_configs["frog"] = new MonsterConfig
		{
			Id = "frog",
			Name = "FROG",
			DamageMin = 36,
			DamageMax = 41,
			HP = 146,
			MonsterType = MonsterType.monsterNormal,
			RewardIdHit = new List<string>
			{
				"rewardFrogHit1",
				"rewardFrogHit2"
			},
			RewardIdDead = new List<string>(),
			Element = Element.E,
			MovePatternForward = "center,center,left,skip,right,center,center",
			MovePatternBackward = "back,back,right,skip,back,back,left",
			AttackAttemptCountMax = 2,
			TokenValue = 30,
			WeaponType = WeaponType.Melee,
			MissRate = 0f,
			LifeDurationRoundCount = 7
		};
		RewardConfigs instance = MonoSingleton<RewardConfigs>.Instance;
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardMushroomHit1",
			RewardType = RewardType.loot,
			RewardTypeId = "lootCoin",
			AmountMin = 4,
			AmountMax = 6,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardMushroomHit2",
			RewardType = RewardType.life,
			RewardTypeId = string.Empty,
			AmountMin = 1,
			AmountMax = 1,
			Weight = 0,
			Percentage = 0.1f,
			ParamInt1 = 5
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardWoodlogHit1",
			RewardType = RewardType.loot,
			RewardTypeId = "lootCoin",
			AmountMin = 9,
			AmountMax = 11,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardWoodlogHit2",
			RewardType = RewardType.life,
			RewardTypeId = string.Empty,
			AmountMin = 1,
			AmountMax = 1,
			Weight = 0,
			Percentage = 0.15f,
			ParamInt1 = 7
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardAcornHit1",
			RewardType = RewardType.loot,
			RewardTypeId = "lootCoin",
			AmountMin = 19,
			AmountMax = 21,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardAcornHit2",
			RewardType = RewardType.life,
			RewardTypeId = string.Empty,
			AmountMin = 1,
			AmountMax = 1,
			Weight = 0,
			Percentage = 0.1f,
			ParamInt1 = 12
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardDragonHit1",
			RewardType = RewardType.loot,
			RewardTypeId = "lootCoin",
			AmountMin = 24,
			AmountMax = 26,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardDragonHit2",
			RewardType = RewardType.life,
			RewardTypeId = string.Empty,
			AmountMin = 1,
			AmountMax = 1,
			Weight = 0,
			Percentage = 0.1f,
			ParamInt1 = 18
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardStrawberryHit1",
			RewardType = RewardType.loot,
			RewardTypeId = "lootCoin",
			AmountMin = 20,
			AmountMax = 25,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardStrawberryHit2",
			RewardType = RewardType.life,
			RewardTypeId = string.Empty,
			AmountMin = 1,
			AmountMax = 1,
			Weight = 0,
			Percentage = 0.15f,
			ParamInt1 = 14
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardSkeletonHit1",
			RewardType = RewardType.loot,
			RewardTypeId = "lootCoin",
			AmountMin = 6,
			AmountMax = 9,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardSkeletonHit2",
			RewardType = RewardType.life,
			RewardTypeId = string.Empty,
			AmountMin = 1,
			AmountMax = 1,
			Weight = 0,
			Percentage = 0.1f,
			ParamInt1 = 8
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardSnakeHit1",
			RewardType = RewardType.loot,
			RewardTypeId = "lootCoin",
			AmountMin = 36,
			AmountMax = 39,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardSnakeHit2",
			RewardType = RewardType.life,
			RewardTypeId = string.Empty,
			AmountMin = 1,
			AmountMax = 1,
			Weight = 0,
			Percentage = 0.1f,
			ParamInt1 = 27
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardGhostHit1",
			RewardType = RewardType.loot,
			RewardTypeId = "lootCoin",
			AmountMin = 29,
			AmountMax = 32,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardGhostHit2",
			RewardType = RewardType.life,
			RewardTypeId = string.Empty,
			AmountMin = 1,
			AmountMax = 1,
			Weight = 0,
			Percentage = 0.1f,
			ParamInt1 = 18
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardSpiderHit1",
			RewardType = RewardType.loot,
			RewardTypeId = "lootCoin",
			AmountMin = 14,
			AmountMax = 17,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardSpiderHit2",
			RewardType = RewardType.life,
			RewardTypeId = string.Empty,
			AmountMin = 1,
			AmountMax = 1,
			Weight = 0,
			Percentage = 0.15f,
			ParamInt1 = 11
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardEyeHit1",
			RewardType = RewardType.loot,
			RewardTypeId = "lootCoin",
			AmountMin = 30,
			AmountMax = 38,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardEyeHit2",
			RewardType = RewardType.life,
			RewardTypeId = string.Empty,
			AmountMin = 1,
			AmountMax = 1,
			Weight = 0,
			Percentage = 0.15f,
			ParamInt1 = 21
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardFishmanHit1",
			RewardType = RewardType.loot,
			RewardTypeId = "lootCoin",
			AmountMin = 9,
			AmountMax = 14,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardFishmanHit2",
			RewardType = RewardType.life,
			RewardTypeId = string.Empty,
			AmountMin = 1,
			AmountMax = 1,
			Weight = 0,
			Percentage = 0.1f,
			ParamInt1 = 11
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardLeechHit1",
			RewardType = RewardType.loot,
			RewardTypeId = "lootCoin",
			AmountMin = 20,
			AmountMax = 25,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardLeechHit2",
			RewardType = RewardType.life,
			RewardTypeId = string.Empty,
			AmountMin = 1,
			AmountMax = 1,
			Weight = 0,
			Percentage = 0.15f,
			ParamInt1 = 16
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardFrogHit1",
			RewardType = RewardType.loot,
			RewardTypeId = "lootCoin",
			AmountMin = 45,
			AmountMax = 56,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardFrogHit2",
			RewardType = RewardType.life,
			RewardTypeId = string.Empty,
			AmountMin = 1,
			AmountMax = 1,
			Weight = 0,
			Percentage = 0.15f,
			ParamInt1 = 32
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardSalamanderHit1",
			RewardType = RewardType.loot,
			RewardTypeId = "lootCoin",
			AmountMin = 43,
			AmountMax = 47,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardSalamanderHit2",
			RewardType = RewardType.life,
			RewardTypeId = string.Empty,
			AmountMin = 1,
			AmountMax = 1,
			Weight = 0,
			Percentage = 0.1f,
			ParamInt1 = 27
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardMushroomToxicHit1",
			RewardType = RewardType.loot,
			RewardTypeId = "lootCoin",
			AmountMin = 54,
			AmountMax = 59,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardMushroomToxicHit2",
			RewardType = RewardType.life,
			RewardTypeId = string.Empty,
			AmountMin = 1,
			AmountMax = 1,
			Weight = 0,
			Percentage = 0.1f,
			ParamInt1 = 41
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardChestDead1",
			RewardType = RewardType.loot,
			RewardTypeId = "lootCoin",
			AmountMin = 0,
			AmountMax = 0,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0,
			IsCard = true
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardChestDead2",
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
			Id = "rewardChestDead3",
			RewardType = RewardType.hero,
			RewardTypeId = string.Empty,
			AmountMin = 1,
			AmountMax = 1,
			Weight = 0,
			Percentage = 0.25f,
			ParamInt1 = 0,
			IsCard = true
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardChestSmallDead",
			RewardType = RewardType.loot,
			RewardTypeId = "lootCoin",
			AmountMin = 0,
			AmountMax = 0,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardChestMediumDead",
			RewardType = RewardType.loot,
			RewardTypeId = "lootCoin",
			AmountMin = 0,
			AmountMax = 0,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0
		});
		instance.AddConfig(new RewardConfig
		{
			Id = "rewardChestBigDead",
			RewardType = RewardType.loot,
			RewardTypeId = "lootCoin",
			AmountMin = 0,
			AmountMax = 0,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0
		});
	}

	public MonsterConfig GetConfig(string id)
	{
 MonsterConfig value;		if (_configs.TryGetValue(id, out value))
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
				MonsterConfig monsterConfig = _configs[@string];
				monsterConfig.Name = file.GetString(i, "Name");
				monsterConfig.DamageMin = file.GetInt(i, "DamageMin");
				monsterConfig.DamageMax = file.GetInt(i, "DamageMax");
				monsterConfig.HP = file.GetInt(i, "HP");
				monsterConfig.RewardIdHit = file.GetListString(i, "RewardIdHit");
				monsterConfig.RewardIdDead = file.GetListString(i, "RewardIdDead");
				monsterConfig.Element = Enum.TryParse(file.GetString(i, "Element"), Element.None);
				monsterConfig.MovePatternForward = file.GetString(i, "MovePatternForward");
				monsterConfig.MovePatternBackward = file.GetString(i, "MovePatternBackward");
				monsterConfig.AttackAttemptCountMax = file.GetInt(i, "AttackAttemptCountMax");
				monsterConfig.TokenValue = file.GetInt(i, "TokenValue");
				monsterConfig.WeaponType = Enum.TryParse(file.GetString(i, "WeaponType"), WeaponType.Melee);
				monsterConfig.MissRate = file.GetFloat(i, "MissRate");
				monsterConfig.AttackEachRoundCount = file.GetInt(i, "AttackEachRoundCount");
				monsterConfig.LifeDurationRoundCount = file.GetInt(i, "LifeDurationRoundCount");
			}
			else
			{
				UnityEngine.Debug.LogWarning("[" + ConfigType + "] failed to overwrite " + @string);
			}
		}
	}

	private string ListToString(List<string> l)
	{
		string text = null;
		foreach (string item in l)
		{
			if (!string.IsNullOrEmpty(text))
			{
				text += ",";
			}
			text += item;
		}
		return text;
	}
}