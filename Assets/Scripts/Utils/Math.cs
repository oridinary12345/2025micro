using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Utils
{
	public static class Math
	{
		public static double ROUND(double value, int decimals)
		{
			if (decimals < 0)
			{
				float num = Mathf.Pow(10f, -decimals);
				return ROUND(value / (double)num, 0) * (double)num;
			}
			return System.Math.Round(value, decimals, MidpointRounding.AwayFromZero);
		}

		public static string GenerateGUID(int length)
		{
			char[] source = new StringBuilder().Insert(0, "0123456789abcdefghkmnpqrstvwxyz", length).ToString().ToCharArray();
			IEnumerable<char> enumerable = (from o in source
				orderby Guid.NewGuid()
				select o).Take(length);
			string text = string.Empty;
			foreach (char item in enumerable)
			{
				text += item;
			}
			return text;
		}
	}
}