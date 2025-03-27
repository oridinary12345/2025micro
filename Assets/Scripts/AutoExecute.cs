using System;
using UnityEngine;

public class AutoExecute : MonoBehaviour
{
	private Action _toExecute;

	public void Init(float time, Action action)
	{
		_toExecute = action;
		Invoke("Execute", time);
	}

	private void Execute()
	{
		if (base.gameObject != null && _toExecute != null)
		{
			_toExecute();
		}
	}
}