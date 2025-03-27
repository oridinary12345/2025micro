using System;
using UnityEngine;

public class GameEvents
{
	public event Action GameOverEvent;

	public event Action<Character> MonsterTouchedEvent;

	public event Action<Character> HeroTouchedEvent;

	public event Action ZoneAttackTouchedEvent;

	public event Action ZoneLootTouchedEvent;

	public event Action<LevelData> LevelFinishedEvent;

	public event Action WaveStartedEvent;

	public event Action GamePausedEvent;

	public event Action GameResumedEvent;

	public event Action<LevelData> GameStartedEvent;

	public event Action<GameStats, GameState, LevelData> GameEndedEvent;

	public event Action<GameStats, GameState> GameAbandonedEvent;

	public event Action GameQuitEvent;

	public event Action<Vector3, string> LootDropEvent;

	public event Action<string, bool> LootPickupEvent;

	public event Action RewardTouchedEvent;

	public event Action<GameStateMessage> GameStateMessageEvent;

	public event Action WeaponRotationStartedEvent;

	public event Action WeaponRotationStoppedEvent;

	public event Action MonsterWaveSpawnedEvent;

	public event Action MonsterSpawnedEvent;

	public event Action MonsterMovedEvent;

	public event Action<string, string> MonsterKilledEvent;

	public event Action MonsterExpulsedEvent;

	public event Action<Vector3> BombExplosionEvent;

	public event Action BombDropEvent;

	public event Action<int> ComboUpdatedEvent;

	public event Action<int> ComboFinishedEvent;

	public event Action<int> RoundStartedEvent;

	public event Action RoundEndedEvent;

	public event Action<string> WeaponUsedEvent;

	public void OnGameOver()
	{
		if (this.GameOverEvent != null)
		{
			this.GameOverEvent();
		}
	}

	public void OnMonsterTouched(Character monster)
	{
		if (this.MonsterTouchedEvent != null)
		{
			this.MonsterTouchedEvent(monster);
		}
	}

	public void OnHeroTouched(Character hero)
	{
		if (this.HeroTouchedEvent != null)
		{
			this.HeroTouchedEvent(hero);
		}
	}

	public void OnLevelFinished(LevelData levelData)
	{
		if (this.LevelFinishedEvent != null)
		{
			this.LevelFinishedEvent(levelData);
		}
	}

	public void OnWaveStarted()
	{
		if (this.WaveStartedEvent != null)
		{
			this.WaveStartedEvent();
		}
	}

	public void OnZoneLootTouched()
	{
		if (this.ZoneLootTouchedEvent != null)
		{
			this.ZoneLootTouchedEvent();
		}
	}

	public void OnZoneAttackTouched()
	{
		if (this.ZoneAttackTouchedEvent != null)
		{
			this.ZoneAttackTouchedEvent();
		}
	}

	public void OnGamePaused()
	{
		if (this.GamePausedEvent != null)
		{
			this.GamePausedEvent();
		}
	}

	public void OnGameResumed()
	{
		if (this.GameResumedEvent != null)
		{
			this.GameResumedEvent();
		}
	}

	public void OnGameStarted(LevelData level)
	{
		if (this.GameStartedEvent != null)
		{
			this.GameStartedEvent(level);
		}
	}

	public void OnGameEnded(GameStats stats, GameState gameState, LevelData level)
	{
		if (this.GameEndedEvent != null)
		{
			this.GameEndedEvent(stats, gameState, level);
		}
	}

	public void OnGameAbandoned(GameStats stats, GameState gameState)
	{
		if (this.GameAbandonedEvent != null)
		{
			this.GameAbandonedEvent(stats, gameState);
		}
	}

	public void OnGameQuit()
	{
		if (this.GameQuitEvent != null)
		{
			this.GameQuitEvent();
		}
	}

	public void OnLootDrop(Vector3 spawningPos, string lootId)
	{
		if (this.LootDropEvent != null)
		{
			this.LootDropEvent(spawningPos, lootId);
		}
	}

	public void OnLootPickUp(string lootId, bool isCard)
	{
		if (this.LootPickupEvent != null)
		{
			this.LootPickupEvent(lootId, isCard);
		}
	}

	public void OnRewardTouched()
	{
		if (this.RewardTouchedEvent != null)
		{
			this.RewardTouchedEvent();
		}
	}

	public void OnGameStateMessage(GameStateMessage message)
	{
		if (this.GameStateMessageEvent != null)
		{
			this.GameStateMessageEvent(message);
		}
	}

	public void OnWeaponRotationStarted()
	{
		if (this.WeaponRotationStartedEvent != null)
		{
			this.WeaponRotationStartedEvent();
		}
	}

	public void OnWeaponRotationStopped()
	{
		if (this.WeaponRotationStoppedEvent != null)
		{
			this.WeaponRotationStoppedEvent();
		}
	}

	public void OnMonsterWaveSpawned()
	{
		if (this.MonsterWaveSpawnedEvent != null)
		{
			this.MonsterWaveSpawnedEvent();
		}
	}

	public void OnMonsterSpawned()
	{
		if (this.MonsterSpawnedEvent != null)
		{
			this.MonsterSpawnedEvent();
		}
	}

	public void OnMonsterMoved()
	{
		if (this.MonsterMovedEvent != null)
		{
			this.MonsterMovedEvent();
		}
	}

	public void OnMonsterKilled(string monsterId, string killedWithWeaponId)
	{
		if (this.MonsterKilledEvent != null)
		{
			this.MonsterKilledEvent(monsterId, killedWithWeaponId);
		}
	}

	public void OnMonsterExpulsed()
	{
		if (this.MonsterExpulsedEvent != null)
		{
			this.MonsterExpulsedEvent();
		}
	}

	public void OnBombExplosion(Vector3 position)
	{
		if (this.BombExplosionEvent != null)
		{
			this.BombExplosionEvent(position);
		}
	}

	public void OnBombDrop()
	{
		if (this.BombDropEvent != null)
		{
			this.BombDropEvent();
		}
	}

	public void OnComboUpdated(int comboCount)
	{
		if (this.ComboUpdatedEvent != null)
		{
			this.ComboUpdatedEvent(comboCount);
		}
	}

	public void OnComboFinished(int comboCount)
	{
		if (this.ComboFinishedEvent != null)
		{
			this.ComboFinishedEvent(comboCount);
		}
	}

	public void OnRoundStarted(int roundCount)
	{
		if (this.RoundStartedEvent != null)
		{
			this.RoundStartedEvent(roundCount);
		}
	}

	public void OnRoundEnded()
	{
		if (this.RoundEndedEvent != null)
		{
			this.RoundEndedEvent();
		}
	}

	public void OnWeaponUsed(string weaponName)
	{
		if (this.WeaponUsedEvent != null)
		{
			this.WeaponUsedEvent(weaponName);
		}
	}
}