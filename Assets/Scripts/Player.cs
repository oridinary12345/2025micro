using UnityEngine;

public class Player : MonoBehaviour
{
	private PlayerProfile _profile;

	public PlayerWeaponManager WeaponManager
	{
		get;
		private set;
	}

	public PlayerHeroManager HeroManager
	{
		get;
		private set;
	}

	public PlayerLevelManager LevelManager
	{
		get;
		private set;
	}

	public PlayerLootManager LootManager
	{
		get;
		private set;
	}

	public PlayerSettingsManager SettingsManager
	{
		get;
		private set;
	}

	public PlayerStatsManager StatsManager
	{
		get;
		private set;
	}

	public PlayerRemoteRewardManager RemoteRewardManager
	{
		get;
		private set;
	}

	public PlayerChestManager ChestManager
	{
		get;
		private set;
	}

	public PlayerMuseumManager MuseumManager
	{
		get;
		private set;
	}

	public LootKeyManager LootKeyManager
	{
		get;
		private set;
	}

	public PlayerItemInventory ItemInventory
	{
		get;
		private set;
	}

	public MonsterMissionManager MonsterMissions
	{
		get;
		private set;
	}

	public LootLimitsManager LootLimitsManager
	{
		get;
		private set;
	}

	public PlayerRewardManager RewardManager
	{
		get;
		private set;
	}

	public string UID => _profile.UID;

	private void Awake()
	{
		_profile = new PlayerProfile("profile-v12-00");
	}

	public Player Init()
	{
		WeaponManager = new PlayerWeaponManager(_profile.Weapons, MonoSingleton<WeaponConfigs>.Instance);
		HeroManager = new PlayerHeroManager(_profile.Heroes, MonoSingleton<HeroConfigs>.Instance, WeaponManager);
		LevelManager = new PlayerLevelManager(_profile.Worlds, MonoSingleton<AdventureLevelConfigs>.Instance, MonoSingleton<WorldConfigs>.Instance);
		LootManager = new PlayerLootManager(_profile.Loots, MonoSingleton<LootConfigs>.Instance);
		SettingsManager = new PlayerSettingsManager(_profile.Settings);
		StatsManager = new PlayerStatsManager(_profile.Stats);
		ChestManager = new PlayerChestManager(_profile.Chests, MonoSingleton<TimeManager>.Instance);
		LootLimitsManager = new LootLimitsManager(_profile.LootLimits);
		MuseumManager = new PlayerMuseumManager(_profile.Museum, MonoSingleton<MuseumConfigs>.Instance, MonoSingleton<TimeManager>.Instance);
		LootKeyManager = new LootKeyManager(_profile.Keys, LootManager, MonoSingleton<TimeManager>.Instance);
		ItemInventory = new PlayerItemInventory(LootManager);
		MonsterMissions = new MonsterMissionManager(_profile.MonsterMissions, MonoSingleton<TimeManager>.Instance, WeaponManager, StatsManager);
		RewardManager = new PlayerRewardManager(this);
		CloudUser currentUser = MonoSingleton<Cloud>.Instance.CurrentUser;
		string uid = (currentUser == null) ? UID : currentUser.PlayfabId;
		RemoteRewardManager = new PlayerRemoteRewardManager(uid, _profile.RemoteRewards, RewardManager);
		return this;
	}

	public bool IsBetaWarningSeen()
	{
		return _profile.Announcement.BetaWarningSeen;
	}

	public void SetBetaWarningSeen()
	{
		_profile.Announcement.BetaWarningSeen = true;
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			StatsManager.OnAppPause();
			MuseumManager.OnAppPause();
			LootKeyManager.OnAppPause();
		}
		else
		{
			StatsManager.OnAppResume();
			MuseumManager.OnAppResume();
			LootKeyManager.OnAppResume();
		}
	}

	private void OnApplicationQuit()
	{
		if (MuseumManager != null)
		{
			MuseumManager.OnAppQuit();
		}
		if (LootKeyManager != null)
		{
			LootKeyManager.OnAppQuit();
		}
		_profile.Save();
	}

	public void Save()
	{
		_profile.Save();
		PlayerPrefs.Save();
	}

	public string ToJson()
	{
		return _profile.ToJson();
	}
}