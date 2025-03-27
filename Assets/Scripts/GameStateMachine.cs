using UnityEngine;

public class GameStateMachine
{
	private IGameState _currentState;

	private GameEvents _gameEvents;

	private readonly GameController _gameController;

	public float TimeInState
	{
		get;
		private set;
	}

	public GameStateMachine(GameController gameController)
	{
		_gameController = gameController;
		_gameEvents = _gameController.Events;
		_gameEvents.GameStateMessageEvent += OnGameStateMessage;
	}

	public void Update(GameController gameController)
	{
		TimeInState += Time.deltaTime;
		if (_currentState != null)
		{
			_currentState.OnStateUpdate(gameController);
		}
	}

	public void GoToState(GameController gameController, IGameState nextState)
	{
		if ((_currentState != GameStateEnded.Instance || nextState == GameStateIntro.Instance) && _currentState != nextState)
		{
			if (_currentState != null)
			{
				_currentState.OnStateExit(gameController);
			}
			TimeInState = 0f;
			_currentState = nextState;
			_currentState.OnStateEnter(gameController);
		}
	}

	public void OnReady()
	{
		GoToState(_gameController, GameStateIntro.Instance);
	}

	private void OnGameStateMessage(GameStateMessage message)
	{
		if (_currentState != null)
		{
			_currentState.OnMessage(_gameController, message);
		}
	}
}