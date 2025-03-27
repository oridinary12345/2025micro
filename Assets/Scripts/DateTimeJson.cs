using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Runtime.Serialization;

[Serializable]
public class DateTimeJson
{
	[JsonProperty("ss")]
	private string _time;

	[JsonIgnore]
	public DateTime Time;

	[OnSerializing]
	internal void OnBeforeSerialize(StreamingContext context)
	{
		_time = Time.ToString("o");
	}

	[OnDeserialized]
	internal void OnAfterDeserialize(StreamingContext context)
	{
 DateTime result;		if (DateTime.TryParse(_time, null, DateTimeStyles.RoundtripKind, out result))
		{
			Time = result;
		}
	}

	public override string ToString()
	{
		return Time.ToString();
	}
}