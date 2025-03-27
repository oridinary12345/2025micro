public class GameStateOutro : IGameState
{
	public static GameStateOutro Instance = new GameStateOutro();

	public void OnStateEnter(GameController gameController)
	{
		gameController.StartEndGameFlow();
	}

	public void OnStateUpdate(GameController gameController)
	{
		if (gameController.IsEndGameFlowFinished())
		{
			gameController.FSM.GoToState(gameController, GameStateEnded.Instance);
		}
	}

	public void OnStateExit(GameController gameController)
	{
	}

	public void OnMessage(GameController gameController, GameStateMessage message)
	{
	}
}