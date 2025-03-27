using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
public class WorldProfile
{
	[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public string Id;

	[JsonProperty(PropertyName = "mi", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public List<MissionProfile> Missions = new List<MissionProfile>();

	[JsonProperty(PropertyName = "unIndex", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public int LastUnlockedLevelIndex;

	[JsonProperty(PropertyName = "comIndex", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public int LastCompletedLevelIndex = -1;

	[JsonProperty(PropertyName = "gp", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public int GamePlayed;

	[JsonProperty(PropertyName = "egcs", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public int EndGameChestSpawned;

	[JsonProperty(PropertyName = "lgl", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public bool LastGameLost;
}