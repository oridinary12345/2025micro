using Newtonsoft.Json;
using System;

[Serializable]
public class WeaponProfile
{
	[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public string Id;

	[JsonProperty(PropertyName = "lvl", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public int Level;

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public int HP;

	[JsonProperty(PropertyName = "cC", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public int CardCollectedCount;

	[JsonProperty(PropertyName = "in", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public bool IsNew;
}