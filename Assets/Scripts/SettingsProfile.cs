using Newtonsoft.Json;
using System;

[Serializable]
public class SettingsProfile
{
	[JsonProperty(PropertyName = "bs", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public bool BatterySavingEnabled;

	[JsonProperty(PropertyName = "mu", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public bool MusicEnabled = true;

	[JsonProperty(PropertyName = "so", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public bool SoundEnabled = true;

	[JsonProperty(PropertyName = "lang", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public string Language = string.Empty;
}