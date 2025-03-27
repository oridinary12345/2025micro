using UnityEngine;

public class MenuFXManager : MonoBehaviour
{
	private CommonFXManager _commonFXManager;

	private MenuEvents _menuEvents;

	public MenuFXManager Setup(CommonFXManager commonFxManager, CharacterEvents characterEvents, MenuEvents menuEvents)
	{
		_commonFXManager = commonFxManager;
		_menuEvents = menuEvents;
		RegisterEvents();
		return this;
	}

	private void RegisterEvents()
	{
		_menuEvents.WeaponEquippedEvent += OnWeaponEquipped;
		_menuEvents.WeaponUnlockTranslatedEvent += OnWeaponUnlockTranslated;
		_menuEvents.WeaponUnlockRevealedEvent += OnWeaponUnlockRevealed;
		_menuEvents.TrompetAppearedEvent += OnTrompetAppeared;
		_menuEvents.KnightAppearedEvent += OnKnightAppeared;
		_menuEvents.TokenTextEnteringEvent += OnTokenTextEntering;
		_menuEvents.TokenTextExitingEvent += OnTokenTextExiting;
		_menuEvents.KeyShownEvent += OnKeyShown;
		_menuEvents.ChestUnlockedEvent += OnChestUnlocked;
	}

	private void UnregisterEvents()
	{
		if (_menuEvents != null)
		{
			_menuEvents.WeaponEquippedEvent -= OnWeaponEquipped;
			_menuEvents.WeaponUnlockTranslatedEvent -= OnWeaponUnlockTranslated;
			_menuEvents.WeaponUnlockRevealedEvent -= OnWeaponUnlockRevealed;
			_menuEvents.TrompetAppearedEvent -= OnTrompetAppeared;
			_menuEvents.KnightAppearedEvent -= OnKnightAppeared;
			_menuEvents.TokenTextEnteringEvent -= OnTokenTextEntering;
			_menuEvents.TokenTextExitingEvent -= OnTokenTextExiting;
			_menuEvents.KeyShownEvent -= OnKeyShown;
			_menuEvents.ChestUnlockedEvent -= OnChestUnlocked;
		}
	}

	private void OnEnable()
	{
		if (_menuEvents != null)
		{
			UnregisterEvents();
			RegisterEvents();
		}
	}

	private void OnDisable()
	{
		UnregisterEvents();
	}

	private void OnWeaponEquipped()
	{
		PlaySoundFX("equipWeapon");
	}

	private void OnWeaponUnlockTranslated()
	{
		PlaySoundFX("whoosh02");
	}

	private void OnWeaponUnlockRevealed(Transform source)
	{
		PlaySoundFX("weaponReveal");
	}

	private void OnTrompetAppeared()
	{
		PlaySoundFX("ending_trumpet_short");
	}

	private void OnKnightAppeared()
	{
		PlaySoundFX("horses");
	}

	private void OnTokenTextEntering()
	{
		PlaySoundFX("heroAttack");
	}

	private void OnTokenTextExiting()
	{
		PlaySoundFX("whoosh_fast");
	}

	private void OnKeyShown()
	{
		PlaySoundFX("chain");
	}

	private void OnChestUnlocked()
	{
		PlaySoundFX("woohoo");
	}

	public void PlaySoundFX(string name, float pitch = 1f)
	{
		_commonFXManager.PlaySoundFX(name, pitch);
	}
}