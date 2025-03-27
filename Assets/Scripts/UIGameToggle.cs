using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIGameToggle : Toggle
{
	public bool Animate = true;

	private float _defaultY = float.MaxValue;

	protected UIGameToggle()
	{
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		base.OnPointerDown(eventData);
		if (Animate)
		{
			if (_defaultY == float.MaxValue)
			{
				Vector3 localPosition = base.transform.localPosition;
				_defaultY = localPosition.y;
			}
			base.transform.DOLocalMoveY(_defaultY - 10f, 0.15f);
		}
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		if (Animate)
		{
			base.transform.DOLocalMoveY(_defaultY, 0.15f);
		}
	}
}