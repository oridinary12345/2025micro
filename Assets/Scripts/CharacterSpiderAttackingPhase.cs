using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CharacterSpiderAttackingPhase : CharacterAttackingPhase
{
	private Monster _spider;

	private Vector3 _originLocalScale;

	public CharacterSpiderAttackingPhase Init(Monster spider)
	{
		Init((Character)spider);
		_spider = spider;
		return this;
	}

	public override void StartAttackEndPhase()
	{
		int damage = _character.GetDamage();
		_character.Play(_character.GetAttackAnimation());
		_character.CurrentAttackTarget.TakeDamage(_character, _character.GetCurrentWeapon(), damage, _character.GetLastJumpPosition());
		if (_character.WasGoingBackward())
		{
			_character.SetMovingBackward();
		}
		else
		{
			_character.SetMovingForward();
		}
		_character.ResetAttackAttemptCount();
		this.Execute(0.4f, ((CharacterAttackingPhase)this).MoveBackToPosition);
	}

	public override void MoveTowardOpponent()
	{
		StartCoroutine(MoveTowardOpponentCR());
	}

	private IEnumerator MoveTowardOpponentCR()
	{
		_originLocalScale = _spider.Visual.MovingPivot.localScale;
		_spider.Visual.HideUI();
		_spider.Visual.HideShadow();
		yield return ScaleTo(Vector3.zero).WaitForCompletion();
		float halfScreenHeight = Camera.main.orthographicSize;
		Vector3 startPos = new Vector3(0f, halfScreenHeight + 3f, -20f);
		TrailRendererLine trail = _spider.transform.GetComponentInChildren<TrailRendererLine>();
		if (trail == null)
		{
			trail = Object.Instantiate(Resources.Load<TrailRendererLine>("GameFX/TrailRendererLine"));
			trail.transform.parent = _spider.transform;
		}
		trail.Init(startPos, _spider.Visual.MovingPivot);
		Vector3 endPos = new Vector3(0f, 1.2f, -20f);
		_spider.Visual.MovingPivot.transform.position = startPos;
		_spider.Visual.MovingPivot.localScale = _originLocalScale;
		Vector3 lookAt = _character.GetPosition();
		lookAt.x -= 10f;
		_character.LookAt(lookAt);
		yield return new WaitForSeconds(0.1f);
		trail.StartFollow();
		Vector3 preEnd = endPos;
		preEnd.y = halfScreenHeight * 0.75f;
		yield return _spider.Visual.MovingPivot.DOMove(preEnd, 0.4f).SetEase(Ease.InQuad).WaitForCompletion();
		yield return _spider.Visual.MovingPivot.DOMove(endPos, 0.2f).SetEase(Ease.OutBack).WaitForCompletion();
		yield return new WaitForSeconds(0.1f);
		StartAttackEndPhase();
	}

	public override void MoveBackToPosition()
	{
		StartCoroutine(MoveBackToPositionCR());
	}

	private IEnumerator MoveBackToPositionCR()
	{
		_character.Play(AnimationState.Idle);
		float halfScreenHeight = Camera.main.orthographicSize;
		Vector3 position = _spider.Visual.MovingPivot.transform.position;
		yield return ShortcutExtensions.DOMove(endValue: new Vector3(0f, position.y - 0.25f, -20f), target: _spider.Visual.MovingPivot, duration: 0.15f).WaitForCompletion();
		yield return ShortcutExtensions.DOMove(endValue: new Vector3(0f, halfScreenHeight + 3f, -20f), target: _spider.Visual.MovingPivot, duration: 0.5f).SetEase(Ease.InQuart).WaitForCompletion();
		TrailRendererLine trail = _spider.transform.GetComponentInChildren<TrailRendererLine>();
		trail.StopFollow();
		yield return new WaitForSeconds(0.1f);
		_spider.SetPosition(_spider.CurrentCell.ToMapPos());
		_spider.Visual.MovingPivot.localScale = Vector3.zero;
		yield return ScaleTo(_originLocalScale).WaitForCompletion();
		_spider.SetLocalScale(_originLocalScale);
		_spider.Visual.ShowUI();
		_spider.Visual.ShowShadow();
		_character.OnAttackFinished();
	}

	public Tweener ScaleTo(Vector3 scaleTarget)
	{
		return _spider.Visual.MovingPivot.DOScale(scaleTarget, 0.25f);
	}
}