using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoExtensions
{
	public static void Execute(float delay, Action action)
	{
		StartCoroutine(ExecuteCR(delay, action));
	}

	public static void Execute(this MonoBehaviour mono, float delay, Action action)
	{
		mono.StartCoroutine(ExecuteCR(delay, action));
	}

	private static IEnumerator ExecuteCR(float delay, Action action)
	{
		if (delay > 0f)
		{
			yield return new WaitForSeconds(delay);
		}
		action();
		yield return null;
	}

	public static MonoBehaviour StartCoroutine(IEnumerator coroutine)
	{
		GameObject gameObject = new GameObject("MonoExtensions");
		MonoBehaviour monoBehaviour = gameObject.AddComponent<EmptyBehaviour>();
		monoBehaviour.StartCoroutine(StartCR(monoBehaviour, coroutine));
		return monoBehaviour;
	}

	private static IEnumerator StartCR(MonoBehaviour mono, IEnumerator coroutine)
	{
		yield return mono.StartCoroutine(coroutine);
		UnityEngine.Object.Destroy(mono.gameObject);
	}

	public static List<float> ToFloatList(this ArrayList array)
	{
		List<float> list = null;
		if (array != null)
		{
			list = new List<float>(array.Count);
			IEnumerator enumerator = array.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.Current;
					float item = 0f;
					if (current is float)
					{
						item = (float)current;
					}
					else if (current is double)
					{
						item = (float)(double)current;
					}
					list.Add(item);
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
		return new List<float>(0);
	}

	public static List<int> ToIntList(this ArrayList array)
	{
		List<int> list = null;
		if (array != null)
		{
			list = new List<int>(array.Count);
			IEnumerator enumerator = array.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.Current;
					int item = 0;
					if (current is float)
					{
						item = (int)current;
					}
					else if (current is double)
					{
						item = (int)(double)current;
					}
					list.Add(item);
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
		return new List<int>(0);
	}
}