using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILockedWeaponsDetailsPopup : UIMenuPopup
{
	[SerializeField]
	private TextMeshProUGUI _missionText;

	[SerializeField]
	private TextMeshProUGUI _missionProgressText;

	[SerializeField]
	private Image _weaponImage;

	[SerializeField]
	private Slider _missionSlider;

	[SerializeField]
	private UIGameButton _buttonClose;

	public WeaponConfig WeaponConfig
	{
		get;
		private set;
	}

	protected override void Awake()
	{
		base.Awake();
		_buttonClose.OnClick(OnCloseButtonClicked);
		_buttonClose.ActivateOnBackKey();
	}

	public UILockedWeaponsDetailsPopup Init(WeaponConfig weaponConfig, WeaponData weaponData)
	{
		ApplyWeapon(weaponConfig, weaponData);
		return this;
	}

	private void ApplyWeapon(WeaponConfig weaponConfig, WeaponData weaponData)
	{
		WeaponConfig = weaponConfig;
		_weaponImage.sprite = Resources.Load<Sprite>("Weapons/" + weaponConfig.Id + "/UI_w_" + weaponConfig.Id);
	}

	private void OnCloseButtonClicked()
	{
		Hide();
	}
}