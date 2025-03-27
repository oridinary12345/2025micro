using Newtonsoft.Json;
using System;

[Serializable]
public class MissionProfile
{
	[JsonProperty(PropertyName = "id", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public string Id;

	[JsonProperty(PropertyName = "ty", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public MissionType MissionType;

	[JsonProperty(PropertyName = "pr", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public float Progress;

	[JsonProperty(PropertyName = "ob", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public float Objective;

	[JsonProperty(PropertyName = "p1", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public string Param1;

	[JsonProperty(PropertyName = "p2", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public string Param2;
}