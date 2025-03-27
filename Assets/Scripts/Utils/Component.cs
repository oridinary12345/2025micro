using UnityEngine;

namespace Utils
{
	public static class Component
	{
		public static bool HitTest(Bounds bounds, float x, float y)
		{
			Vector3 center = bounds.center;
			float x2 = center.x;
			Vector3 extents = bounds.extents;
			int result;
			if (x >= x2 - extents.x)
			{
				Vector3 center2 = bounds.center;
				float x3 = center2.x;
				Vector3 extents2 = bounds.extents;
				if (x <= x3 + extents2.x)
				{
					Vector3 center3 = bounds.center;
					float y2 = center3.y;
					Vector3 extents3 = bounds.extents;
					if (y >= y2 - extents3.y)
					{
						Vector3 center4 = bounds.center;
						float y3 = center4.y;
						Vector3 extents4 = bounds.extents;
						result = ((y <= y3 + extents4.y) ? 1 : 0);
						goto IL_009b;
					}
				}
			}
			result = 0;
			goto IL_009b;
			IL_009b:
			return (byte)result != 0;
		}

		public static GameObject GetChild(this GameObject component, string name)
		{
			UnityEngine.Component[] componentsInChildren = component.GetComponentsInChildren(typeof(Transform), true);
			UnityEngine.Component[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				Transform transform = (Transform)array[i];
				if (transform.gameObject.name == name)
				{
					return transform.gameObject;
				}
			}
			return null;
		}
	}
}