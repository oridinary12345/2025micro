using UnityEngine;
using UnityEngine.UI;

public class UIHeroesPanelBox : MonoBehaviour
{
	[SerializeField]
	private Text _titleLabel;

	[SerializeField]
	private ResourceDisplay _hpDisplay;

	[SerializeField]
	private Text _hpLabel; // 兼容旧版本

	[SerializeField]
	private Text _levelLabel;

	[SerializeField]
	private Image _heroImage;

	[SerializeField]
	private Text _cardProgressText;

	[SerializeField]
	private Slider _cardSlider;

	[SerializeField]
	private UIBadge _upgradeBadge;

	[SerializeField]
	private UIBadge _newBadge;

	private HeroData _hero;

	public HeroData HeroData => _hero;

	public UIHeroesPanelBox Init(HeroData heroData, bool isSelected)
	{
		_hero = heroData;
		if (_hero != null)
		{
			_hero.Events.HeroLevelUpEvent += OnHeroLevelUp;

			if (_heroImage != null)
			{
				_heroImage.sprite = Resources.Load<Sprite>("Heroes/UI_" + _hero.HeroConfig.Id);
			}

			if (_titleLabel != null)
			{
				_titleLabel.text = _hero.HeroConfig.Name;
			}

			UpdateHeroStats();
			UpdateCardsInfo();
			UpdateUpgradeBadge();
		}
		return this;
	}

	private void OnDestroy()
	{
		if (_hero != null)
		{
			_hero.Events.HeroLevelUpEvent -= OnHeroLevelUp;
		}
	}

	private void OnEnable()
	{
		App.Instance.Player.LootManager.Events.LootUpdatedEvent += OnLootUpdated;
	}

	private void OnDisable()
	{
		if (App.IsCreated())
		{
			App.Instance.Player.LootManager.Events.LootUpdatedEvent -= OnLootUpdated;
		}
	}

	private void OnLootUpdated(LootProfile loot, int delta, CurrencyReason reason)
	{
		string lootId = loot.LootId;
		if (lootId == "lootCoin")
		{
			UpdateUpgradeBadge();
		}
	}

	public void Refresh()
	{
		UpdateHeroStats();
		UpdateCardsInfo();
		UpdateUpgradeBadge();
	}

	public void Select()
	{
	}

	public void Unselect()
	{
	}

	private void OnHeroLevelUp(HeroData hero)
	{
		UpdateHeroStats();
		UpdateCardsInfo();
		UpdateUpgradeBadge();
	}

	private void UpdateUpgradeBadge()
	{
		_upgradeBadge.SetVisible(IsUpgradeBadgeVisible(), true);
		_newBadge.SetVisible(!IsUpgradeBadgeVisible() && _hero.Profile.IsNew && IsUnlocked(), true);
	}

	private bool IsUpgradeBadgeVisible()
	{
		return CanAffordUpgrade() && !_hero.HasReachMaxLevel && IsUnlocked();
	}

	private bool CanAffordUpgrade()
	{
		return _hero.CardObjectiveReached && App.Instance.Player.LootManager.CanAfford("lootCoin", _hero.GetLevelUpPriceAmount());
	}

	private bool IsUnlocked()
	{
		return _hero.Unlocked;
	}

	private void UpdateHeroStats()
	{
		if (_hero == null)
		{
			Debug.LogWarning("Hero is null in UIHeroesPanelBox.UpdateHeroStats");
			return;
		}

		// 获取HP值
		int hpValue = _hero.HeroConfig.GetHPMax(_hero.Profile.Level);
		
		// 优先使用ResourceDisplay
		if (_hpDisplay != null)
		{
			// 尝试从SpriteAssetManager获取图标
			Sprite hpIcon = null;
			
			if (SpriteAssetManager.Instance != null)
			{
				hpIcon = SpriteAssetManager.Instance.GetHeartSprite();
			}
			// 如果SpriteAssetManager不可用，尝试使用ResourceManager
			else if (ResourceManager.Instance != null)
			{
				hpIcon = ResourceManager.Instance.GetResourceIconBySpriteIndex(1);
			}
			
			_hpDisplay.SetValue(hpIcon, hpValue);
		}
		// 兼容旧版本
		else if (_hpLabel != null)
		{
			_hpLabel.text = "<sprite=1>" + hpValue.ToString();
		}

		if (_levelLabel != null)
		{
			_levelLabel.text = "Lv. " + _hero.Profile.Level.ToString();
		}
	}

	private void UpdateCardsInfo()
	{
		if (_hero == null)
		{
			Debug.LogWarning("Hero is null in UIHeroesPanelBox.UpdateCardsInfo");
			return;
		}

		if (_cardSlider != null)
		{
			_cardSlider.value = _hero.NextLevelObjective01;
		}

		if (_cardProgressText != null)
		{
			_cardProgressText.text = _hero.NextLevelProgressString;
		}
	}
}