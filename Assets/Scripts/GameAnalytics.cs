using System.Collections.Generic;
using UnityEngine;

public static class GameAnalytics
{
	public static void OnEvent(RPGAnalytics.EventType type, RPGAnalytics.EventAction action, Dictionary<string, object> parameters)
	{
		if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
		{
			return;
		}
		if (action == RPGAnalytics.EventAction.InApp)
		{
			float f = 70f;
			string empty = string.Empty;
			string empty2 = string.Empty;
			return;
		}
		if (type == RPGAnalytics.EventType.Economy)
		{
			string currency = parameters["lootId"] as string;
			int num = (int)parameters["amount"];
			string text = parameters["reason"] as string;
			string itemId = text;
			if (num != 0)
			{
				switch (action)
				{
				case RPGAnalytics.EventAction.Expense:
					break;
				case RPGAnalytics.EventAction.Income:
					break;
				}
			}
			return;
		}
		switch (action)
		{
		case RPGAnalytics.EventAction.LevelStarted:
		case RPGAnalytics.EventAction.LevelCompleted:
		case RPGAnalytics.EventAction.LevelFailed:
		{
			int num3;
			switch (action)
			{
			case RPGAnalytics.EventAction.LevelStarted:
				num3 = 1;
				break;
			case RPGAnalytics.EventAction.LevelCompleted:
				num3 = 2;
				break;
			case RPGAnalytics.EventAction.LevelFailed:
				num3 = 3;
				break;
			default:
				num3 = 0;
				break;
			}
			break;
		}
		case RPGAnalytics.EventAction.CoinsPerLevel:
		{
			string str2 = parameters["levelId"] as string;
			int num2 = (int)parameters["amount"];
			string eventName2 = action.ToString() + ":" + str2;
			break;
		}
		case RPGAnalytics.EventAction.LevelUnlocked:
		{
			string str = parameters["levelId"] as string;
			float eventValue = (float)parameters["timePlayed"];
			string eventName = action.ToString() + ":" + str;
			break;
		}
		default:
			break;
		}
	}
}