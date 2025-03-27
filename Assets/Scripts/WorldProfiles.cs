using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
public class WorldProfiles
{
	[JsonProperty(PropertyName = "w", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public List<WorldProfile> Worlds = new List<WorldProfile>();

	[JsonProperty(PropertyName = "lastS", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public string LastSelectedWorldId;

	[JsonProperty(PropertyName = "lastU", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public string LastUnlockedWorldId;
}