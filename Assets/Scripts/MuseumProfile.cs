using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
public class MuseumProfile
{
	[JsonProperty(PropertyName = "wId", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public string WorldId;

	[JsonProperty(PropertyName = "m", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public List<MuseumMonsterProfile> Monsters = new List<MuseumMonsterProfile>();
}