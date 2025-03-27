using System;
using UnityEngine;

namespace Utils
{
	public class Enum
	{
		public static T TryParse<T>(string s, T defaultValue) where T : struct, IConvertible
		{
			if (!typeof(T).IsEnum)
			{
				UnityEngine.Debug.LogWarning("Invalid Enum: " + s);
				return defaultValue;
			}
			T result = defaultValue;
			try
			{
				T val = (T)System.Enum.Parse(typeof(T), s, true);
				if (!System.Enum.IsDefined(typeof(T), val))
				{
					return result;
				}
				result = val;
				return result;
			}
			catch (ArgumentException)
			{
				return result;
			}
		}
	}
}