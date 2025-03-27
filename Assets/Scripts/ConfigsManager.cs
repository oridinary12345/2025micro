using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ConfigsManager : MonoSingleton<ConfigsManager>
{
	private bool _error;

	private readonly ConfigsData _configData = new ConfigsData();

	protected override void Init()
	{
	}

	public void DownloadConfigsVersion(Action<int> onVersionDownloaded, Action<string> onError)
	{
		Action<ConfigType, string> onConfigDownloaded = delegate(ConfigType c, string text)
		{
			onVersionDownloaded(ParseVersion(text));
		};
		DownloadConfig(ConfigType.Version, onConfigDownloaded, delegate(ConfigType type, string error)
		{
			onError(error);
		});
	}

	private int ParseVersion(string data)
	{
		int result = 0;
		string[] array = data.Split("\n"[0]);
		if (array != null && array.Length > 0)
		{
			string[] array2 = array[0].Split("\t"[0]);
			if (array2 != null && array2.Length > 0)
			{
				int.TryParse(array2[0], out result);
				return result;
			}
		}
		return 0;
	}

	public void DownloadConfigs(Action<ConfigsData> onConfigAllUpdated, Action<string> onError)
	{
		StartCoroutine(DownloadConfigsCR(onConfigAllUpdated, onError));
	}

	public void DownloadConfig(ConfigType config, Action<ConfigType, string> onConfigDownloaded, Action<ConfigType, string> onError)
	{
		StartCoroutine(DownloadConfigCR(config, onConfigDownloaded, onError));
	}

	private IEnumerator DownloadConfigsCR(Action<ConfigsData> onConfigAllUpdated, Action<string> onErrorContinue)
	{
		Action<ConfigType, string> onConfigDownloaded = delegate(ConfigType configType, string text)
		{
			if (!_error)
			{
				if (configType == ConfigType.invalid)
				{
					UnityEngine.Debug.LogWarning("ConfigType is invalid");
				}
				_configData.SetData(configType, text);
				if (configType == ConfigType.Version)
				{
					_configData.Version = ParseVersion(text);
				}
				if (AreDownloadFinished())
				{
					UnityEngine.Debug.Log("ALL downloaded from web..");
					onConfigAllUpdated(_configData);
				}
			}
		};
		Action<ConfigType, string> onError = delegate(ConfigType configType, string err)
		{
			UnityEngine.Debug.LogWarning(configType + " download failed. " + err);
			_error = true;
			onErrorContinue(err);
		};
		IEnumerator enumerator = Enum.GetValues(typeof(ConfigType)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ConfigType configType2 = (ConfigType)enumerator.Current;
				ConfigType configCopy = configType2;
				if (configType2 != ConfigType.invalid)
				{
					StartCoroutine(DownloadConfigCR(configCopy, onConfigDownloaded, onError));
				}
				yield return null;
			}
		}
		finally
		{
			IDisposable disposable;
			IDisposable disposable2 = disposable = (enumerator as IDisposable);
			if (disposable != null)
			{
				disposable2.Dispose();
			}
		}
	}

	private IEnumerator DownloadConfigCR(ConfigType configType, Action<ConfigType, string> onConfigDownloaded, Action<ConfigType, string> onError)
	{
		if (configType != ConfigType.invalid)
		{
			string configUrl = ConfigProperties.GetUrl(configType);
			if (!string.IsNullOrEmpty(configUrl))
			{
				WWW www = new WWW(configUrl);
				try
				{
					while (!www.isDone)
					{
						yield return null;
					}
					if (!string.IsNullOrEmpty(www.error))
					{
						onError(configType, www.error);
					}
					else
					{
						onConfigDownloaded(configType, www.text);
					}
				}
				finally
				{
				}
			}
		}
	}

	private bool AreDownloadFinished()
	{
		foreach (ConfigType key in ConfigProperties._configIds.Keys)
		{
			ConfigType configType = key;
			if (configType != ConfigType.invalid && configType != 0 && !_configData.Data.ContainsKey(configType))
			{
				return false;
			}
		}
		return _configData.Version > 0;
	}

	public ConfigsData LoadFromDiskCached()
	{
		string path = Application.persistentDataPath + "/Cache/master.bytes";
		if (!File.Exists(path))
		{
			return null;
		}
		FileStream fileStream = File.OpenRead(path);
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		ConfigsData result = binaryFormatter.Deserialize(fileStream) as ConfigsData;
		fileStream.Close();
		return result;
	}

	public void SaveToDiskCached(ConfigsData configsData)
	{
		string text = Application.persistentDataPath + "/Cache";
		string path = text + "/master.bytes";
		Directory.CreateDirectory(text);
		FileStream fileStream = (!File.Exists(path)) ? File.Create(path) : File.OpenWrite(path);
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		binaryFormatter.Serialize(fileStream, configsData);
		fileStream.Close();
	}

	public ConfigsData LoadFromDiskBuiltIn()
	{
		string text = "Configs/" + Path.GetFileNameWithoutExtension("master.bytes");
		UnityEngine.Debug.Log("Loading built-in config at " + text);
		TextAsset textAsset = Resources.Load<TextAsset>(text);
		if (textAsset == null)
		{
			UnityEngine.Debug.LogWarning("Built-in file not found");
			return null;
		}
		MemoryStream serializationStream = new MemoryStream(textAsset.bytes);
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		return binaryFormatter.Deserialize(serializationStream) as ConfigsData;
	}
}