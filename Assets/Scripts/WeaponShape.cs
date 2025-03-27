using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShape : MonoBehaviour
{
	public const float FlashDuration = 0.4f;

	private bool _rotationActivated;

	private Tweener _tweener;

	private SpriteRenderer _sprite;

	private PolygonCollider2D[] _colliders;

	private GameObject _shapeField;

	private WeaponConfig _currentWeaponConfig;

	private float _speedMultiplier;

	private readonly HashSet<GameObject> _lastTouchedObjects = new HashSet<GameObject>();

	private readonly Dictionary<GameObject, Collider2D> _lastTouchedObjectsWithColliders = new Dictionary<GameObject, Collider2D>();

	private GameObject _lastCollisionObject;

	public bool IsActive
	{
		get;
		private set;
	}

	public WeaponConfig Config => _currentWeaponConfig;

	private void Awake()
	{
		_sprite = base.transform.GetComponent<SpriteRenderer>();
		_colliders = base.transform.GetComponents<PolygonCollider2D>();
		_speedMultiplier = 1f;
	}

	private void OnDestroy()
	{
		if (_shapeField != null && _shapeField.gameObject != null)
		{
			UnityEngine.Object.Destroy(_shapeField.gameObject);
			_shapeField = null;
		}
		if (_tweener != null)
		{
			_tweener.Kill();
			_tweener = null;
		}
	}

	public void ResetTouchedObjects()
	{
		if (IsActive)
		{
			UnityEngine.Debug.LogWarning("ResetTouchedObjects() while the weapon is active is dangerous. Current collision will be lost...");
		}
		_lastTouchedObjects.Clear();
		_lastTouchedObjectsWithColliders.Clear();
	}

	public void SetShape(WeaponData weapon)
	{
		if (_currentWeaponConfig == null || _currentWeaponConfig.Id != weapon.Id)
		{
			switch (weapon.SpinSpeed)
			{
			case WeaponSpeedType.Slow:
				_speedMultiplier = 1.25f;
				break;
			case WeaponSpeedType.Normal:
				_speedMultiplier = 1f;
				break;
			case WeaponSpeedType.Fast:
				_speedMultiplier = 0.75f;
				break;
			}
			bool flag = false;
			if (_shapeField != null)
			{
				flag = _shapeField.gameObject.activeSelf;
				UnityEngine.Object.Destroy(_shapeField.gameObject);
				_shapeField = null;
			}
			WeaponPrefab prefab = Resources.Load<WeaponPrefab>("Weapons/" + weapon.Id + "/" + weapon.Id);
			_sprite.sprite = prefab.WeaponShapeSprite;
			int colliderIndex = 0;
			Array.ForEach(_colliders, delegate(PolygonCollider2D collider)
			{
				if (colliderIndex < prefab.Colliders.Count)
				{
					collider.enabled = true;
					collider.offset = prefab.Colliders[colliderIndex].offset;
					collider.isTrigger = prefab.Colliders[colliderIndex].isTrigger;
					collider.points = prefab.Colliders[colliderIndex].points;
				}
				else
				{
					collider.enabled = false;
				}
				colliderIndex++;
			});
			IEnumerator enumerator = prefab.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.Current;
					Transform transform2 = base.transform.Find(transform.name);
					if (transform2 != null)
					{
						transform2.localPosition = transform.localPosition;
					}
					else
					{
						UnityEngine.Debug.LogWarning("The weapon prefab has a pivot that doesn't exist on the WeaponShape: " + transform.name);
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			_shapeField = UnityEngine.Object.Instantiate(prefab.ShapeField);
			_shapeField.transform.position = new Vector3(0f, 0f, 5f);
			if (flag)
			{
				ShowShapeField();
			}
			else
			{
				HideShapeField();
			}
			_currentWeaponConfig = weapon.Config;
		}
	}

	public void StartRotation()
	{
		base.gameObject.SetActive( true);
		Show();
		_rotationActivated = true;
		Rotate();
	}

	public void StopRotation()
	{
		_rotationActivated = false;
		if (_tweener != null)
		{
			_tweener.Kill();
			_tweener = null;
		}
	}

	public void Show()
	{
		if (!IsActive)
		{
			IsActive = true;
			_sprite.enabled = true;
			base.transform.rotation = Quaternion.identity;
			Array.ForEach(_colliders, delegate(PolygonCollider2D collider)
			{
				if (collider.enabled)
				{
					collider.enabled = false;
					collider.enabled = true;
				}
			});
		}
	}

	public void Hide()
	{
		_sprite.enabled = false;
		IsActive = false;
		HideShapeField();
	}

	public void Flash()
	{
		_sprite.DOColor(Color.white, 0.4f).SetEase(Ease.Flash, 6f);
	}

	public List<GameObject> GetLastTouchedObjects(string tag)
	{
		List<GameObject> list = new List<GameObject>();
		foreach (GameObject lastTouchedObject in _lastTouchedObjects)
		{
			if (lastTouchedObject != null && lastTouchedObject.CompareTag(tag))
			{
				list.Add(lastTouchedObject);
			}
		}
		return list;
	}

	public Dictionary<Collider2D, HashSet<GameObject>> GetLastTouchedObjectsWithColliders(string tag)
	{
		Dictionary<Collider2D, HashSet<GameObject>> dictionary = new Dictionary<Collider2D, HashSet<GameObject>>();
		foreach (KeyValuePair<GameObject, Collider2D> lastTouchedObjectsWithCollider in _lastTouchedObjectsWithColliders)
		{
			GameObject key = lastTouchedObjectsWithCollider.Key;
			if (key != null && key.CompareTag(tag))
			{
				Collider2D value = lastTouchedObjectsWithCollider.Value;
				if (value != null)
				{
					if (!dictionary.ContainsKey(value))
					{
						dictionary[value] = new HashSet<GameObject>();
					}
					dictionary[value].Add(key);
				}
			}
		}
		return dictionary;
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (!(collider.gameObject.tag == "Hero"))
		{
			_lastCollisionObject = collider.gameObject;
			if (!_lastTouchedObjects.Contains(collider.gameObject))
			{
				_lastTouchedObjects.Add(collider.gameObject);
				Array.ForEach(_colliders, delegate(PolygonCollider2D c)
				{
					if (collider != null && collider.IsTouching(c))
					{
						_lastTouchedObjectsWithColliders[collider.gameObject] = c;
					}
				});
			}
		}
	}

	private void OnTriggerStay2D(Collider2D collider)
	{
		if (!(collider.gameObject.tag == "Hero") && _lastCollisionObject == null)
		{
			OnTriggerEnter2D(collider);
		}
	}

	private void OnTriggerExit2D(Collider2D collider)
	{
		if (!(collider.gameObject.tag == "Hero"))
		{
			_lastCollisionObject = null;
			_lastTouchedObjectsWithColliders.Remove(collider.gameObject);
			if (_lastTouchedObjects.Remove(collider.gameObject))
			{
			}
		}
	}

	private void OnRotationCompleted()
	{
		_tweener = null;
		if (_rotationActivated)
		{
			Rotate();
		}
	}

	private void Rotate()
	{
		if (_tweener == null)
		{
			base.transform.rotation = Quaternion.identity;
			_tweener = base.transform.DORotate(new Vector3(0f, 0f, -360f), 1.2f * _speedMultiplier, RotateMode.FastBeyond360).SetEase(Ease.Linear).OnComplete(OnRotationCompleted);
		}
	}

	public void ShowShapeField()
	{
		if (_shapeField != null)
		{
			_shapeField.gameObject.SetActive( true);
		}
	}

	public void HideShapeField()
	{
		if (_shapeField != null)
		{
			_shapeField.gameObject.SetActive( false);
		}
	}

	public List<Vector3> GetCollidersCenter()
	{
		List<Vector3> i = new List<Vector3>();
		Array.ForEach(_colliders, delegate(PolygonCollider2D collider)
		{
			i.Add(collider.bounds.center);
		});
		return i;
	}

	public List<Bounds> GetColliderBounds()
	{
		return new List<PolygonCollider2D>(_colliders).ConvertAll((PolygonCollider2D c) => c.bounds);
	}

	public List<Collider2D> GetColliders()
	{
		return new List<Collider2D>(_colliders);
	}

	public List<Vector3> GetExtraPivotPos()
	{
		List<Vector3> list = new List<Vector3>();
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				if (transform.name.StartsWith("ExtraPivot"))
				{
					list.Add(transform.position);
				}
			}
			return list;
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}
}