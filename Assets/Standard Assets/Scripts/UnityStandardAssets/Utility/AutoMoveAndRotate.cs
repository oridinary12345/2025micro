using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
	public class AutoMoveAndRotate : MonoBehaviour
	{
		[Serializable]
		public class Vector3andSpace
		{
			public Vector3 value;

			public Space space = Space.Self;
		}

		public Vector3andSpace moveUnitsPerSecond;

		public Vector3andSpace rotateDegreesPerSecond;

		public bool ignoreTimescale;

		private float m_LastRealTime;

		private void Start()
		{
			m_LastRealTime = Time.realtimeSinceStartup;
		}

		private void Update()
		{
			float d = Time.deltaTime;
			if (ignoreTimescale)
			{
				d = Time.realtimeSinceStartup - m_LastRealTime;
				m_LastRealTime = Time.realtimeSinceStartup;
			}
			base.transform.Translate(moveUnitsPerSecond.value * d, moveUnitsPerSecond.space);
			base.transform.Rotate(rotateDegreesPerSecond.value * d, moveUnitsPerSecond.space);
		}
	}
}