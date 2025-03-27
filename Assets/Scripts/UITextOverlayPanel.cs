using DG.Tweening;
using TMPro;
using UnityEngine;

public class UITextOverlayPanel : MonoBehaviour
{
	[SerializeField]
	private RectTransform _canvasRect;

	[SerializeField]
	private Canvas _canvas;

	[SerializeField]
	private Camera _camera;

	[SerializeField]
	private TextMeshProUGUI _text;

	private MenuEvents _menuEvents;

	private Sequence _currentAnimation;

	public void Init(MenuEvents menuEvents)
	{
		UnityCanvasContainer componentInChildren = base.transform.GetComponentInChildren<UnityCanvasContainer>();
		if (componentInChildren != null)
		{
			componentInChildren.Init();
		}
		_menuEvents = menuEvents;
		menuEvents.TextOverlayRequestedEvent -= OnTextOverlayRequested;
		menuEvents.TextOverlayRequestedEvent += OnTextOverlayRequested;
	}

	private void OnDestroy()
	{
		if (_menuEvents != null)
		{
			_menuEvents.TextOverlayRequestedEvent -= OnTextOverlayRequested;
		}
		if (_currentAnimation != null)
		{
			_currentAnimation.Kill();
		}
	}

	private void OnTextOverlayRequested(string text, Vector3 inputPosition)
	{
		if (_currentAnimation != null)
		{
			_currentAnimation.Kill( true);
			_currentAnimation = null;
		}
		_text.text = text;
		_text.gameObject.SetActive( true);
		Vector3 localPosition = _text.GetComponent<RectTransform>().localPosition;
		Color color = new Color(1f, 1f, 1f, 0f);
		PlaceRectAtWorldPosition(_canvas, _camera, inputPosition, _canvasRect);
		localPosition = _text.GetComponent<RectTransform>().localPosition;
		_text.color = color;
		_currentAnimation = DOTween.Sequence();
		_currentAnimation.Append(_text.transform.DOLocalMoveY(localPosition.y + 20f, 2f).OnComplete(OnAnimationEnded));
		_currentAnimation.Join(_text.DOColor(Color.white, 0.2f));
		_currentAnimation.Join(_text.transform.DOScale(Vector3.one, 0.2f));
		_currentAnimation.Join(_text.DOColor(color, 0.2f).SetDelay(1.6f));
	}

	public static void PlaceRectAtWorldPosition(Canvas canvas, Camera camera, Vector2 targetPosition, RectTransform rect)
	{
		int num = (int)((float)Screen.width / canvas.scaleFactor);
		int num2 = (int)((float)Screen.height / canvas.scaleFactor);
		Vector2 anchorMax = rect.anchorMax;
		float x = (0f - anchorMax.x) * 2f * (float)num;
		Vector2 anchorMax2 = rect.anchorMax;
		Vector2 vector = new Vector2(x, (0f - anchorMax2.y) * 1f * (float)num2);
		Vector2 anchoredPosition = rect.anchoredPosition;
		anchoredPosition.y = vector.y + targetPosition.y + 90f;
		rect.anchoredPosition = anchoredPosition;
	}

	private void OnAnimationEnded()
	{
		_text.gameObject.SetActive( false);
	}
}