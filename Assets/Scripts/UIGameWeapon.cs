using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIGameWeapon : MonoBehaviour
{
	[SerializeField]
	private Text _atkLabel;

	[SerializeField]
	private Image _atkBackground;

	[SerializeField]
	private Image _hpSlider;

	[SerializeField]
	private GameObject _hpLayout;

	[SerializeField]
	private Image _weaponIcon;

	[SerializeField]
	private Image _elementIcon;

	[SerializeField]
	private Image _weaponIconExtra;

	[SerializeField]
	private Image _backgroundIcon;

	[SerializeField]
	private Image _backgroundBlinkIcon;

	[SerializeField]
	private Text _hpText;

	[SerializeField]
	private UIGameButton _repairButton;

	[SerializeField]
	private GameObject _emptyBox;

	private WeaponData _weaponData;

	private GameState _gameState;

	private WeaponPrefab _weaponPrefab;

	private const float SelectedBackgroundScaleFactor = 1.25f;

	private const float SelectedBackgroundPulseScaleFactor = 1.45f;

	private Tweener _backgroundTweener;

	public void Init(GameState gameState, WeaponData weapon)
	{
		if (gameState == null || weapon == null)
		{
			Debug.LogWarning("GameState or WeaponData is null in UIGameWeapon.Init");
			return;
		}

		if (_emptyBox != null)
		{
			_emptyBox.SetActive(false);
		}

		_gameState = gameState;
		_weaponData = weapon;

		_weaponPrefab = Resources.Load<WeaponPrefab>("Weapons/" + weapon.Id + "/" + weapon.GetSpriteName());
		if (_weaponPrefab == null)
		{
			Debug.LogWarning($"Failed to load weapon prefab for {weapon.Id}");
			return;
		}

		if (_weaponIcon != null)
		{
			_weaponIcon.sprite = ((!weapon.Broken) ? _weaponPrefab.WeaponSpriteUI : _weaponPrefab.WeaponSpriteBrokenUI);
		}

		if (_hpSlider != null)
		{
			_hpSlider.fillAmount = weapon.HP01;
		}

		if (_hpText != null)
		{
			_hpText.text = weapon.HP.ToString();
		}

		weapon.DamageTakenEvent -= OnDamageTaken;
		weapon.DamageTakenEvent += OnDamageTaken;
		weapon.BrokenEvent -= OnBroken;
		weapon.BrokenEvent += OnBroken;
		_gameState.LootUpdatedEvent -= OnLootUpdated;
		_gameState.LootUpdatedEvent += OnLootUpdated;

		UpdateRepairButton();
		if (_repairButton != null)
		{
			_repairButton.OnClick(OnRepairButtonClicked);
		}

		UpdateAtkLabel();
	}

	public void SetEmpty()
	{
		_repairButton.gameObject.SetActive( false);
		_hpLayout.SetActive( false);
	}

	private void OnEnable()
	{
		App.Instance.Player.WeaponManager.Events.RepairedEvent += OnRepaired;
	}

	private void OnDisable()
	{
		if (App.IsCreated())
		{
			App.Instance.Player.WeaponManager.Events.RepairedEvent -= OnRepaired;
		}
	}

	private void OnDestroy()
	{
		if (_weaponData != null)
		{
			_weaponData.DamageTakenEvent -= OnDamageTaken;
			_weaponData.BrokenEvent -= OnBroken;
		}
	}

	private void OnDamageTaken()
	{
		UpdateHp();
	}

	private void OnBroken(string weaponId)
	{
		UpdateAtkLabel();
		UpdateRepairButton();
		_weaponIcon.sprite = _weaponPrefab.WeaponSpriteBrokenUI;
	}

	private void UpdateAtkLabel()
	{
		if (_atkLabel != null && _weaponData != null)
		{
			_atkLabel.text = Mathf.RoundToInt(_weaponData.GetMinDamage()).ToString();
			_atkLabel.color = Color.white;
			Color color = "#122B4AFF".ToColor();
			Color color2 = "#F74140FF".ToColor();
			if (_atkBackground != null && _weaponData != null)
			{
				_atkBackground.color = ((!_weaponData.Broken) ? color : color2);
			}
		}
	}

	private void UpdateHp()
	{
		if (_weaponData == null)
		{
			return;
		}

		if (_hpSlider != null)
		{
			_hpSlider.DOFillAmount(_weaponData.HP01, 0.2f);
		}

		if (_hpText != null)
		{
			_hpText.text = _weaponData.HP.ToString();
			_hpText.color = (!_weaponData.Broken) ? Color.white : Color.red;
		}
	}

	public void SetBackgroundSprite(Sprite sprite)
	{
		_backgroundIcon.sprite = sprite;
	}

	public void StartNotifyAnimation()
	{
		if (_backgroundTweener != null)
		{
			_backgroundTweener.Complete();
		}
		_backgroundTweener = _backgroundIcon.rectTransform.DOScale(Vector3.one * 1.45f, 0.75f).SetEase(Ease.Flash, 6f).OnComplete(delegate
		{
			_backgroundTweener = null;
			_backgroundIcon.rectTransform.localScale = Vector3.one * 1.25f;
		});
		_backgroundBlinkIcon.gameObject.SetActive( true);
		_backgroundBlinkIcon.DOFade(1f, 0.75f).SetEase(Ease.Flash, 6f).OnComplete(delegate
		{
			_backgroundBlinkIcon.gameObject.SetActive( false);
			_backgroundBlinkIcon.color = new Color(1f, 1f, 1f, 0f);
		});
	}

	public void SelectWeapon()
	{
		if (_backgroundTweener != null)
		{
			_backgroundTweener.Complete();
		}
		_backgroundTweener = _backgroundIcon.rectTransform.DOScale(Vector3.one * 1.25f, 0.2f).OnComplete(delegate
		{
			_backgroundTweener = null;
		});
	}

	public void UnselectWeapon()
	{
		if (_backgroundTweener != null)
		{
			_backgroundTweener.Complete();
		}
		_backgroundTweener = _backgroundIcon.rectTransform.DOScale(Vector3.one, 0.2f).OnComplete(delegate
		{
			_backgroundTweener = null;
		});
	}

	private void UpdateRepairButton()
	{
		if (_weaponData == null || _repairButton == null)
		{
			return;
		}

		_repairButton.gameObject.SetActive(_weaponData.Broken);
		if (_weaponData.Broken)
		{
			var priceText = _repairButton.transform.Find("PriceText")?.GetComponent<Text>();
			if (priceText != null)
			{
				priceText.text = InlineSprites.GetLootInlineSprite("lootRuby") + _weaponData.GetRepairPriceAmount();
			}

			_repairButton.interactable = App.Instance.Player.LootManager.CanAfford("lootRuby", _weaponData.GetRepairPriceAmount());
			_repairButton.SetDisabledExplanation("You don't have enough coins");
		}
	}

	private void OnRepairButtonClicked()
	{
		if (App.Instance.Player.LootManager.TryExpense("lootRuby", _weaponData.GetRepairPriceAmount(), CurrencyReason.weaponRepair))
		{
			App.Instance.Player.WeaponManager.FullInstantRepair(_weaponData.Id);
		}
		else
		{
			UnityEngine.Debug.Log("You can't afford this. Let the user know");
		}
	}

	private void OnRepaired(string weaponId, int repairedAmount, bool skipFX)
	{
		if (_weaponData != null && _weaponData.Id == weaponId)
		{
			UpdateAtkLabel();
			UpdateRepairButton();
			_weaponIcon.sprite = _weaponPrefab.WeaponSpriteUI;
			UpdateHp();
		}
	}

	private void OnLootUpdated(string lootId, int lootAmount, int delta)
	{
		if (lootId == "lootRuby")
		{
			UpdateRepairButton();
		}
	}
}