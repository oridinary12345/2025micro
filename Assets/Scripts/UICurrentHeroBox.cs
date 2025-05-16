using UnityEngine;
using UnityEngine.UI;

public class UICurrentHeroBox : MonoBehaviour
{
	[SerializeField]
	private UIGameButton _upgradeButton;

	[SerializeField]
	private UIGameButton _instantHealButton;

	[SerializeField]
	private Text _upgradePriceText;

	[SerializeField]
	private Text _instantHealPriceText;

	[SerializeField]
	private Text _levelText;
	
	[SerializeField]
	private ResourceDisplay _upgradePriceDisplay; // 升级价格显示
	
	[SerializeField]
	private ResourceDisplay _instantHealPriceDisplay; // 立即治疗价格显示

	[SerializeField]
	private UIStatusBarCharacter _heroHpBar;

	private Hero _hero;

	public void Init(Hero hero, CharacterEvents characterEvents)
	{
		_hero = hero;
		_heroHpBar.Init(hero, characterEvents, true, true);
		_upgradeButton.OnClick(OnUpgradeButtonClicked);
		_instantHealButton.OnClick(OnInstantHealButtonClicked);
		UpdateUpgradePrice();
		UpdateHealPrice();
	}

	private void OnEnable()
	{
		App.Instance.Player.LootManager.Events.LootUpdatedEvent -= OnLootUpdated;
		App.Instance.Player.LootManager.Events.LootUpdatedEvent += OnLootUpdated;
		MonoSingleton<GameAdsController>.Instance.Events.VideoPlayFailedEvent += OnVideoPlayFailed;
		HeroEvents events = App.Instance.Player.HeroManager.Events;
		events.HeroLevelUpEvent -= OnHeroLevelUp;
		events.HeroHealedEvent -= OnHeroHealed;
		events.HeroLevelUpEvent += OnHeroLevelUp;
		events.HeroHealedEvent += OnHeroHealed;
	}

	private void OnDisable()
	{
		if (App.IsCreated())
		{
			App.Instance.Player.LootManager.Events.LootUpdatedEvent -= OnLootUpdated;
			HeroEvents events = App.Instance.Player.HeroManager.Events;
			events.HeroLevelUpEvent -= OnHeroLevelUp;
			events.HeroHealedEvent -= OnHeroHealed;
		}
		if (MonoSingleton<GameAdsController>.IsCreated())
		{
			MonoSingleton<GameAdsController>.Instance.Events.VideoPlayFailedEvent -= OnVideoPlayFailed;
		}
	}

	private void OnLootUpdated(LootProfile loot, int delta, CurrencyReason reason)
	{
		if (loot.LootId == "lootCoin")
		{
			UpdateUpgradePrice();
		}
		else if (loot.LootId == "lootRuby")
		{
			UpdateHealPrice();
		}
	}

	private void OnUpgradeButtonClicked()
	{
		if (App.Instance.Player.LootManager.TryExpense("lootCoin", App.Instance.Player.HeroManager.GetLevelUpPriceAmount(_hero.Id), CurrencyReason.heroUpgrade))
		{
			App.Instance.Player.HeroManager.LevelUp(_hero.Id);
		}
	}

	private void OnInstantHealButtonClicked()
	{
	}

	private void UpdateUpgradePrice()
	{
		int levelUpPriceAmount = App.Instance.Player.HeroManager.GetLevelUpPriceAmount(_hero.Id);
		
		// 优先使用ResourceDisplay显示升级价格
		if (_upgradePriceDisplay != null)
		{
			Sprite coinIcon = null;
			
			// 尝试从SpriteAssetManager获取图标
			if (SpriteAssetManager.Instance != null)
			{
				coinIcon = SpriteAssetManager.Instance.GetSprite(3); // 金币对应sprite=3
			}
			// 如果SpriteAssetManager不可用，尝试使用ResourceManager
			else if (ResourceManager.Instance != null)
			{
				coinIcon = ResourceManager.Instance.GetResourceIconBySpriteIndex(3);
			}
			
			_upgradePriceDisplay.SetValue(coinIcon, levelUpPriceAmount);
		}
		// 兼容旧版本
		else if (_upgradePriceText != null)
		{
			_upgradePriceText.text = levelUpPriceAmount + InlineSprites.GetLootInlineSprite("lootCoin");
		}
		
		bool interactable = App.Instance.Player.LootManager.CanAfford("lootCoin", levelUpPriceAmount);
		_upgradeButton.interactable = interactable;
		_upgradeButton.SetDisabledExplanation("You don't have enough coins");
		_levelText.text = $"Lv. {_hero.Level}";
	}

	private void UpdateHealPrice()
	{
	}

	private void OnHeroLevelUp(HeroData hero)
	{
		UpdateUpgradePrice();
		UpdateHealPrice();
	}

	private void OnHeroHealed(HeroData hero, int healAmount, bool skipFX)
	{
		UpdateUpgradePrice();
		UpdateHealPrice();
	}

	private void OnVideoPlayFailed()
	{
		UpdateHealPrice();
	}
}