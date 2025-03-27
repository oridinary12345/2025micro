using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHeroesPanelBox : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _titleLabel;

	[SerializeField]
	private TextMeshProUGUI _hpLabel;

	[SerializeField]
	private TextMeshProUGUI _levelLabel;

	[SerializeField]
	private Image _heroImage;

	[SerializeField]
	private TextMeshProUGUI _cardProgressText;

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
		}
		_heroImage.sprite = Resources.Load<Sprite>("Heroes/UI_" + _hero.HeroConfig.Id);
		_titleLabel.text = _hero.HeroConfig.Name;
		UpdateHeroStats();
		UpdateCardsInfo();
		UpdateUpgradeBadge();
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
		_hpLabel.text = "<sprite=1>" + ((_hero == null) ? _hero.HeroConfig.HpMaxBase : _hero.HeroConfig.GetHPMax(_hero.Profile.Level)).ToString();
		_levelLabel.text = "Lv. " + ((_hero == null) ? "1" : _hero.Profile.Level.ToString());
	}

	private void UpdateCardsInfo()
	{
		_cardSlider.value = _hero.NextLevelObjective01;
		_cardProgressText.text = _hero.NextLevelProgressString;
	}
}