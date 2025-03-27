using Newtonsoft.Json;
using System;

[Serializable]
public class FreeRubiesProfile
{
	[JsonProperty(PropertyName = "c", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public bool Consumed;

	[JsonProperty(PropertyName = "cd", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public DateTimeJson ConsumedDate = new DateTimeJson();
}