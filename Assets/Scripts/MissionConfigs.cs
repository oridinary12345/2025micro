using System.Collections.Generic;
using Utils;

public class MissionConfigs : Configs<MissionConfigs>
{
	private readonly Dictionary<string, MissionConfig> _configs = new Dictionary<string, MissionConfig>();

	public override ConfigType ConfigType => ConfigType.Mission;

	protected override void Init()
	{
		base.Init();
	}

	public MissionConfig GetConfig(string id)
	{
 MissionConfig value;		if (!string.IsNullOrEmpty(id) && _configs.TryGetValue(id, out value))
		{
			return value;
		}
		return MissionConfig.Invalid;
	}

	public void AddConfig(MissionConfig config)
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
				MissionConfig missionConfig = _configs[@string];
				missionConfig.Id = file.GetString(i, "Id");
				missionConfig.MissionType = Enum.TryParse(file.GetString(i, "MissionType"), MissionType.None);
				missionConfig.Objective = file.GetFloat(i, "Objective");
				missionConfig.ParamStr1 = file.GetString(i, "ParamStr1");
				missionConfig.ParamStr2 = file.GetString(i, "ParamStr2");
				missionConfig.ParamInt1 = file.GetInt(i, "ParamInt1");
			}
		}
	}
}