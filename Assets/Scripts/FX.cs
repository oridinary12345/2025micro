using System;
using System.Collections.Generic;
using UnityEngine;

public class FX
{
	private const string FXHitImpact = "FXHitImpact";

	private const string FXHitImpactCritical = "FXHitImpactCritical";

	private const string FXStars = "FXStars";

	private const string FXToxicGaz = "FXToxicGaz";

	private const string FXPetrifyRay = "FXPetrifyRay";

	private const string FXPetrifyTarget = "FXPetrifyTarget";

	private const string FXHealing = "FXHealing";

	private const string FXDiveSplash = "FXDiveSplash";

	private readonly Dictionary<string, int> PoolSize = new Dictionary<string, int>
	{
		{
			"FXHitImpact",
			3
		},
		{
			"FXHitImpactCritical",
			3
		},
		{
			"FXStars",
			5
		},
		{
			"FXToxicGaz",
			3
		},
		{
			"FXPetrifyRay",
			1
		},
		{
			"FXPetrifyTarget",
			1
		},
		{
			"FXHealing",
			2
		},
		{
			"FXDiveSplash",
			4
		}
	};

	private readonly Dictionary<string, ObjectPool<GameObject>> _pool = new Dictionary<string, ObjectPool<GameObject>>();

	public FX()
	{
		foreach (KeyValuePair<string, int> entry in PoolSize)
		{
			Func<GameObject> create = delegate
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("GameFX/" + entry.Key));
				gameObject.SetActive( false);
				return gameObject;
			};
			ObjectPool<GameObject> value = new ObjectPool<GameObject>(create, entry.Key, entry.Value);
			_pool[entry.Key] = value;
		}
	}

	private GameObject GetFX(string fxName)
	{
		if (_pool.ContainsKey(fxName))
		{
			GameObject @object = _pool[fxName].GetObject();
			@object.SetActive( true);
			return @object;
		}
		return null;
	}

	private void Release(string fxName, GameObject go)
	{
		if (go != null && _pool.ContainsKey(fxName))
		{
			go.SetActive( false);
			_pool[fxName].ReleaseObject(go);
		}
	}

	public void CreateImpact(Transform parent, Vector3 position, bool isCritical)
	{
		string fxName = (!isCritical) ? "FXHitImpact" : "FXHitImpactCritical";
		GameObject impactFX = GetFX(fxName);
		if (impactFX != null)
		{
			if (isCritical)
			{
				position.x -= 0.57f;
				position.y -= 0.37f;
			}
			impactFX.transform.parent = parent;
			impactFX.transform.position = position;
			impactFX.AddComponent<AutoExecute>().Init(0.2f, delegate
			{
				Release(fxName, impactFX);
			});
		}
	}

	public GameObject CreateToxicGaz(Transform parent, Vector3 position)
	{
		string fxName = "FXToxicGaz";
		GameObject gazFX = GetFX(fxName);
		if (gazFX != null)
		{
			position.z -= 5f;
			gazFX.transform.parent = parent;
			gazFX.transform.position = position;
			gazFX.AddComponent<AutoExecute>().Init(0.5f, delegate
			{
				Release(fxName, gazFX);
			});
			return gazFX;
		}
		return null;
	}

	public GameObject CreatePetrifyRay(Transform parent, Vector3 position)
	{
		string fxName = "FXPetrifyRay";
		GameObject fx = GetFX(fxName);
		if (fx != null)
		{
			position.z -= 0.1f;
			fx.transform.parent = parent;
			fx.transform.position = position;
			fx.AddComponent<AutoExecute>().Init(1f, delegate
			{
				Release(fxName, fx);
			});
			return fx;
		}
		return null;
	}

	public GameObject CreatePetrifyTarget(Transform parent, Vector3 position)
	{
		string fxName = "FXPetrifyTarget";
		GameObject fx = GetFX(fxName);
		if (fx != null)
		{
			position.z -= 0.1f;
			fx.transform.parent = parent;
			fx.transform.position = position;
			fx.AddComponent<AutoExecute>().Init(1f, delegate
			{
				Release(fxName, fx);
			});
			return fx;
		}
		return null;
	}

	public GameObject CreateHealing(Transform parent, Vector3 position)
	{
		string fxName = "FXHealing";
		GameObject fx = GetFX(fxName);
		if (fx != null)
		{
			position.z -= 0.1f;
			fx.transform.parent = parent;
			fx.transform.position = position;
			fx.AddComponent<AutoExecute>().Init(1f, delegate
			{
				Release(fxName, fx);
			});
			return fx;
		}
		return null;
	}

	public GameObject CreateDiveSplash(Transform parent, Vector3 position)
	{
		string fxName = "FXDiveSplash";
		GameObject fx = GetFX(fxName);
		if (fx != null)
		{
			position.z -= 0.1f;
			fx.transform.parent = parent;
			fx.transform.position = position;
			fx.AddComponent<AutoExecute>().Init(1f, delegate
			{
				Release(fxName, fx);
			});
			return fx;
		}
		return null;
	}

	public void CreateStatusMiss(Transform parent, Vector3 position)
	{
		CreateStatusFX(parent, position, "GameFX/FXStatusMiss");
	}

	public void CreateStatusCritical(Transform parent, Vector3 position)
	{
		CreateStatusFX(parent, position, "GameFX/FXStatusCritical");
	}

	public void CreateStatusWeak(Transform parent, Vector3 position)
	{
		CreateStatusFX(parent, position, "GameFX/FXStatusWeak");
	}

	private void CreateStatusFX(Transform parent, Vector3 position, string prefabPath)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(prefabPath));
		gameObject.transform.parent = parent;
		gameObject.transform.position = position;
		gameObject.AddComponent<AutoDestruct>().Init(2f);
	}

	public void CreateProjectileLanded(Transform parent, Vector3 position, WeaponData weapon, float sign, float groundDuration)
	{
		GameObject gameObject = null;
		if (weapon != null)
		{
			WeaponPrefab weaponPrefab = Resources.Load<WeaponPrefab>("Weapons/" + weapon.Id + "/" + weapon.Id);
			if (weaponPrefab.ProjectileLandingFX != null)
			{
				gameObject = UnityEngine.Object.Instantiate(weaponPrefab.ProjectileLandingFX);
				Vector3 localScale = gameObject.transform.localScale;
				gameObject.transform.localScale = new Vector3(Mathf.Sign(sign) * Mathf.Abs(localScale.x), localScale.y, localScale.z);
				gameObject.transform.parent = parent;
				gameObject.transform.position = position;
				gameObject.AddComponent<AutoDestruct>().Init(groundDuration);
			}
		}
	}

	public GameObject CreateAttackFX(Transform parent, string attackerId)
	{
		GameObject gameObject = Resources.Load<GameObject>("GameFX/FXAttack" + attackerId);
		if (gameObject != null)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
			Vector3 localPosition = gameObject2.transform.localPosition;
			gameObject2.transform.parent = parent;
			gameObject2.transform.localPosition = localPosition;
			gameObject2.transform.localScale = Vector3.one;
			gameObject2.AddComponent<AutoDestruct>().Init(0.25f);
			return gameObject2;
		}
		return null;
	}

	public void CreateBombExplosion(Transform parent, Vector3 position)
	{
		GameObject gameObject = Resources.Load<GameObject>("GameFX/FXBombExplosion");
		if (gameObject != null)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
			gameObject2.transform.parent = parent;
			gameObject2.transform.localPosition = position;
			gameObject2.AddComponent<AutoDestruct>().Init(1f);
		}
	}

	public static GameObject CreateUnderwaterShadow(Transform parent, Vector3 position)
	{
		GameObject gameObject = Resources.Load<GameObject>("GameFX/FXUnderwater");
		if (gameObject != null)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
			gameObject2.transform.parent = parent;
			gameObject2.transform.localPosition = position;
			return gameObject2;
		}
		return null;
	}

	public void CreateStarFX(Vector3 pos)
	{
		GameObject collectFx = GetFX("FXStars");
		collectFx.transform.position = pos;
		collectFx.AddComponent<AutoExecute>().Init(1f, delegate
		{
			Release("FXStars", collectFx);
		});
	}
}