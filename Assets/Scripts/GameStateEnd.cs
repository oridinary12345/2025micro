using UnityEngine;

public class GameStateEnd : IGameState
{
	public static GameStateEnd Instance = new GameStateEnd();

	public void OnStateEnter(GameController gameController)
	{
		if (gameController.State.IsLevelCompleted())
		{
			gameController.OnLevelCompleted();
			gameController.FSM.GoToState(gameController, GameStateOutro.Instance);
		}
		else if (!gameController.IsHeroDead())
		{
			UnityEngine.Debug.LogWarning("Hero is supposed to be dead, if the game has ended and monster are still on the map");
			gameController.FSM.GoToState(gameController, GameStateOutro.Instance);
		}
		else
		{
			gameController.OnHeroDead();
			gameController.FSM.GoToState(gameController, GameStateHeroDead.Instance);
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
	}
}