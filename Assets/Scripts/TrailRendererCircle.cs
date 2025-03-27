using System;
using UnityEngine;

public class TrailRendererCircle : MonoBehaviour
{
	private LineRenderer _line;

	[SerializeField]
	private int _segments = 100;

	[SerializeField]
	private float _radiusX = 1f;

	[SerializeField]
	private float _radiusY = 1f;

	private void Start()
	{
		_line = base.gameObject.GetComponent<LineRenderer>();
		_line.positionCount = _segments + 1;
		_line.useWorldSpace = false;
		CreatePoints();
	}

	private void CreatePoints()
	{
		float z = 0f;
		float num = 20f;
		for (int i = 0; i < _segments + 1; i++)
		{
			float x = Mathf.Sin((float)Math.PI / 180f * num) * _radiusX;
			float y = Mathf.Cos((float)Math.PI / 180f * num) * _radiusY;
			_line.SetPosition(i, new Vector3(x, y, z));
			num += 360f / (float)_segments;
		}
	}
}