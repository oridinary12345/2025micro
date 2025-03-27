using System.Collections.Generic;
using UnityEngine;

public class HeroConfigs : Configs<HeroConfigs>
{
	private readonly Dictionary<string, HeroConfig> _configs = new Dictionary<string, HeroConfig>();

	public override ConfigType ConfigType => ConfigType.Hero;

	protected override void Init()
	{
		base.Init();
		_configs["hero00"] = new HeroConfig
		{
			Id = "hero00",
			Name = "THEOBALD",
			HpMaxBase = 60,
			HpLevelUpPriceVariant = 100,
			MissRate = 0.08f
		};
	}

	public HeroConfig GetConfig(string id)
	{
 HeroConfig value;		if (_configs.TryGetValue(id, out value))
		{
			return value;
		}
		return null;
	}

	public List<HeroConfig> GetConfigs()
	{
		return new List<HeroConfig>(_configs.Values);
	}

	public HeroConfig GetDefaultConfig()
	{
		return _configs["hero00"];
	}

	public override void LoadFromCSV(CSVFile file)
	{
		for (int i = 0; i < file.EntriesCount; i++)
		{
			string @string = file.GetString(i, "Id");
			if (_configs.ContainsKey(@string))
			{
				HeroConfig heroConfig = _configs[@string];
				heroConfig.HpMaxBase = file.GetInt(i, "HpMaxBase");
				heroConfig.Name = file.GetString(i, "Name");
				heroConfig.HpLevelUpPriceVariant = file.GetInt(i, "HpLevelUpPriceVariant");
				heroConfig.MissRate = file.GetFloat(i, "MissRate");
			}
			else
			{
				UnityEngine.Debug.LogWarning("[" + ConfigType + "] failed to overwrite " + @string);
			}
		}
	}
}