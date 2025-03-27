public class GameStateHandleMapObjects : IGameState
{
	public static GameStateHandleMapObjects Instance = new GameStateHandleMapObjects();

	public void OnStateEnter(GameController gameController)
	{
		if (!gameController.HandleMapObjects())
		{
			gameController.FSM.GoToState(gameController, GameStateHeroAttacking.Instance);
		}
	}

	public void OnStateUpdate(GameController gameController)
	{
		if (gameController.FSM.TimeInState >= 0.5f)
		{
			gameController.FSM.GoToState(gameController, GameStateHeroAttacking.Instance);
		}
	}

	public void OnStateExit(GameController gameController)
	{
	}

	public void OnMessage(GameController gameController, GameStateMessage message)
	{
	}
}