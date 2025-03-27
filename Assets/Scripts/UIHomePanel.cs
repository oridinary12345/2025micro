using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class UIHomePanel : UIMenu
{
	[SerializeField]
	private UIGameButton _playButton;

	[SerializeField]
	private UIGameButton _settingsButton;

	[SerializeField]
	private UIGameButton _heroButton;

	[SerializeField]
	private UIGameButton _weaponButton;

	[SerializeField]
	private UIGameButton _shopButton;

	[SerializeField]
	private UIBadge _weaponBadgeUpgrade;

	[SerializeField]
	private UIBadge _weaponBadgeNew;

	[SerializeField]
	private UIBadge _heroBadgeUpgrade;

	[SerializeField]
	private UIBadge _heroBadgeNew;

	[SerializeField]
	private UIBadge _shopBadge;

	[SerializeField]
	private UIWeaponsPanel _weaponsPanel;

	[SerializeField]
	private UIShopPanel _shopPanel;

	[SerializeField]
	private UIMenuSettingsPanel _settingsPanel;

	[SerializeField]
	private UIWorldSelector _worldSelector;

	[SerializeField]
	private GameController _gameController;

	[SerializeField]
	private GameObject _gamePanel;

	[SerializeField]
	private UIGameHUD _gameHud;

	[SerializeField]
	private UIHeroesPanel _heroesPanel;

	[SerializeField]
	private UICurrentHeroBox _heroBox;

	[SerializeField]
	private RectTransform _bottomMenu;

	[SerializeField]
	private RectTransform _heroHpBar;

	[SerializeField]
	private RectTransform _heroHpBarTarget;

	[SerializeField]
	private Canvas _canvas;

	[SerializeField]
	private WorldRenderer _worldRenderer;

	[SerializeField]
	private TextMeshProUGUI _playButtonPriceText;

	[SerializeField]
	private Image _playButtonImage;

	[SerializeField]
	private Image _playButtonFadeImage;

	[SerializeField]
	private Slider _keySlider;

	[SerializeField]
	private TextMeshProUGUI _keyCounterText;

	[SerializeField]
	private TextMeshProUGUI _keyTimerText;

	private UIRewardPanel _rewardPopup;

	private PlayerLevelManager _levelManager;

	private Sequence _playButtonTween;

	private bool _isFirstPlayTween = true;

	private readonly WaitForSeconds wait = new WaitForSeconds(0.4f);

	protected override void Awake()
	{
		base.Awake();
		_rewardPopup = UIRewardPanel.Create();
		_levelManager = App.Instance.Player.LevelManager;
		if (App.Instance.NextMenuType == MenuType.World)
		{
			OnPlayButtonClicked();
		}
		if (_gameController.IsReadyToStart())
		{
			Pop();
		}
		else
		{
			_playButton.OnClick(OnPlayButtonClicked);
			_heroButton.OnClick(OnHeroesButtonClicked);
			_weaponButton.OnClick(OnWeaponButtonClicked);
			_settingsButton.OnClick(OnSettingsButtonClicked);
			_shopButton.OnClick(OnShopButtonClicked);
			UpdateWeaponBadge();
			UpdateHeroBadge();
			UpdateShopBadge();
			UpdateLootKeys();
		}
		App.Instance.NextMenuType = MenuType.Default;
		_keyTimerText.SetText(string.Empty);
		_heroBox.Init(_gameController.State.CurrentHero, _gameController.CharacterEvents);
		_shopPanel.Init(App.Instance.Player.ChestManager);
		UpdateWorldPrice(_levelManager.LastSelectedWorldId);
		UpdateUnlockButton();
		StartCoroutine(UpdateKeysTimerCR());
	}

	private void OnWorldSelectorReady()
	{
	}

	public override IEnumerator PopAnimationCR()
	{
		yield return new WaitForSeconds(0.1f);
		_worldRenderer.gameObject.SetActive( true);
		_worldRenderer.ShowTarget();
		_worldSelector.gameObject.SetActive( false);
		_heroBox.GetComponent<RectTransform>().DOScale(0.85f, 0.25f);
		CanvasGroup groupCanvas = _heroBox.GetComponent<CanvasGroup>();
		if (groupCanvas != null)
		{
			groupCanvas.alpha = 1f;
			DOTween.To(() => groupCanvas.alpha, delegate(float a)
			{
				groupCanvas.alpha = a;
			}, 0f, 0.25f);
		}
		_bottomMenu.DOAnchorPosY(0f - _bottomMenu.rect.height, 0.25f);
		RectTransform heroHpBar = _heroHpBar;
		Vector3 position = _heroHpBarTarget.position;
		float y = position.y;
		Vector3 localScale = _canvas.transform.localScale;
		float num = y / localScale.y - _heroHpBarTarget.rect.height * 0.5f;
		Vector2 anchoredPosition = _heroHpBarTarget.anchoredPosition;
		heroHpBar.DOAnchorPosY(num + anchoredPosition.y, 0.25f);
		RectTransform component = _settingsButton.GetComponent<RectTransform>();
		float num2 = Screen.height;
		Vector3 localScale2 = _canvas.transform.localScale;
		component.DOAnchorPosY(num2 * localScale2.y * 0.5f + 200f, 0.25f);
		yield return new WaitForSeconds(0.25f);
		if (!_gameController.IsReadyToStart())
		{
			UnityEngine.Debug.LogWarning("GameController should be ready to start a game!!");
		}
		yield return null;
		WorldAssetCaching.PreloadAssets(_worldSelector.WorldId);
		yield return null;
		_gameController.StartGame();
		yield return null;
		_gameHud.PrepareTransition();
		_gamePanel.SetActive( true);
	}

	private void OnEnable()
	{
		RegisterEvents();
	}

	private void OnDisable()
	{
		UnRegisterEvents();
	}

	private void RegisterEvents()
	{
		UnRegisterEvents();
		App.Instance.Events.InAppPurchaseDoneEvent += OnInAppPurchaseDone;
		App.Instance.MenuEvents.WorldSelectorReadyEvent += OnWorldSelectorReady;
		App.Instance.Player.LootManager.Events.LootUpdatedEvent += OnLootUpdated;
		App.Instance.Player.WeaponManager.Events.LevelUpEvent += OnWeaponLevelUp;
		App.Instance.Player.WeaponManager.Events.WeaponCardCollectedEvent += OnWeaponCardCollected;
		App.Instance.Player.WeaponManager.Events.NewWeaponSeenEvent += OnNewWeaponSeen;
		App.Instance.Player.HeroManager.Events.HeroCardCollectedEvent += OnHeroCardCollected;
		App.Instance.Player.HeroManager.Events.HeroLevelUpEvent += OnHeroLevelUp;
		App.Instance.Player.HeroManager.Events.NewHeroSeenEvent += OnNewHeroSeen;
		App.Instance.Player.RemoteRewardManager.Events.RemoteRewardReceivedEvent += OnRemoteRewardReceived;
		_worldSelector.WorldSelectedEvent += OnWorldSelected;
		_levelManager.Events.WorldUnlockedEvent += OnWorldUnlocked;
	}

	private void UnRegisterEvents()
	{
		if (App.IsCreated())
		{
			App.Instance.Events.InAppPurchaseDoneEvent -= OnInAppPurchaseDone;
			App.Instance.MenuEvents.WorldSelectorReadyEvent -= OnWorldSelectorReady;
			App.Instance.Player.LootManager.Events.LootUpdatedEvent -= OnLootUpdated;
			App.Instance.Player.WeaponManager.Events.WeaponCardCollectedEvent -= OnWeaponCardCollected;
			App.Instance.Player.WeaponManager.Events.LevelUpEvent -= OnWeaponLevelUp;
			App.Instance.Player.WeaponManager.Events.NewWeaponSeenEvent -= OnNewWeaponSeen;
			App.Instance.Player.HeroManager.Events.HeroCardCollectedEvent -= OnHeroCardCollected;
			App.Instance.Player.HeroManager.Events.HeroLevelUpEvent -= OnHeroLevelUp;
			App.Instance.Player.HeroManager.Events.NewHeroSeenEvent -= OnNewHeroSeen;
			App.Instance.Player.RemoteRewardManager.Events.RemoteRewardReceivedEvent -= OnRemoteRewardReceived;
			_levelManager.Events.WorldUnlockedEvent -= OnWorldUnlocked;
		}
		_worldSelector.WorldSelectedEvent -= OnWorldSelected;
	}

	public override void OnFocusLost()
	{
		if (!_gameController.IsReadyToStart())
		{
			_worldSelector.OnFocusLost();
		}
	}

	public override void OnFocusGained()
	{
		_worldSelector.OnFocusGained();
	}

	public override void OnFocusGaining()
	{
		_worldSelector.OnFocusGaining();
	}

	private void OnWorldSelected(string worldId)
	{
		WorldData worldData = _levelManager.GetWorldData(worldId);
		bool isUnlocked = worldData.IsUnlocked;
		if (!_isFirstPlayTween && (worldData.IsUnlocked || _levelManager.GetNextLockedWorldData().Config.Id == worldData.Config.Id))
		{
			if (_playButtonTween != null)
			{
				_playButtonTween.Complete();
				_playButtonTween = null;
			}
			_playButtonFadeImage.color = new Color(1f, 1f, 1f, 0f);
			_playButtonFadeImage.sprite = Resources.Load<Sprite>(GetPlayImagePath(isUnlocked));
			_playButtonTween = DOTween.Sequence();
			_playButtonTween.Insert(0f, _playButtonImage.DOFade(0f, 0.4f));
			_playButtonTween.Insert(0f, _playButtonFadeImage.DOFade(1f, 0.4f).OnStart(delegate
			{
				_playButtonFadeImage.enabled = true;
			}));
			_playButtonTween.OnComplete(delegate
			{
				_playButtonImage.sprite = _playButtonFadeImage.sprite;
				_playButtonImage.color = new Color(1f, 1f, 1f, 1f);
				_playButtonFadeImage.enabled = false;
				_playButtonTween = null;
			});
		}
		else
		{
			UpdatePlayButtonImage(isUnlocked);
			_isFirstPlayTween = false;
		}
		_keySlider.gameObject.SetActive(isUnlocked);
		_keyTimerText.gameObject.SetActive(isUnlocked);
		UpdateWorldPrice(worldId);
		UpdateUnlockButton();
	}

	private void UpdatePlayButtonImage(bool isCurrentWorldUnlocked)
	{
		_playButtonImage.sprite = Resources.Load<Sprite>(GetPlayImagePath(isCurrentWorldUnlocked));
	}

	private string GetPlayImagePath(bool isCurrentWorldUnlocked)
	{
		string str = "play_button";
		if (!isCurrentWorldUnlocked)
		{
			WorldData worldData = _levelManager.GetWorldData(_levelManager.LastSelectedWorldId);
			str = ((!worldData.AreMissionsCompleted()) ? "UI_lock_off" : "UI_lock_on");
		}
		return "UI/" + str;
	}

	private void OnWorldUnlocked(string worldId)
	{
		WorldData worldData = _levelManager.GetWorldData(worldId);
		bool isUnlocked = worldData.IsUnlocked;
		_keyTimerText.SetText(string.Empty);
		_keySlider.gameObject.SetActive(isUnlocked);
		_keyTimerText.gameObject.SetActive(isUnlocked);
		UpdateWorldPrice(worldId);
		UpdateUnlockButton();
		UpdatePlayButtonImage( true);
	}

	private void UpdateWorldPrice(string worldId)
	{
		WorldData worldData = _levelManager.GetWorldData(worldId);
		bool isUnlocked = worldData.IsUnlocked;
		_playButtonPriceText.text = ((!isUnlocked) ? worldData.Config.PriceToUnlock.ToString() : string.Empty);
	}

	private void OnInAppPurchaseDone(Reward reward, Product product)
	{
		_rewardPopup.Show(reward);
	}

	private void OnRemoteRewardReceived(Reward reward)
	{
		_rewardPopup.Show(reward);
	}

	private void OnLootUpdated(LootProfile loot, int deltaAmount, CurrencyReason reason)
	{
		if (loot.LootId == "lootCoin")
		{
			UpdateWeaponBadge();
			UpdateHeroBadge();
		}
		if (loot.LootId == "lootKey")
		{
			UpdateLootKeys();
		}
		WorldData worldData = _levelManager.GetWorldData(_levelManager.LastSelectedWorldId);
		if (worldData != null && !worldData.IsUnlocked)
		{
			if (worldData.Config == null || worldData.Config.PriceToUnlock == null)
			{
				UnityEngine.Debug.LogWarning("OnLootUpdated failed for lootId = " + loot.LootId + ", because world data is null for " + _levelManager.LastSelectedWorldId);
			}
			else if (loot.LootId == worldData.Config.PriceToUnlock.LootId)
			{
				UpdateUnlockButton();
			}
		}
	}

	private void OnWeaponLevelUp(string weaponId)
	{
		UpdateWeaponBadge();
	}

	private void OnWeaponCardCollected(string weaponId)
	{
		UpdateWeaponBadge();
	}

	private void OnNewWeaponSeen(string weaponId)
	{
		UpdateWeaponBadge();
	}

	private void OnNewHeroSeen()
	{
		UpdateHeroBadge();
	}

	private void UpdateWeaponBadge()
	{
		bool flag = App.Instance.Player.WeaponManager.CanUpgradeOneCard();
		bool animate = App.Instance.Player.WeaponManager.GetUpgradableWeaponCount() > 0;
		_weaponBadgeUpgrade.SetVisible(flag, animate);
		bool flag2 = App.Instance.Player.WeaponManager.HasNewCards();
		_weaponBadgeNew.SetVisible(!flag && flag2, true);
	}

	private void OnHeroCardCollected(HeroData heroData)
	{
		UpdateHeroBadge();
	}

	private void OnHeroLevelUp(HeroData heroData)
	{
		UpdateHeroBadge();
	}

	private void UpdateHeroBadge()
	{
		bool flag = App.Instance.Player.HeroManager.CanUpgradeOneCard();
		bool animate = App.Instance.Player.HeroManager.GetUpgradableHeroCount() > 0;
		_heroBadgeUpgrade.SetVisible(flag, animate);
		bool flag2 = App.Instance.Player.HeroManager.HasNewCards();
		_heroBadgeNew.SetVisible(!flag && flag2, true);
	}

	private void UpdateShopBadge()
	{
		ChestData chestData = App.Instance.Player.ChestManager.GetChestData("chestTimedFree");
		_shopBadge.SetVisible(chestData.IsReadyForRedeem(), true);
	}

	private void UpdateLootKeys()
	{
		int currentAmount = App.Instance.Player.LootKeyManager.GetCurrentAmount();
		int maxCapacitySoft = App.Instance.Player.LootKeyManager.GetMaxCapacitySoft();
		_keySlider.value = (float)currentAmount / (float)maxCapacitySoft;
		_keyCounterText.text = $"{currentAmount}/{maxCapacitySoft}";
	}

	private IEnumerator UpdateKeysTimerCR()
	{
		while (!App.Instance.Player.LootKeyManager.IsFull())
		{
			_keyTimerText.text = App.Instance.Player.LootKeyManager.GetRemainingTimeBeforeNextKey();
			yield return wait;
		}
		_keyTimerText.SetText(string.Empty);
	}

	private void OnPlayButtonClicked()
	{
		WorldData worldData = _levelManager.GetWorldData(_levelManager.LastSelectedWorldId);
		if (worldData.IsUnlocked)
		{
			if (App.Instance.Player.LootKeyManager.CanExpense() && App.Instance.Player.LootKeyManager.ExpenseKey())
			{
				_gameController.State.UnlockChest();
			}
			LevelData nextCustomLevel = _levelManager.GetNextCustomLevel(worldData, _gameController.State.HasUsedChestKey);
			_levelManager.OnGameReadyToStart(nextCustomLevel);
			_gameController.SetLevel(nextCustomLevel);
			_gameController.SetReadyToStart();
			Pop();
		}
		else
		{
			WorldData worldData2 = _levelManager.GetWorldData(_levelManager.LastSelectedWorldId);
			LootProfile priceToUnlock = worldData2.Config.PriceToUnlock;
			if (App.Instance.Player.LootManager.TryExpense(priceToUnlock.LootId, priceToUnlock.Amount, CurrencyReason.worldUnlocked))
			{
				App.Instance.Player.LevelManager.UnlockWorld(worldData2);
			}
			else
			{
				UINotEnoughLootPopup.Create(priceToUnlock.LootId, _shopPanel.Show).Show();
			}
		}
	}

	private void UpdateUnlockButton()
	{
		WorldData worldData = _levelManager.GetWorldData(_levelManager.LastSelectedWorldId);
		if (worldData.IsUnlocked)
		{
			_playButton.interactable = true;
			return;
		}
		WorldData nextLockedWorldData = App.Instance.Player.LevelManager.GetNextLockedWorldData();
		if (!nextLockedWorldData.Config.IsValid() || worldData.Config.Id == nextLockedWorldData.Config.Id)
		{
			bool flag = worldData.AreMissionsCompleted();
			_playButton.interactable = flag;
			if (!flag)
			{
				_playButton.SetDisabledExplanation("You need to complete the missions first!");
			}
		}
		else
		{
			_playButton.interactable = false;
			_playButton.SetDisabledExplanation("You must unlock previous worlds");
		}
	}

	private void OnWeaponButtonClicked()
	{
		_weaponBadgeNew.SetVisible( false, false);
		_weaponsPanel.Show();
	}

	private void OnShopButtonClicked()
	{
		_shopBadge.SetVisible( false, false);
		_shopPanel.Show();
	}

	private void OnSettingsButtonClicked()
	{
		_settingsPanel.Show();
	}

	private void OnHeroesButtonClicked()
	{
		_heroBadgeNew.SetVisible( false, false);
		_heroesPanel.Show();
	}
}