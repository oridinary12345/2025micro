using System;
using System.Collections.Generic;
using UnityEngine;

public class RPGAnalytics : MonoBehaviour
{
	public enum EventType
	{
		App,
		Gameplay,
		Economy
	}

	public enum EventAction
	{
		Boot,
		LevelStarted,
		LevelCompleted,
		LevelFailed,
		LevelUnlocked,
		Expense,
		Income,
		InApp,
		CoinsPerLevel
	}

	private Analytics _analytics;

	private LootEvents _lootEvents;

	private LevelEvents _levelEvents;

	public RPGAnalytics Setup(Analytics analytics, LootEvents lootEvents, LevelEvents levelEvents)
	{
		_analytics = analytics;
		_lootEvents = lootEvents;
		_levelEvents = levelEvents;
		_lootEvents.LootUpdatedEvent += OnLootUpdated;
		_levelEvents.LevelUnlockedEvent += OnLevelUnlocked;
		return this;
	}

	public void RegisterGameEvents(GameEvents gameEvents)
	{
		gameEvents.GameStartedEvent += OnGameStarted;
		gameEvents.GameEndedEvent += OnGameEnded;
		gameEvents.GameAbandonedEvent += OnGameAbandoned;
	}

	public void UnregisterGameEvents(GameEvents gameEvents)
	{
		gameEvents.GameStartedEvent -= OnGameStarted;
		gameEvents.GameEndedEvent -= OnGameEnded;
		gameEvents.GameAbandonedEvent -= OnGameAbandoned;
	}

	private void OnLootUpdated(LootProfile loot, int delta, CurrencyReason reason)
	{
		switch (reason)
		{
		case CurrencyReason.inGameDrop:
			return;
		case CurrencyReason.unknown:
			UnityEngine.Debug.LogWarning("LootUpdated for an unknown reason! Loot = " + loot.LootId + ", loot delta = " + delta);
			break;
		}
		EventType type = EventType.Economy;
		EventAction action = EventAction.Income;
		if (delta < 0)
		{
			action = EventAction.Expense;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("lootId", loot.LootId);
		dictionary.Add("amount", Math.Abs(delta));
		dictionary.Add("reason", reason.ToString());
		Dictionary<string, object> parameters = dictionary;
		SendEvent(type, action, parameters);
	}

	private void OnLevelUnlocked(string levelId)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("levelId", levelId);
		dictionary.Add("timePlayed", App.Instance.Player.StatsManager.GetTimePlayedSec());
		Dictionary<string, object> parameters = dictionary;
		SendEvent(EventType.Gameplay, EventAction.LevelUnlocked, parameters);
	}

	private void OnGameStarted(LevelData level)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("worldId", level.WorldId);
		dictionary.Add("levelId", level.Id);
		Dictionary<string, object> parameters = dictionary;
		SendEvent(EventType.Gameplay, EventAction.LevelStarted, parameters);
	}

	private void OnGameEnded(GameStats stats, GameState gameState, LevelData level)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("worldId", level.WorldId);
		dictionary.Add("levelId", level.Id);
		Dictionary<string, object> parameters = dictionary;
		bool flag = gameState.IsLevelCompleted();
		SendEvent(EventType.Gameplay, (!flag) ? EventAction.LevelFailed : EventAction.LevelCompleted, parameters);
		foreach (KeyValuePair<string, int> item in gameState.GetLootGathered())
		{
			dictionary = new Dictionary<string, object>();
			dictionary.Add("lootId", item.Key);
			dictionary.Add("amount", item.Value);
			dictionary.Add("reason", CurrencyReason.gameEnded.ToString());
			dictionary.Add("levelId", level.Id);
			parameters = dictionary;
			SendEvent(EventType.Economy, EventAction.Income, parameters);
			if (flag && item.Key == "lootCoin")
			{
				SendEvent(EventType.Gameplay, EventAction.CoinsPerLevel, parameters);
			}
		}
	}

	private void OnGameAbandoned(GameStats stats, GameState gameState)
	{
		foreach (KeyValuePair<string, int> item in gameState.GetLootGathered())
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("lootId", item.Key);
			dictionary.Add("amount", item.Value);
			dictionary.Add("reason", CurrencyReason.gameAbandoned.ToString());
			Dictionary<string, object> parameters = dictionary;
			SendEvent(EventType.Economy, EventAction.Income, parameters);
		}
	}

	private void SendEvent(EventType type, EventAction action, Dictionary<string, object> parameters = null)
	{
		_analytics.OnEvent(type, action, parameters);
	}
}