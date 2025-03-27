using System;
using System.Collections.Generic;
using UnityEngine;

public class AppConfigs : Configs<AppConfigs>
{
	private readonly Dictionary<string, string> _configs = new Dictionary<string, string>();

	public override ConfigType ConfigType => ConfigType.App;

	protected override void Init()
	{
		base.Init();
		_configs["UsePlayfab"] = "TRUE";
	}

	public T GetValue<T>(string key, T defaultValue)
	{
		if (_configs.ContainsKey(key))
		{
			try
			{
				return (T)Convert.ChangeType(_configs[key], typeof(T));
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogWarning("AppConfigs failed to convert " + key + ", error: " + ex);
				return defaultValue;
			}
		}
		UnityEngine.Debug.LogWarning("AppConfigs can't find key: " + key);
		return defaultValue;
	}

	public override void LoadFromCSV(CSVFile file)
	{
		for (int i = 0; i < file.EntriesCount; i++)
		{
			string @string = file.GetString(i, "Key");
			if (_configs.ContainsKey(@string))
			{
				_configs[@string] = file.GetString(i, "Value");
			}
			else
			{
				UnityEngine.Debug.LogWarning("[" + ConfigType + "] failed to overwrite " + @string);
			}
		}
	}
}