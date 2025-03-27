public class StateMoving : ICharacterState
{
	public static StateMoving Instance = new StateMoving();

	public void OnStateEnter(Character character)
	{
		character.IsMoving = true;
		character.LookAt(character.MoveToPos);
		character.OnPreMove();
		if (character is Monster && (character as Monster).IsUnderwater())
		{
			character.Play(AnimationState.RunUnderwater);
		}
		else
		{
			character.Play(AnimationState.Run);
		}
	}

	public void OnStateUpdate(Character character)
	{
		if (!character.IsPreMoveOver())
		{
			return;
		}
		if (character.HasReachTarget())
		{
			character.OnTargetReached();
			if (character.IsMoving && (!character.MaybeProcessNextMovement() || character.IsHero()))
			{
				character.SetPositionAtTarget();
				character.FSM.GoToState(character, StateMovingPost.Instance);
			}
		}
		else
		{
			character.OnMoving();
		}
	}

	public void OnStateExit(Character character)
	{
	}

	public void OnMessage(CharacterStateMessage message)
	{
		CharacterDamageTaken characterDamageTaken = message as CharacterDamageTaken;
		characterDamageTaken?.To.FSM.GoToState(characterDamageTaken.To, StateDamaged.Instance);
	}
}