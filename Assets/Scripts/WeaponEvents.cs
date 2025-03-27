using System;

public class WeaponEvents
{
	public event Action<string, int, bool> RepairedEvent;

	public event Action<string> LevelUpEvent;

	public event Action<string> WeaponUnlockedEvent;

	public event Action<string> BrokenEvent;

	public event Action<string> WeaponCardCollectedEvent;

	public event Action<string> NewWeaponSeenEvent;

	public void OnRepaired(string weaponId, int healAmount, bool skipFX)
	{
		if (this.RepairedEvent != null)
		{
			this.RepairedEvent(weaponId, healAmount, skipFX);
		}
	}

	public void OnLevelUp(string weaponId)
	{
		if (this.LevelUpEvent != null)
		{
			this.LevelUpEvent(weaponId);
		}
	}

	public void OnWeaponUnlocked(string weaponId)
	{
		if (this.WeaponUnlockedEvent != null)
		{
			this.WeaponUnlockedEvent(weaponId);
		}
	}

	public void OnBroken(string weaponId)
	{
		if (this.BrokenEvent != null)
		{
			this.BrokenEvent(weaponId);
		}
	}

	public void OnWeaponCardCollected(string weaponId)
	{
		if (this.WeaponCardCollectedEvent != null)
		{
			this.WeaponCardCollectedEvent(weaponId);
		}
	}

	public void OnNewWeaponSeen(string weaponId)
	{
		if (this.NewWeaponSeenEvent != null)
		{
			this.NewWeaponSeenEvent(weaponId);
		}
	}
}