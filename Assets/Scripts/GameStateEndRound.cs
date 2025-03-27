public class GameStateEndRound : IGameState
{
	public static GameStateEndRound Instance = new GameStateEndRound();

	public void OnStateEnter(GameController gameController)
	{
		gameController.EndRound();
	}

	public void OnStateUpdate(GameController gameController)
	{
		if (gameController.IsHeroDead())
		{
			gameController.FSM.GoToState(gameController, GameStateEnd.Instance);
		}
		else if (gameController.IsCurrentWaveCompleted())
		{
			gameController.FSM.GoToState(gameController, WaveCompleted.Instance);
		}
		else
		{
			gameController.FSM.GoToState(gameController, GameStateStartRound.Instance);
		}
	}

	public void OnStateExit(GameController gameController)
	{
	}

	public void OnMessage(GameController gameController, GameStateMessage message)
	{
	}
}