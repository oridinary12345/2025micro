using PlayFab.Internal;
using System;
using System.Globalization;

namespace PlayFab.Json
{
	public class SimpleJsonInstance : ISerializer
	{
		public class PlayFabSimpleJsonCuztomization : PocoJsonSerializerStrategy
		{
			public override object DeserializeObject(object value, Type type)
			{
				string text = value as string;
				if (text == null)
				{
					return base.DeserializeObject(value, type);
				}
				Type underlyingType = Nullable.GetUnderlyingType(type);
				if (underlyingType != null)
				{
					return DeserializeObject(value, underlyingType);
				}
				if (type.GetTypeInfo().IsEnum)
				{
					return Enum.Parse(type, (string)value, true);
				}
				double result3;
				if (type == typeof(DateTime))
				{
 DateTime result;					if (DateTime.TryParseExact(text, PlayFabUtil._defaultDateTimeFormats, CultureInfo.CurrentCulture, PlayFabUtil.DateTimeStyles, out result))
					{
						return result;
					}
				}
				else if (type == typeof(DateTimeOffset))
				{
 DateTimeOffset result2;					if (DateTimeOffset.TryParseExact(text, PlayFabUtil._defaultDateTimeFormats, CultureInfo.CurrentCulture, PlayFabUtil.DateTimeStyles, out result2))
					{
						return result2;
					}
				}
				else if (type == typeof(TimeSpan) && double.TryParse(text, out result3))
				{
					return TimeSpan.FromSeconds(result3);
				}
				return base.DeserializeObject(value, type);
			}

			protected override bool TrySerializeKnownTypes(object input, out object output)
			{
				if (input.GetType().GetTypeInfo().IsEnum)
				{
					output = input.ToString();
					return true;
				}
				if (input is DateTime)
				{
					output = ((DateTime)input).ToString(PlayFabUtil._defaultDateTimeFormats[2], CultureInfo.CurrentCulture);
					return true;
				}
				if (input is DateTimeOffset)
				{
					output = ((DateTimeOffset)input).ToString(PlayFabUtil._defaultDateTimeFormats[2], CultureInfo.CurrentCulture);
					return true;
				}
				if (input is TimeSpan)
				{
					output = ((TimeSpan)input).TotalSeconds;
					return true;
				}
				return base.TrySerializeKnownTypes(input, out output);
			}
		}

		public static PlayFabSimpleJsonCuztomization ApiSerializerStrategy = new PlayFabSimpleJsonCuztomization();

		public T DeserializeObject<T>(string json)
		{
			return PlayFabSimpleJson.DeserializeObject<T>(json, ApiSerializerStrategy);
		}

		public T DeserializeObject<T>(string json, object jsonSerializerStrategy)
		{
			return PlayFabSimpleJson.DeserializeObject<T>(json, (IJsonSerializerStrategy)jsonSerializerStrategy);
		}

		public object DeserializeObject(string json)
		{
			return PlayFabSimpleJson.DeserializeObject(json, typeof(object), ApiSerializerStrategy);
		}

		public string SerializeObject(object json)
		{
			return PlayFabSimpleJson.SerializeObject(json, ApiSerializerStrategy);
		}

		public string SerializeObject(object json, object jsonSerializerStrategy)
		{
			return PlayFabSimpleJson.SerializeObject(json, (IJsonSerializerStrategy)jsonSerializerStrategy);
		}
	}
}