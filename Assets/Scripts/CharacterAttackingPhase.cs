using System;
using UnityEngine;

public class CharacterAttackingPhase : MonoBehaviour
{
	protected Character _character;

	public CharacterAttackingPhase Init(Character character)
	{
		_character = character;
		return this;
	}

	public virtual void StartAttackEndPhase()
	{
		int damage = _character.GetDamage();
		_character.Play(_character.GetAttackAnimation());
		Action action = delegate
		{
			Monster monster = _character.CurrentAttackTarget as Monster;
			if (monster == null || !monster.HasTeleported() || _character.IsCounterAttacking())
			{
				int num = 0;
				if (!_character.IsFireballAttack())
				{
					num = _character.CurrentAttackTarget.TakeDamage(_character, _character.GetCurrentWeapon(), damage, _character.GetLastJumpPosition());
				}
				int num2 = (_character.GetComboDamageBonus() <= 1) ? ((num > 0) ? _character.CurrentAttackTarget.DamageToWeapon : 0) : 0;
				if (num2 > 0)
				{
					_character.WeaponTakingDamage(num2);
				}
			}
			_character.PlayAttackLandingAnimation(_character.OnAttackFinished);
			if (!_character.IsFireballAttack() || _character.WasGoingBackward())
			{
				if (_character.PendingTargets != null && _character.PendingTargets.Count == 0)
				{
					_character.SetMovingBackward();
				}
			}
			else
			{
				_character.SetMovingForward();
			}
			_character.ResetAttackAttemptCount();
		};
		float delay = (!_character.IsRangedWeapon()) ? 0f : 0.25f;
		MonoExtensions.Execute(delay, action);
		_character.MaybeThrowProjectiles(damage, _character.CurrentAttackTarget, _character.CurrentAttackTarget.GetPosition(), 0.2f);
	}

	private bool IsCharacterAttacking()
	{
		return _character.PendingTargets.Count != 0;
	}

	private bool IsCharacterIdle()
	{
		return _character.IsIdle;
	}

	public virtual bool IsOver()
	{
		return !IsCharacterAttacking() && IsCharacterIdle();
	}

	public virtual void MoveTowardOpponent()
	{
	}

	public virtual void MoveBackToPosition()
	{
	}
}