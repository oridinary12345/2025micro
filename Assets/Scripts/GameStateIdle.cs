public class GameStateIdle : IGameState
{
	public static GameStateIdle Instance = new GameStateIdle();

	public static bool FirstIdle = true;

	public void OnStateEnter(GameController gameController)
	{
		if (gameController.IsHeroPetrified() && gameController.GetPetrifiedTurnRemaining() == 0)
		{
			gameController.FSM.GoToState(gameController, GameStateUnpetrifyHero.Instance);
			return;
		}
		gameController.OnIdle();
		if (gameController.IsHeroPetrified())
		{
			gameController.FSM.GoToState(gameController, GameStateHeroPetrified.Instance);
		}
		else if (FirstIdle)
		{
			gameController.FSM.GoToState(gameController, GameStateRotating.Instance);
		}
		else
		{
			gameController.ShowWeapons();
		}
	}

	public void OnStateUpdate(GameController gameController)
	{
	}

	public void OnStateExit(GameController gameController)
	{
		FirstIdle = false;
	}

	public void OnMessage(GameController gameController, GameStateMessage message)
	{
		WeaponSelected weaponSelected = message as WeaponSelected;
		if (weaponSelected != null)
		{
			gameController.FSM.GoToState(gameController, GameStateRotating.Instance);
			if (!FirstIdle)
			{
				if (weaponSelected.IsNewWeapon)
				{
					gameController.ShowShapeField();
				}
			}
			else
			{
				gameController.ShowShapeField();
			}
		}
		UserTapMessage userTapMessage = message as UserTapMessage;
		if (userTapMessage != null)
		{
			gameController.OnScreenTapWhileWeaponRotating();
		}
	}
}