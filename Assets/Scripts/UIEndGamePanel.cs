using System.Collections.Generic;

public class UIEndGamePanel : UIMenu
{
	private UIRewardsController _menuRewardController;

	private List<Reward> _cardRewards;

	public void Init(List<Reward> cardRewards)
	{
		_cardRewards = cardRewards;
		_menuRewardController = UIRewardsController.Create();
	}

	public override void OnFocusGained()
	{
		base.OnFocusGained();
		_menuRewardController.Show(_cardRewards, GoToMainMenu);
	}

	private void GoToMainMenu()
	{
		App.Instance.LoadMainMenuFromGame();
	}
}