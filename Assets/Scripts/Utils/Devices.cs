using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
	public class Devices
	{
		private static readonly HashSet<string> NotchPhoneModel = new HashSet<string>
		{
			"iPhone10,3",
			"iPhone10,6",
			"iPhone11,2",
			"iPhone11,4",
			"iPhone11,6",
			"iPhone11,8"
		};

		public static void AddNotchDevices(string[] modelNames)
		{
			foreach (string item in modelNames)
			{
				NotchPhoneModel.Add(item);
			}
		}

		public static bool HasNotch()
		{
			return NotchPhoneModel.Contains(SystemInfo.deviceModel);
		}
	}
}