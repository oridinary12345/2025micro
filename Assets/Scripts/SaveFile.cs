using System.Collections;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class SaveFile : MonoSingleton<SaveFile>
{
	private const string KEY = "*3j15/^+cakB?zu+IeoPQLI}^TS>m<7r`D|R%Xh(O2<Y:Uu~s{-CRgx/x-,fjdoy";

	private const int _version = 1;

	private Hashtable _data;

	protected override void Init()
	{
		base.Init();
		Object.DontDestroyOnLoad(base.gameObject);
		int @int = PlayerPrefs.GetInt("version");
		string @string = PlayerPrefs.GetString("hash");
		string string2 = PlayerPrefs.GetString("data");
		string b = ComputeHash(string2);
		bool flag = @string == b || string.IsNullOrEmpty(string2);
		if (!flag)
		{
		}
		if (string.IsNullOrEmpty(string2) || @int != 1 || !flag)
		{
			_data = new Hashtable();
		}
		else
		{
			_data = string2.hashtableFromJson();
		}
		UnityEngine.Debug.Log("Loading progress: " + _data.toJson());
	}

	public void Save()
	{
		string text = _data.toJson();
		string value = ComputeHash(text);
		PlayerPrefs.SetString("data", text);
		PlayerPrefs.SetString("hash", value);
		PlayerPrefs.SetInt("version", 1);
		UnityEngine.Debug.Log("Saving progress... " + _data.toJson());
	}

	public void Reset()
	{
		PlayerPrefs.DeleteAll();
		Destroy();
	}

	public void SaveOnDisk()
	{
		PlayerPrefs.Save();
	}

	public void SetData(string key, string value)
	{
		_data[key] = value;
		Save();
	}

	public string GetData(string key, string def = "")
	{
		return (!_data.ContainsKey(key)) ? def : (_data[key] as string);
	}

	public void SaveString(string key, string value)
	{
		SetData(key, value);
	}

	public string LoadString(string key, string def)
	{
		return GetData(key, def);
	}

	public string ComputeHash(string strToEncrypt)
	{
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		byte[] bytes = uTF8Encoding.GetBytes(strToEncrypt);
		byte[] bytes2 = uTF8Encoding.GetBytes("*3j15/^+cakB?zu+IeoPQLI}^TS>m<7r`D|R%Xh(O2<Y:Uu~s{-CRgx/x-,fjdoy");
		HMACSHA1 hMACSHA = new HMACSHA1(bytes2);
		byte[] array = hMACSHA.ComputeHash(bytes);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("x2"));
		}
		return stringBuilder.ToString();
	}
}