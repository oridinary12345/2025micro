using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeroManager
{
	public HeroEvents Events;

	private readonly HeroProfiles _heroes;

	private readonly HeroConfigs _heroConfigs;

	private readonly PlayerWeaponManager _weaponManager;

	public List<string> EquippedWeaponIds => _heroes.EquippedWeaponIds;

	public PlayerHeroManager(HeroProfiles heroes, HeroConfigs heroConfigs, PlayerWeaponManager weaponManager)
	{
		_heroes = heroes;
		_heroConfigs = heroConfigs;
		_weaponManager = weaponManager;
		Events = new HeroEvents();
		if (_heroes.Heroes.Count == 0)
		{
			HeroConfig defaultConfig = _heroConfigs.GetDefaultConfig();
			HeroProfile newProfile = GetNewProfile(defaultConfig);
			newProfile.IsNew = false;
			newProfile.CardCollectedCount = 1;
			_heroes.Heroes.Add(newProfile);
		}
		if (_heroes.EquippedWeaponIds.Count == 0)
		{
			_heroes.EquippedWeaponIds.Add(_weaponManager.GetDefaultWeaponId());
			_heroes.FreeHealAvailable = 5;
		}
		if (string.IsNullOrEmpty(_heroes.CurrentHeroId))
		{
			_heroes.CurrentHeroId = _heroConfigs.GetDefaultConfig().Id;
		}
		_weaponManager.Events.WeaponUnlockedEvent += OnWeaponUnlocked;
		InstantHealAllHeroes();
	}

	private void OnWeaponUnlocked(string weaponId)
	{
		if (EquippedWeaponIds.Count < 3)
		{
			EquippedWeaponIds.Add(weaponId);
		}
	}

	public int GetLevelUpPriceAmount(string heroId)
	{
		return GetHeroData(heroId).GetLevelUpPriceAmount();
	}

	public bool IsNextHealFree()
	{
		return _heroes.FreeHealAvailable > 0;
	}

	private int GetHPMax(HeroConfig config, int level)
	{
		return config.GetHPMax(level);
	}

	public HeroData GetCurrentHeroData()
	{
		return GetHeroData(_heroes.CurrentHeroId);
	}

	public string GetCurrentHeroId()
	{
		return _heroes.CurrentHeroId;
	}

	public int GetUpgradableHeroCount()
	{
		int num = 0;
		foreach (HeroProfile hero in _heroes.Heroes)
		{
			HeroData heroData = GetHeroData(hero.Id);
			if (!heroData.HasReachMaxLevel && heroData.Unlocked && heroData.CardObjectiveReached && App.Instance.Player.LootManager.CanAfford("lootCoin", heroData.GetLevelUpPriceAmount()))
			{
				num++;
			}
		}
		return num;
	}

	public bool CanUpgradeOneCard()
	{
		foreach (HeroProfile hero in _heroes.Heroes)
		{
			HeroData heroData = GetHeroData(hero.Id);
			if (!heroData.HasReachMaxLevel && heroData.Unlocked && heroData.CardObjectiveReached)
			{
				return true;
			}
		}
		return false;
	}

	public bool HasNewCards()
	{
		foreach (HeroProfile hero in _heroes.Heroes)
		{
			HeroData heroData = GetHeroData(hero.Id);
			if (heroData.Unlocked && hero.IsNew)
			{
				return true;
			}
		}
		return false;
	}

	public void SetHasSeen(string heroId)
	{
		HeroProfile profile = GetProfile(heroId);
		if (profile != null)
		{
			profile.IsNew = false;
		}
		Events.OnNewHeroSeen();
	}

	public void SetCurrentHero(string heroId)
	{
		if (_heroes.CurrentHeroId != heroId)
		{
			_heroes.CurrentHeroId = heroId;
			Events.OnHeroSelected(GetCurrentHeroData());
		}
	}

	public void AddCards(string heroId, int cardAmount)
	{
		HeroProfile profile = GetProfile(heroId);
		if (profile != null)
		{
			int cardCollectedCount = profile.CardCollectedCount;
			profile.CardCollectedCount += cardAmount;
			if (cardCollectedCount == 0)
			{
			}
			Events.OnHeroCardCollected(GetCurrentHeroData());
		}
	}

	public void LevelUp(string heroId)
	{
		HeroProfile profile = GetProfile(heroId);
		if (profile != null)
		{
			HeroData heroData = GetHeroData(heroId);
			if (!heroData.CardObjectiveReached)
			{
				UnityEngine.Debug.Log("LevelUp failed! UI shouldn't have let the player level up this");
				return;
			}
			profile.Level++;
			profile.HP = GetHPMax(MonoSingleton<HeroConfigs>.Instance.GetConfig(heroId), profile.Level);
			Events.OnHeroLevelUp(GetHeroData(heroId));
		}
	}

	public string GetRandomCardId(string worldId)
	{
		return MonoSingleton<HeroConfigs>.Instance.GetDefaultConfig().Id;
	}

	public void InstantHeal(string heroId)
	{
		HeroProfile profile = GetProfile(heroId);
		if (profile != null)
		{
			if (_heroes.FreeHealAvailable > 0)
			{
				_heroes.FreeHealAvailable--;
			}
			HeroData heroData = GetHeroData(heroId);
			heroData.Profile.HP = heroData.GetMaxHP();
		}
	}

	public void InstantHealAllHeroes()
	{
		foreach (HeroProfile hero in _heroes.Heroes)
		{
			InstantHeal(hero.Id);
		}
	}

	public void Heal(int healAmount)
	{
		Heal(GetCurrentHeroData(), healAmount);
	}

	public void Heal(HeroData hero, int healAmount, bool skipFX = false)
	{
		int a = hero.GetMaxHP() - hero.Profile.HP;
		hero.Profile.HP = Math.Min(hero.Profile.HP + healAmount, hero.GetMaxHP());
		Events.OnHeroHealed(hero, Mathf.Min(a, healAmount), skipFX);
	}

	public void SetEquippedWeapons(List<string> equippedWeapons)
	{
		List<string> equippedWeaponIds = _heroes.EquippedWeaponIds;
		equippedWeaponIds.Clear();
		equippedWeaponIds.AddRange(equippedWeapons);
		Events.OnWeaponsUpdated(_weaponManager.GetWeapons(equippedWeapons));
	}

	public HeroData GetHeroData(string heroId)
	{
		HeroProfile heroProfile = GetProfile(heroId);
		if (heroProfile == null)
		{
			HeroConfig config = _heroConfigs.GetConfig(heroId);
			heroProfile = GetNewProfile(config);
			_heroes.Heroes.Add(heroProfile);
		}
		List<WeaponData> weapons = _weaponManager.GetWeapons(_heroes.EquippedWeaponIds);
		return new HeroData(_heroConfigs.GetConfig(heroProfile.Id), heroProfile, weapons, Events);
	}

	private HeroProfile GetProfile(string id)
	{
		return _heroes.Heroes.Find((HeroProfile p) => p.Id == id);
	}

	private HeroProfile GetNewProfile(HeroConfig config)
	{
		HeroProfile heroProfile = new HeroProfile();
		heroProfile.Id = config.Id;
		heroProfile.Level = 1;
		heroProfile.HP = config.HpMaxBase;
		heroProfile.CardCollectedCount = 0;
		heroProfile.IsNew = true;
		return heroProfile;
	}
}