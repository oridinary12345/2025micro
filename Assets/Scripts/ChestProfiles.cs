using Newtonsoft.Json;
using System;

[Serializable]
public class ChestProfiles
{
	[JsonProperty(PropertyName = "free", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public ChestProfile ChestFree;

	[JsonProperty(PropertyName = "ads", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public ChestProfile ChestAds;
}