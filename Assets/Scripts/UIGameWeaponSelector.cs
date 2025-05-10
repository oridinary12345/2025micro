using System.Collections.Generic;
using UnityEngine;

public class UIGameWeaponSelector : MonoBehaviour
{
	[SerializeField]
	private List<UIGameWeapon> _weapons;

	[SerializeField]
	private Sprite _selectedSprite;

	[SerializeField]
	private Sprite _unselectedSprite;

	private Character _hero;

	private GameEvents _gameEvents;

	private GameState _gameState;

	private UIGameWeapon _selectedWeapon;

	private WeaponData _lastWeaponClicked;

	private AudioClip _clickedSound;

	public void Init(Character hero, GameState gameState, GameEvents gameEvents, List<WeaponData> equippedWeapons)
	{
		if (hero == null || gameState == null || gameEvents == null)
		{
			Debug.LogWarning("Required parameters are null in UIGameWeaponSelector.Init");
			return;
		}

		_hero = hero;
		_gameEvents = gameEvents;
		_gameState = gameState;

		if (equippedWeapons == null)
		{
			Debug.LogWarning("equippedWeapons is null in UIGameWeaponSelector.Init");
			return;
		}

		UpdateWeapons(equippedWeapons);
	}

	public void UpdateWeapons(List<WeaponData> equippedWeapons)
	{
		if (_weapons == null || equippedWeapons == null)
		{
			Debug.LogWarning("_weapons or equippedWeapons is null in UIGameWeaponSelector.UpdateWeapons");
			return;
		}

		int index = 0;
		int num = 0;
		foreach (UIGameWeapon weapon in _weapons)
		{
			if (weapon == null)
			{
				Debug.LogWarning("Weapon is null in _weapons list");
				continue;
			}

			if (num < equippedWeapons.Count)
			{
				WeaponData data = equippedWeapons[num];
				if (data == null)
				{
					Debug.LogWarning($"WeaponData at index {num} is null");
					num++;
					continue;
				}

				if (_hero != null && _hero.GetCurrentWeapon() != null && _hero.GetCurrentWeapon().Id == data.Id)
				{
					index = num;
				}

				if (weapon.gameObject != null)
				{
					weapon.gameObject.SetActive(true);
					weapon.Init(_gameState, data);

					var button = weapon.gameObject.GetComponent<UIGameButton>();
					if (button != null)
					{
						button.Animate = false;
						button.ClearOnDownAction();
						UIGameWeapon widget = weapon;
						button.OnDown(delegate
						{
							OnWeaponClicked(widget, data);
						}, false);
					}
				}
			}
			else
			{
				weapon.SetEmpty();
			}
			num++;
		}
		SelectWeapon(_weapons[index], equippedWeapons[index]);
		_lastWeaponClicked = equippedWeapons[index];
	}

	public void EquipCurrentWeapon()
	{
		if (_lastWeaponClicked != null)
		{
			SelectWeapon(_selectedWeapon, _lastWeaponClicked);
		}
	}

	public void OnRoundEnded()
	{
		_lastWeaponClicked = null;
	}

	public void OnScreenTapWhileWeaponRotating()
	{
		_selectedWeapon.StartNotifyAnimation();
	}

	private void OnWeaponClicked(UIGameWeapon weaponWidget, WeaponData selectedWeapon)
	{
		if (_lastWeaponClicked != null)
		{
			if (_clickedSound == null)
			{
				_clickedSound = Resources.Load<AudioClip>("buttonClicked");
			}
			MonoSingleton<AudioManager>.Instance.PlaySound(_clickedSound);
		}
		SelectWeapon(weaponWidget, selectedWeapon);
		if (_lastWeaponClicked != null && _lastWeaponClicked.Id == selectedWeapon.Id)
		{
			_gameEvents.OnGameStateMessage(new WeaponConfirmed());
		}
		_lastWeaponClicked = selectedWeapon;
	}

	private void SelectWeapon(UIGameWeapon weaponWidget, WeaponData selectedWeapon)
	{
		_selectedWeapon = weaponWidget;
		UpdateWeaponUI(weaponWidget);
		_gameEvents.OnGameStateMessage(new WeaponSelected
		{
			IsNewWeapon = (selectedWeapon != null && _hero.GetCurrentWeapon().Id != selectedWeapon.Id)
		});
		_hero.OnWeaponChanged(selectedWeapon);
	}

	private void UpdateWeaponUI(UIGameWeapon weaponWidget)
	{
		foreach (UIGameWeapon weapon in _weapons)
		{
			weapon.SetBackgroundSprite(_unselectedSprite);
			if (weaponWidget != weapon)
			{
				weapon.UnselectWeapon();
			}
		}
		weaponWidget.SetBackgroundSprite(_selectedSprite);
		weaponWidget.SelectWeapon();
	}
}