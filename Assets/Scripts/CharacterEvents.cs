using System;
using UnityEngine;

public class CharacterEvents
{
	public event Action<Character, Character, WeaponData, int> DamagedEvent;

	public event Action<Character, Character> AttackThrownEvent;

	public event Action<Character> UnpetrificationStartedEvent;

	public event Action<Character, Character> PetrificationStartedEvent;

	public event Action<Character> PetrificationEndedEvent;

	public event Action<Character, int> HealedEvent;

	public event Action<Character> ScaleToDeadEvent;

	public event Action<Character> AttackMovementStartedEvent;

	public event Action<Character, WeaponData> WeaponChangedEvent;

	public event Action<Character> WaitingToAttackEvent;

	public event Action TargetReachedEvent;

	public event Action FirebreathEvent;

	public event Action SippedEvent;

	public event Action<Vector3, Vector3, WeaponData, float> ProjectileLandedEvent;

	public event Action<Character> DivedInEvent;

	public event Action<Character> DivedOutEvent;

	public event Action<Character, Character, Vector3> DodgedEvent;

	public void OnDamaged(Character attacker, Character defender, WeaponData weapon, int damage)
	{
		if (this.DamagedEvent != null)
		{
			this.DamagedEvent(attacker, defender, weapon, damage);
		}
	}

	public void OnAttackThrown(Character attacker, Character defender)
	{
		if (this.AttackThrownEvent != null)
		{
			this.AttackThrownEvent(attacker, defender);
		}
	}

	public void OnSipped()
	{
		if (this.SippedEvent != null)
		{
			this.SippedEvent();
		}
	}

	public void OnScaleToDead(Character character)
	{
		if (this.ScaleToDeadEvent != null)
		{
			this.ScaleToDeadEvent(character);
		}
	}

	public void OnUnpetrificationStarted(Character petrifiedCharacter)
	{
		if (this.UnpetrificationStartedEvent != null)
		{
			this.UnpetrificationStartedEvent(petrifiedCharacter);
		}
	}

	public void OnPetrificationStarted(Character attacker, Character defender)
	{
		if (this.PetrificationStartedEvent != null)
		{
			this.PetrificationStartedEvent(attacker, defender);
		}
	}

	public void OnPetrificationEnded(Character defender)
	{
		if (this.PetrificationEndedEvent != null)
		{
			this.PetrificationEndedEvent(defender);
		}
	}

	public void OnProjectileLanded(Vector3 from, Vector3 to, WeaponData weapon, float groundDuration)
	{
		if (this.ProjectileLandedEvent != null)
		{
			this.ProjectileLandedEvent(from, to, weapon, groundDuration);
		}
	}

	public void OnHealed(Character character, int healAmount)
	{
		if (this.HealedEvent != null)
		{
			this.HealedEvent(character, healAmount);
		}
	}

	public void OnDodged(Character attacker, Character defender, Vector3 defaultPos)
	{
		if (this.DodgedEvent != null)
		{
			this.DodgedEvent(attacker, defender, defaultPos);
		}
	}

	public void OnAttackMovementStarted(Character attacker)
	{
		if (this.AttackMovementStartedEvent != null)
		{
			this.AttackMovementStartedEvent(attacker);
		}
	}

	public void OnWeaponChanged(Character hero, WeaponData weapon)
	{
		if (this.WeaponChangedEvent != null)
		{
			this.WeaponChangedEvent(hero, weapon);
		}
	}

	public void OnWaitingToAttack(Character character)
	{
		if (this.WaitingToAttackEvent != null)
		{
			this.WaitingToAttackEvent(character);
		}
	}

	public void OnTargetReached()
	{
		if (this.TargetReachedEvent != null)
		{
			this.TargetReachedEvent();
		}
	}

	public void OnFirebreath()
	{
		if (this.FirebreathEvent != null)
		{
			this.FirebreathEvent();
		}
	}

	public void OnDivedIn(Character character)
	{
		if (this.DivedInEvent != null)
		{
			this.DivedInEvent(character);
		}
	}

	public void OnDivedOut(Character character)
	{
		if (this.DivedOutEvent != null)
		{
			this.DivedOutEvent(character);
		}
	}
}