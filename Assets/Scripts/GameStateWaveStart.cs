public class GameStateWaveStart : IGameState
{
	public static GameStateWaveStart Instance = new GameStateWaveStart();

	public void OnStateEnter(GameController gameController)
	{
		gameController.StartWave();
	}

	public void OnStateUpdate(GameController gameController)
	{
		if (!gameController.IsHeroPetrified() || !(gameController.FSM.TimeInState < 1.5f))
		{
			gameController.FSM.GoToState(gameController, GameStateStartRound.Instance);
		}
	}

	public void OnStateExit(GameController gameController)
	{
	}

	public void OnMessage(GameController gameController, GameStateMessage message)
	{
	}
}