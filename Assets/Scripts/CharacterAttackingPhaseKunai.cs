using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackingPhaseKunai : CharacterAttackingPhase
{
	private WeaponShape _weaponShape;

	private HashSet<Character> _targetMonsters = new HashSet<Character>();

	public CharacterAttackingPhaseKunai Init(Character character, WeaponShape weaponShape)
	{
		Init(character);
		_weaponShape = weaponShape;
		Dictionary<Collider2D, HashSet<GameObject>> lastTouchedObjectsWithColliders = _weaponShape.GetLastTouchedObjectsWithColliders("Monster");
		List<Character> list = new List<Character>();
		foreach (KeyValuePair<Collider2D, HashSet<GameObject>> item in lastTouchedObjectsWithColliders)
		{
			foreach (GameObject item2 in item.Value)
			{
				Transform transform = item2.transform;
				Character character2 = null;
				do
				{
					character2 = transform.GetComponent<Character>();
					transform = transform.parent;
				}
				while (character2 == null && transform != null);
				if (character2 != null)
				{
					_targetMonsters.Add(character2);
					if (!list.Contains(character2))
					{
						list.Add(character2);
					}
				}
			}
		}
		if (_targetMonsters.Count == 0)
		{
			return this;
		}
		_character.SetAttackTargets(list);
		_character.SetCurrentTarget(list[0]);
		_character.LookAt(weaponShape.GetCollidersCenter()[1]);
		_character.FSM.OnMessage(new CharacterAttackBegin(_character));
		return this;
	}

	public override void StartAttackEndPhase()
	{
		int damage = _character.GetDamage();
		_character.Play(_character.GetAttackAnimation());
		Action<Character, bool> attack = delegate(Character targetCharacter, bool isLastTarget)
		{
			if (_character.CurrentAttackTarget != targetCharacter)
			{
				_character.SetCurrentTarget(targetCharacter);
			}
			Monster monster2 = targetCharacter as Monster;
			if (monster2 == null || !monster2.HasTeleported() || _character.IsCounterAttacking())
			{
				int num3 = targetCharacter.TakeDamage(_character, _character.GetCurrentWeapon(), damage, _character.GetLastJumpPosition());
				int num4 = (_character.GetComboDamageBonus() <= 1) ? ((num3 > 0) ? targetCharacter.DamageToWeapon : 0) : 0;
				if (num4 > 0)
				{
					_character.WeaponTakingDamage(num4);
				}
			}
			if (!_character.PendingTargets.Remove(targetCharacter))
			{
				UnityEngine.Debug.Log("StartAttackEndPhase() wasn't able to remove " + targetCharacter + " from the pending target...");
			}
			_character.PendingTargets.RemoveAll((Character c) => c == null || c.IsDead() || c.CurrentCell == null);
			if (isLastTarget)
			{
				_character.OnAttackFinished();
			}
		};
		float num = 0f;
		int num2 = 0;
		foreach (Character monster in _targetMonsters)
		{
			bool isLast = num2 + 1 == _targetMonsters.Count;
			MonoExtensions.Execute(num, delegate
			{
				_character.MaybeThrowProjectiles(damage, null, monster.GetPosition(), 0.2f);
			});
			MonoExtensions.Execute(num + 0.1f, delegate
			{
				attack(monster, isLast);
			});
			num += 0.15f;
			num2++;
		}
		MonoExtensions.Execute(0.1f, delegate
		{
			_character.PlayAttackLandingAnimation(delegate
			{
			});
		});
	}
}