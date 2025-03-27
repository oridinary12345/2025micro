public class GameStateUnpetrifyHero : IGameState
{
	public static GameStateUnpetrifyHero Instance = new GameStateUnpetrifyHero();

	public void OnStateEnter(GameController gameController)
	{
		gameController.UnpetrifyHero();
	}

	public void OnStateUpdate(GameController gameController)
	{
		if (gameController.FSM.TimeInState > 1.5f)
		{
			gameController.FSM.GoToState(gameController, GameStateIdle.Instance);
		}
	}

	public void OnStateExit(GameController gameController)
	{
	}

	public void OnMessage(GameController gameController, GameStateMessage message)
	{
	}
}