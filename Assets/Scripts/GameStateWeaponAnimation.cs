public class GameStateWeaponAnimation : IGameState
{
	public static GameStateWeaponAnimation Instance = new GameStateWeaponAnimation();

	public void OnStateEnter(GameController gameController)
	{
		gameController.OnWeaponShapeAnim();
	}

	public void OnStateUpdate(GameController gameController)
	{
		if (gameController.FSM.TimeInState >= 0.4f)
		{
			gameController.FSM.GoToState(gameController, GameStateCollectLoot.Instance);
		}
	}

	public void OnStateExit(GameController gameController)
	{
		gameController.HideWeaponShape();
	}

	public void OnMessage(GameController gameController, GameStateMessage message)
	{
	}
}