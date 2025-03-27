public class GameStatePrepareMonsterTurn : IGameState
{
	public static GameStatePrepareMonsterTurn Instance = new GameStatePrepareMonsterTurn();

	public void OnStateEnter(GameController gameController)
	{
		gameController.PrepareMonsterTurn();
		gameController.FSM.GoToState(gameController, GameStateMonsterAttacking.Instance);
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