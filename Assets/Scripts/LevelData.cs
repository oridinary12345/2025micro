using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
	private int _waveIndex;

	private WaveConfig _currentWave;

	private List<WaveConfig> _waves;

	private Mission _mission;

	public string Id
	{
		get;
		private set;
	}

	public AdventureLevelConfig Config
	{
		get;
		private set;
	}

	public string BaseId
	{
		get;
		private set;
	}

	public string WorldId
	{
		get;
		private set;
	}

	public string LevelTitle
	{
		get;
		private set;
	}

	public int LevelIndex
	{
		get;
		private set;
	}

	public int WorldIndex
	{
		get;
		private set;
	}

	public string WorldTitle
	{
		get;
		private set;
	}

	public WorldData WorldData
	{
		get;
		private set;
	}

	public LevelEvents Events
	{
		get;
		private set;
	}

	public bool Completed
	{
		get;
		private set;
	}

	public bool IsObjectiveWaveBased
	{
		get;
		private set;
	}

	public bool IsObjectiveMissionBased
	{
		get;
		private set;
	}

	public int RoundCountBetweenWave => _currentWave.RoundCountBetweenWave;

	private LevelData(AdventureLevelConfig levelConfig, WorldData worldData)
	{
		WorldData = worldData;
		_waveIndex = 0;
		IsObjectiveWaveBased = false;
		IsObjectiveMissionBased = false;
		Id = levelConfig.Id;
		Config = levelConfig;
		BaseId = levelConfig.Id;
		WorldId = levelConfig.WorldId;
		LevelTitle = levelConfig.Title;
		LevelIndex = levelConfig.Index;
		WorldIndex = worldData.Config.Index;
		WorldTitle = worldData.Config.Title;
		Completed = false;
		Events = new LevelEvents();
		_waves = levelConfig.Waves;
		_currentWave = _waves[_waveIndex];
	}

	private LevelData(AdventureLevelConfig levelConfig, WorldData worldData, string levelId, int levelIndexMax)
	{
		WorldData = worldData;
		IsObjectiveWaveBased = true;
		IsObjectiveMissionBased = false;
		int num = 5;
		if (levelIndexMax <= 0)
		{
			num = 2;
		}
		else if (levelIndexMax <= 3)
		{
			num = 3;
		}
		else if (levelIndexMax <= 4)
		{
			num = 4;
		}
		_waves = new List<WaveConfig>();
		for (int i = 0; i < num; i++)
		{
			WaveConfig item = levelConfig.Waves[0];
			_waves.Add(item);
		}
		_waveIndex = 0;
		Id = levelId;
		Config = levelConfig;
		BaseId = levelConfig.Id;
		WorldId = levelConfig.WorldId;
		LevelTitle = levelConfig.Title;
		LevelIndex = 0;
		WorldIndex = worldData.Config.Index;
		WorldTitle = worldData.Config.Title;
		Completed = false;
		Events = new LevelEvents();
		_currentWave = _waves[_waveIndex];
	}

	public static LevelData CreateLevelAdventure(AdventureLevelConfig config, WorldData worldData)
	{
		return new LevelData(config, worldData);
	}

	public static LevelData CreateCustomLevel(AdventureLevelConfig config, WorldData worldData, string levelId, int levelIndex)
	{
		return new LevelData(config, worldData, levelId, levelIndex);
	}

	public void RegisterGameEvents(GameEvents gameEvents)
	{
		if (_mission != null)
		{
			_mission.Register(gameEvents);
		}
	}

	public void UnRegisterGameEvents(GameEvents gameEvents)
	{
		if (_mission != null)
		{
			_mission.UnRegister(gameEvents);
		}
	}

	public float GetProgress01()
	{
		if (IsObjectiveWaveBased)
		{
			return Mathf.Clamp01((float)GetWaveIndex() / ((float)GetWaveMaxCount() - 1f));
		}
		if (IsObjectiveMissionBased && _mission != null)
		{
			return _mission.Progress01;
		}
		return 0f;
	}

	public int GetWaveIndex()
	{
		return _waveIndex;
	}

	public int GetWaveMaxCount()
	{
		return _waves.Count;
	}

	public bool IsMissionCompleted()
	{
		if (IsObjectiveMissionBased)
		{
			return _mission.Completed;
		}
		return false;
	}

	public string GetMissionDescription()
	{
		if (IsObjectiveMissionBased)
		{
			return _mission.GetDescription();
		}
		return string.Empty;
	}

	public bool IsLastWaveReached()
	{
		return _waveIndex == _waves.Count - 1;
	}

	public int GetMonsterPerWaveMinCount()
	{
		return _currentWave.MonsterPerWaveMin;
	}

	public int GetMonsterPerWaveMaxCount()
	{
		return _currentWave.MonsterPerWaveMax;
	}

	public int GetMonsterSpawnLayerMin()
	{
		return _currentWave.SpawnLayerMin;
	}

	public int GetMonsterSpawnLayerMax()
	{
		return _currentWave.SpawnLayerMax;
	}

	public int GetMaxMonsters()
	{
		return _currentWave.MaxMonster;
	}

	public WeightedList<LevelMonster> GetMonsterIds()
	{
		return _currentWave.Monsters;
	}

	public void StartWave()
	{
		_currentWave = _waves[_waveIndex];
		Events.OnWaveStarted();
	}

	public void EndWave()
	{
		Events.OnWaveCompleted();
		if (IsLastWaveReached())
		{
			if (IsObjectiveMissionBased)
			{
				Completed = _mission.Completed;
			}
			else
			{
				Completed = true;
			}
			Events.OnAllWaveCompleted();
			if (Completed)
			{
				App.Instance.Player.LevelManager.OnLevelCompleted(BaseId);
			}
		}
		else
		{
			_waveIndex++;
		}
	}
}