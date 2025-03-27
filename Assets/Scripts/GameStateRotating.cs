public class GameStateRotating : IGameState
{
	public static GameStateRotating Instance = new GameStateRotating();

	public void OnStateEnter(GameController gameController)
	{
		gameController.Events.OnWeaponRotationStarted();
		gameController.StartRotation();
		if (gameController.State.TotalRoundCount == 1)
		{
			gameController.ShowShapeField();
		}
	}

	public void OnStateUpdate(GameController gameController)
	{
	}

	public void OnStateExit(GameController gameController)
	{
		gameController.Events.OnWeaponRotationStopped();
		gameController.StopRotation();
		gameController.HideWeaponsMenu();
	}

	public void OnMessage(GameController gameController, GameStateMessage message)
	{
		if (message is WeaponConfirmed)
		{
			gameController.FSM.GoToState(gameController, GameStateWeaponAnimation.Instance);
			return;
		}
		WeaponSelected weaponSelected = message as WeaponSelected;
		if (weaponSelected != null)
		{
			if (weaponSelected.IsNewWeapon)
			{
				gameController.ShowShapeField();
			}
			return;
		}
		UserTapMessage userTapMessage = message as UserTapMessage;
		if (userTapMessage != null)
		{
			gameController.OnScreenTapWhileWeaponRotating();
		}
	}
}