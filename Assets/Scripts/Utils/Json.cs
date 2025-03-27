using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
	public class Json
	{
		public static string GetString(Hashtable data, string key, string defaultValue = "")
		{
			string result = defaultValue;
			if (data != null && data.ContainsKey(key) && data[key] is string)
			{
				result = (data[key] as string);
			}
			return result;
		}

		public static float GetFloat(Hashtable data, string key, float defaultValue = 0f)
		{
			float result = defaultValue;
			if (data != null && data.ContainsKey(key))
			{
				if (data[key] is float)
				{
					result = (float)data[key];
				}
				else if (data[key] is double)
				{
					result = (float)(double)data[key];
				}
				else
				{
					UnityEngine.Debug.LogWarning("Json.GetFloat() unexpected type.");
				}
			}
			return result;
		}

		public static long GetLong(Hashtable data, string key, long defaultValue = 0L)
		{
			long result = defaultValue;
			if (data != null && data.ContainsKey(key))
			{
				if (data[key] is float)
				{
					result = (long)data[key];
				}
				else if (data[key] is double)
				{
					result = (long)(double)data[key];
				}
				else
				{
					UnityEngine.Debug.LogWarning("Json.GetLong() unexpected type.");
				}
			}
			return result;
		}

		public static int GetInt(Hashtable data, string key, int defaultValue = 0)
		{
			return Mathf.RoundToInt(GetFloat(data, key, defaultValue));
		}

		public static bool GetBool(Hashtable data, string key, bool defaultValue = false)
		{
			bool result = defaultValue;
			if (data != null && data.ContainsKey(key) && data[key] is bool)
			{
				result = (bool)data[key];
			}
			return result;
		}

		public static Hashtable GetHashtable(Hashtable data, string key, Hashtable def = null)
		{
			if (data != null && data.ContainsKey(key) && data[key] is Hashtable)
			{
				return data[key] as Hashtable;
			}
			return def;
		}

		public static ArrayList GetArrayList(Hashtable data, string key, bool logWarning = true)
		{
			ArrayList result = new ArrayList();
			if (data != null)
			{
				if (data.ContainsKey(key))
				{
					if (data[key] is ArrayList)
					{
						ArrayList arrayList = data[key] as ArrayList;
						if (logWarning && (arrayList == null || arrayList.Count > 0))
						{
							UnityEngine.Debug.LogWarning("JSON: GetArrayList is EMPTY");
						}
						return arrayList;
					}
				}
				else if (logWarning)
				{
					UnityEngine.Debug.LogWarning("JSON: GetArrayList can't find: " + key);
				}
			}
			return result;
		}

		public static List<float> GetFloatList(Hashtable data, string key)
		{
			ArrayList arrayList = GetArrayList(data, key);
			List<float> list = new List<float>();
			if (arrayList != null)
			{
				IEnumerator enumerator = arrayList.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object current = enumerator.Current;
						if (current is float)
						{
							float item = (float)current;
							list.Add(item);
						}
						else if (current is double)
						{
							float item2 = (float)(double)current;
							list.Add(item2);
						}
					}
					return list;
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
			}
			return list;
		}
	}
}