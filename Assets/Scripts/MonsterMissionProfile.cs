using Newtonsoft.Json;
using System;

[Serializable]
public class MonsterMissionProfile
{
	[JsonProperty(PropertyName = "mId", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public string MonsterId;

	[JsonProperty(PropertyName = "mCC", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public int MissionsCompletedCount;

	[JsonProperty(PropertyName = "mi", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public MissionProfile CurrentMission;

	[JsonProperty(PropertyName = "crm", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public bool CanReplaceMission;
}