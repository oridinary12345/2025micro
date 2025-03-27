using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

public static class Extensions
{
	public static void Shuffle<T>(this IList<T> list)
	{
		RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
		int num = list.Count;
		while (num > 1)
		{
			byte[] array = new byte[1];
			do
			{
				rNGCryptoServiceProvider.GetBytes(array);
			}
			while (array[0] >= num * (255 / num));
			int index = (int)array[0] % num;
			num--;
			T value = list[index];
			list[index] = list[num];
			list[num] = value;
		}
	}

	public static T Pick<T>(this List<T> list)
	{
		if (list.Count == 0)
		{
			return default(T);
		}
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	public static void OnClick(this UIGameButton button, UnityAction onClick)
	{
		button.onClick.RemoveListener(onClick);
		button.onClick.AddListener(onClick);
		button.AddOnUpSound();
	}

	public static void OnDown(this UIGameButton button, UnityAction onDown, bool playSound = true)
	{
		button.onDown.AddListener(onDown);
		if (playSound)
		{
			button.AddOnDownSound();
		}
	}

	public static void SetLayer(this GameObject go, int layer)
	{
		go.layer = layer;
		Transform transform = go.transform;
		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).gameObject.SetLayer(layer);
		}
	}

	public static void SetSortingOrder(this GameObject go, int order)
	{
		SpriteRenderer[] componentsInChildren = go.GetComponentsInChildren<SpriteRenderer>( true);
		if (componentsInChildren.Length > 0)
		{
			SpriteRenderer[] array = componentsInChildren;
			foreach (SpriteRenderer spriteRenderer in array)
			{
				spriteRenderer.sortingOrder = order;
			}
		}
	}

	public static Vector2 Clamp(this Vector2 vector)
	{
		float val = Math.Abs(vector.x);
		val = Math.Max(val, Math.Abs(vector.y));
		return vector / val;
	}

	public static Color ToColor(this string hexColor)
	{
 Color color;		ColorUtility.TryParseHtmlString(hexColor.ToUpper(), out color);
		return color;
	}

	public static Vector2 XY(this Vector3 vector)
	{
		return new Vector2(vector.x, vector.y);
	}

	public static T GetCopyOf<T>(this Component comp, T other) where T : Component
	{
		Type type = comp.GetType();
		if (type != other.GetType())
		{
			return (T)null;
		}
		BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
		PropertyInfo[] properties = type.GetProperties(bindingAttr);
		PropertyInfo[] array = properties;
		foreach (PropertyInfo propertyInfo in array)
		{
			if (propertyInfo.CanWrite)
			{
				try
				{
					propertyInfo.SetValue(comp, propertyInfo.GetValue(other, null), null);
				}
				catch
				{
				}
			}
		}
		FieldInfo[] fields = type.GetFields(bindingAttr);
		FieldInfo[] array2 = fields;
		foreach (FieldInfo fieldInfo in array2)
		{
			fieldInfo.SetValue(comp, fieldInfo.GetValue(other));
		}
		return comp as T;
	}

	public static string ToUpperFirst(this string s)
	{
		if (string.IsNullOrEmpty(s))
		{
			return s;
		}
		return s.Replace(s.Substring(0, 1), s.Substring(0, 1).ToUpper());
	}
}