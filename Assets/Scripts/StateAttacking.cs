public class StateAttacking : ICharacterState
{
	public static StateAttacking Instance = new StateAttacking();

	public void OnStateEnter(Character character)
	{
		character.IsAttacking = true;
		if (!character.HasWeapon() || character.GetCurrentWeapon().IsMeleeAttack())
		{
			character.Play(AnimationState.Jump);
		}
		else if (character.GetCurrentWeapon().IsRangedAttack())
		{
			character.Play(AnimationState.JumpRanged);
		}
		else if (character.GetCurrentWeapon().IsBombAttack())
		{
			character.Play(AnimationState.Idle);
		}
		character.OnAttacking();
	}

	public void OnStateUpdate(Character character)
	{
	}

	public void OnStateExit(Character character)
	{
		character.IsAttacking = false;
	}

	public void OnMessage(CharacterStateMessage message)
	{
	}
}