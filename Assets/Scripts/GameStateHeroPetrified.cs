public class GameStateHeroPetrified : IGameState
{
	public static GameStateHeroPetrified Instance = new GameStateHeroPetrified();

	public void OnStateEnter(GameController gameController)
	{
		gameController.OnPetrifiedHeroRoundStarted();
	}

	public void OnStateUpdate(GameController gameController)
	{
		if (gameController.FSM.TimeInState > 2f)
		{
			gameController.FSM.GoToState(gameController, GameStatePrepareMonsterTurn.Instance);
		}
	}

	public void OnStateExit(GameController gameController)
	{
	}

	public void OnMessage(GameController gameController, GameStateMessage message)
	{
	}
}