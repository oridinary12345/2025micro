public class GameStateStartRound : IGameState
{
	public static GameStateStartRound Instance = new GameStateStartRound();

	public void OnStateEnter(GameController gameController)
	{
		gameController.StartRound();
	}

	public void OnStateUpdate(GameController gameController)
	{
		if (gameController.IsNewRoundReady())
		{
			if (gameController.MustSpawnMoreMonsters())
			{
				gameController.FSM.GoToState(gameController, GameSpawnMonsters.Instance);
			}
			else if (!GameSpawnChests.Instance.MaybeChangeState(gameController))
			{
				gameController.FSM.GoToState(gameController, GameStateIdle.Instance);
			}
		}
	}

	public void OnStateExit(GameController gameController)
	{
	}

	public void OnMessage(GameController gameController, GameStateMessage message)
	{
	}
}