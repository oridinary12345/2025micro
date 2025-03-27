using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager
{
	public const int AutoRegenHpAmount = 5;

	private const float AutoRegenTime = 600f;

	private readonly WeaponProfiles _weaponProfiles;

	private readonly WeaponConfigs _configs;

	private readonly Dictionary<string, WeaponData> _weaponDataCache = new Dictionary<string, WeaponData>();

	public WeaponEvents Events
	{
		get;
		private set;
	}

	public PlayerWeaponManager(WeaponProfiles weapons, WeaponConfigs configs)
	{
		_weaponProfiles = weapons;
		_configs = configs;
		Events = new WeaponEvents();
		if (_weaponProfiles.Weapons.Count == 0)
		{
			ForceUnlockWeapon(GetDefaultWeaponId());
		}
		foreach (WeaponConfig config in configs.GetConfigs())
		{
			GetWeapon(config.Id);
		}
		FullInstantRepairAllWeapons();
	}

	private void ForceUnlockWeapon(string weaponId)
	{
		WeaponProfile profile = GetProfile(weaponId);
		if (profile.CardCollectedCount > 0)
		{
			UnityEngine.Debug.LogWarning("Weapon was already unlocked... " + weaponId);
			return;
		}
		profile.CardCollectedCount = 1;
		profile.IsNew = false;
		Events.OnWeaponUnlocked(weaponId);
	}

	public void AddCards(string weaponId, int cardAmount)
	{
		WeaponProfile profile = GetProfile(weaponId);
		if (profile != null)
		{
			int cardCollectedCount = profile.CardCollectedCount;
			profile.CardCollectedCount += cardAmount;
			if (cardCollectedCount == 0)
			{
				Events.OnWeaponUnlocked(weaponId);
			}
			Events.OnWeaponCardCollected(weaponId);
		}
	}

	public void LevelUp(string weaponId)
	{
		WeaponProfile profile = GetProfile(weaponId);
		if (profile != null)
		{
			WeaponData weapon = GetWeapon(weaponId);
			if (weapon != null && !weapon.CardObjectiveReached)
			{
				UnityEngine.Debug.Log("LevelUp failed! UI shouldn't have let the player level up this");
				return;
			}
			profile.Level++;
			profile.HP = GetHPMax(weaponId, profile.Level);
			Events.OnLevelUp(weaponId);
		}
	}

	public void Heal(string weaponId, int repairAmount, bool skipFX = false)
	{
		WeaponProfile profile = GetProfile(weaponId);
		if (profile != null)
		{
			profile.HP = Mathf.Min(profile.HP + repairAmount, _configs.GetConfig(weaponId).GetHPMax(profile.Level));
			Events.OnRepaired(weaponId, repairAmount, skipFX);
		}
	}

	public void FullInstantRepair(string weaponId)
	{
		WeaponProfile profile = GetProfile(weaponId);
		if (profile != null)
		{
			Heal(weaponId, GetHPMax(weaponId, profile.Level) - profile.HP);
		}
	}

	public void FullInstantRepairAllWeapons()
	{
		foreach (KeyValuePair<string, WeaponData> item in _weaponDataCache)
		{
			item.Value.Profile.HP = _configs.GetConfig(item.Key).GetHPMax(item.Value.Profile.Level);
		}
	}

	public void RegisterGameEvents(GameEvents gameEvents)
	{
	}

	public void UnRegisterGameEvents(GameEvents gameEvents)
	{
	}

	public string GetDefaultWeaponId()
	{
		return _configs.GetDefaultConfig().Id;
	}

	public string GetRandomCardId(string worldId, WeaponRangeType range)
	{
		List<string> list = new List<string>();
		if (string.IsNullOrEmpty(worldId))
		{
			foreach (KeyValuePair<string, WeaponData> item in _weaponDataCache)
			{
				WeaponData value = item.Value;
				if (value.Unlocked && (range == WeaponRangeType.Invalid || value.Config.RangeType == range))
				{
					list.Add(value.Id);
				}
			}
			if (list.Count == 0)
			{
				list.Add(MonoSingleton<WeaponConfigs>.Instance.GetDefaultConfigForRange(range).Id);
			}
		}
		else
		{
			WorldData worldData = App.Instance.Player.LevelManager.GetWorldData(worldId);
			int endGameChestSpawned = worldData.Profile.EndGameChestSpawned;
			int num = 0;
			foreach (WeaponData worldWeapon in GetWorldWeapons(worldId))
			{
				if (!worldWeapon.Unlocked && num < endGameChestSpawned)
				{
					return worldWeapon.Config.Id;
				}
				num++;
			}
			list = GetWeaponIds(worldId, range);
		}
		return list[Random.Range(0, list.Count)];
	}

	public List<string> GetWeaponIds(string worldId, WeaponRangeType range)
	{
		List<string> list = new List<string>();
		WorldConfig config = MonoSingleton<WorldConfigs>.Instance.GetConfig(worldId);
		int num = (!config.IsValid()) ? int.MaxValue : config.Index;
		foreach (KeyValuePair<string, WeaponData> item in _weaponDataCache)
		{
			WeaponData value = item.Value;
			config = MonoSingleton<WorldConfigs>.Instance.GetConfig(value.Config.WorldId);
			if (config.IsValid() && config.Index <= num && (range == WeaponRangeType.Invalid || value.Config.RangeType == range))
			{
				list.Add(value.Id);
			}
		}
		return list;
	}

	public List<string> GetUnlockedWeaponIds()
	{
		List<string> list = new List<string>();
		foreach (WeaponData value in _weaponDataCache.Values)
		{
			if (value.Unlocked)
			{
				list.Add(value.Id);
			}
		}
		return list;
	}

	public List<WeaponData> GetWorldWeapons(string worldId)
	{
		List<WeaponData> list = new List<WeaponData>();
		foreach (KeyValuePair<string, WeaponData> item in _weaponDataCache)
		{
			WeaponData value = item.Value;
			if (value.Config.WorldId == worldId)
			{
				list.Add(value);
			}
		}
		return list;
	}

	public int GetUpgradableWeaponCount()
	{
		int num = 0;
		foreach (KeyValuePair<string, WeaponData> item in _weaponDataCache)
		{
			WeaponData value = item.Value;
			if (!value.HasReachMaxLevel && value.Unlocked && value.CardObjectiveReached && App.Instance.Player.LootManager.CanAfford("lootCoin", value.GetLevelUpPriceAmount()))
			{
				num++;
			}
		}
		return num;
	}

	public bool CanUpgradeOneCard()
	{
		foreach (KeyValuePair<string, WeaponData> item in _weaponDataCache)
		{
			WeaponData value = item.Value;
			if (!value.HasReachMaxLevel && value.Unlocked && value.CardObjectiveReached)
			{
				return true;
			}
		}
		return false;
	}

	public bool HasNewCards()
	{
		foreach (KeyValuePair<string, WeaponData> item in _weaponDataCache)
		{
			WeaponData value = item.Value;
			if (value.Unlocked && value.Profile.IsNew)
			{
				return true;
			}
		}
		return false;
	}

	public WeaponData GetWeapon(string weaponId)
	{
		WeaponProfile profile = GetProfile(weaponId);
		if (profile != null)
		{
 WeaponData value;			if (!_weaponDataCache.TryGetValue(weaponId, out value))
			{
				WeaponConfig config = _configs.GetConfig(profile.Id);
				if (config != null)
				{
					value = new WeaponData(config, profile);
					_weaponDataCache[weaponId] = value;
				}
				else
				{
					UnityEngine.Debug.LogWarning("Can't find weapon config for " + profile.Id);
				}
			}
			return value;
		}
		UnityEngine.Debug.LogWarning("GetWeapon() can't find a weapon profile for " + weaponId);
		return null;
	}

	public List<WeaponData> GetWeapons(List<string> ids)
	{
		List<WeaponData> list = new List<WeaponData>();
		foreach (string id in ids)
		{
			WeaponProfile profile = GetProfile(id);
			if (profile != null)
			{
				WeaponData weapon = GetWeapon(profile.Id);
				if (weapon != null)
				{
					list.Add(weapon);
				}
			}
			else
			{
				UnityEngine.Debug.LogWarningFormat("Can't get weapon [{0}] because player doesn't own it!", id);
			}
		}
		return list;
	}

	private int GetHPMax(string weaponId, int level)
	{
		return _configs.GetConfig(weaponId).GetHPMax(level);
	}

	public void SetHasSeen(string weaponId)
	{
		WeaponProfile weaponProfile = _weaponProfiles.Weapons.Find((WeaponProfile p) => p.Id == weaponId);
		if (weaponProfile != null)
		{
			weaponProfile.IsNew = false;
		}
		Events.OnNewWeaponSeen(weaponId);
	}

	private WeaponProfile GetProfile(string weaponId)
	{
		WeaponProfile weaponProfile = _weaponProfiles.Weapons.Find((WeaponProfile p) => p.Id == weaponId);
		if (weaponProfile == null)
		{
			WeaponConfig config = _configs.GetConfig(weaponId);
			weaponProfile = CreateNewProfile(config);
			_weaponProfiles.Weapons.Add(weaponProfile);
		}
		return weaponProfile;
	}

	private WeaponProfile CreateNewProfile(WeaponConfig config)
	{
		WeaponProfile weaponProfile = new WeaponProfile();
		weaponProfile.Id = config.Id;
		weaponProfile.Level = 1;
		weaponProfile.HP = config.HpMaxBase;
		weaponProfile.CardCollectedCount = 0;
		weaponProfile.IsNew = true;
		return weaponProfile;
	}

	public WeaponConfig GetWeaponConfig(string weaponId)
	{
		return _configs.GetConfig(weaponId);
	}
}