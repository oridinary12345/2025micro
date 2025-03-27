using System.Collections.Generic;
using UnityEngine;
using Utils;

public class WeaponConfigs : Configs<WeaponConfigs>
{
	public const string BOMB_ID = "weapon04Bomb";

	public const string IRONGLOVE_ID = "weapon04";

	public const string KUNAI_ID = "weapon09";

	private readonly Dictionary<string, WeaponConfig> _configs = new Dictionary<string, WeaponConfig>();

	public override ConfigType ConfigType => ConfigType.Weapon;

	protected override void Init()
	{
		base.Init();
		_configs["weapon01"] = new WeaponConfig
		{
			Id = "weapon01",
			Title = "SWORD",
			WorldId = "w00",
			DamageMin = 12,
			DamageMax = 14,
			HpMaxBase = 18,
			UpgradePriceBase = 20,
			DamageLevelRatio = 3,
			HpMaxLevelRatio = 3,
			ElementWeak = Element.C,
			ElementCritical = Element.D,
			ElementMiss = Element.None,
			WeaponType = WeaponType.Melee,
			RangeType = WeaponRangeType.Short,
			PushForce = 1,
			CriticalChances = 0.05f,
			CanCounterGhost = false
		};
		_configs["weapon02"] = new WeaponConfig
		{
			Id = "weapon02",
			Title = "HAMMER",
			WorldId = "w00",
			DamageMin = 11,
			DamageMax = 17,
			HpMaxBase = 10,
			UpgradePriceBase = 20,
			DamageLevelRatio = 3,
			HpMaxLevelRatio = 3,
			ElementWeak = Element.F,
			ElementCritical = Element.E,
			ElementMiss = Element.None,
			WeaponType = WeaponType.Melee,
			RangeType = WeaponRangeType.Medium,
			PushForce = 2,
			CriticalChances = 0.01f,
			CanCounterGhost = false
		};
		_configs["weapon03"] = new WeaponConfig
		{
			Id = "weapon03",
			Title = "BOW",
			WorldId = "w00",
			DamageMin = 14,
			DamageMax = 15,
			HpMaxBase = 16,
			UpgradePriceBase = 20,
			DamageLevelRatio = 3,
			HpMaxLevelRatio = 3,
			ElementWeak = Element.E,
			ElementCritical = Element.A,
			ElementMiss = Element.None,
			WeaponType = WeaponType.Ranged,
			RangeType = WeaponRangeType.Long,
			PushForce = 1,
			CriticalChances = 0.01f,
			CanCounterGhost = false
		};
		_configs["weapon04"] = new WeaponConfig
		{
			Id = "weapon04",
			Title = "IRON GLOVE",
			WorldId = "w01",
			DamageMin = 15,
			DamageMax = 16,
			HpMaxBase = 14,
			UpgradePriceBase = 20,
			DamageLevelRatio = 3,
			HpMaxLevelRatio = 3,
			ElementWeak = Element.A,
			ElementCritical = Element.C,
			ElementMiss = Element.None,
			WeaponType = WeaponType.Melee,
			RangeType = WeaponRangeType.Short,
			PushForce = 1,
			CriticalChances = 0.01f,
			CanCounterGhost = false
		};
		_configs["weapon05"] = new WeaponConfig
		{
			Id = "weapon05",
			Title = "AXE",
			WorldId = "w01",
			DamageMin = 9,
			DamageMax = 18,
			HpMaxBase = 16,
			UpgradePriceBase = 20,
			DamageLevelRatio = 3,
			HpMaxLevelRatio = 3,
			ElementWeak = Element.D,
			ElementCritical = Element.A,
			ElementMiss = Element.None,
			WeaponType = WeaponType.Melee,
			RangeType = WeaponRangeType.Medium,
			PushForce = 2,
			CriticalChances = 0.05f,
			CanCounterGhost = true
		};
		_configs["weapon06"] = new WeaponConfig
		{
			Id = "weapon06",
			Title = "LANCE",
			WorldId = "w01",
			DamageMin = 15,
			DamageMax = 19,
			HpMaxBase = 20,
			UpgradePriceBase = 20,
			DamageLevelRatio = 3,
			HpMaxLevelRatio = 3,
			ElementWeak = Element.E,
			ElementCritical = Element.B,
			ElementMiss = Element.None,
			WeaponType = WeaponType.Melee,
			RangeType = WeaponRangeType.Long,
			PushForce = 1,
			CriticalChances = 0.01f,
			CanCounterGhost = false
		};
		_configs["weapon07"] = new WeaponConfig
		{
			Id = "weapon07",
			Title = "HATCHET",
			WorldId = "w02",
			DamageMin = 16,
			DamageMax = 21,
			HpMaxBase = 8,
			UpgradePriceBase = 20,
			DamageLevelRatio = 3,
			HpMaxLevelRatio = 3,
			ElementWeak = Element.B,
			ElementCritical = Element.F,
			ElementMiss = Element.None,
			WeaponType = WeaponType.Melee,
			RangeType = WeaponRangeType.Short,
			PushForce = 1,
			CriticalChances = 0.01f,
			CanCounterGhost = false
		};
		_configs["weapon08"] = new WeaponConfig
		{
			Id = "weapon08",
			Title = "MACE",
			WorldId = "w02",
			DamageMin = 8,
			DamageMax = 20,
			HpMaxBase = 16,
			UpgradePriceBase = 20,
			DamageLevelRatio = 3,
			HpMaxLevelRatio = 3,
			ElementWeak = Element.A,
			ElementCritical = Element.C,
			ElementMiss = Element.None,
			WeaponType = WeaponType.Melee,
			RangeType = WeaponRangeType.Medium,
			PushForce = 2,
			CriticalChances = 0.01f,
			CanCounterGhost = false
		};
		_configs["weapon09"] = new WeaponConfig
		{
			Id = "weapon09",
			Title = "KUNAI",
			WorldId = "w02",
			DamageMin = 16,
			DamageMax = 19,
			HpMaxBase = 15,
			UpgradePriceBase = 20,
			DamageLevelRatio = 3,
			HpMaxLevelRatio = 3,
			ElementWeak = Element.C,
			ElementCritical = Element.E,
			ElementMiss = Element.None,
			WeaponType = WeaponType.Ranged,
			RangeType = WeaponRangeType.Long,
			PushForce = 1,
			CriticalChances = 0.1f,
			CanCounterGhost = false
		};
	}

	public WeaponConfig GetConfig(string id)
	{
 WeaponConfig value;		if (_configs.TryGetValue(id, out value))
		{
			return value;
		}
		return null;
	}

	public List<WeaponConfig> GetConfigs()
	{
		return new List<WeaponConfig>(_configs.Values);
	}

	public WeaponConfig GetDefaultConfig()
	{
		return _configs["weapon01"];
	}

	public WeaponConfig GetDefaultConfigForRange(WeaponRangeType range)
	{
		switch (range)
		{
		case WeaponRangeType.Short:
			return GetConfig("weapon01");
		case WeaponRangeType.Medium:
			return GetConfig("weapon02");
		case WeaponRangeType.Long:
			return GetConfig("weapon03");
		default:
			return GetDefaultConfig();
		}
	}

	public List<WeaponConfig> GetConfigs(WeaponRangeType rangeType)
	{
		List<WeaponConfig> configs = GetConfigs();
		configs.RemoveAll((WeaponConfig w) => w.RangeType != rangeType);
		return configs;
	}

	public override void LoadFromCSV(CSVFile file)
	{
		for (int i = 0; i < file.EntriesCount; i++)
		{
			string @string = file.GetString(i, "Id");
			if (_configs.ContainsKey(@string))
			{
				WeaponConfig weaponConfig = _configs[@string];
				weaponConfig.Title = file.GetString(i, "Title");
				weaponConfig.HpMaxBase = file.GetInt(i, "HpMaxBase");
				weaponConfig.DamageMin = file.GetInt(i, "DamageMin");
				weaponConfig.DamageMax = file.GetInt(i, "DamageMax");
				weaponConfig.UpgradePriceBase = file.GetInt(i, "UpgradePriceBase");
				weaponConfig.HpMaxLevelRatio = file.GetInt(i, "HpMaxLevelRatio");
				weaponConfig.DamageLevelRatio = file.GetInt(i, "DamageLevelRatio");
				weaponConfig.ElementWeak = Enum.TryParse(file.GetString(i, "ElementWeak"), Element.None);
				weaponConfig.ElementCritical = Enum.TryParse(file.GetString(i, "ElementCritical"), Element.None);
				weaponConfig.ElementMiss = Enum.TryParse(file.GetString(i, "ElementMiss"), Element.None);
				weaponConfig.WeaponType = Enum.TryParse(file.GetString(i, "WeaponType"), WeaponType.Melee);
				weaponConfig.RangeType = Enum.TryParse(file.GetString(i, "RangeType"), WeaponRangeType.Short);
				weaponConfig.PushForce = file.GetInt(i, "PushForce");
				weaponConfig.CriticalChances = file.GetFloat(i, "CriticalChances");
			}
			else
			{
				UnityEngine.Debug.LogWarning("[" + ConfigType + "] failed to overwrite " + @string);
			}
		}
	}
}