using UnityEngine;

public class CharacterStateMachine
{
	private ICharacterState _currentState;

	public float TimeInState
	{
		get;
		private set;
	}

	public CharacterStateMachine(Character character)
	{
		GoToState(character, StateIdle.Instance);
	}

	public void Update(Character character)
	{
		TimeInState += Time.deltaTime;
		if (_currentState != null)
		{
			_currentState.OnStateUpdate(character);
		}
	}

	public void GoToState(Character character, ICharacterState nextState)
	{
		if (_currentState == nextState && _currentState != StateAttacking.Instance)
		{
			UnityEngine.Debug.LogWarning("Re-entering the same state... " + _currentState);
		}
		if (_currentState != null)
		{
			_currentState.OnStateExit(character);
		}
		_currentState = nextState;
		TimeInState = 0f;
		_currentState.OnStateEnter(character);
	}

	public void OnMessage(CharacterStateMessage message)
	{
		CharacterAttackBegin characterAttackBegin = message as CharacterAttackBegin;
		if (characterAttackBegin != null)
		{
			if (_currentState != StateIdle.Instance && _currentState != StateAttacking.Instance)
			{
				UnityEngine.Debug.LogWarning("This character is starting an attack while not being in IDLE/ATTACKING state: " + _currentState);
			}
			Character from = message.From;
			from.FSM.GoToState(from, StateAttacking.Instance);
		}
		_currentState.OnMessage(message);
	}
}