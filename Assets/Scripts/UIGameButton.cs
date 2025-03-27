using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIGameButton : Button
{
	[Serializable]
	public class ButtonDownEvent : UnityEvent
	{
	}

	[Serializable]
	public class ButtonUpEvent : UnityEvent
	{
	}

	[SerializeField]
	private ButtonDownEvent _onDown = new ButtonDownEvent();

	[SerializeField]
	private ButtonUpEvent _onUp = new ButtonUpEvent();

	public bool Animate = true;

	private UIGameButtonStyle _style;

	private string _disabledExplanationText;

	private AudioClip _clickedSound;

	private bool _activateOnBackKey;

	private Tweener _scaleTween;

	public ButtonDownEvent onDown
	{
		get
		{
			return _onDown;
		}
		set
		{
			_onDown = value;
		}
	}

	public ButtonUpEvent onUp
	{
		get
		{
			return _onUp;
		}
		set
		{
			_onUp = value;
		}
	}

	protected UIGameButton()
	{
	}

	protected override void Awake()
	{
		base.Awake();
		_style = GetComponent<UIGameButtonStyle>();
		GetComponent<RectTransform>().SetPivot(Vector2.one * 0.5f);
		ColorBlock defaultColorBlock = ColorBlock.defaultColorBlock;
		defaultColorBlock.disabledColor = Color.white;
		defaultColorBlock.pressedColor = Color.white;
		base.colors = defaultColorBlock;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (_scaleTween != null)
		{
			_scaleTween.Kill();
			_scaleTween = null;
		}
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		base.OnPointerDown(eventData);
		if (eventData.button != 0)
		{
			return;
		}
		_onDown.Invoke();
		if (Animate)
		{
			if (_scaleTween != null && _scaleTween.IsActive())
			{
				_scaleTween.Complete();
			}
			_scaleTween = base.transform.DOScale(0.9f, 0.15f);
		}
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		if (eventData.button != 0)
		{
			return;
		}
		if (!eventData.dragging)
		{
			_onUp.Invoke();
		}
		if (Animate)
		{
			if (_scaleTween != null && _scaleTween.IsActive())
			{
				_scaleTween.Complete();
			}
			_scaleTween = base.transform.DOScale(1f, 0.15f);
		}
		if (!IsInteractable() && !eventData.dragging)
		{
			OnDisabledClicked(eventData.pressPosition);
		}
	}

	private void OnDisabledClicked(Vector3 pressPosition)
	{
		if (App.IsCreated() && !string.IsNullOrEmpty(_disabledExplanationText))
		{
			App.Instance.MenuEvents.OnTextOverlayRequested(_disabledExplanationText, pressPosition);
		}
	}

	public void ActivateOnBackKey()
	{
		_activateOnBackKey = true;
	}

	protected virtual void Update()
	{
		if (_activateOnBackKey && UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			_onDown.Invoke();
			_onUp.Invoke();
			if (base.onClick != null)
			{
				base.onClick.Invoke();
			}
		}
	}

	public void ClearOnDownAction()
	{
		onDown.RemoveAllListeners();
		_clickedSound = null;
	}

	public void ClearOnClickAction()
	{
		onUp.RemoveAllListeners();
		base.onClick.RemoveAllListeners();
		_clickedSound = null;
	}

	public void SetDisabledExplanation(string text)
	{
		_disabledExplanationText = text;
	}

	public void AddOnUpSound()
	{
		if (_clickedSound == null)
		{
			_clickedSound = Resources.Load<AudioClip>("buttonClicked");
			onUp.AddListener(delegate
			{
				MonoSingleton<AudioManager>.Instance.PlaySound(_clickedSound);
			});
		}
	}

	public void AddOnDownSound()
	{
		if (_clickedSound == null)
		{
			_clickedSound = Resources.Load<AudioClip>("buttonClicked");
			onDown.AddListener(delegate
			{
				MonoSingleton<AudioManager>.Instance.PlaySound(_clickedSound);
			});
		}
	}

	public void RemoveSound()
	{
		_clickedSound = null;
	}

	protected override void DoStateTransition(SelectionState state, bool instant)
	{
		base.DoStateTransition(state, instant);
		OnStateUpdated(state);
	}

	private void OnStateUpdated(SelectionState state)
	{
		if (base.image != null && _style != null)
		{
			Sprite stateSprite = _style.GetStateSprite(state == SelectionState.Disabled);
			if (stateSprite != null)
			{
				base.image.sprite = stateSprite;
			}
		}
	}
}