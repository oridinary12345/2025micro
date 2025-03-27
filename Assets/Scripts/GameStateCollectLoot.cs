public class GameStateCollectLoot : IGameState
{
	public static GameStateCollectLoot Instance = new GameStateCollectLoot();

	public void OnStateEnter(GameController gameController)
	{
		if (!gameController.CollectLoot())
		{
			gameController.FSM.GoToState(gameController, GameStateHandleMapObjects.Instance);
		}
	}

	public void OnStateUpdate(GameController gameController)
	{
		if (gameController.FSM.TimeInState >= 0.5f)
		{
			gameController.FSM.GoToState(gameController, GameStateHandleMapObjects.Instance);
		}
	}

	public void OnStateExit(GameController gameController)
	{
	}

	public void OnMessage(GameController gameController, GameStateMessage message)
	{
	}
}