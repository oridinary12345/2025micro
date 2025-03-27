public class GameStateEnded : IGameState
{
	public static GameStateEnded Instance = new GameStateEnded();

	public void OnStateEnter(GameController gameController)
	{
		gameController.OnEndGame();
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