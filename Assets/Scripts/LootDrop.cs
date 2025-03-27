using DG.Tweening;
using System;
using UnityEngine;
using Utils;

public class LootDrop : MonoBehaviour
{
	private bool _isAnimating;

	private bool _isReadyForCollecting;

	private int _roundDuration;

	private Action _onCollect;

	private Vector3 _collectingTarget;

	private GameEvents _gameEvents;

	private GameObject _target;

	private SpriteRenderer _shadowSelect;

	private string _lootId;

	private Sequence _colorTween;

	private Tweener _scaleTween;

	public bool IsCard
	{
		get;
		private set;
	}

	public event Action<LootDrop> AnimationStartedEvent;

	public event Action<LootDrop> ExpiredEvent;

	public LootDrop Init(Action onCollect, Vector3 collectingTarget, int roundDuration, GameEvents gameEvents, string lootId, bool isCard)
	{
		_onCollect = onCollect;
		_collectingTarget = collectingTarget;
		_roundDuration = roundDuration;
		_gameEvents = gameEvents;
		_target = base.gameObject.GetChild("Target");
		_lootId = lootId;
		GameObject child = base.gameObject.GetChild("ShadowSelect");
		if (child != null)
		{
			_shadowSelect = child.GetComponent<SpriteRenderer>();
		}
		IsCard = isCard;
		_gameEvents.RoundStartedEvent += OnRoundStarted;
		return this;
	}

	public void Collect()
	{
		if (!_isAnimating && _isReadyForCollecting)
		{
			if (_target != null)
			{
				_target.SetActive( false);
			}
			if (_shadowSelect != null)
			{
				_shadowSelect.enabled = false;
			}
			if (this.AnimationStartedEvent != null)
			{
				this.AnimationStartedEvent(this);
			}
			GetComponent<SpriteRenderer>().sortingOrder = 105;
			_isAnimating = true;
			DOTween.Kill(base.transform, true);
			base.transform.DOMove(_collectingTarget, 0.4f).OnComplete(OnLootCollected);
		}
	}

	private void OnLootCollected()
	{
		if (_onCollect != null)
		{
			_onCollect();
		}
		_gameEvents.OnLootPickUp(_lootId, IsCard);
		base.gameObject.SetActive( false);
		Invoke("OnDestroyReady", 1f);
	}

	private void OnDestroyReady()
	{
		Destroy();
	}

	public void ForceCollect()
	{
		_isReadyForCollecting = true;
		Collect();
	}

	public void OnLootDropped()
	{
		_isReadyForCollecting = true;
	}

	private void Destroy()
	{
		_gameEvents.RoundStartedEvent -= OnRoundStarted;
		UnityEngine.Object.Destroy(base.transform.parent.gameObject);
	}

	private void OnDestroy()
	{
		if (_colorTween != null && _colorTween.IsActive())
		{
			_colorTween.Kill();
		}
		if (_scaleTween != null && _scaleTween.IsActive())
		{
			_scaleTween.Kill();
		}
		DOTween.Kill(base.transform);
		DOTween.Kill(base.transform.parent);
	}

	private void OnRoundStarted(int roundCount)
	{
		_roundDuration--;
		if (_roundDuration == 0)
		{
			_colorTween = DOTween.Sequence();
			_colorTween.Append(base.transform.GetComponent<SpriteRenderer>().DOColor(new Color(1f, 1f, 1f, 0f), 0.5f));
			_colorTween.Append(base.transform.GetComponent<SpriteRenderer>().DOColor(new Color(1f, 1f, 1f, 1f), 0.5f));
			_colorTween.SetLoops(-1);
			_colorTween.OnKill(delegate
			{
				_colorTween = null;
			});
			_colorTween.Play();
		}
		else if (_roundDuration < 0)
		{
			_scaleTween = base.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBounce).OnComplete(Destroy)
				.OnKill(delegate
				{
					_scaleTween = null;
				});
			if (this.ExpiredEvent != null)
			{
				this.ExpiredEvent(this);
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		SpriteRenderer component = collider.gameObject.GetComponent<SpriteRenderer>();
		if (component != null && component.enabled && collider.gameObject.tag == "HeroWeapon")
		{
			_target.SetActive( true);
			if (_shadowSelect != null)
			{
				_shadowSelect.enabled = true;
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collider)
	{
		SpriteRenderer component = collider.gameObject.GetComponent<SpriteRenderer>();
		if (component != null && component.enabled && collider.gameObject.tag == "HeroWeapon")
		{
			_target.SetActive( false);
			if (_shadowSelect != null)
			{
				_shadowSelect.enabled = false;
			}
		}
	}
}