using System;
using UnityEngine;

public class ConfigLoader : MonoBehaviour
{
	public void LoadAll(Action<ConfigsData> onConfigAllUpdated)
	{
		ConfigsData configsDataFromDisk = LoadFromDisk();
		Action onWebError = delegate
		{
			onConfigAllUpdated(configsDataFromDisk);
		};
		Action onConfigUpdateNeeded = delegate
		{
			Action<ConfigsData> onConfigAllUpdated2 = delegate(ConfigsData dataConfig)
			{
				MonoSingleton<ConfigsManager>.Instance.SaveToDiskCached(dataConfig);
				onConfigAllUpdated(dataConfig);
			};
			LoadFromWeb(onConfigAllUpdated2, delegate
			{
				onWebError();
			});
		};
		CheckWebVersion((configsDataFromDisk != null) ? configsDataFromDisk.Version : 0, onWebError, onConfigUpdateNeeded);
	}

	private ConfigsData LoadFromDisk()
	{
		ConfigsData configsData = MonoSingleton<ConfigsManager>.Instance.LoadFromDiskBuiltIn();
		if (configsData == null)
		{
			UnityEngine.Debug.LogWarning("Major fail. Can't load built-in configurations!");
		}
		ConfigsData configsData2 = MonoSingleton<ConfigsManager>.Instance.LoadFromDiskCached();
		if (configsData2 != null && configsData != null && configsData2.Version > configsData.Version)
		{
			return configsData2;
		}
		return configsData;
	}

	private void CheckWebVersion(int currentVersion, Action onNoUpdatedNeeded, Action onConfigUpdateNeeded)
	{
		Action<int> onVersionDownloaded = delegate(int webVersion)
		{
			if (webVersion > currentVersion)
			{
				onConfigUpdateNeeded();
			}
			else
			{
				onNoUpdatedNeeded();
			}
		};
		Action<string> onError = delegate
		{
			onNoUpdatedNeeded();
		};
		MonoSingleton<ConfigsManager>.Instance.DownloadConfigsVersion(onVersionDownloaded, onError);
	}

	private void LoadFromWeb(Action<ConfigsData> onConfigAllUpdated, Action<string> onErrorContinue)
	{
		MonoSingleton<ConfigsManager>.Instance.DownloadConfigs(onConfigAllUpdated, onErrorContinue);
	}
}