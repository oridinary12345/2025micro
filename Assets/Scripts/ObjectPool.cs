using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class ObjectPool<ObjectType> where ObjectType : class
{
	private int _size;

	private bool _isDynamic;

	private Func<ObjectType> _create;

	private List<ObjectType> _available;

	private List<ObjectType> _inUse;

	private string _name;

	public int InUseCount => _inUse.Count;

	public int AvailableCount => _available.Count;

	public ObjectPool(Func<ObjectType> create, string name, int size = 0, bool isDynamic = true)
	{
		_name = name;
		if (!isDynamic && size <= 0)
		{
			throw new ArgumentOutOfRangeException("size", "Size must be larger than zero if pool is not dynamic.");
		}
		_available = new List<ObjectType>(size);
		_inUse = new List<ObjectType>();
		_size = size;
		_isDynamic = isDynamic;
		_create = create;
		PopulatePool();
	}

	public ObjectType GetItemAt(int index)
	{
		return _inUse[index];
	}

	public ObjectType GetObject()
	{
		lock (_available)
		{
			int num = _available.Count - 1;
			ObjectType val;
			if (num >= 0)
			{
				val = _available[num];
				_available.RemoveAt(num);
			}
			else
			{
				if (!_isDynamic)
				{
					return (ObjectType)null;
				}
				UnityEngine.Debug.LogWarning("PoolObjet isn't big enough for " + _name);
				val = _create();
			}
			_inUse.Add(val);
			return val;
		}
	}

	public void ReleaseObject(ObjectType obj)
	{
		lock (_available)
		{
			_inUse.Remove(obj);
			_available.Add(obj);
		}
	}

	private void PopulatePool()
	{
		for (int num = _size; num >= 0; num--)
		{
			_available.Add(_create());
		}
	}
}