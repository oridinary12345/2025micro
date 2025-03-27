using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChestManager
{
	private readonly ChestProfiles _profiles;

	private readonly TimeManager _timeManager;

	private Dictionary<string, ChestData> _chestDataCache = new Dictionary<string, ChestData>();

	public ChestEvents Events
	{
		get;
		private set;
	}

	public PlayerChestManager(ChestProfiles profiles, TimeManager timeManager)
	{
		_profiles = profiles;
		_timeManager = timeManager;
		Events = new ChestEvents();
		if (_profiles.ChestFree == null)
		{
			ChestConfig config = MonoSingleton<ChestConfigs>.Instance.GetConfig("chestAds");
			ChestProfile chestAds = new ChestProfile
			{
				Id = config.Id,
				ChestStartedDate = 
				{
					Time = DateTime.UtcNow
				}
			};
			_profiles.ChestAds = chestAds;
			ChestConfig config2 = MonoSingleton<ChestConfigs>.Instance.GetConfig("chestTimedFree");
			ChestProfile chestFree = new ChestProfile
			{
				Id = config2.Id,
				ChestStartedDate = 
				{
					Time = DateTime.UtcNow
				}
			};
			_profiles.ChestFree = chestFree;
		}
		_chestDataCache["chestAds"] = GetChestData("chestAds");
		_chestDataCache["chestTimedFree"] = GetChestData("chestTimedFree");
		_chestDataCache["chestPremium1"] = GetChestData("chestPremium1");
		_chestDataCache["chestPremium2"] = GetChestData("chestPremium2");
	}

	private void Validate()
	{
	}

	public bool RedeemChest(ChestData chestData)
	{
		UnityEngine.Debug.Log("attempt at redeeming " + chestData.Config.Id);
		ChestProfile chestProfile = GetChestProfile(chestData.Config.Id);
		if (chestProfile == null)
		{
			return false;
		}
		if (chestProfile.ChestOpenedCount >= chestData.Config.Max)
		{
			UnityEngine.Debug.LogWarning(chestProfile.Id + ". Chest can't be opened. Max opening count reached. Currently at " + chestProfile.ChestOpenedCount + ", while max is " + chestData.Config.Max);
			return false;
		}
		chestProfile.ChestOpenedCount++;
		UnityEngine.Debug.Log("Redeeming " + chestData.Config.Id + ", ChestOpenedCount = " + chestProfile.ChestOpenedCount);
		chestData.OnRedeemed();
		Events.OnChestRedeemed(chestData.Config.Id);
		return true;
	}

	public ChestData GetChestData(string id)
	{
		ChestConfig config = MonoSingleton<ChestConfigs>.Instance.GetConfig(id);
		ChestProfile chestProfile = GetChestProfile(id);
		if (_chestDataCache.ContainsKey(id))
		{
			return _chestDataCache[id];
		}
		return new ChestData(config, chestProfile, Events, _timeManager);
	}

	private ChestProfile GetChestProfile(string chestId)
	{
		if (_profiles.ChestAds.Id == chestId)
		{
			return _profiles.ChestAds;
		}
		if (_profiles.ChestFree.Id == chestId)
		{
			return _profiles.ChestFree;
		}
		ChestProfile chestProfile = new ChestProfile();
		chestProfile.ChestOpenedCount = 0;
		chestProfile.ChestStartedDate = new DateTimeJson
		{
			Time = DateTime.UtcNow
		};
		chestProfile.Id = chestId;
		return chestProfile;
	}
}