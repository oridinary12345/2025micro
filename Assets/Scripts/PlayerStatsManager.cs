using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager
{
	private readonly StatsProfile _profile;

	private float _sessionStartTime;

	public PlayerStatsManager(StatsProfile profile)
	{
		_profile = profile;
		_sessionStartTime = Time.realtimeSinceStartup;
	}

	private void UpdateTimePlayed()
	{
		int num = Mathf.FloorToInt(Time.realtimeSinceStartup - _sessionStartTime);
		_profile.TimePlayedSec += num;
		_sessionStartTime = Time.realtimeSinceStartup;
	}

	public void MergeGameStats(GameStats stats)
	{
		foreach (KeyValuePair<string, int> item in stats.MonsterKilled)
		{
			if (!_profile.MonsterKills.ContainsKey(item.Key))
			{
				_profile.MonsterKills[item.Key] = 0;
			}
			Dictionary<string, int> monsterKills;
			string key;
			(monsterKills = _profile.MonsterKills)[key = item.Key] = monsterKills[key] + item.Value;
		}
	}

	public int GetMonsterKillCount(string monsterId)
	{
		if (_profile.MonsterKills.ContainsKey(monsterId))
		{
			return _profile.MonsterKills[monsterId];
		}
		return 0;
	}

	public float GetTimePlayedSec()
	{
		UpdateTimePlayed();
		return _profile.TimePlayedSec;
	}

	public void OnAppPause()
	{
		UpdateTimePlayed();
	}

	public void OnAppResume()
	{
		_sessionStartTime = Time.realtimeSinceStartup;
	}
}