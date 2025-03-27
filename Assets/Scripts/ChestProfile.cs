using Newtonsoft.Json;
using System;

[Serializable]
public class ChestProfile
{
	[JsonProperty(PropertyName = "id", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public string Id;

	[JsonProperty(PropertyName = "c", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public int ChestOpenedCount;

	[JsonProperty(PropertyName = "dt", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public DateTimeJson ChestStartedDate = new DateTimeJson();
}