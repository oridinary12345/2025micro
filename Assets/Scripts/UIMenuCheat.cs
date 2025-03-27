using TMPro;
using UnityEngine;

public class UIMenuCheat : UIMenuPopup
{
	[SerializeField]
	private UIGameButton _buttonClose;

	[SerializeField]
	private UIGameButton _buttonAddLootCoins;

	[SerializeField]
	private UIGameButton _buttonAddLootRubies;

	[SerializeField]
	private UIGameButton _buttonAddWeaponsCards;

	[SerializeField]
	private UIGameButton _buttonAddMonsterCards;

	[SerializeField]
	private UIGameButton _buttonIncreaseQuestProgress;

	[SerializeField]
	private UIGameButton _buttonForceQuestExpiration;

	[SerializeField]
	private UIGameButton _buttonUnlockNextLevel;

	[SerializeField]
	private UIGameButton _buttonUnlockNextWorld;

	[SerializeField]
	private UIGameButton _buttonDebugHUD;

	[SerializeField]
	private TextMeshProUGUI _buttonDebugHUDLabel;

	private bool _mustReloadMenu;

	public static bool ShowDebugHUD
	{
		get;
		private set;
	}

	protected override void Awake()
	{
		base.Awake();
		_buttonClose.OnClick(OnCloseButtonClicked);
		_buttonClose.ActivateOnBackKey();
		_buttonAddLootCoins.OnClick(OnAddLootCoinsButtonClicked);
		_buttonAddLootRubies.OnClick(OnAddLootRubiesButtonClicked);
		_buttonAddWeaponsCards.OnClick(OnAddWeaponCardsButtonClicked);
		_buttonAddMonsterCards.OnClick(OnIncreaseMonsterLevels);
		_buttonIncreaseQuestProgress.OnClick(OnIncreaseQuestMissionClicked);
		_buttonForceQuestExpiration.OnClick(OnForceQuestExpirationClicked);
		_buttonUnlockNextLevel.OnClick(OnUnlockNextLevelButtonClicked);
		_buttonUnlockNextWorld.OnClick(OnUnlockNextWorldButtonClicked);
		_buttonDebugHUD.OnClick(OnDebugHUDButtonClicked);
		UpdateDebugHUDButtonLabel();
	}

	private void OnCloseButtonClicked()
	{
		if (_mustReloadMenu)
		{
			MonoSingleton<SceneSwitcher>.Instance.LoadScene("Game");
		}
		else
		{
			Hide();
		}
	}

	private void OnAddLootCoinsButtonClicked()
	{
		App.Instance.Player.LootManager.Add("lootCoin", 9999, CurrencyReason.unknown);
	}

	private void OnAddLootRubiesButtonClicked()
	{
		App.Instance.Player.LootManager.Add("lootRuby", 100, CurrencyReason.unknown);
	}

	private void OnAddWeaponCardsButtonClicked()
	{
		foreach (WeaponConfig config in MonoSingleton<WeaponConfigs>.Instance.GetConfigs())
		{
			App.Instance.Player.WeaponManager.AddCards(config.Id, 1);
		}
		foreach (HeroConfig config2 in MonoSingleton<HeroConfigs>.Instance.GetConfigs())
		{
			App.Instance.Player.HeroManager.AddCards(config2.Id, 1);
		}
		_mustReloadMenu = true;
	}

	private void OnIncreaseMonsterLevels()
	{
		App.Instance.Player.MuseumManager.AddCardsToAll(1);
		_mustReloadMenu = true;
	}

	private void OnIncreaseQuestMissionClicked()
	{
		App.Instance.Player.MonsterMissions.IncreaseMissionObjective();
		_mustReloadMenu = true;
	}

	private void OnForceQuestExpirationClicked()
	{
		App.Instance.Player.MonsterMissions.ForceExpiration();
	}

	private void OnUnlockNextLevelButtonClicked()
	{
		WorldData worldData = App.Instance.Player.LevelManager.GetWorldData(App.Instance.Player.LevelManager.LastSelectedWorldId);
		App.Instance.Player.LevelManager.UnlockNextLevel(worldData);
		_mustReloadMenu = true;
	}

	private void OnUnlockNextWorldButtonClicked()
	{
		App.Instance.Player.LevelManager.UnlockNextWorld();
		_mustReloadMenu = true;
	}

	private void OnDebugHUDButtonClicked()
	{
		ShowDebugHUD = !ShowDebugHUD;
		UpdateDebugHUDButtonLabel();
	}

	private void UpdateDebugHUDButtonLabel()
	{
		_buttonDebugHUDLabel.text = ((!ShowDebugHUD) ? "SHOW DEBUG HUD" : "HIDE DEBUG HUD");
	}
}