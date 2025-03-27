using UnityEngine;

public class GameStateHeroDead : IGameState
{
	public static GameStateHeroDead Instance = new GameStateHeroDead();

	public void OnStateEnter(GameController gameController)
	{
		if (!gameController.IsHeroDead())
		{
			UnityEngine.Debug.LogWarning("GameStateHeroDead() Hero is supposed to be dead. We can't offer the continue...");
			gameController.FSM.GoToState(gameController, GameStateOutro.Instance);
		}
		else if (gameController.GameOverManager.CanOfferContinue())
		{
			MonoExtensions.Execute(0.35f, gameController.ShowGameOverContinue);
		}
		else
		{
			gameController.FSM.GoToState(gameController, GameStateOutro.Instance);
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
		GameOverContinueAcceptedMessage gameOverContinueAcceptedMessage = message as GameOverContinueAcceptedMessage;
		if (gameOverContinueAcceptedMessage != null)
		{
			gameController.PushAllMonstersBack(1);
			gameController.FSM.GoToState(gameController, GameStateEndRound.Instance);
			return;
		}
		GameOverContinueDeclinedMessage gameOverContinueDeclinedMessage = message as GameOverContinueDeclinedMessage;
		if (gameOverContinueDeclinedMessage != null)
		{
			gameController.FSM.GoToState(gameController, GameStateOutro.Instance);
		}
	}
}