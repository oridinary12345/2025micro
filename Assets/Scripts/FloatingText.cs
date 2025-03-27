using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
	[SerializeField]
	private TextMeshPro _label;

	private Tweener _movingTween;

	private Tweener _scalingTween;

	public static FloatingText Create(string text, string prefabName, Vector3 startPosition, Vector3 endPosition, FloatingTextFadeOutType fadeOut = FloatingTextFadeOutType.WhileMoving)
	{
		FloatingText floatingText = UnityEngine.Object.Instantiate(Resources.Load<FloatingText>(prefabName));
		FloatingText floatingText2 = floatingText.Init(text, startPosition, Color.white);
		floatingText2.StartCoroutine(floatingText2.FloatingUpCR(endPosition, fadeOut));
		return floatingText2;
	}

	public static FloatingText CreateGoingUp(string text, Vector3 startPosition, Color color)
	{
		Vector3 endPosition = startPosition;
		endPosition.y += 0.6f;
		return CreateGoingUp(text, startPosition, endPosition, color);
	}

	public static FloatingText CreateGoingUp(string text, Vector3 startPosition, Vector3 endPosition, Color color)
	{
		FloatingText floatingText = UnityEngine.Object.Instantiate(Resources.Load<FloatingText>("FloatingText"));
		FloatingText floatingText2 = floatingText.Init(text, startPosition, color);
		floatingText2.StartCoroutine(floatingText2.FloatingUpCR(endPosition));
		return floatingText2;
	}

	public static FloatingText CreateScaleUp(string text, Vector3 startPosition, Color startColor)
	{
		FloatingText floatingText = UnityEngine.Object.Instantiate(Resources.Load<FloatingText>("FloatingText"));
		FloatingText floatingText2 = floatingText.Init(text, startPosition, startColor);
		floatingText2.StartCoroutine(floatingText2.FloatingScaleUpCR());
		return floatingText2;
	}

	private FloatingText Init(string text, Vector3 startPosition, Color startColor)
	{
		_label.text = text;
		_label.color = startColor;
		base.transform.position = startPosition;
		return this;
	}

	private IEnumerator FloatingUpCR(Vector3 endPosition, FloatingTextFadeOutType fadeOut = FloatingTextFadeOutType.WhileMoving)
	{
		float fadingDelay = (fadeOut != 0) ? 0.65f : 0.13f;
		float fadingDuration = (fadeOut != 0) ? 0.3f : (0.65f - fadingDelay);
		Color startingColor = _label.color;
		Color endingColor = startingColor;
		endingColor.a = 0f;
		_movingTween = base.transform.DOMove(endPosition, 0.585f);
		yield return new WaitForSeconds(fadingDelay);
		float timer = 0f;
		while (timer < fadingDuration)
		{
			yield return null;
			timer += Time.deltaTime;
			_label.color = Color.Lerp(startingColor, endingColor, timer / fadingDuration);
		}
		if (_movingTween.IsActive())
		{
			_movingTween.Kill();
		}
		_movingTween = null;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private IEnumerator FloatingScaleUpCR()
	{
		Color startingColor = _label.color;
		Color endingColor = startingColor;
		endingColor.a = 0f;
		_scalingTween = base.transform.DOScale(Vector3.one * 1.5f, 1.05f).SetUpdate( true);
		float timer = 0f;
		while (timer < 1.5f)
		{
			yield return null;
			timer += Time.deltaTime;
			if (!(timer < 1.35f))
			{
				_label.color = Color.Lerp(startingColor, endingColor, (timer - 1.35f) / 1.5f);
			}
		}
		if (_scalingTween.IsActive())
		{
			_scalingTween.Kill();
		}
		_scalingTween = null;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnDestroy()
	{
		if (_scalingTween != null)
		{
			_scalingTween.Kill();
			_scalingTween = null;
		}
		if (_movingTween != null)
		{
			_movingTween.Kill();
			_movingTween = null;
		}
	}
}