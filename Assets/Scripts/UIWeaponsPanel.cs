using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIWeaponsPanel : UIMenuPage
{
	[SerializeField]
	private UIGameButton _closeButton;

	[SerializeField]
	private RectTransform _viewportContentRangeShort;

	[SerializeField]
	private RectTransform _viewportContentRangeMedium;

	[SerializeField]
	private RectTransform _viewportContentRangeLong;

	[SerializeField]
	private RectTransform _weaponRoot;

	[SerializeField]
	private UIWeaponsDetailsPopup _detailsPopup;

	[SerializeField]
	private UILockedWeaponsDetailsPopup _lockedDetailsPopup;

	[SerializeField]
	private UIWeaponUnlockedPanel _unlockedPanel;

	[SerializeField]
	private List<UIWeaponsPanelBox> _selectedWeapons;

	private List<Dictionary<string, UIWeaponsPanelBox>> _weaponBoxes = new List<Dictionary<string, UIWeaponsPanelBox>>();

	protected override void Awake()
	{
		base.Awake();
		_closeButton.OnClick(OnCloseButtonClicked);
		_closeButton.ActivateOnBackKey();
		AddWeapons(WeaponRangeType.Short, _viewportContentRangeShort, 0);
		AddWeapons(WeaponRangeType.Medium, _viewportContentRangeMedium, 1);
		AddWeapons(WeaponRangeType.Long, _viewportContentRangeLong, 2);
		foreach (UIWeaponsPanelBox selectedWeapon in _selectedWeapons)
		{
			if (selectedWeapon.WeaponData == null)
			{
				selectedWeapon.SetEmpty();
			}
		}
	}

	private void OnEnable()
	{
		foreach (UIWeaponsPanelBox selectedWeapon in _selectedWeapons)
		{
			if (selectedWeapon.WeaponData != null)
			{
				selectedWeapon.Refresh();
			}
		}
		foreach (Dictionary<string, UIWeaponsPanelBox> weaponBox in _weaponBoxes)
		{
			foreach (KeyValuePair<string, UIWeaponsPanelBox> item in weaponBox)
			{
				item.Value.Refresh();
			}
		}
	}

	private void AddWeapons(WeaponRangeType rangeType, RectTransform viewport, int rangeTypeIndex)
	{
		if (_weaponBoxes.Count <= rangeTypeIndex)
		{
			_weaponBoxes.Add(new Dictionary<string, UIWeaponsPanelBox>());
		}
		List<WeaponConfig> configs = MonoSingleton<WeaponConfigs>.Instance.GetConfigs(rangeType);
		List<string> equippedWeaponIds = App.Instance.Player.HeroManager.EquippedWeaponIds;
		int num = 0;
		foreach (WeaponConfig weaponConfig in configs)
		{
			bool flag = equippedWeaponIds.FindIndex((string id) => id == weaponConfig.Id) >= 0;
			UIWeaponsPanelBox weaponBox = CreateWeaponBox(weaponConfig, num);
			weaponBox.GetComponent<RectTransform>().SetParent(viewport, false);
			UIGameButton component = weaponBox.GetComponent<UIGameButton>();
			component.OnClick(delegate
			{
				OnWeaponClicked(weaponBox, weaponConfig, rangeTypeIndex);
			});
			component.interactable = weaponBox.WeaponData.Unlocked;
			component.SetDisabledExplanation("You haven't found this card yet");
			_weaponBoxes[rangeTypeIndex][weaponConfig.Id] = weaponBox;
			if (flag && rangeTypeIndex < _selectedWeapons.Count)
			{
				Equip(weaponBox, weaponConfig, rangeTypeIndex);
				_selectedWeapons[rangeTypeIndex].Select();
				num++;
				_selectedWeapons[rangeTypeIndex].GetComponent<UIGameButton>().OnClick(delegate
				{
					WeaponConfig weaponConfig2 = _selectedWeapons[rangeTypeIndex].WeaponConfig;
					WeaponData weaponData = _selectedWeapons[rangeTypeIndex].WeaponData;
					_selectedWeapons[rangeTypeIndex].HideBadgeNew();
					_detailsPopup.Show();
					_detailsPopup.Init(weaponConfig2, weaponData, true, null);
				});
			}
		}
	}

	private void OnWeaponClicked(UIWeaponsPanelBox weaponBox, WeaponConfig weaponConfig, int rangeTypeIndex)
	{
		if (weaponBox.WeaponData.Unlocked)
		{
			Action onEquipConfirmed = delegate
			{
				this.Execute(0.1f, delegate
				{
					App.Instance.MenuEvents.OnWeaponEquipped();
					UIWeaponsPanelBox weaponBoxCopy = UnityEngine.Object.Instantiate(weaponBox);
					weaponBoxCopy.GetComponent<RectTransform>().sizeDelta = weaponBox.GetComponent<RectTransform>().sizeDelta;
					weaponBoxCopy.GetComponent<RectTransform>().SetParent(_weaponRoot.transform, false);
					weaponBoxCopy.transform.position = weaponBox.transform.position;
					CanvasGroup component = weaponBoxCopy.gameObject.GetComponent<CanvasGroup>();
					weaponBox.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
					this.Execute(0.15f, delegate
					{
						Equip(weaponBox, weaponConfig, rangeTypeIndex);
						_selectedWeapons[rangeTypeIndex].transform.DOPunchScale(Vector3.one * 0.25f, 0.25f);
					});
					Transform transform = weaponBoxCopy.transform;
					Vector3 position = _selectedWeapons[rangeTypeIndex].transform.position;
					transform.DOMoveY(position.y, 0.4f).OnComplete(delegate
					{
						UnityEngine.Object.Destroy(weaponBoxCopy.gameObject);
					});
					component.DOFade(0f, 0.2f).SetDelay(0.15f);
				});
			};
			weaponBox.HideBadgeNew();
			_detailsPopup.Show();
			_detailsPopup.Init(weaponConfig, weaponBox.WeaponData, false, onEquipConfirmed);
		}
	}

	private void Equip(UIWeaponsPanelBox weaponBox, WeaponConfig weaponConfig, int rangeTypeIndex)
	{
		if (_selectedWeapons[rangeTypeIndex].WeaponConfig != null)
		{
			string id = _selectedWeapons[rangeTypeIndex].WeaponConfig.Id;
			if (_weaponBoxes[rangeTypeIndex].ContainsKey(id))
			{
				_weaponBoxes[rangeTypeIndex][id].Refresh();
				_weaponBoxes[rangeTypeIndex][id].gameObject.SetActive( true);
				_weaponBoxes[rangeTypeIndex][id].gameObject.GetComponent<CanvasGroup>().alpha = 1f;
			}
		}
		_selectedWeapons[rangeTypeIndex].Equip(weaponConfig, weaponBox.WeaponData, weaponBox.Index);
		weaponBox.gameObject.SetActive( false);
	}

	private UIWeaponsPanelBox CreateWeaponBox(WeaponConfig weaponConfig, int index)
	{
		UIWeaponsPanelBox uIWeaponsPanelBox = UnityEngine.Object.Instantiate(Resources.Load<UIWeaponsPanelBox>("UI/WeaponBox"));
		return uIWeaponsPanelBox.Init(weaponConfig, App.Instance.Player.WeaponManager.GetWeapon(weaponConfig.Id), index);
	}

	private void OnCloseButtonClicked()
	{
		if (MonoSingleton<UIMenuStack>.Instance.Peek() == this)
		{
			List<string> list = new List<string>();
			foreach (UIWeaponsPanelBox selectedWeapon in _selectedWeapons)
			{
				if (selectedWeapon.WeaponConfig != null)
				{
					list.Add(selectedWeapon.WeaponConfig.Id);
				}
			}
			App.Instance.Player.HeroManager.SetEquippedWeapons(list);
			Hide();
		}
	}
}