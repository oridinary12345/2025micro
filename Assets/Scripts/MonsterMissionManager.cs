using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class MonsterMissionManager
{
	public const int MonsterLevelMax = 10;

	public const int MissionCompletedLevelUpCount = 7;

	private const int QuestDurationMinutes = 1440;

	private const string QuestRewardCard1 = "QuestCard1";

	public MonsterMisionEvents Events = new MonsterMisionEvents();

	private readonly MonsterMissionProfiles _profiles;

	private Dictionary<string, MonsterMissionData> _monsterCache = new Dictionary<string, MonsterMissionData>();

	private TimeManager _timeManager;

	private Timer _questGenerationTimer;

	private PlayerWeaponManager _weaponManager;

	private PlayerStatsManager _statsManager;

	private GameEvents _gameEvents;

	public MonsterMissionManager(MonsterMissionProfiles profiles, TimeManager timeManager, PlayerWeaponManager weaponManager, PlayerStatsManager statsManager)
	{
		_profiles = profiles;
		_timeManager = timeManager;
		_weaponManager = weaponManager;
		_statsManager = statsManager;
		if (_profiles.Worlds.Count == 0)
		{
			_profiles.LastReset.Time = DateTime.MinValue;
		}
		foreach (WorldConfig allConfig in MonoSingleton<WorldConfigs>.Instance.GetAllConfigs())
		{
			string id = allConfig.Id;
			foreach (string monsterId in MonoSingleton<AdventureLevelConfigs>.Instance.GetMonsterIds(id))
			{
				MonsterMissionProfile monsterProfile = GetMonsterProfile(GetWorldProfile(id), monsterId);
				Mission mission = null;
				if (monsterProfile.CurrentMission != null)
				{
					MissionConfig config = CreateMissionConfig(id, monsterId, monsterProfile.CurrentMission);
					mission = MissionFactory.Create(monsterProfile.CurrentMission, config);
				}
				MonsterMissionData value = new MonsterMissionData(monsterProfile, mission, id);
				_monsterCache[monsterId] = value;
			}
		}
		timeManager.Events.TimerFinishedEvent += OnTimerFinished;
		UpdateTimer();
		RewardConfigs instance = MonoSingleton<RewardConfigs>.Instance;
		instance.AddConfig(new RewardConfig
		{
			Id = "QuestCard1",
			RewardType = RewardType.weapon,
			RewardTypeId = string.Empty,
			AmountMin = 1,
			AmountMax = 1,
			Weight = 0,
			Percentage = 1f,
			ParamInt1 = 0,
			IsCard = true
		});
	}

	public List<MonsterMissionData> GetWorldMonsterData(string worldId)
	{
		List<MonsterMissionData> list = new List<MonsterMissionData>();
		foreach (string monsterId in MonoSingleton<AdventureLevelConfigs>.Instance.GetMonsterIds(worldId))
		{
			MonsterMissionData monsterData = GetMonsterData(monsterId);
			if (monsterData != null)
			{
				list.Add(monsterData);
			}
		}
		return list;
	}

	public MonsterMissionData GetMonsterData(string monsterId)
	{
		if (_monsterCache.ContainsKey(monsterId))
		{
			return _monsterCache[monsterId];
		}
		return null;
	}

	private MonsterMissionWorldProfile GetWorldProfile(string worldId)
	{
		MonsterMissionWorldProfile monsterMissionWorldProfile = _profiles.Worlds.Find((MonsterMissionWorldProfile p) => p.WorldId == worldId);
		if (monsterMissionWorldProfile == null)
		{
			MonsterMissionWorldProfile monsterMissionWorldProfile2 = new MonsterMissionWorldProfile();
			monsterMissionWorldProfile2.WorldId = worldId;
			monsterMissionWorldProfile2.Monsters = new List<MonsterMissionProfile>();
			monsterMissionWorldProfile = monsterMissionWorldProfile2;
			_profiles.Worlds.Add(monsterMissionWorldProfile);
		}
		return monsterMissionWorldProfile;
	}

	private MonsterMissionProfile GetMonsterProfile(MonsterMissionWorldProfile worldProfile, string monsterId)
	{
		MonsterMissionProfile monsterMissionProfile = worldProfile.Monsters.Find((MonsterMissionProfile p) => p.MonsterId == monsterId);
		if (monsterMissionProfile == null)
		{
			MissionConfig missionConfig = CreateMissionConfig(worldProfile.WorldId, monsterId, null);
			MonsterMissionProfile monsterMissionProfile2 = new MonsterMissionProfile();
			monsterMissionProfile2.MonsterId = monsterId;
			monsterMissionProfile2.MissionsCompletedCount = 0;
			monsterMissionProfile2.CurrentMission = CreateMissionProfile(monsterId, missionConfig);
			monsterMissionProfile = monsterMissionProfile2;
			worldProfile.Monsters.Add(monsterMissionProfile);
		}
		return monsterMissionProfile;
	}

	private MissionConfig CreateMissionConfig(string worldId, string monsterId, MissionProfile profile)
	{
		List<int> list = new List<int>();
		list.Add(5);
		list.Add(10);
		list.Add(15);
		List<int> list2 = list;
		int num = list2.Pick();
		List<string> unlockedWeaponIds = _weaponManager.GetUnlockedWeaponIds();
		string paramStr = unlockedWeaponIds.Pick();
		if (profile != null)
		{
			num = Mathf.RoundToInt(profile.Objective);
			paramStr = profile.Param2;
		}
		MissionConfig missionConfig = new MissionConfig();
		missionConfig.Id = worldId + monsterId;
		missionConfig.MissionType = MissionType.MonsterKill;
		missionConfig.Objective = num;
		missionConfig.ParamStr1 = monsterId;
		missionConfig.ParamStr2 = paramStr;
		missionConfig.ParamInt1 = 0;
		return missionConfig;
	}

	private MissionProfile CreateMissionProfile(string monsterId, MissionConfig missionConfig, bool forceCreation = false)
	{
		if (!IsUnlocked(monsterId) && !forceCreation)
		{
			return null;
		}
		MissionProfile missionProfile = new MissionProfile();
		missionProfile.Id = missionConfig.Id;
		missionProfile.MissionType = missionConfig.MissionType;
		missionProfile.Objective = missionConfig.Objective;
		missionProfile.Progress = 0f;
		missionProfile.Param2 = missionConfig.ParamStr2;
		return missionProfile;
	}

	private bool IsUnlocked(string monsterId)
	{
		return _statsManager.GetMonsterKillCount(monsterId) > 0;
	}

	private MonsterMissionProfile GetMonsterProfile(string worldId, string monsterId)
	{
		MonsterMissionWorldProfile worldProfile = GetWorldProfile(worldId);
		return GetMonsterProfile(worldProfile, monsterId);
	}

	public void RegisterGameEvents(GameEvents gameEvents)
	{
		_gameEvents = gameEvents;
		foreach (MonsterMissionData value in _monsterCache.Values)
		{
			if (value.Mission != null)
			{
				value.Mission.Register(gameEvents);
				value.Mission.CompletedEvent += OnMissionCompleted;
			}
		}
		gameEvents.MonsterKilledEvent += OnMonsterKilled;
	}

	private void OnMissionCompleted(Mission mission)
	{
		MissionKillMonsters missionKillMonsters = mission as MissionKillMonsters;
		if (missionKillMonsters != null)
		{
			Events.OnMonsterMissionCompleted(missionKillMonsters.MonsterId);
		}
	}

	public void UnRegisterGameEvents(GameEvents gameEvents)
	{
		foreach (MonsterMissionData value in _monsterCache.Values)
		{
			if (value.Mission != null)
			{
				value.Mission.UnRegister(gameEvents);
				value.Mission.CompletedEvent -= OnMissionCompleted;
			}
		}
		gameEvents.MonsterKilledEvent -= OnMonsterKilled;
		_gameEvents = null;
	}

	private void OnMonsterKilled(string monsterId, string killedWithWeaponId)
	{
		if (IsUnlocked(monsterId) || _gameEvents == null)
		{
			return;
		}
		MonsterMissionData monsterData = GetMonsterData(monsterId);
		if (monsterData != null)
		{
			MonsterMissionProfile profile = monsterData.Profile;
			if (profile.CurrentMission == null && profile.MissionsCompletedCount == 0)
			{
				MissionConfig missionConfig = CreateMissionConfig(monsterData.WorldId, monsterId, null);
				profile.CurrentMission = CreateMissionProfile(monsterId, missionConfig, true);
				profile.CanReplaceMission = false;
				Mission mission = MissionFactory.Create(profile.CurrentMission, missionConfig);
				mission.Register(_gameEvents);
				mission.CompletedEvent += OnMissionCompleted;
				monsterData.UpdateMission(mission);
			}
		}
	}

	public string GetRemainingTimeBeforeNextQuests()
	{
		if (_questGenerationTimer == null || _questGenerationTimer.IsFinished())
		{
			return "-";
		}
		int num = Mathf.RoundToInt(_questGenerationTimer.GetRemainingTimeSec());
		return $"{Utils.Time.SecondToLongString(num)}";
	}

	public bool CanClaimQuests(string worldId)
	{
		MonsterMissionWorldProfile worldProfile = GetWorldProfile(worldId);
		foreach (MonsterMissionProfile monster in worldProfile.Monsters)
		{
			MonsterMissionData monsterData = GetMonsterData(monster.MonsterId);
			if (monsterData.Mission != null && monsterData.Mission.Completed)
			{
				return true;
			}
		}
		return false;
	}

	public void ClaimMission(MonsterMissionData data)
	{
		if (data.Mission != null && data.Mission.Completed)
		{
			data.Profile.MissionsCompletedCount++;
			data.Profile.CurrentMission = null;
			data.UpdateMission(null);
			if (data.Profile.CanReplaceMission)
			{
				MissionConfig missionConfig = CreateMissionConfig(data.WorldId, data.MonsterId, null);
				data.Profile.CurrentMission = CreateMissionProfile(data.MonsterId, missionConfig);
				data.Profile.CanReplaceMission = false;
				Mission mission = MissionFactory.Create(data.Profile.CurrentMission, missionConfig);
				MonsterMissionData monsterData = GetMonsterData(data.MonsterId);
				monsterData.UpdateMission(mission);
			}
			int rewardCoinsAmount = GetRewardCoinsAmount(data.MonsterId);
			LootProfile lootProfile = LootProfile.Create("lootCoin");
			lootProfile.Amount = rewardCoinsAmount;
			RewardLoot item = new RewardLoot(lootProfile, 1f, true);
			RewardContext rewardContext = new RewardContext();
			rewardContext.worldId = data.WorldId;
			RewardContext rewardContext2 = rewardContext;
			Reward item2 = App.Instance.RewardFactory.Create("QuestCard1", rewardContext2);
			List<Reward> list = new List<Reward>();
			list.Add(item);
			list.Add(item2);
			List<Reward> rewards = list;
			Events.OnMonsterRewardClaimed(data, rewards);
		}
	}

	public int GetRewardCoinsAmount(string monsterId)
	{
		MonsterConfig config = MonoSingleton<MonsterConfigs>.Instance.GetConfig(monsterId);
		foreach (string item in config.RewardIdHit)
		{
			if (!string.IsNullOrEmpty(item))
			{
				RewardConfig config2 = MonoSingleton<RewardConfigs>.Instance.GetConfig(item);
				if (config2.RewardTypeId == "lootCoin")
				{
					return config2.AmountMax * 10;
				}
			}
		}
		return 0;
	}

	private void UpdateTimer()
	{
		if (_profiles.LastReset.Time == DateTime.MinValue)
		{
			_profiles.LastReset.Time = DateTime.UtcNow;
		}
		if (_questGenerationTimer == null)
		{
			TimeSpan timeSpan = DateTime.UtcNow - _profiles.LastReset.Time;
			if (timeSpan.TotalMinutes > 1440.0)
			{
				RefreshedExpiredMissions();
				int num = Mathf.FloorToInt((float)timeSpan.TotalMinutes / 1440f);
				DateTime time = _profiles.LastReset.Time.AddMinutes(num * 1440);
				_profiles.LastReset.Time = time;
				Events.OnMonsterMissionRefreshed();
			}
			_questGenerationTimer = Timer.Create(_profiles.LastReset.Time, 86400f);
			_timeManager.Register(_questGenerationTimer);
		}
	}

	private void OnTimerFinished(Timer timer)
	{
		if (_questGenerationTimer == timer)
		{
			_questGenerationTimer = null;
			UpdateTimer();
		}
	}

	private void RefreshedExpiredMissions()
	{
		foreach (WorldConfig allConfig in MonoSingleton<WorldConfigs>.Instance.GetAllConfigs())
		{
			string id = allConfig.Id;
			foreach (string monsterId in MonoSingleton<AdventureLevelConfigs>.Instance.GetMonsterIds(id))
			{
				MonsterMissionProfile monsterProfile = GetMonsterProfile(GetWorldProfile(id), monsterId);
				if (monsterProfile != null)
				{
					if (monsterProfile.CurrentMission != null && monsterProfile.CurrentMission.Progress >= monsterProfile.CurrentMission.Objective)
					{
						monsterProfile.CanReplaceMission = true;
					}
					else
					{
						MissionConfig missionConfig = CreateMissionConfig(id, monsterId, null);
						monsterProfile.CurrentMission = CreateMissionProfile(monsterId, missionConfig);
						monsterProfile.CanReplaceMission = false;
						Mission mission = MissionFactory.Create(monsterProfile.CurrentMission, missionConfig);
						MonsterMissionData monsterData = GetMonsterData(monsterId);
						monsterData.UpdateMission(mission);
					}
				}
			}
		}
	}

	public void IncreaseMissionObjective()
	{
		foreach (MonsterMissionData value in _monsterCache.Values)
		{
			if (value.Mission != null)
			{
				value.Mission.ForceIncreasePogress();
			}
		}
	}

	public void ForceExpiration()
	{
		if (_questGenerationTimer != null)
		{
			_timeManager.Unregister(_questGenerationTimer);
		}
		RefreshedExpiredMissions();
		_profiles.LastReset.Time = DateTime.UtcNow;
		_questGenerationTimer = Timer.Create(_profiles.LastReset.Time, 86400f);
		_timeManager.Register(_questGenerationTimer);
		Events.OnMonsterMissionRefreshed();
	}
}