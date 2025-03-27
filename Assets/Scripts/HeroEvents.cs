using System;
using System.Collections.Generic;

public class HeroEvents
{
	public event Action<List<WeaponData>> WeaponsUpdatedEvent;

	public event Action<HeroData> HeroSelectedEvent;

	public event Action<HeroData> HeroLevelUpEvent;

	public event Action<HeroData, int, bool> HeroHealedEvent;

	public event Action<HeroData> HeroCardCollectedEvent;

	public event Action NewHeroSeenEvent;

	public void OnWeaponsUpdated(List<WeaponData> weapons)
	{
		if (this.WeaponsUpdatedEvent != null)
		{
			this.WeaponsUpdatedEvent(weapons);
		}
	}

	public void OnHeroSelected(HeroData currentHero)
	{
		if (this.HeroSelectedEvent != null)
		{
			this.HeroSelectedEvent(currentHero);
		}
	}

	public void OnHeroLevelUp(HeroData heroData)
	{
		if (this.HeroLevelUpEvent != null)
		{
			this.HeroLevelUpEvent(heroData);
		}
	}

	public void OnHeroHealed(HeroData heroData, int healAmount, bool skipFX)
	{
		if (this.HeroHealedEvent != null)
		{
			this.HeroHealedEvent(heroData, healAmount, skipFX);
		}
	}

	public void OnHeroCardCollected(HeroData heroData)
	{
		if (this.HeroCardCollectedEvent != null)
		{
			this.HeroCardCollectedEvent(heroData);
		}
	}

	public void OnNewHeroSeen()
	{
		if (this.NewHeroSeenEvent != null)
		{
			this.NewHeroSeenEvent();
		}
	}
}