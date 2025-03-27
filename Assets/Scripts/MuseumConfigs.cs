using System.Collections.Generic;
using UnityEngine;

public class MuseumConfigs : Configs<MuseumConfigs>
{
	private readonly Dictionary<string, List<MuseumConfig>> _configs = new Dictionary<string, List<MuseumConfig>>();

	public override ConfigType ConfigType => ConfigType.Museum;

	protected override void Init()
	{
		base.Init();
		string text = "w00";
		List<MuseumConfig> list = new List<MuseumConfig>();
		_configs[text] = list;
		int paymentBaseAmount = 4;
		float paymentAmountLevelUpPow = 1.25f;
		int awakeTimeMin = 4;
		int awakeTimeMinLevelUpBonus = 5;
		list.Add(new MuseumConfig(paymentBaseAmount, paymentAmountLevelUpPow, awakeTimeMin, awakeTimeMinLevelUpBonus)
		{
			Id = "mushroom",
			WorldId = text,
			PaymentDelaySec = 15,
			KillAmountNeededBase = 15
		});
		paymentBaseAmount = 4;
		paymentAmountLevelUpPow = 1.25f;
		awakeTimeMin = 5;
		awakeTimeMinLevelUpBonus = 5;
		list.Add(new MuseumConfig(paymentBaseAmount, paymentAmountLevelUpPow, awakeTimeMin, awakeTimeMinLevelUpBonus)
		{
			Id = "woodlog",
			WorldId = text,
			PaymentDelaySec = 15,
			KillAmountNeededBase = 15
		});
		awakeTimeMin = 15;
		list.Add(new MuseumConfig(paymentBaseAmount, paymentAmountLevelUpPow, awakeTimeMin, awakeTimeMinLevelUpBonus)
		{
			Id = "acorn",
			WorldId = text,
			PaymentDelaySec = 15,
			KillAmountNeededBase = 15
		});
		paymentBaseAmount = 6;
		paymentAmountLevelUpPow = 1.55f;
		awakeTimeMin = 90;
		awakeTimeMinLevelUpBonus = 30;
		list.Add(new MuseumConfig(paymentBaseAmount, paymentAmountLevelUpPow, awakeTimeMin, awakeTimeMinLevelUpBonus)
		{
			Id = "strawberry",
			WorldId = text,
			PaymentDelaySec = 15,
			KillAmountNeededBase = 15
		});
		paymentBaseAmount = 8;
		paymentAmountLevelUpPow = 1.8f;
		awakeTimeMin = 180;
		awakeTimeMinLevelUpBonus = 60;
		list.Add(new MuseumConfig(paymentBaseAmount, paymentAmountLevelUpPow, awakeTimeMin, awakeTimeMinLevelUpBonus)
		{
			Id = "dragon",
			WorldId = text,
			PaymentDelaySec = 15,
			KillAmountNeededBase = 15
		});
		text = "w01";
		list = new List<MuseumConfig>();
		_configs[text] = list;
		paymentBaseAmount = 5;
		paymentAmountLevelUpPow = 1.435f;
		awakeTimeMin = 18;
		awakeTimeMinLevelUpBonus = 10;
		list.Add(new MuseumConfig(paymentBaseAmount, paymentAmountLevelUpPow, awakeTimeMin, awakeTimeMinLevelUpBonus)
		{
			Id = "skeleton",
			WorldId = text,
			PaymentDelaySec = 15,
			KillAmountNeededBase = 15
		});
		paymentBaseAmount = 5;
		paymentAmountLevelUpPow = 1.435f;
		awakeTimeMin = 9;
		awakeTimeMinLevelUpBonus = 10;
		list.Add(new MuseumConfig(paymentBaseAmount, paymentAmountLevelUpPow, awakeTimeMin, awakeTimeMinLevelUpBonus)
		{
			Id = "snake",
			WorldId = text,
			PaymentDelaySec = 15,
			KillAmountNeededBase = 15
		});
		paymentBaseAmount = 5;
		paymentAmountLevelUpPow = 1.435f;
		awakeTimeMin = 5;
		awakeTimeMinLevelUpBonus = 10;
		list.Add(new MuseumConfig(paymentBaseAmount, paymentAmountLevelUpPow, awakeTimeMin, awakeTimeMinLevelUpBonus)
		{
			Id = "spider",
			WorldId = text,
			PaymentDelaySec = 15,
			KillAmountNeededBase = 15
		});
		paymentBaseAmount = 7;
		paymentAmountLevelUpPow = 1.685f;
		awakeTimeMin = 85;
		awakeTimeMinLevelUpBonus = 30;
		list.Add(new MuseumConfig(paymentBaseAmount, paymentAmountLevelUpPow, awakeTimeMin, awakeTimeMinLevelUpBonus)
		{
			Id = "ghost",
			WorldId = text,
			PaymentDelaySec = 15,
			KillAmountNeededBase = 15
		});
		paymentBaseAmount = 10;
		paymentAmountLevelUpPow = 1.862f;
		awakeTimeMin = 210;
		awakeTimeMinLevelUpBonus = 60;
		list.Add(new MuseumConfig(paymentBaseAmount, paymentAmountLevelUpPow, awakeTimeMin, awakeTimeMinLevelUpBonus)
		{
			Id = "eye",
			WorldId = text,
			PaymentDelaySec = 15,
			KillAmountNeededBase = 15
		});
		text = "w02";
		list = new List<MuseumConfig>();
		_configs[text] = list;
		paymentBaseAmount = 6;
		paymentAmountLevelUpPow = 1.6f;
		awakeTimeMin = 25;
		awakeTimeMinLevelUpBonus = 10;
		list.Add(new MuseumConfig(paymentBaseAmount, paymentAmountLevelUpPow, awakeTimeMin, awakeTimeMinLevelUpBonus)
		{
			Id = "fishman",
			WorldId = text,
			PaymentDelaySec = 15,
			KillAmountNeededBase = 15
		});
		paymentBaseAmount = 6;
		paymentAmountLevelUpPow = 1.6f;
		awakeTimeMin = 8;
		awakeTimeMinLevelUpBonus = 10;
		list.Add(new MuseumConfig(paymentBaseAmount, paymentAmountLevelUpPow, awakeTimeMin, awakeTimeMinLevelUpBonus)
		{
			Id = "leech",
			WorldId = text,
			PaymentDelaySec = 15,
			KillAmountNeededBase = 15
		});
		paymentBaseAmount = 6;
		paymentAmountLevelUpPow = 1.6f;
		awakeTimeMin = 12;
		awakeTimeMinLevelUpBonus = 10;
		list.Add(new MuseumConfig(paymentBaseAmount, paymentAmountLevelUpPow, awakeTimeMin, awakeTimeMinLevelUpBonus)
		{
			Id = "mushroomToxic",
			WorldId = text,
			PaymentDelaySec = 15,
			KillAmountNeededBase = 15
		});
		paymentBaseAmount = 8;
		paymentAmountLevelUpPow = 1.8f;
		awakeTimeMin = 80;
		awakeTimeMinLevelUpBonus = 30;
		list.Add(new MuseumConfig(paymentBaseAmount, paymentAmountLevelUpPow, awakeTimeMin, awakeTimeMinLevelUpBonus)
		{
			Id = "salamander",
			WorldId = text,
			PaymentDelaySec = 15,
			KillAmountNeededBase = 15
		});
		paymentBaseAmount = 11;
		paymentAmountLevelUpPow = 1.96f;
		awakeTimeMin = 240;
		awakeTimeMinLevelUpBonus = 60;
		list.Add(new MuseumConfig(paymentBaseAmount, paymentAmountLevelUpPow, awakeTimeMin, awakeTimeMinLevelUpBonus)
		{
			Id = "frog",
			WorldId = text,
			PaymentDelaySec = 15,
			KillAmountNeededBase = 15
		});
	}

	public Dictionary<string, List<MuseumConfig>> GetConfigs()
	{
		return _configs;
	}

	public List<MuseumConfig> GetConfigByWorldId(string worldId)
	{
		if (string.IsNullOrEmpty(worldId) || !_configs.ContainsKey(worldId))
		{
			return new List<MuseumConfig>();
		}
		return _configs[worldId];
	}

	public override void LoadFromCSV(CSVFile file)
	{
		for (int i = 0; i < file.EntriesCount; i++)
		{
			string @string = file.GetString(i, "Id");
			if (_configs.ContainsKey(@string))
			{
				List<MuseumConfig> list = _configs[@string];
			}
			else
			{
				UnityEngine.Debug.LogWarning("[" + ConfigType + "] failed to overwrite " + @string);
			}
		}
	}
}