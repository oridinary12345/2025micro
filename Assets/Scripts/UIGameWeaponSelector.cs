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
		_hero = hero;
		_gameEvents = gameEvents;
		_gameState = gameState;
		UpdateWeapons(equippedWeapons);
	}

	public void UpdateWeapons(List<WeaponData> equippedWeapons)
	{
		int index = 0;
		int num = 0;
		foreach (UIGameWeapon weapon in _weapons)
		{
			if (num < equippedWeapons.Count)
			{
				WeaponData data = equippedWeapons[num];
				UIGameWeapon widget = weapon;
				if (_hero.GetCurrentWeapon().Id == data.Id)
				{
					index = num;
				}
				weapon.gameObject.SetActive( true);
				weapon.Init(_gameState, equippedWeapons[num]);
				weapon.gameObject.GetComponent<UIGameButton>().Animate = false;
				weapon.gameObject.GetComponent<UIGameButton>().ClearOnDownAction();
				weapon.gameObject.GetComponent<UIGameButton>().OnDown(delegate
				{
					OnWeaponClicked(widget, data);
				}, false);
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