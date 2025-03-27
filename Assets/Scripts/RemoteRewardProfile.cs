using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
public class RemoteRewardProfile
{
	[JsonProperty(PropertyName = "codes", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public List<string> RedeemedCode = new List<string>();
}