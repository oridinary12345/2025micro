using System.Collections.Generic;
using UnityEngine;

public class WorldData
{
	public WorldProfile Profile;

	public WorldConfig Config;

	public bool IsUnlocked;

	public List<Mission> UnlockMissions
	{
		get;
		private set;
	}

	public WorldData(WorldConfig config, WorldProfile profile)
	{
		Config = config;
		Profile = profile;
		UnlockMissions = new List<Mission>();
		if (config.MissionIdsToUnlock != null)
		{
			foreach (string item in config.MissionIdsToUnlock)
			{
				MissionConfig missionConfig = MonoSingleton<MissionConfigs>.Instance.GetConfig(item);
				MissionProfile missionProfile = profile.Missions.Find((MissionProfile p) => p.Id == missionConfig.Id);
				if (missionProfile == null)
				{
					missionProfile = new MissionProfile
					{
						Id = missionConfig.Id,
						MissionType = missionConfig.MissionType,
						Objective = missionConfig.Objective,
						Progress = 0f
					};
					profile.Missions.Add(missionProfile);
				}
				Mission mission = MissionFactory.Create(missionProfile, missionConfig);
				if (!(mission is NoMission))
				{
					UnlockMissions.Add(mission);
				}
			}
		}
	}

	public LootProfile GetPlayPrice()
	{
		int gamePlayed = Profile.GamePlayed;
		LootProfile lootProfile = new LootProfile();
		lootProfile.LootId = Config.PriceToPlay.LootId;
		lootProfile.Amount = Mathf.Min(Config.PriceAmountMax, Config.PriceToPlay.Amount + gamePlayed * Config.PricePlayIncrement);
		return lootProfile;
	}

	public float GetCompletion01()
	{
		return Mathf.Max(0f, (float)Profile.LastCompletedLevelIndex / (float)(GetLevelCount() - 1));
	}

	public float GetUnlockCompletion01()
	{
		if (UnlockMissions != null)
		{
			int count = UnlockMissions.Count;
			float num = 0f;
			foreach (Mission unlockMission in UnlockMissions)
			{
				num += unlockMission.Progress01;
			}
			return num / (float)count;
		}
		return 1f;
	}

	public string GetLevelId(int levelIndex)
	{
		AdventureLevelConfig configByIndex = MonoSingleton<AdventureLevelConfigs>.Instance.GetConfigByIndex(Config.Id, levelIndex);
		if (configByIndex != null)
		{
			return configByIndex.Id;
		}
		return string.Empty;
	}

	public int GetLevelCount()
	{
		return MonoSingleton<AdventureLevelConfigs>.Instance.GetConfigs(Config.Id).Count;
	}

	public bool AreMissionsCompleted()
	{
		if (UnlockMissions != null)
		{
			foreach (Mission unlockMission in UnlockMissions)
			{
				if (!unlockMission.Completed)
				{
					return false;
				}
			}
		}
		return true;
	}
}