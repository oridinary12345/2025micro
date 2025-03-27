using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelManager
{
	public readonly LevelEvents Events;

	private readonly WorldProfiles _worldProfiles;

	private readonly AdventureLevelConfigs _levelConfigs;

	private readonly WorldConfigs _worldConfigs;

	private readonly Dictionary<string, WorldProfile> _worldProfileCache = new Dictionary<string, WorldProfile>();

	private readonly Dictionary<string, WorldData> _worldDataCache = new Dictionary<string, WorldData>();

	public string LastSelectedWorldId
	{
		get
		{
			return _worldProfiles.LastSelectedWorldId;
		}
		set
		{
			_worldProfiles.LastSelectedWorldId = value;
			Events.OnLastSelectedWorldIdChanged(value);
		}
	}

	public string LastUnlockedWorldId
	{
		get
		{
			return _worldProfiles.LastUnlockedWorldId;
		}
		set
		{
			_worldProfiles.LastUnlockedWorldId = value;
			Events.OnWorldUnlocked(value);
		}
	}

	public PlayerLevelManager(WorldProfiles profiles, AdventureLevelConfigs levels, WorldConfigs worlds)
	{
		_worldProfiles = profiles;
		_levelConfigs = levels;
		_worldConfigs = worlds;
		Events = new LevelEvents();
		if (string.IsNullOrEmpty(LastSelectedWorldId))
		{
			LastSelectedWorldId = "w00";
		}
		if (string.IsNullOrEmpty(LastUnlockedWorldId))
		{
			LastUnlockedWorldId = "w00";
		}
		foreach (WorldConfig allConfig in MonoSingleton<WorldConfigs>.Instance.GetAllConfigs())
		{
			GetWorldData(allConfig.Id);
		}
	}

	public LevelData GetNextCustomLevel(WorldData worldData, bool hasUsedChestKey)
	{
		int count = MonoSingleton<AdventureLevelConfigs>.Instance.GetConfigs(worldData.Config.Id).Count;
		int lastCompletedLevelIndex = worldData.Profile.LastCompletedLevelIndex;
		bool flag = lastCompletedLevelIndex >= count - 1;
		LevelData lastNonCompletedLevel = GetLastNonCompletedLevel(worldData.Config.Id);
		if (flag || worldData.Profile.LastGameLost || !hasUsedChestKey)
		{
			int levelIndex = lastNonCompletedLevel.LevelIndex;
			int min = Math.Max(0, levelIndex - 10);
			int levelIndex2 = UnityEngine.Random.Range(min, levelIndex + (flag ? 1 : 0));
			AdventureLevelConfig configByIndex = MonoSingleton<AdventureLevelConfigs>.Instance.GetConfigByIndex(lastNonCompletedLevel.WorldId, levelIndex2);
			if (configByIndex.WorldId != worldData.Config.Id)
			{
				UnityEngine.Debug.LogWarning("Something went wrong with Level generation. Was expecting a level from world " + worldData.Config.Id + ", but got one from world " + configByIndex.WorldId);
			}
			worldData = GetWorldData(configByIndex.WorldId);
			return LevelData.CreateCustomLevel(configByIndex, worldData, lastNonCompletedLevel.Id, levelIndex);
		}
		AdventureLevelConfig configByIndex2 = MonoSingleton<AdventureLevelConfigs>.Instance.GetConfigByIndex(lastNonCompletedLevel.WorldId, lastNonCompletedLevel.LevelIndex);
		return LevelData.CreateCustomLevel(configByIndex2, worldData, lastNonCompletedLevel.BaseId, lastNonCompletedLevel.LevelIndex);
	}

	public void OnGameReadyToStart(LevelData level)
	{
		GetWorldProfile(level.WorldId).GamePlayed++;
	}

	public void OnEndGameChestSpawned(LevelData level)
	{
		GetWorldProfile(level.WorldId).EndGameChestSpawned++;
	}

	public WorldData GetWorldData(string worldId)
	{
 WorldData value;		if (!_worldDataCache.TryGetValue(worldId, out value))
		{
			WorldConfig config = MonoSingleton<WorldConfigs>.Instance.GetConfig(worldId);
			WorldProfile worldProfile = GetWorldProfile(worldId);
			value = new WorldData(config, worldProfile);
			WorldConfig config2 = MonoSingleton<WorldConfigs>.Instance.GetConfig(LastUnlockedWorldId);
			value.IsUnlocked = (config.Index <= config2.Index);
			_worldDataCache[worldId] = value;
		}
		return value;
	}

	public WorldData GetNextLockedWorldData()
	{
		string lastUnlockedWorldId = LastUnlockedWorldId;
		string id = MonoSingleton<WorldConfigs>.Instance.GetNextWorld(lastUnlockedWorldId).Id;
		return GetWorldData(id);
	}

	public LevelData GetLastNonCompletedLevel(string worldId)
	{
		if (string.IsNullOrEmpty(worldId))
		{
			worldId = LastSelectedWorldId;
		}
		string worldId2 = _levelConfigs.GetDefaultConfig().WorldId;
		int num = _levelConfigs.GetDefaultConfig().Index;
		foreach (WorldConfig allConfig in _worldConfigs.GetAllConfigs())
		{
			if (allConfig.Id == worldId)
			{
				WorldData worldData = GetWorldData(allConfig.Id);
				worldId2 = allConfig.Id;
				num = worldData.Profile.LastUnlockedLevelIndex;
				if (num != worldData.Profile.LastCompletedLevelIndex)
				{
					break;
				}
			}
		}
		string levelId = GetWorldData(worldId2).GetLevelId(num);
		AdventureLevelConfig config = _levelConfigs.GetConfig(levelId);
		return LevelData.CreateLevelAdventure(config, GetWorldData(config.WorldId));
	}

	public void UnlockNextLevel(WorldData worldData)
	{
		OnLevelCompleted(GetLastNonCompletedLevel(worldData.Config.Id).Id);
	}

	public void UnlockNextWorld()
	{
		WorldData nextLockedWorldData = GetNextLockedWorldData();
		if (nextLockedWorldData.Config.IsValid())
		{
			UnlockWorld(nextLockedWorldData);
		}
	}

	public WorldProfile GetWorldProfile(string worldId)
	{
 WorldProfile value;		if (!_worldProfileCache.TryGetValue(worldId, out value))
		{
			WorldConfig config = _worldConfigs.GetConfig(worldId);
			value = _worldProfiles.Worlds.Find((WorldProfile w) => w.Id.Equals(worldId));
			if (value == null)
			{
				value = GetNewWorldProfile(config);
				if (worldId != WorldConfig.Invalid.Id)
				{
					_worldProfiles.Worlds.Add(value);
				}
			}
			_worldProfileCache[worldId] = value;
		}
		return value;
	}

	public void OnLevelLost(string levelId)
	{
		AdventureLevelConfig config = _levelConfigs.GetConfig(levelId);
		WorldData worldData = GetWorldData(config.WorldId);
		worldData.Profile.LastGameLost = true;
	}

	public void OnLevelCompleted(string levelId)
	{
		AdventureLevelConfig config = _levelConfigs.GetConfig(levelId);
		WorldData worldData = GetWorldData(config.WorldId);
		int count = MonoSingleton<AdventureLevelConfigs>.Instance.GetConfigs(worldData.Config.Id).Count;
		int lastUnlockedLevelIndex = worldData.Profile.LastUnlockedLevelIndex;
		worldData.Profile.LastGameLost = false;
		if (config.Index != lastUnlockedLevelIndex)
		{
			return;
		}
		worldData.Profile.LastCompletedLevelIndex = Mathf.Min(worldData.Profile.LastCompletedLevelIndex + 1, count - 1);
		worldData.Profile.LastUnlockedLevelIndex = Mathf.Min(worldData.Profile.LastUnlockedLevelIndex + 1, count - 1);
		if (lastUnlockedLevelIndex != worldData.Profile.LastUnlockedLevelIndex)
		{
			AdventureLevelConfig configByIndex = MonoSingleton<AdventureLevelConfigs>.Instance.GetConfigByIndex(worldData.Config.Id, worldData.Profile.LastUnlockedLevelIndex);
			if (configByIndex != null)
			{
				Events.OnLevelUnlocked(configByIndex.Id);
			}
		}
	}

	public void UnlockWorld(WorldData worldToUnlock)
	{
		worldToUnlock.IsUnlocked = true;
		LastSelectedWorldId = worldToUnlock.Config.Id;
		LastUnlockedWorldId = worldToUnlock.Config.Id;
		App.Instance.Player.LootKeyManager.RefillKeys();
	}

	public void RegisterGameEvents(GameEvents gameEvents)
	{
		bool flag = false;
		foreach (WorldData value in _worldDataCache.Values)
		{
			if (value.UnlockMissions != null && flag && !value.IsUnlocked)
			{
				foreach (Mission unlockMission in value.UnlockMissions)
				{
					unlockMission.Register(gameEvents);
				}
				break;
			}
			flag = value.IsUnlocked;
		}
	}

	public void UnRegisterGameEvents(GameEvents gameEvents)
	{
		foreach (WorldData value in _worldDataCache.Values)
		{
			if (value.UnlockMissions != null)
			{
				foreach (Mission unlockMission in value.UnlockMissions)
				{
					unlockMission.UnRegister(gameEvents);
				}
			}
		}
	}

	private WorldProfile GetNewWorldProfile(WorldConfig worldConfig)
	{
		WorldProfile worldProfile = new WorldProfile();
		worldProfile.Id = worldConfig.Id;
		worldProfile.EndGameChestSpawned = 0;
		WorldProfile worldProfile2 = worldProfile;
		if (worldConfig.Id == "w00")
		{
			worldProfile2.EndGameChestSpawned = 1;
		}
		if (worldConfig.MissionIdsToUnlock != null)
		{
			foreach (string item2 in worldConfig.MissionIdsToUnlock)
			{
				MissionConfig config = MonoSingleton<MissionConfigs>.Instance.GetConfig(item2);
				if (config.IsValid())
				{
					MissionProfile missionProfile = new MissionProfile();
					missionProfile.Id = config.Id;
					missionProfile.MissionType = config.MissionType;
					missionProfile.Objective = config.Objective;
					missionProfile.Progress = 0f;
					MissionProfile item = missionProfile;
					worldProfile2.Missions.Add(item);
				}
			}
			return worldProfile2;
		}
		return worldProfile2;
	}
}