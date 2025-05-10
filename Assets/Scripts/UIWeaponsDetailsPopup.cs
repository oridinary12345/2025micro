using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponsDetailsPopup : UIMenuPopup
{
	[SerializeField]
	private Text _title;

	[SerializeField]
	private Text _levelText;

	[SerializeField]
	private Text _damageText;

	[SerializeField]
	private Text _damageBonusText;

	[SerializeField]
	private Text _hpBonusText;

	[SerializeField]
	private GameObject _upgradeArrowLevel;

	[SerializeField]
	private GameObject _upgradeArrowDamage;

	[SerializeField]
	private GameObject _upgradeArrowHP;

	[SerializeField]
	private Text _hpText;

	[SerializeField]
	private Image _weaponImage;

	[SerializeField]
	private Image _weaponShapeImage;

	[SerializeField]
	private Text _weaponShapeText;

	[SerializeField]
	private UIGameButton _buttonUpgrade;

	[SerializeField]
	private Text _buttonUpgradeText;

	[SerializeField]
	private Text _buttonUpgradePriceText;

	[SerializeField]
	private UIGameButton _buttonEquip;

	[SerializeField]
	private Text _buttonEquipText;

	[SerializeField]
	private UIGameButton _buttonClose;

	[SerializeField]
	private Slider _cardSlider;

	[SerializeField]
	private Text _cardProgressText;

	private WeaponConfig _weaponConfig;

	private WeaponData _weaponData;

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

	public UIWeaponsDetailsPopup Init(WeaponConfig weaponConfig, WeaponData weaponData, bool isEquipped, Action onEquipConfirmed)
	{
		_onEquipConfirmed = onEquipConfirmed;
		ApplyWeapon(weaponConfig, weaponData);
		App.Instance.Player.WeaponManager.SetHasSeen(weaponData.Id);
		return this;
	}

	private void OnEnable()
	{
		App.Instance.Player.LootManager.Events.LootUpdatedEvent += OnLootUpdated;
		App.Instance.Player.WeaponManager.Events.LevelUpEvent += OnLevelUp;
	}

	private void OnDisable()
	{
		if (App.IsCreated())
		{
			App.Instance.Player.LootManager.Events.LootUpdatedEvent -= OnLootUpdated;
			App.Instance.Player.WeaponManager.Events.LevelUpEvent -= OnLevelUp;
		}
	}

	public override void OnPop()
	{
		_sliderLastValue = -1f;
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

	private void ApplyWeapon(WeaponConfig weaponConfig, WeaponData weaponData)
	{
		_weaponConfig = weaponConfig;
		_weaponData = weaponData;
		_weaponShapeImage.sprite = Resources.Load<Sprite>("Weapons/" + weaponConfig.Id + "/target-shape_" + weaponConfig.Id);
		UpdateWeaponStats();
		UpdateWeaponVisual();
		UpdateButtons();
		UpdateCardsInfo();
		_upgradeArrowLevel.GetComponent<Image>().color = new Color(1f, 1f, 1f, (!CanUpgrade()) ? 0f : 1f);
		_upgradeArrowDamage.GetComponent<Image>().color = new Color(1f, 1f, 1f, (!CanUpgrade()) ? 0f : 1f);
		_upgradeArrowHP.GetComponent<Image>().color = new Color(1f, 1f, 1f, (!CanUpgrade()) ? 0f : 1f);
		_sliderLastValue = -1f;
	}

	private void OnUpgradeButtonClicked()
	{
		if (_weaponData.HasReachMaxLevel)
		{
			UnityEngine.Debug.LogWarning("Weapon's upgrade button should be disabled if it has reach the max level");
		}
		else if (_weaponData.CardObjectiveReached && App.Instance.Player.LootManager.TryExpense("lootCoin", _weaponData.GetLevelUpPriceAmount(), CurrencyReason.weaponUpgrade))
		{
			App.Instance.Player.WeaponManager.LevelUp(_weaponData.Id);
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

	private void OnLevelUp(string weaponId)
	{
		if (_weaponData.Id == weaponId)
		{
			UpdateWeaponStats();
			UpdateButtons();
			UpdateWeaponVisual();
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
			Vector3 damageOriginPos = _upgradeArrowDamage.transform.position;
			_levelUpTween.Insert(0f, _damageText.rectTransform.DOPunchScale(Vector3.one * 0.5f, 0.5f).SetDelay(0.25f));
			_levelUpTween.Insert(0f, _upgradeArrowDamage.transform.DOMoveY(damageOriginPos.y + 0.35f, 1f).SetDelay(0.25f).OnComplete(delegate
			{
				_upgradeArrowDamage.transform.position = damageOriginPos;
			}));
			_levelUpTween.Insert(0f, _upgradeArrowDamage.GetComponent<Image>().DOFade(0f, 1f).SetDelay(0.25f));
			_levelUpTween.OnComplete(delegate
			{
				if (CanUpgrade())
				{
					_upgradeArrowHP.GetComponent<Image>().color = Color.white;
					_upgradeArrowLevel.GetComponent<Image>().color = Color.white;
					_upgradeArrowDamage.GetComponent<Image>().color = Color.white;
				}
				_levelUpTween = null;
			});
		}
	}

	private void UpdateWeaponVisual()
	{
		_weaponImage.sprite = Resources.Load<Sprite>("Weapons/" + _weaponConfig.Id + "/UI_w_" + _weaponConfig.Id);
	}

	private void UpdateButtons()
	{
		if (_buttonUpgradeText != null)
		{
			_buttonUpgradeText.text = "UPGRADE";
		}

		if (_buttonEquipText != null)
		{
			_buttonEquipText.text = "EQUIP";
		}

		if (_buttonUpgradePriceText != null)
		{
			if (_weaponData.HasReachMaxLevel)
			{
				_buttonUpgradePriceText.text = "MAXED";
				_buttonUpgrade.interactable = false;
				_buttonUpgrade.SetDisabledExplanation("Max level reached!");
			}
			else
			{
				int levelUpPriceAmount = _weaponData.GetLevelUpPriceAmount();
				_buttonUpgradePriceText.text = levelUpPriceAmount.ToString("### ### ###").Trim() + InlineSprites.GetLootInlineSprite("lootCoin");
				_buttonUpgrade.interactable = _weaponData.CardObjectiveReached;
				_buttonUpgrade.SetDisabledExplanation("You need more cards");
				if (_weaponData.CardObjectiveReached)
				{
					_buttonUpgrade.interactable = CanAfford();
					_buttonUpgrade.SetDisabledExplanation("You don't have enough coins!");
				}
			}
		}

		if (_damageBonusText != null)
		{
			_damageBonusText.color = ((!CanUpgrade()) ? "#C6AA94FF".ToColor() : "#FF4364FF".ToColor());
		}

		if (_hpBonusText != null)
		{
			_hpBonusText.color = ((!CanUpgrade()) ? "#C6AA94FF".ToColor() : "#FF4364FF".ToColor());
		}
	}

	private bool CanUpgrade()
	{
		return CanAfford() && !_weaponData.HasReachMaxLevel && _weaponData.CardObjectiveReached;
	}

	private bool CanAfford()
	{
		int levelUpPriceAmount = _weaponData.GetLevelUpPriceAmount();
		return App.Instance.Player.LootManager.CanAfford("lootCoin", levelUpPriceAmount);
	}

	private void UpdateWeaponStats()
	{
		if (_title != null)
		{
			_title.text = _weaponConfig.Title.ToUpper();
		}

		if (_weaponShapeText != null)
		{
			_weaponShapeText.text = _weaponConfig.RangeType + " Range";
		}

		if (_damageText != null)
		{
			_damageText.text = _weaponData.GetMinDamage( true).ToString();
		}

		if (_damageBonusText != null)
		{
			_damageBonusText.text = "+" + _weaponData.GetNextLevelDamageBonus();
		}

		if (_hpText != null)
		{
			_hpText.text = _weaponData.HPMax.ToString();
		}

		if (_hpBonusText != null)
		{
			_hpBonusText.text = "+" + _weaponData.GetNextLevelHPBonus();
		}

		if (_levelText != null)
		{
			_levelText.text = "Lv. " + ((_weaponData == null) ? "1" : _weaponData.Level.ToString());
		}
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
			_cardSlider.value = _weaponData.NextLevelObjective01;
		}
		else
		{
			_cardSliderTween = _cardSlider.DOValue(_weaponData.NextLevelObjective01, 0.5f).OnComplete(delegate
			{
				_cardSliderTween = null;
			});
		}
		_sliderLastValue = _weaponData.NextLevelObjective01;
		_cardProgressText.text = _weaponData.NextLevelProgressString;
	}
}