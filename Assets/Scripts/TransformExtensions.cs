using System;
using System.Collections;
using UnityEngine;

public static class TransformExtensions
{
	public static void SetPivot(this RectTransform rectTransform, Vector2 pivot)
	{
		if (!(rectTransform == null))
		{
			Vector2 size = rectTransform.rect.size;
			Vector2 vector = rectTransform.pivot - pivot;
			Vector3 vector2 = new Vector3(vector.x * size.x, vector.y * size.y);
			rectTransform.pivot = pivot;
			rectTransform.localPosition -= vector2;
		}
	}

	public static float GetHalfWidth(this Transform t)
	{
		if (t != null)
		{
			Vector3 extents = t.ComputeBounds().extents;
			return extents.x;
		}
		return 0f;
	}

	public static float GetWidth(this Transform t)
	{
		return t.GetHalfWidth() * 2f;
	}

	public static float GetHeight(this Transform t)
	{
		if (t != null)
		{
			Vector3 size = t.ComputeBounds().size;
			return size.y;
		}
		return 0f;
	}

	public static Bounds ComputeBounds(this Transform t)
	{
		Bounds result = new Bounds(t.position, Vector3.zero);
		MeshRenderer[] componentsInChildren = t.GetComponentsInChildren<MeshRenderer>();
		if (componentsInChildren.Length > 0)
		{
			MeshRenderer[] array = componentsInChildren;
			foreach (MeshRenderer meshRenderer in array)
			{
				result.Encapsulate(meshRenderer.bounds);
			}
		}
		SpriteRenderer[] componentsInChildren2 = t.GetComponentsInChildren<SpriteRenderer>();
		if (componentsInChildren2.Length > 0)
		{
			SpriteRenderer[] array2 = componentsInChildren2;
			foreach (SpriteRenderer spriteRenderer in array2)
			{
				result.Encapsulate(spriteRenderer.bounds);
			}
		}
		return result;
	}

	public static T FindChildComponent<T>(this Transform t, string childName) where T : MonoBehaviour
	{
		Transform transform = t.Find(childName);
		if (transform != null)
		{
			return transform.GetComponent<T>();
		}
		return (T)null;
	}

	public static void Berp(this Transform t, float scale, float duration)
	{
		MonoExtensions.StartCoroutine(AnimatePingPong(t, scale, duration));
	}

	public static void BerpIn(this Transform t, float duration)
	{
		Vector3 localScale = t.localScale;
		BerpIn(t, duration, localScale);
	}

	public static void BerpIn(this MonoBehaviour mono, float duration)
	{
		Vector3 localScale = mono.transform.localScale;
		mono.BerpIn(duration, localScale);
	}

	public static void BerpIn(this MonoBehaviour mono, float duration, Vector3 targetScale)
	{
		mono.StartCoroutine(AnimateBerpCR(start: new Vector3(0.1f, 0.1f, 0.1f), t: mono.transform, end: targetScale, duration: duration));
	}

	public static void BerpIn(Transform t, float duration, Vector3 targetScale)
	{
		Vector3 start = new Vector3(0.1f, 0.1f, 0.1f);
		MonoExtensions.StartCoroutine(AnimateBerpCR(t, start, targetScale, duration));
	}

	private static IEnumerator AnimatePingPong(Transform t, float scale, float duration)
	{
		if (t != null)
		{
			Vector3 startingPos = t.localScale;
			Vector3 targetPos = startingPos * scale;
			yield return MonoExtensions.StartCoroutine(AnimateBerpCR(t, startingPos, targetPos, duration));
			yield return MonoExtensions.StartCoroutine(AnimateBerpCR(t, targetPos, startingPos, duration));
		}
	}

	private static IEnumerator AnimateBerpCR(Transform t, Vector3 start, Vector3 end, float duration)
	{
		float timer = 0f;
		while (timer < duration && t != null)
		{
			float x = Mathfx.Berp(start.x, end.x, Mathf.Clamp01(timer / duration));
			float y = Mathfx.Berp(start.y, end.y, Mathf.Clamp01(timer / duration));
			if (t != null)
			{
				t.localScale = new Vector3(x, y, end.z);
			}
			else
			{
				UnityEngine.Debug.Log("Trying to execute a Berp anim on a destroyed transform");
			}
			timer += Time.deltaTime;
			yield return null;
		}
		if (t != null)
		{
			t.localScale = end;
		}
		else
		{
			UnityEngine.Debug.Log("Trying to execute a Berp anim on a destroyed transform");
		}
	}

	public static IEnumerator Stamp(this Transform t, float duration, float startScale, Shaker shaker = null)
	{
		yield return MonoExtensions.StartCoroutine(StampDownPivotCR(t, duration, startScale, shaker));
	}

	private static IEnumerator StampDownPivotCR(Transform t, float duration, float startScale, Shaker shaker)
	{
		Vector3 targetScale = t.localScale;
		float timer = 0f;
		bool almostOver = false;
		while (timer < duration)
		{
			float currenTime = timer / duration;
			t.localScale = Vector3.one * Mathf.Lerp(startScale, targetScale.x, currenTime);
			if (!almostOver && currenTime > 0.5f)
			{
				almostOver = true;
			}
			timer += Time.deltaTime;
			yield return null;
		}
		if (shaker != null)
		{
			shaker.Shake();
		}
		t.localScale = targetScale;
		yield return new WaitForSeconds(0.2f);
	}

	public static void ActiveRecursively(this GameObject rootObject, bool active)
	{
		rootObject.SetActive(active);
		IEnumerator enumerator = rootObject.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				transform.gameObject.ActiveRecursively(active);
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
	}
}