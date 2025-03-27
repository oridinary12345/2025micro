using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
public class StatsProfile
{
	[JsonProperty(PropertyName = "played", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public float TimePlayedSec;

	[JsonProperty(PropertyName = "mkill", DefaultValueHandling = DefaultValueHandling.Ignore)]
	public Dictionary<string, int> MonsterKills = new Dictionary<string, int>();
}