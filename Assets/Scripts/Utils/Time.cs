using System;
using System.Globalization;
using UnityEngine;

namespace Utils
{
	public static class Time
	{
		public static string SecondToString(float seconds)
		{
			int num = Mathf.CeilToInt(seconds) / 60;
			int num2 = Mathf.CeilToInt(seconds) % 60;
			return $"{num:0}:{num2:00}";
		}

		public static string SecondToLongString(float seconds)
		{
			float num = seconds / 60f;
			int num2 = Mathf.FloorToInt(num / 60f);
			int num3 = Mathf.FloorToInt(num);
			if (num2 == 0)
			{
				if ((float)num3 < 1f)
				{
					return $"{seconds:00}s";
				}
				seconds = Mathf.FloorToInt(seconds % 60f);
				return $"{num3}m {seconds:00}s";
			}
			num3 = Mathf.FloorToInt((float)num3 % 60f);
			if (num3 == 0)
			{
				return $"{num2}h";
			}
			return $"{num2}h {num3:00}m";
		}

		public static DateTime Parse(string dateString, DateTime defaultDate = default(DateTime))
		{
			DateTime result;
			DateTime dateTime = (!DateTime.TryParse(dateString, null, DateTimeStyles.RoundtripKind, out result)) ? defaultDate : result;
			if ((DateTime.MaxValue - dateTime).TotalSeconds <= 1.0)
			{
				return DateTime.MaxValue;
			}
			if ((dateTime - DateTime.MinValue).TotalSeconds <= 1.0)
			{
				return DateTime.MinValue;
			}
			return dateTime;
		}
	}
}