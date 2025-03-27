using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardSpawner : MonoBehaviour
{
	private const string LootKingToken = "Loot/lootKingToken";

	private const string LootCoin = "Loot/lootCoin";

	private readonly Dictionary<string, int> PoolSize = new Dictionary<string, int>
	{
		{
			"Loot/lootKingToken",
			10
		},
		{
			"Loot/lootCoin",
			30
		}
	};

	private readonly Dictionary<string, ObjectPool<GameObject>> _pool = new Dictionary<string, ObjectPool<GameObject>>();

	[SerializeField]
	private float _pullForce = 20f;

	[SerializeField]
	private ParticleSystem _particles;

	private GameObject GetPrefab(string prefabName)
	{
		if (_pool.ContainsKey(prefabName))
		{
			GameObject @object = _pool[prefabName].GetObject();
			@object.SetActive( true);
			@object.transform.position = Vector3.zero;
			return @object;
		}
		UnityEngine.Debug.LogWarning("This object isn't pooled: " + prefabName);
		return null;
	}

	private void Release(string prefabName, GameObject go)
	{
		if (go != null && _pool.ContainsKey(prefabName))
		{
			go.SetActive( false);
			go.GetComponent<RewardRigidbody>().Velocity = Vector2.zero;
			_pool[prefabName].ReleaseObject(go);
		}
	}

	private void Awake()
	{
		foreach (KeyValuePair<string, int> entry in PoolSize)
		{
			Func<GameObject> create = delegate
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(entry.Key));
				gameObject.SetActive( false);
				return gameObject;
			};
			ObjectPool<GameObject> value = new ObjectPool<GameObject>(create, entry.Key, entry.Value);
			_pool[entry.Key] = value;
		}
	}

	public void Spawn(string prefabPath, int amount, Vector3 startingPos, GameObject target, Action onCollect)
	{
		for (int i = 0; i < amount; i++)
		{
			GameObject prefab = GetPrefab(prefabPath);
			SpriteRenderer component = prefab.GetComponent<SpriteRenderer>();
			if (component != null)
			{
				component.sortingOrder = 100;
			}
			StartCoroutine(SpawnCR(prefab, prefabPath, startingPos, target));
		}
		this.Execute(0.7f, onCollect);
	}

	private IEnumerator SpawnCR(GameObject go, string prefabName, Vector3 startingPos, GameObject target)
	{
		go.transform.localScale = Vector3.one * 0.5f;
		go.transform.position = startingPos;
		yield return null;
		RewardRigidbody reward = go.GetComponent<RewardRigidbody>();
		if (reward == null)
		{
			reward = go.AddComponent<RewardRigidbody>();
		}
		reward.Velocity = new Vector2(UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-2f, 2f));
		yield return new WaitForSeconds(UnityEngine.Random.Range(0.3f, 0.35f));
		reward.SetTarget(target.transform, _pullForce, delegate
		{
			OnRewardTargetReached(go, prefabName);
		});
	}

	private void OnRewardTargetReached(GameObject go, string prefabName)
	{
		Release(prefabName, go);
		if (_particles != null)
		{
			_particles.Emit(5);
		}
	}
}