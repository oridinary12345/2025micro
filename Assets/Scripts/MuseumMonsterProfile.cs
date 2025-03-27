using Newtonsoft.Json;
using System;

[Serializable]
public class MuseumMonsterProfile
{
	[JsonProperty(PropertyName = "mId", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public string MonsterId;

	[JsonProperty(PropertyName = "mCC", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public int MonsterCollectedCount;

	[JsonProperty(PropertyName = "tt", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public DateTimeJson LastTickTime = new DateTimeJson();

	[JsonProperty(PropertyName = "wt", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public DateTimeJson LastWakeUpTime = new DateTimeJson();
}