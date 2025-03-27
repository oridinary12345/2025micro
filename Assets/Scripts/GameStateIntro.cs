public class GameStateIntro : IGameState
{
	public static GameStateIntro Instance = new GameStateIntro();

	public void OnStateEnter(GameController gameController)
	{
		GameStateIdle.FirstIdle = true;
		gameController.StartIntroGameFlow();
	}

	public void OnStateUpdate(GameController gameController)
	{
		if (gameController.IsIntroGameFlowFinished())
		{
			gameController.FSM.GoToState(gameController, GameStateWaveStart.Instance);
		}
	}

	public void OnStateExit(GameController gameController)
	{
	}

	public void OnMessage(GameController gameController, GameStateMessage message)
	{
	}
}