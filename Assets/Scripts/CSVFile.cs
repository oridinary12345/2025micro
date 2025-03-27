using System.Collections.Generic;
using UnityEngine;

public class CSVFile
{
	private Dictionary<string, int> _headersIndex;

	private string[][] _data;

	public string Version
	{
		get;
		private set;
	}

	public bool IsValid => !string.IsNullOrEmpty(Version);

	public int EntriesCount => _data.Length;

	public CSVFile(TextAsset file)
	{
		Parse(file.text);
	}

	public CSVFile(string data)
	{
		Parse(data);
	}

	private void Parse(string data)
	{
		if (string.IsNullOrEmpty(data))
		{
			UnityEngine.Debug.LogWarning("CSVFile Parse failed. data is null or empty");
			return;
		}
		string[] array = data.Split("\n"[0]);
		int num = array.Length - 1;
		_data = new string[num][];
		_headersIndex = new Dictionary<string, int>();
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split("\t"[0]);
			Version = "undefined";
			if (i == 0)
			{
				for (int j = 0; j < array2.Length; j++)
				{
					_headersIndex[array2[j].Replace("\r", string.Empty)] = j;
				}
			}
			else
			{
				_data[i - 1] = array2;
			}
		}
	}

	public int GetInt(int line, string key, int defaultValue = 0)
	{
		return Mathf.RoundToInt(GetFloat(line, key, defaultValue));
	}

	public float GetFloat(int line, string key, float defaultValue = 0f)
	{
		float result = defaultValue;
		if (_headersIndex.ContainsKey(key))
		{
			int num = _headersIndex[key];
			string text = _data[line][num];
			if (!float.TryParse(text, out result))
			{
 double result2;				if (double.TryParse(text, out result2))
				{
					result = (float)result2;
				}
				else if (text == null)
				{
					UnityEngine.Debug.LogWarning("CSVReader.GetFloat() unexpected type. Type found is " + text);
				}
			}
		}
		else
		{
			UnityEngine.Debug.LogWarning("CSVReader.GetFloat() can't find header : " + key);
		}
		return result;
	}

	public string GetString(int line, string key, string defaultValue = null)
	{
		if (_headersIndex.ContainsKey(key))
		{
			int num = _headersIndex[key];
			string text = _data[line][num];
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
		}
		else
		{
			UnityEngine.Debug.LogWarning("CSVReader.GetString() can't find header : " + key);
		}
		return defaultValue;
	}

	public bool GetBool(int line, string key, bool defaultValue = false)
	{
		bool result = defaultValue;
		if (_headersIndex.ContainsKey(key))
		{
			int num = _headersIndex[key];
			string value = _data[line][num];
			if (!bool.TryParse(value, out result))
			{
				UnityEngine.Debug.LogWarning("CSVReader.GetBool() unexpected type.");
			}
		}
		else
		{
			UnityEngine.Debug.LogWarning("CSVReader.GetBool() can't find header : " + key);
		}
		return result;
	}

	public List<string> GetListString(int line, string key)
	{
		List<string> list = new List<string>();
		if (_headersIndex.ContainsKey(key))
		{
			int num = _headersIndex[key];
			string text = _data[line][num];
			if (!string.IsNullOrEmpty(text))
			{
				string[] array = text.Split(',');
				for (int i = 0; i < array.Length; i++)
				{
					if (!string.IsNullOrEmpty(array[i]))
					{
						list.Add(array[i]);
					}
				}
			}
		}
		else
		{
			UnityEngine.Debug.LogWarning("CSVReader.GetListString() can't find header : " + key);
		}
		return list;
	}
}