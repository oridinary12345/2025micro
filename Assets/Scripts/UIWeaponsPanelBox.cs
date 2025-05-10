using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponsPanelBox : MonoBehaviour
{
	[SerializeField]
	private Text _levelLabel;

	[SerializeField]
	private Text _damageLabel;

	[SerializeField]
	private Text _hpLabel;

	[SerializeField]
	private Image _weaponImage;

	[SerializeField]
	private Image _weaponBackgroundImage;

	[SerializeField]
	private Image _statusIconImage;

	[SerializeField]
	private Image _statusIconOverlayImage;

	[SerializeField]
	private GameObject _statsPanel;

	[SerializeField]
	private Text _lockedIconText;

	[SerializeField]
	private Text _lockedText;

	[SerializeField]
	private Slider _hpSlider;

	[SerializeField]
	private Image _sliderFillImage;

	[SerializeField]
	private UIBadge _badgeUpgrade;

	[SerializeField]
	private UIBadge _badgeNew;

	[SerializeField]
	private GameObject _emptyBox;

	[SerializeField]
	private GameObject _cardIcons;

	[SerializeField]
	private Text _cardProgressText;

	[SerializeField]
	private UIGradient _weaponBackgroundGradient;

	private WeaponData _weaponData;

	private Sequence _statusIconOverlaySequence;

	private Tweener _sliderColorTween;

	private bool _isSelected;

	public int Index
	{
		get;
		private set;
	}

	public WeaponConfig WeaponConfig
	{
		get;
		private set;
	}

	public WeaponData WeaponData => _weaponData;

	public UIWeaponsPanelBox Init(WeaponConfig weaponConfig, WeaponData weaponData, int index)
	{
		ApplyWeapon(weaponConfig, weaponData, index);
		return this;
	}

	public void HideStatusIcons()
	{
		_statusIconImage.enabled = false;
		_statusIconOverlayImage.enabled = false;
		_badgeUpgrade.SetVisible( false, false);
		_badgeNew.SetVisible( false, false);
		_cardIcons.SetActive( false);
	}

	public void HideBadgeNew()
	{
		_badgeNew.SetVisible( false, false);
	}

	private void OnEnable()
	{
		App.Instance.Player.LootManager.Events.LootUpdatedEvent += OnLootUpdated;
		App.Instance.Player.WeaponManager.Events.RepairedEvent += OnRepaired;
		App.Instance.Player.WeaponManager.Events.LevelUpEvent += OnLevelUp;
	}

	private void OnDisable()
	{
		if (App.IsCreated())
		{
			App.Instance.Player.LootManager.Events.LootUpdatedEvent -= OnLootUpdated;
			App.Instance.Player.WeaponManager.Events.RepairedEvent -= OnRepaired;
			App.Instance.Player.WeaponManager.Events.LevelUpEvent -= OnLevelUp;
		}
	}

	private void OnDestroy()
	{
		if (_sliderColorTween != null)
		{
			_sliderColorTween.Kill( true);
			_sliderColorTween = null;
		}
	}

	private void OnLootUpdated(LootProfile loot, int delta, CurrencyReason reason)
	{
		if (!IsEmpty())
		{
			string lootId = loot.LootId;
			if (lootId == "lootCoin" || lootId == "lootRuby")
			{
				UpdateStatusIcon();
			}
		}
	}

	public void Equip(WeaponConfig weaponConfig, WeaponData weaponData, int index)
	{
		ApplyWeapon(weaponConfig, weaponData, index);
	}

	public void SetEmpty()
	{
		HideStatusIcons();
	}

	private bool IsEmpty()
	{
		return _weaponData == null;
	}

	private void ApplyWeapon(WeaponConfig weaponConfig, WeaponData weaponData, int index)
	{
		if (_emptyBox != null)
		{
			_emptyBox.SetActive( false);
		}
		if (_sliderColorTween != null)
		{
			_sliderColorTween.Kill( true);
			_sliderColorTween = null;
		}
		_weaponBackgroundGradient.GetComponent<Image>().color = Color.white;
		bool unlocked = weaponData.Unlocked;
		if (unlocked)
		{
			if (weaponConfig.RangeType == WeaponRangeType.Short)
			{
				_weaponBackgroundGradient.m_color1 = "#de742d".ToColor();
				_weaponBackgroundGradient.m_color2 = "#f7c740".ToColor();
			}
			else if (weaponConfig.RangeType == WeaponRangeType.Medium)
			{
				_weaponBackgroundGradient.m_color1 = "#df335c".ToColor();
				_weaponBackgroundGradient.m_color2 = "#f0865a".ToColor();
			}
			else if (weaponConfig.RangeType == WeaponRangeType.Long)
			{
				_weaponBackgroundGradient.m_color1 = "#499a7f".ToColor();
				_weaponBackgroundGradient.m_color2 = "#6fc3ce".ToColor();
			}
		}
		Color color = "#B89B85FF".ToColor();
		Color color2 = "#C5C5C5FF".ToColor();
		Color color3 = "#00000077".ToColor();
		if (!_isSelected && !unlocked)
		{
			_weaponBackgroundImage.color = ((!unlocked) ? color2 : color);
		}
		_weaponImage.color = ((!unlocked) ? color3 : Color.white);
		_lockedText.gameObject.SetActive(!unlocked);
		_lockedIconText.gameObject.SetActive(!unlocked);
		_statsPanel.SetActive(unlocked);
		_cardIcons.SetActive(unlocked && !IsEmpty());
		Index = index;
		WeaponConfig = weaponConfig;
		_weaponData = weaponData;
		UpdateStatusIcon();
		UpdateSpriteVisual();
		if (unlocked)
		{
			UpdateWeaponStats();
		}
		UpdateCardsInfo();
	}

	public void Refresh()
	{
		ApplyWeapon(WeaponConfig, WeaponData, Index);
	}

	public void Select()
	{
		_isSelected = true;
		_weaponBackgroundImage.color = new Color32(49, 74, 87, byte.MaxValue);
	}

	public void Unselect()
	{
	}

	private void OnLevelUp(string weaponId)
	{
		if (!IsEmpty() && weaponId == _weaponData.Id)
		{
			UpdateWeaponStats();
			UpdateStatusIcon();
			UpdateSpriteVisual();
			UpdateCardsInfo();
		}
	}

	private void OnRepaired(string weaponId, int healAmount, bool skipFX)
	{
		if (!IsEmpty() && weaponId == _weaponData.Id)
		{
			UpdateWeaponStats();
			UpdateStatusIcon();
			UpdateSpriteVisual();
		}
	}

	private bool NeedsRepairing()
	{
		return GetMissingHP() > 0;
	}

	private int GetMissingHP()
	{
		return _weaponData.HPMax - _weaponData.HP;
	}

	private void UpdateSpriteVisual()
	{
		_weaponImage.sprite = Resources.Load<Sprite>("Weapons/" + WeaponConfig.Id + "/UI_w_" + WeaponConfig.Id + ((!IsBroken()) ? string.Empty : "_broken"));
	}

	private void UpdateStatusIcon()
	{
		string text = string.Empty;
		if (IsRepairing())
		{
			text = "UI_weaponStatusClock";
		}
		else if (NeedsRepairing() && CanAffordRepair())
		{
			text = "UI_weaponStatusRepair";
		}
		if (!string.IsNullOrEmpty(text) && _weaponData.Unlocked)
		{
			_statusIconImage.sprite = Resources.Load<Sprite>("UI/" + text);
		}
		_statusIconImage.enabled = false;
		_statusIconOverlayImage.enabled = false;
		UpdateUpgradeBadge();
	}

	private void UpdateUpgradeBadge()
	{
		bool flag = IsUpgradeBadgeVisible();
		bool animate = IsUpgradeBadgeAnimated();
		_badgeUpgrade.SetVisible(flag, animate);
		_badgeNew.SetVisible(!flag && _weaponData.Unlocked && _weaponData.Profile.IsNew, true);
	}

	private bool IsUpgradeBadgeVisible()
	{
		return _weaponData.CardObjectiveReached && !WeaponData.HasReachMaxLevel && IsUnlocked();
	}

	private bool IsUpgradeBadgeAnimated()
	{
		return CanAffordUpgrade() && !WeaponData.HasReachMaxLevel && IsUnlocked();
	}

	private bool IsBroken()
	{
		return _weaponData.Broken;
	}

	private bool CanAffordUpgrade()
	{
		return _weaponData.CardObjectiveReached && App.Instance.Player.LootManager.CanAfford("lootCoin", _weaponData.GetLevelUpPriceAmount());
	}

	private bool CanAffordRepair()
	{
		return App.Instance.Player.LootManager.CanAfford("lootRuby", _weaponData.GetRepairPriceAmount());
	}

	private bool IsRepairing()
	{
		return false;
	}

	private bool IsUnlocked()
	{
		return WeaponData.Unlocked;
	}

	private void UpdateWeaponStats()
	{
		_damageLabel.text = $"{_weaponData.GetMinDamage()}";
		_damageLabel.color = ((!IsBroken()) ? new Color32(49, 74, 87, byte.MaxValue) : new Color32(185, 0, 77, byte.MaxValue));
		_levelLabel.text = "Lv. " + ((_weaponData == null) ? "1" : _weaponData.Level.ToString());
		_hpLabel.text = $"{_weaponData.HP}";
		_hpLabel.color = ((!IsBroken()) ? new Color32(49, 74, 87, byte.MaxValue) : new Color32(185, 0, 77, byte.MaxValue));
	}

	private void UpdateCardsInfo()
	{
		_hpSlider.value = _weaponData.NextLevelObjective01;
		_cardProgressText.text = _weaponData.NextLevelProgressString;
	}
}