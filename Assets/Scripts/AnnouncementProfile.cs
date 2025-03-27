using Newtonsoft.Json;
using System;

[Serializable]
public class AnnouncementProfile
{
	[JsonProperty(PropertyName = "seen", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public bool BetaWarningSeen;
}