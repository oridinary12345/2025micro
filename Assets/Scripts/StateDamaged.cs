public class StateDamaged : ICharacterState
{
	public static StateDamaged Instance = new StateDamaged();

	public void OnStateEnter(Character character)
	{
		character.IsReceivingDamage = true;
		AnimationState anim = AnimationState.Damaged;
		if (character.IsPetrified())
		{
			anim = AnimationState.DamagedPetrified;
		}
		else if (character.IsReceivingCriticalDamage)
		{
			anim = AnimationState.DamagedBig;
		}
		character.Play(anim);
		character.OnDamageTaken();
	}

	public void OnStateUpdate(Character character)
	{
		if (character.FSM.TimeInState >= 0.4f || character.IsDead())
		{
			if (character.IsDead())
			{
				character.FSM.GoToState(character, StateDead.Instance);
			}
			else
			{
				character.FSM.GoToState(character, StateIdle.Instance);
			}
		}
	}

	public void OnStateExit(Character character)
	{
		character.IsReceivingDamage = false;
		character.IsReceivingCriticalDamage = false;
		character.IsDamageResistant = false;
	}

	public void OnMessage(CharacterStateMessage message)
	{
	}
}