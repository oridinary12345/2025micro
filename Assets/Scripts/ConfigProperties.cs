using System.Collections.Generic;

public static class ConfigProperties
{
	private static string _allConfigsUrl = "https://docs.google.com/spreadsheets/d/1ZiOTfsCXpoN_yCAhBDetre2mFFfRs9fmvtbYrVfhnOk/export?format=tsv";

	private static string _stringConfigsUrl = "https://docs.google.com/spreadsheets/d/1lEF1ITG2exTdH9Z85LZ_D5FD5cErgg0EfU_FA2NJmwY/export?format=tsv";

	private const string VersionPageId = "580132204";

	public const string MasterFileName = "master.bytes";

	public static readonly Dictionary<ConfigType, string> _configIds = new Dictionary<ConfigType, string>
	{
		{
			ConfigType.Version,
			"580132204"
		},
		{
			ConfigType.Monster,
			"0"
		},
		{
			ConfigType.Hero,
			"2084682311"
		},
		{
			ConfigType.Weapon,
			"1044919361"
		},
		{
			ConfigType.Loot,
			"6902872"
		},
		{
			ConfigType.Reward,
			"1492128481"
		},
		{
			ConfigType.App,
			"1182061305"
		},
		{
			ConfigType.Mission,
			"151267315"
		}
	};

	public static string GetUrl(ConfigType type)
	{
		if (!_configIds.ContainsKey(type))
		{
			return string.Empty;
		}
		return _allConfigsUrl + "&gid=" + _configIds[type];
	}

	public static string GetStringUrl()
	{
		return _stringConfigsUrl + "&gid=0";
	}
}