public class GameSpawnChests : IGameState
{
	public static GameSpawnChests Instance = new GameSpawnChests();

	public void OnStateEnter(GameController gameController)
	{
		gameController.SpawnChests();
	}

	public void OnStateUpdate(GameController gameController)
	{
	}

	public void OnStateExit(GameController gameController)
	{
	}

	public void OnMessage(GameController gameController, GameStateMessage message)
	{
		if (message is MonsterSpawnedMessage)
		{
			gameController.FSM.GoToState(gameController, GameStateIdle.Instance);
		}
	}

	public bool MaybeChangeState(GameController gameController)
	{
		if (gameController.CanSpawnEndGameChest())
		{
			gameController.FSM.GoToState(gameController, Instance);
			return true;
		}
		return false;
	}
}