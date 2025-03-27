using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class CharacterEyeAttackingPhase : CharacterAttackingPhase
{
	public CharacterEyeAttackingPhase Init(Monster monster)
	{
		Init((Character)monster);
		return this;
	}

	public override void StartAttackEndPhase()
	{
		Action action = delegate
		{
			if (_character.WasGoingBackward())
			{
				_character.SetMovingBackward();
			}
			else
			{
				_character.SetMovingForward();
			}
			_character.ResetAttackAttemptCount();
			_character.OnAttackFinished();
		};
		if (_character.CurrentAttackTarget.IsPetrified())
		{
			action();
			return;
		}
		_character.Play(_character.GetAttackAnimation());
		Hero hero = _character.CurrentAttackTarget as Hero;
		if (hero != null)
		{
			hero.SetPetrifiedTurn(_character, 1);
			StartCoroutine(PetrificationCR(hero, action));
		}
	}

	private IEnumerator PetrificationCR(Character defender, Action onDefenderPetrified)
	{
		yield return null;
		Vector3 originLocalScale = defender.Visual.MovingPivot.localScale;
		defender.Visual.HideShadow();
		yield return ScaleTo(defender.Visual.MovingPivot, Vector3.zero).WaitForCompletion();
		string skinPath = CharacterVisual.GetSkinPath("HeroPetrified");
		defender.Visual.ApplySkin(skinPath);
		defender.Play(defender.GetIdleAnimId());
		yield return ScaleTo(defender.Visual.MovingPivot, originLocalScale).SetEase(Ease.InQuad).WaitForCompletion();
		defender.Events.OnPetrificationEnded(defender);
		defender.SetLocalScale(originLocalScale);
		defender.Visual.ShowShadow();
		yield return defender.Visual.MovingPivot.DOShakeRotation(0.25f, new Vector3(0f, 0f, 70f), 20, 70f).WaitForCompletion();
		yield return new WaitForSeconds(0.25f);
		onDefenderPetrified();
	}

	public Tweener ScaleTo(Transform t, Vector3 scaleTarget)
	{
		return t.DOScale(scaleTarget, 0.22f);
	}
}