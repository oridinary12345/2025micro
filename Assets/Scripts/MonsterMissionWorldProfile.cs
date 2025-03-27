using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
public class MonsterMissionWorldProfile
{
	[JsonProperty(PropertyName = "wId", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public string WorldId;

	[JsonProperty(PropertyName = "ms", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public List<MonsterMissionProfile> Monsters = new List<MonsterMissionProfile>();
}