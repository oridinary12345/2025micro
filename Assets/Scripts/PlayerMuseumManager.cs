using System;
using System.Collections.Generic;

public class PlayerMuseumManager
{
	public const int MonsterCardLevelMax = 6;

	public MuseumEvents Events = new MuseumEvents();

	private readonly MuseumProfiles _profiles;

	private readonly MuseumConfigs _configs;

	private readonly TimeManager _timeManager;

	private Dictionary<string, MuseumData> _museumDataCache = new Dictionary<string, MuseumData>();

	public PlayerMuseumManager(MuseumProfiles profiles, MuseumConfigs configs, TimeManager timeManager)
	{
		_profiles = profiles;
		_configs = configs;
		_timeManager = timeManager;
		Dictionary<string, List<MuseumConfig>> configs2 = _configs.GetConfigs();
		foreach (KeyValuePair<string, List<MuseumConfig>> item in configs2)
		{
			string key = item.Key;
			List<MuseumConfig> value = item.Value;
			MuseumProfile profile = GetProfile(key);
			foreach (MuseumConfig item2 in value)
			{
				MuseumMonsterProfile monsterProfile = GetMonsterProfile(profile, item2.Id);
				MuseumData value2 = new MuseumData(item2, monsterProfile, Events, _timeManager);
				_museumDataCache[item2.Id] = value2;
			}
		}
	}

	public int GetAmountPerMinute()
	{
		int num = 0;
		foreach (MuseumData value in _museumDataCache.Values)
		{
			num += value.GetAmountPerMinute();
		}
		return num;
	}

	public int GetAmountPerMinuteMax()
	{
		int num = 0;
		foreach (MuseumData value in _museumDataCache.Values)
		{
			num += value.GetAmountPerMinuteMax();
		}
		return num;
	}

	public MuseumData GetMuseumData(string monsterId)
	{
		if (_museumDataCache.ContainsKey(monsterId))
		{
			return _museumDataCache[monsterId];
		}
		return null;
	}

	private MuseumProfile GetProfile(string worldId)
	{
		MuseumProfile museumProfile = _profiles.Museums.Find((MuseumProfile p) => p.WorldId == worldId);
		if (museumProfile == null)
		{
			MuseumProfile museumProfile2 = new MuseumProfile();
			museumProfile2.WorldId = worldId;
			museumProfile2.Monsters = new List<MuseumMonsterProfile>();
			museumProfile = museumProfile2;
			_profiles.Museums.Add(museumProfile);
		}
		return museumProfile;
	}

	private MuseumMonsterProfile GetMonsterProfile(MuseumProfile worldProfile, string monsterId)
	{
		MuseumMonsterProfile museumMonsterProfile = worldProfile.Monsters.Find((MuseumMonsterProfile p) => p.MonsterId == monsterId);
		if (museumMonsterProfile == null)
		{
			MuseumMonsterProfile museumMonsterProfile2 = new MuseumMonsterProfile();
			museumMonsterProfile2.MonsterId = monsterId;
			museumMonsterProfile2.MonsterCollectedCount = 0;
			museumMonsterProfile2.LastWakeUpTime = new DateTimeJson();
			museumMonsterProfile2.LastTickTime = new DateTimeJson
			{
				Time = DateTime.MinValue
			};
			museumMonsterProfile = museumMonsterProfile2;
			worldProfile.Monsters.Add(museumMonsterProfile);
		}
		return museumMonsterProfile;
	}

	private MuseumMonsterProfile GetMonsterProfile(string worldId, string monsterId)
	{
		MuseumProfile profile = GetProfile(worldId);
		return GetMonsterProfile(profile, monsterId);
	}

	public int GetMonsterLevel(string monsterId)
	{
		return GetMuseumData(monsterId)?.Level ?? 0;
	}

	public void RegisterGameEvents(GameEvents gameEvents)
	{
		foreach (MuseumData value in _museumDataCache.Values)
		{
			value.Register(gameEvents);
		}
	}

	public void UnRegisterGameEvents(GameEvents gameEvents)
	{
		foreach (MuseumData value in _museumDataCache.Values)
		{
			value.Unregister(gameEvents);
		}
	}

	public void OnAppQuit()
	{
		foreach (MuseumProfile museum in _profiles.Museums)
		{
			foreach (MuseumMonsterProfile monster in museum.Monsters)
			{
				monster.LastTickTime.Time = DateTime.UtcNow;
			}
		}
	}

	public void OnAppPause()
	{
		foreach (MuseumData value in _museumDataCache.Values)
		{
			value.OnAppPause();
		}
	}

	public void OnAppResume()
	{
	}

	public void AddCardsToAll(int cardCount)
	{
		Dictionary<string, List<MuseumConfig>> configs = _configs.GetConfigs();
		foreach (KeyValuePair<string, List<MuseumConfig>> item in configs)
		{
			string key = item.Key;
			List<MuseumConfig> value = item.Value;
			MuseumProfile profile = GetProfile(key);
			foreach (MuseumConfig item2 in value)
			{
				MuseumMonsterProfile monsterProfile = GetMonsterProfile(profile, item2.Id);
				monsterProfile.MonsterCollectedCount += cardCount;
			}
		}
	}
}