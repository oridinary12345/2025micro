using UnityEngine;

public class StateIdle : ICharacterState
{
	public static StateIdle Instance = new StateIdle();

	public void OnStateEnter(Character character)
	{
		if (character.Surprised)
		{
			character.Play(AnimationState.IdleSurprised);
		}
		else
		{
			AnimationState idleAnimId = character.GetIdleAnimId();
			character.Play((!character.HasFinishedLevel || character.IsPetrified()) ? idleAnimId : AnimationState.IdleWeaponFound);
		}
		character.IsIdle = true;
	}

	public void OnStateUpdate(Character character)
	{
		Vector3 moveToPos = character.MoveToPos;
		if (moveToPos.x != 1000f)
		{
			character.FSM.GoToState(character, StateMoving.Instance);
		}
	}

	public void OnStateExit(Character character)
	{
		character.IsIdle = false;
	}

	public void OnMessage(CharacterStateMessage message)
	{
		CharacterDamageTaken characterDamageTaken = message as CharacterDamageTaken;
		characterDamageTaken?.To.FSM.GoToState(characterDamageTaken.To, StateDamaged.Instance);
	}
}