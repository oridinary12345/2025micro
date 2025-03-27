using System.Collections.Generic;
using UnityEngine;

public class GameStats
{
	private readonly Dictionary<string, int> _weaponUsed = new Dictionary<string, int>();

	private float _startTime;

	public int ComboCount
	{
		get;
		private set;
	}

	public int RoundCount
	{
		get;
		private set;
	}

	public int MonsterWaveCount
	{
		get;
		private set;
	}

	public int DurationSec
	{
		get;
		private set;
	}

	public Dictionary<string, int> MonsterKilled
	{
		get;
		private set;
	}

	public Dictionary<string, int> WeaponUsed => _weaponUsed;

	public GameStats(GameEvents gameEvents)
	{
		MonsterKilled = new Dictionary<string, int>();
		gameEvents.ComboFinishedEvent += OnComboFinished;
		gameEvents.MonsterKilledEvent += OnMonsterKilled;
		gameEvents.RoundStartedEvent += OnRoundStarted;
		gameEvents.MonsterWaveSpawnedEvent += OnMonsterWaveSpawned;
		gameEvents.WeaponUsedEvent += OnWeaponUsed;
	}

	private void OnComboFinished(int comboCount)
	{
		ComboCount += comboCount;
	}

	private void OnMonsterKilled(string monsterId, string killedWithWeaponId)
	{
		if (!MonsterKilled.ContainsKey(monsterId))
		{
			MonsterKilled[monsterId] = 0;
		}
		Dictionary<string, int> monsterKilled;
		string key;
		(monsterKilled = MonsterKilled)[key = monsterId] = monsterKilled[key] + 1;
	}

	private void OnRoundStarted(int total)
	{
		RoundCount++;
	}

	private void OnMonsterWaveSpawned()
	{
		MonsterWaveCount++;
	}

	private void OnWeaponUsed(string weaponName)
	{
		if (!_weaponUsed.ContainsKey(weaponName))
		{
			_weaponUsed[weaponName] = 0;
		}
		Dictionary<string, int> weaponUsed;
		string key;
		(weaponUsed = _weaponUsed)[key = weaponName] = weaponUsed[key] + 1;
	}

	public int GetMonsterKill(string monsterId)
	{
		if (MonsterKilled.ContainsKey(monsterId))
		{
			return MonsterKilled[monsterId];
		}
		return 0;
	}

	public void OnGameStarted()
	{
		_startTime = Time.time;
	}

	public void OnGameEnded()
	{
		DurationSec = Mathf.FloorToInt(Time.time - _startTime);
	}
}