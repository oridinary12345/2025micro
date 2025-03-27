using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class UISliderLimited : MonoBehaviour
{
	[SerializeField]
	private int _valueLimit;

	private Slider _slider;

	private void Awake()
	{
		_slider = GetComponent<Slider>();
		_slider.onValueChanged.AddListener(OnSliderValueChanged);
	}

	private void Update()
	{
		if (_slider.value > (float)_valueLimit)
		{
			_slider.value = _valueLimit;
		}
	}

	private void OnSliderValueChanged(float value)
	{
	}

	public void SetLimit(int limit)
	{
		_valueLimit = limit;
	}
}