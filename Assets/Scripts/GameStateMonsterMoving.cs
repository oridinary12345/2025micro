public class GameStateMonsterMoving : IGameState
{
	public static GameStateMonsterMoving Instance = new GameStateMonsterMoving();

	public void OnStateEnter(GameController gameController)
	{
		if (!IsReadyForNextState(gameController))
		{
			gameController.StartMonsterMovingPhase();
		}
	}

	public void OnStateUpdate(GameController gameController)
	{
		bool flag = IsReadyForNextState(gameController);
		if (!flag)
		{
			gameController.UpdateMovingMonsters();
		}
		if (flag || !gameController.AreMonstersMoving())
		{
			gameController.FSM.GoToState(gameController, GameStateEndRound.Instance);
		}
	}

	private bool IsReadyForNextState(GameController gameController)
	{
		return gameController.State.IsLevelCompleted() || gameController.AreMonstersAllDead() || gameController.IsHeroDead();
	}

	public void OnStateExit(GameController gameController)
	{
	}

	public void OnMessage(GameController gameController, GameStateMessage message)
	{
	}
}