using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
public class MuseumProfiles
{
	[JsonProperty(PropertyName = "mu", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public List<MuseumProfile> Museums = new List<MuseumProfile>();
}