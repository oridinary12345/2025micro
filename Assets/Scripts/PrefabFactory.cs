using System.Collections.Generic;
using UnityEngine;

public static class PrefabFactory
{
	private static readonly Dictionary<string, Object> _cache = new Dictionary<string, Object>();

	public static GameObject CreatePrefab(string prefab)
	{
		return CreatePrefab(prefab, null);
	}

	public static GameObject CreatePrefab(string prefab, Object source)
	{
		if (!string.IsNullOrEmpty(prefab))
		{
			if (source == null && _cache.ContainsKey(prefab))
			{
				source = _cache[prefab];
			}
			Object @object = source ?? Resources.Load(prefab);
			if (@object != null)
			{
				return Object.Instantiate(@object) as GameObject;
			}
		}
		UnityEngine.Debug.LogWarning("Sprite instance creation failed. Prefab doesn't exist: " + prefab);
		return null;
	}

	public static void Cache(string prefab)
	{
		if (!_cache.ContainsKey(prefab) || _cache[prefab] == null)
		{
			_cache[prefab] = Resources.Load(prefab);
		}
	}

	public static void ClearCache()
	{
		_cache.Clear();
	}
}