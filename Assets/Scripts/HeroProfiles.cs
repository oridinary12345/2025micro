using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
public class HeroProfiles
{
	public List<HeroProfile> Heroes = new List<HeroProfile>();

	[JsonProperty(PropertyName = "curr", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public string CurrentHeroId;

	[JsonProperty(PropertyName = "eq", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public List<string> EquippedWeaponIds = new List<string>();

	[JsonProperty(PropertyName = "free", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public int FreeHealAvailable;
}