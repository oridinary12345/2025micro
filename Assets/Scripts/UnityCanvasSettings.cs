using UnityEngine;
using UnityEngine.UI;

public class UnityCanvasSettings : MonoBehaviour
{
	[SerializeField]
	private CanvasScaler _scaler;

	[SerializeField]
	private Camera _camera;

	private void Awake()
	{
		if (_camera.aspect < 0.5294118f)
		{
			_scaler.matchWidthOrHeight = 0f;
			_camera.orthographicSize = 7.5f;
		}
	}
}