public class WaveCompleted : IGameState
{
	public static WaveCompleted Instance = new WaveCompleted();

	public void OnStateEnter(GameController gameController)
	{
		gameController.EndWave();
		if (gameController.State.IsLevelCompleted())
		{
			gameController.FSM.GoToState(gameController, GameStateEnd.Instance);
		}
		else
		{
			gameController.FSM.GoToState(gameController, GameStateWaveStart.Instance);
		}
	}

	public void OnStateUpdate(GameController gameController)
	{
	}

	public void OnStateExit(GameController gameController)
	{
	}

	public void OnMessage(GameController gameController, GameStateMessage message)
	{
	}
}