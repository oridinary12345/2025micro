using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConfigsData : ISerializationCallbackReceiver
{
	[SerializeField]
	public int Version;

	[SerializeField]
	public string HeroConfigs;

	[SerializeField]
	public string WeaponConfigs;

	[SerializeField]
	public string MonsterConfigs;

	[SerializeField]
	public string LootConfigs;

	[SerializeField]
	public string RewardConfigs;

	[SerializeField]
	public string AppConfigs;

	[SerializeField]
	public string MissionConfigs;

	private Dictionary<ConfigType, string> _data = new Dictionary<ConfigType, string>();

	public Dictionary<ConfigType, string> Data => _data;

	public void SetData(ConfigType config, string value)
	{
		_data[config] = value;
		UpdateData();
	}

	private void UpdateData()
	{
		foreach (KeyValuePair<ConfigType, string> datum in _data)
		{
			switch (datum.Key)
			{
			case ConfigType.Hero:
				HeroConfigs = datum.Value;
				break;
			case ConfigType.Weapon:
				WeaponConfigs = datum.Value;
				break;
			case ConfigType.Monster:
				MonsterConfigs = datum.Value;
				break;
			case ConfigType.Loot:
				LootConfigs = datum.Value;
				break;
			case ConfigType.Reward:
				RewardConfigs = datum.Value;
				break;
			case ConfigType.App:
				AppConfigs = datum.Value;
				break;
			case ConfigType.Mission:
				MissionConfigs = datum.Value;
				break;
			default:
				UnityEngine.Debug.LogWarning("ConfigsData not handling configs for type: " + datum.Key);
				break;
			case ConfigType.Version:
				break;
			}
		}
	}

	public void OnBeforeSerialize()
	{
		UpdateData();
	}

	public void OnAfterDeserialize()
	{
	}
}