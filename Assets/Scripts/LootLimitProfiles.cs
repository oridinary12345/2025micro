using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
public class LootLimitProfiles
{
	[JsonProperty(PropertyName = "w", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public List<LootLimitProfile> Loots = new List<LootLimitProfile>();
}