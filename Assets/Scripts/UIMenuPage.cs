using DG.Tweening;
using System.Collections;
using UnityEngine;

public class UIMenuPage : UIMenu
{
	private float _defaultY;

	protected override void Awake()
	{
		base.Awake();
		RectTransform component = base.transform.Find("MainContainer").GetComponent<RectTransform>();
		Vector2 anchoredPosition = component.anchoredPosition;
		_defaultY = anchoredPosition.y;
		component.DOAnchorPosY(0f - component.rect.height, 0f);
	}

	public override IEnumerator PushAnimationCR()
	{
		RectTransform mainContainer = base.transform.Find("MainContainer").GetComponent<RectTransform>();
		if (mainContainer != null)
		{
			mainContainer.DOAnchorPosY(_defaultY, 0.25f);
		}
		yield return new WaitForSeconds(0.25f);
	}

	public override IEnumerator PopAnimationCR()
	{
		RectTransform mainContainer = base.transform.Find("MainContainer").GetComponent<RectTransform>();
		if (mainContainer != null)
		{
			mainContainer.DOAnchorPosY(0f - mainContainer.rect.height, 0.25f);
		}
		yield return new WaitForSeconds(0.25f);
	}
}