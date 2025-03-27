using Newtonsoft.Json;
using System;

[Serializable]
public class LootLimitProfile
{
	[JsonProperty(PropertyName = "id", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public string Id;

	[JsonProperty(PropertyName = "a", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public int Amount;

	[JsonProperty(PropertyName = "t", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public DateTimeJson TimeStart = new DateTimeJson();
}