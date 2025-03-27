public class GameStateHeroAttacking : IGameState
{
	public static GameStateHeroAttacking Instance = new GameStateHeroAttacking();

	public void OnStateEnter(GameController gameController)
	{
		gameController.StartHeroAttackPhase();
	}

	public void OnStateUpdate(GameController gameController)
	{
		if (gameController.IsHeroAttackPhaseOver())
		{
			if (gameController.AreMonstersAllDead())
			{
				gameController.FSM.GoToState(gameController, GameStateEndRound.Instance);
			}
			else
			{
				gameController.FSM.GoToState(gameController, GameStatePrepareMonsterTurn.Instance);
			}
		}
	}

	public void OnStateExit(GameController gameController)
	{
		gameController.EndHeroAttackPhase();
	}

	public void OnMessage(GameController gameController, GameStateMessage message)
	{
	}
}