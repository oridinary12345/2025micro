using DG.Tweening;
using UnityEngine;

public class StateDead : ICharacterState
{
	public static StateDead Instance = new StateDead();

	public void OnStateEnter(Character character)
	{
		DOTween.timeScale = 1f;
		Time.timeScale = DOTween.timeScale;
		character.Visual.ApplyDefaultSkin();
		character.Play(AnimationState.Death);
		character.OnDead();
	}

	public void OnStateUpdate(Character character)
	{
	}

	public void OnStateExit(Character character)
	{
	}

	public void OnMessage(CharacterStateMessage message)
	{
		CharacterHealed characterHealed = message as CharacterHealed;
		characterHealed?.To.FSM.GoToState(characterHealed.To, StateIdle.Instance);
	}
}