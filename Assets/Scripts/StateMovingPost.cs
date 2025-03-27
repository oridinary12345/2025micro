public class StateMovingPost : ICharacterState
{
	public static StateMovingPost Instance = new StateMovingPost();

	public void OnStateEnter(Character character)
	{
		character.OnPostMove();
	}

	public void OnStateUpdate(Character character)
	{
		if (character.IsPostMoveOver())
		{
			character.FSM.GoToState(character, StateIdle.Instance);
		}
	}

	public void OnStateExit(Character character)
	{
		character.ResetMovingTarget();
		character.IsMoving = false;
	}

	public void OnMessage(CharacterStateMessage message)
	{
	}
}