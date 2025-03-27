public class GameStateMonsterAttacking : IGameState
{
	public static GameStateMonsterAttacking Instance = new GameStateMonsterAttacking();

	public void OnStateEnter(GameController gameController)
	{
		gameController.StartMonsterAttackingPhase();
	}

	public void OnStateUpdate(GameController gameController)
	{
		gameController.UpdateAttackingMonsters();
		if (!gameController.AreMonstersAttacking() && (gameController.IsHeroIdle() || gameController.IsHeroDead()))
		{
			gameController.FSM.GoToState(gameController, GameStateMonsterMoving.Instance);
		}
	}

	public void OnStateExit(GameController gameController)
	{
	}

	public void OnMessage(GameController gameController, GameStateMessage message)
	{
	}
}