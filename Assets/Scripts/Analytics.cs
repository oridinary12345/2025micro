using System;
using System.Collections.Generic;
using UnityEngine;

public class Analytics : MonoBehaviour
{
	private RPGAnalytics _analytics;

	public Analytics Setup(LootEvents lootEvents, LevelEvents levelEvents)
	{
		_analytics = base.gameObject.AddComponent<RPGAnalytics>().Setup(this, lootEvents, levelEvents);
		List<string> availableResourceCurrencies = new List<string>(Enum.GetNames(typeof(Currency)));
		List<string> availableResourceItemTypes = new List<string>(Enum.GetNames(typeof(CurrencyReason)));
		OnBoot();
		return this;
	}

	public void OnBoot()
	{
		OnEvent(RPGAnalytics.EventType.App, RPGAnalytics.EventAction.Boot, new Dictionary<string, object>());
	}

	public void RegisterGameEvents(GameEvents gameEvents)
	{
		_analytics.RegisterGameEvents(gameEvents);
	}

	public void UnregisterGameEvents(GameEvents gameEvents)
	{
		if (_analytics != null)
		{
			_analytics.UnregisterGameEvents(gameEvents);
		}
	}

	public void OnEvent(RPGAnalytics.EventType type, RPGAnalytics.EventAction action, Dictionary<string, object> parameters)
	{
		UnityAnalytics.OnEvent(type, action, parameters);
		GameAnalytics.OnEvent(type, action, parameters);
	}
}