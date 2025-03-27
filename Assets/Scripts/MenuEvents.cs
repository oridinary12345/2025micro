using System;
using UnityEngine;

public class MenuEvents
{
	public event Action WeaponEquippedEvent;

	public event Action WeaponUnlockTranslatedEvent;

	public event Action<Transform> WeaponUnlockRevealedEvent;

	public event Action LevelUnlockedStartedEvent;

	public event Action LevelUnlockedEndedEvent;

	public event Action<string, Vector3> TextOverlayRequestedEvent;

	public event Action TrompetAppearedEvent;

	public event Action KnightAppearedEvent;

	public event Action TokenTextEnteringEvent;

	public event Action TokenTextExitingEvent;

	public event Action WorldSelectorReadyEvent;

	public event Action KeyShownEvent;

	public event Action ChestUnlockedEvent;

	public void OnWeaponEquipped()
	{
		if (this.WeaponEquippedEvent != null)
		{
			this.WeaponEquippedEvent();
		}
	}

	public void OnLevelUnlockedStarted()
	{
		if (this.LevelUnlockedStartedEvent != null)
		{
			this.LevelUnlockedStartedEvent();
		}
	}

	public void OnLevelUnlockedEnded()
	{
		if (this.LevelUnlockedEndedEvent != null)
		{
			this.LevelUnlockedEndedEvent();
		}
	}

	public void OnWeaponUnlockTranslated()
	{
		if (this.WeaponUnlockTranslatedEvent != null)
		{
			this.WeaponUnlockTranslatedEvent();
		}
	}

	public void OnWeaponUnlockRevealed(Transform source)
	{
		if (this.WeaponUnlockRevealedEvent != null)
		{
			this.WeaponUnlockRevealedEvent(source);
		}
	}

	public void OnTextOverlayRequested(string text, Vector3 inputPosition)
	{
		if (this.TextOverlayRequestedEvent != null)
		{
			this.TextOverlayRequestedEvent(text, inputPosition);
		}
	}

	public void OnTrompetAppeared()
	{
		if (this.TrompetAppearedEvent != null)
		{
			this.TrompetAppearedEvent();
		}
	}

	public void OnKnightAppeared()
	{
		if (this.KnightAppearedEvent != null)
		{
			this.KnightAppearedEvent();
		}
	}

	public void OnTokenTextEntering()
	{
		if (this.TokenTextEnteringEvent != null)
		{
			this.TokenTextEnteringEvent();
		}
	}

	public void OnTokenTextExiting()
	{
		if (this.TokenTextExitingEvent != null)
		{
			this.TokenTextExitingEvent();
		}
	}

	public void OnWorldSelectorReady()
	{
		if (this.WorldSelectorReadyEvent != null)
		{
			this.WorldSelectorReadyEvent();
		}
	}

	public void OnKeyShown()
	{
		if (this.KeyShownEvent != null)
		{
			this.KeyShownEvent();
		}
	}

	public void OnChestUnlocked()
	{
		if (this.ChestUnlockedEvent != null)
		{
			this.ChestUnlockedEvent();
		}
	}
}