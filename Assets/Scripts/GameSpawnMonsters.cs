public class GameSpawnMonsters : IGameState
{
	public static GameSpawnMonsters Instance = new GameSpawnMonsters();

	public void OnStateEnter(GameController gameController)
	{
		if (!gameController.SpawnMonsters() && !GameSpawnChests.Instance.MaybeChangeState(gameController))
		{
			gameController.FSM.GoToState(gameController, GameStateIdle.Instance);
		}
	}

	public void OnStateUpdate(GameController gameController)
	{
	}

	public void OnStateExit(GameController gameController)
	{
	}

	public void OnMessage(GameController gameController, GameStateMessage message)
	{
		if (message is MonsterSpawnedMessage && !GameSpawnChests.Instance.MaybeChangeState(gameController))
		{
			gameController.FSM.GoToState(gameController, GameStateIdle.Instance);
		}
	}
}