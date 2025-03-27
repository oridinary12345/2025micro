using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuPopup : UIMenu
{
	private const float _animDuration = 0.15f;

	public override IEnumerator PushAnimationCR()
	{
		Image maskImage = base.transform.FindChildComponent<Image>("BGMask");
		if (maskImage != null)
		{
			Image image = maskImage;
			Color color = maskImage.color;
			float r = color.r;
			Color color2 = maskImage.color;
			float g = color2.g;
			Color color3 = maskImage.color;
			image.color = new Color(r, g, color3.b, 0f);
			maskImage.DOFade(0.85f, 0.15f);
		}
		Transform mainContainer = base.transform.Find("MainContainer");
		if (mainContainer != null)
		{
			mainContainer.DOScale(0.85f, 0f);
			mainContainer.DOScale(1f, 0.15f);
			CanvasGroup groupCanvas = mainContainer.GetComponent<CanvasGroup>();
			if (groupCanvas != null)
			{
				groupCanvas.alpha = 0f;
				DOTween.To(() => groupCanvas.alpha, delegate(float a)
				{
					groupCanvas.alpha = a;
				}, 1f, 0.15f);
			}
		}
		yield return new WaitForSeconds(0.2f);
	}

	public override IEnumerator PopAnimationCR()
	{
		Image maskImage = base.transform.FindChildComponent<Image>("BGMask");
		if (maskImage != null)
		{
			maskImage.DOFade(0f, 0.15f);
		}
		Transform mainContainer = base.transform.Find("MainContainer");
		if (mainContainer != null)
		{
			mainContainer.DOScale(0.85f, 0.15f);
			CanvasGroup groupCanvas = mainContainer.GetComponent<CanvasGroup>();
			if (groupCanvas != null)
			{
				groupCanvas.alpha = 1f;
				DOTween.To(() => groupCanvas.alpha, delegate(float a)
				{
					groupCanvas.alpha = a;
				}, 0f, 0.15f);
			}
		}
		yield return new WaitForSeconds(0.2f);
	}
}