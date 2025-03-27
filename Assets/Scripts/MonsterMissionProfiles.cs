using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
public class MonsterMissionProfiles
{
	[JsonProperty(PropertyName = "mw", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public List<MonsterMissionWorldProfile> Worlds = new List<MonsterMissionWorldProfile>();

	[JsonProperty(PropertyName = "wt", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public DateTimeJson LastReset = new DateTimeJson();
}