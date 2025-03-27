using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHeroDetailsPopup : UIMenuPopup
{
	[SerializeField]
	private TextMeshProUGUI _title;

	[SerializeField]
	private TextMeshProUGUI _levelText;

	[SerializeField]
	private GameObject _upgradeArrowLevel;

	[SerializeField]
	private GameObject _upgradeArrowHP;

	[SerializeField]
	private TextMeshProUGUI _hpText;

	[SerializeField]
	private TextMeshProUGUI _hpBonusText;

	[SerializeField]
	private Image _heroImage;

	[SerializeField]
	private UIGameButton _buttonUpgrade;

	[SerializeField]
	private TextMeshProUGUI _buttonUpgradeText;

	[SerializeField]
	private TextMeshProUGUI _buttonUpgradePriceText;

	[SerializeField]
	private UIGameButton _buttonEquip;

	[SerializeField]
	private TextMeshProUGUI _buttonEquipText;

	[SerializeField]
	private UIGameButton _buttonClose;

	[SerializeField]
	private Slider _cardSlider;

	[SerializeField]
	private TextMeshProUGUI _cardProgressText;

	private HeroData _heroData;

	private Action _onEquipConfirmed;

	private Sequence _levelUpTween;

	private float _sliderLastValue = -1f;

	private Tweener _cardSliderTween;

	protected override void Awake()
	{
		base.Awake();
		_buttonUpgrade.OnClick(OnUpgradeButtonClicked);
		_buttonEquip.OnClick(OnEquipButtonClicked);
		_buttonClose.OnClick(OnCloseButtonClicked);
		_buttonClose.ActivateOnBackKey();
	}

	public UIHeroDetailsPopup Init(HeroData heroData, Action onEquipConfirmed)
	{
		_onEquipConfirmed = onEquipConfirmed;
		ApplyHero(heroData);
		App.Instance.Player.HeroManager.SetHasSeen(heroData.HeroConfig.Id);
		return this;
	}

	private void OnEnable()
	{
		App.Instance.Player.LootManager.Events.LootUpdatedEvent += OnLootUpdated;
		App.Instance.Player.HeroManager.Events.HeroLevelUpEvent += OnHeroLevelUp;
	}

	private void OnDisable()
	{
		if (App.IsCreated())
		{
			App.Instance.Player.LootManager.Events.LootUpdatedEvent -= OnLootUpdated;
			App.Instance.Player.HeroManager.Events.HeroLevelUpEvent -= OnHeroLevelUp;
		}
	}

	public override void OnPop()
	{
		if (_cardSliderTween != null)
		{
			_cardSliderTween.Kill( true);
			_cardSliderTween = null;
		}
		if (_levelUpTween != null)
		{
			_levelUpTween.Kill( true);
			_levelUpTween = null;
		}
		base.OnPop();
	}

	private void OnLootUpdated(LootProfile loot, int delta, CurrencyReason reason)
	{
		string lootId = loot.LootId;
		if (lootId == "lootCoin" || lootId == "lootRuby")
		{
			UpdateButtons();
		}
	}

	private void ApplyHero(HeroData heroData)
	{
		_heroData = heroData;
		UpdateHeroStats();
		UpdateHeroVisual();
		UpdateButtons();
		UpdateCardsInfo();
		_upgradeArrowLevel.GetComponent<Image>().color = new Color(1f, 1f, 1f, (!CanUpgrade()) ? 0f : 1f);
		_upgradeArrowHP.GetComponent<Image>().color = new Color(1f, 1f, 1f, (!CanUpgrade()) ? 0f : 1f);
		_sliderLastValue = -1f;
	}

	private void OnUpgradeButtonClicked()
	{
		if (_heroData.HasReachMaxLevel)
		{
			UnityEngine.Debug.LogWarning("Hero's upgrade button should be disabled if it has reach the max level");
		}
		else if (_heroData.CardObjectiveReached && App.Instance.Player.LootManager.TryExpense("lootCoin", _heroData.GetLevelUpPriceAmount(), CurrencyReason.heroUpgrade))
		{
			App.Instance.Player.HeroManager.LevelUp(_heroData.HeroConfig.Id);
		}
	}

	private void OnEquipButtonClicked()
	{
		if (_onEquipConfirmed != null)
		{
			_onEquipConfirmed();
		}
		Hide();
	}

	private void OnCloseButtonClicked()
	{
		Hide();
	}

	private void OnHeroLevelUp(HeroData hero)
	{
		if (_heroData.HeroConfig.Id == hero.HeroConfig.Id)
		{
			UpdateHeroStats();
			UpdateButtons();
			UpdateHeroVisual();
			UpdateCardsInfo();
			if (_levelUpTween != null)
			{
				_levelUpTween.Complete( true);
				_levelUpTween = null;
			}
			_levelUpTween = DOTween.Sequence();
			Vector3 levelOriginPos = _upgradeArrowLevel.transform.position;
			_levelUpTween.Insert(0f, _upgradeArrowLevel.transform.DOMoveY(levelOriginPos.y + 0.35f, 1f).OnComplete(delegate
			{
				_upgradeArrowLevel.transform.position = levelOriginPos;
			}));
			_levelUpTween.Insert(0f, _upgradeArrowLevel.GetComponent<Image>().DOFade(0f, 1f));
			Vector3 hpOriginPos = _upgradeArrowHP.transform.position;
			_levelUpTween.Insert(0f, _hpText.rectTransform.DOPunchScale(Vector3.one * 0.5f, 0.5f));
			_levelUpTween.Insert(0f, _upgradeArrowHP.transform.DOMoveY(hpOriginPos.y + 0.35f, 1f).OnComplete(delegate
			{
				_upgradeArrowHP.transform.position = hpOriginPos;
			}));
			_levelUpTween.Insert(0f, _upgradeArrowHP.GetComponent<Image>().DOFade(0f, 1f));
			_levelUpTween.OnComplete(delegate
			{
				if (CanUpgrade())
				{
					_upgradeArrowHP.GetComponent<Image>().color = Color.white;
					_upgradeArrowLevel.GetComponent<Image>().color = Color.white;
				}
				_levelUpTween = null;
			});
		}
	}

	private void UpdateHeroVisual()
	{
		_heroImage.sprite = Resources.Load<Sprite>("Heroes/UI_" + _heroData.HeroConfig.Id);
	}

	private void UpdateButtons()
	{
		_buttonUpgradeText.text = "UPGRADE";
		_buttonEquipText.text = "SELECT";
		if (_heroData.HasReachMaxLevel)
		{
			_buttonUpgradePriceText.text = "MAXED";
			_buttonUpgrade.interactable = false;
			_buttonUpgrade.SetDisabledExplanation("Max level reached!");
		}
		else
		{
			int levelUpPriceAmount = _heroData.GetLevelUpPriceAmount();
			_buttonUpgradePriceText.text = levelUpPriceAmount.ToString("### ### ###").Trim() + InlineSprites.GetLootInlineSprite("lootCoin");
			_buttonUpgrade.interactable = _heroData.CardObjectiveReached;
			_buttonUpgrade.SetDisabledExplanation("You need more cards");
			if (_heroData.CardObjectiveReached)
			{
				_buttonUpgrade.interactable = CanAfford();
				_buttonUpgrade.SetDisabledExplanation("You don't have enough coins!");
			}
		}
		_hpBonusText.color = ((!CanUpgrade()) ? "#C6AA94FF".ToColor() : "#FF4364FF".ToColor());
	}

	private bool CanUpgrade()
	{
		return CanAfford() && !_heroData.HasReachMaxLevel && _heroData.CardObjectiveReached;
	}

	private bool CanAfford()
	{
		int levelUpPriceAmount = _heroData.GetLevelUpPriceAmount();
		return App.Instance.Player.LootManager.CanAfford("lootCoin", levelUpPriceAmount);
	}

	private void UpdateHeroStats()
	{
		_title.text = _heroData.HeroConfig.Name.ToUpper();
		_hpText.text = _heroData.GetMaxHP().ToString();
		_hpBonusText.text = "+" + _heroData.GetNextLevelHPBonus();
		_levelText.text = "Lv. " + ((_heroData == null) ? "1" : _heroData.Level.ToString());
	}

	private void UpdateCardsInfo()
	{
		if (_cardSliderTween != null)
		{
			_cardSliderTween.Complete( true);
			_cardSliderTween = null;
		}
		if (_sliderLastValue == -1f)
		{
			_cardSlider.value = _heroData.NextLevelObjective01;
		}
		else
		{
			_cardSliderTween = _cardSlider.DOValue(_heroData.NextLevelObjective01, 0.5f).OnComplete(delegate
			{
				_cardSliderTween = null;
			});
		}
		_sliderLastValue = _heroData.NextLevelObjective01;
		_cardProgressText.text = _heroData.NextLevelProgressString;
	}
}