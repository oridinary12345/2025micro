using System.Collections.Generic;
using UnityEngine.Analytics;

public static class UnityAnalytics
{
	public static void OnEvent(RPGAnalytics.EventType type, RPGAnalytics.EventAction action, Dictionary<string, object> parameters)
	{
		UnityEngine.Analytics.Analytics.CustomEvent(action.ToString(), parameters);
	}
}