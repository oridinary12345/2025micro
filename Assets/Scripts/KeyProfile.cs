using Newtonsoft.Json;
using System;

[Serializable]
public class KeyProfile
{
	[JsonProperty(PropertyName = "sd", DefaultValueHandling = DefaultValueHandling.Ignore)]
	public DateTimeJson NextKeyStartDate = new DateTimeJson();
}