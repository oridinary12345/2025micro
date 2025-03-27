using UnityEngine;
using Utils;

public class UnityCanvasContainer : MonoBehaviour
{
	[SerializeField]
	private bool _autoInit;

	public void Awake()
	{
		if (_autoInit)
		{
			Init();
		}
	}

	public void Init()
	{
		if (Devices.HasNotch())
		{
			GetComponent<RectTransform>().offsetMin = new Vector3(0f, 10f);
			GetComponent<RectTransform>().offsetMax = new Vector3(0f, -60f);
		}
	}
}