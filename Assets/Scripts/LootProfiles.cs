using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
public class LootProfiles
{
	[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public Dictionary<string, LootProfile> Loots = new Dictionary<string, LootProfile>();
}