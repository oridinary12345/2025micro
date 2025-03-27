using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CharacterLeechAttackingPhase : CharacterAttackingPhase
{
	private Monster _leech;

	private Vector3 _originLocalScale;

	private int _damageDealt;

	public CharacterLeechAttackingPhase Init(Monster leech)
	{
		Init((Character)leech);
		_leech = leech;
		return this;
	}

	public override void StartAttackEndPhase()
	{
		_character.Events.OnSipped();
		int damage = _character.GetDamage();
		_character.Play(_character.GetAttackAnimation());
		_damageDealt = _character.CurrentAttackTarget.TakeDamage(_character, _character.GetCurrentWeapon(), damage, _character.GetLastJumpPosition());
		bool flag = _damageDealt == 0;
		if (!flag)
		{
			Vector3 currentLocalScale = _leech.Visual.MovingPivot.localScale;
			Sequence sequence = DOTween.Sequence();
			Transform movingPivot = _leech.Visual.MovingPivot;
			sequence.Append(movingPivot.DOScale(new Vector3(currentLocalScale.x * 0.7f, currentLocalScale.y * 1.1f, currentLocalScale.z), 0.12f));
			sequence.Append(movingPivot.DOScale(new Vector3(currentLocalScale.x * 1.3f, currentLocalScale.y * 1f, currentLocalScale.z), 0.12f));
			sequence.Append(movingPivot.DOScale(currentLocalScale, 0.1f));
			sequence.OnComplete(delegate
			{
				_leech.Visual.MovingPivot.localScale = currentLocalScale;
			});
		}
		_character.SetMovingBackward();
		_character.ResetAttackAttemptCount();
		float delay = (!flag) ? 1f : 0.25f;
		this.Execute(delay, ((CharacterAttackingPhase)this).MoveBackToPosition);
	}

	public override void MoveTowardOpponent()
	{
		StartCoroutine(MoveTowardOpponentCR());
	}

	private IEnumerator MoveTowardOpponentCR()
	{
		_originLocalScale = _leech.Visual.MovingPivot.localScale;
		_damageDealt = 0;
		yield return new WaitForSeconds(0.25f);
		_leech.Visual.HideUI();
		_leech.Visual.HideShadow();
		Vector3 endPos = new Vector3(0f, -0.1f, -2f);
		_leech.Visual.MovingPivot.transform.position = endPos;
		StartAttackEndPhase();
	}

	public override void MoveBackToPosition()
	{
		StartCoroutine(MoveBackToPositionCR());
	}

	private IEnumerator MoveBackToPositionCR()
	{
		_character.Play(AnimationState.Idle);
		_leech.SetPosition(_leech.CurrentCell.ToMapPos());
		_leech.SetLocalScale(_originLocalScale);
		Vector3 currentLocalScale = _leech.Visual.MovingPivot.localScale;
		Sequence sequence = DOTween.Sequence();
		Transform t = _leech.Visual.MovingPivot;
		sequence.Append(t.DOScale(new Vector3(currentLocalScale.x * 1.2f, currentLocalScale.y * 0.7f, currentLocalScale.z), 0.15f));
		sequence.Append(t.DOScale(new Vector3(currentLocalScale.x * 0.8f, currentLocalScale.y * 1.2f, currentLocalScale.z), 0.15f));
		sequence.Append(t.DOScale(currentLocalScale, 0.15f));
		sequence.OnComplete(delegate
		{
			_leech.Visual.MovingPivot.localScale = currentLocalScale;
		});
		if (_damageDealt > 0)
		{
			_character.Heal(_damageDealt);
		}
		yield return new WaitForSeconds(1f);
		_leech.Visual.ShowUI();
		_leech.Visual.ShowShadow();
		_character.OnAttackFinished();
	}

	public Tweener ScaleTo(Vector3 scaleTarget)
	{
		return _leech.Visual.MovingPivot.DOScale(scaleTarget, 0.25f);
	}
}