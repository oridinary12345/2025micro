using Newtonsoft.Json;
using System;
using UnityEngine;
using Utils;

[Serializable]
public class PlayerProfile
{
	public HeroProfiles Heroes = new HeroProfiles();

	public KeyProfile Keys = new KeyProfile();

	public WeaponProfiles Weapons = new WeaponProfiles();

	public WorldProfiles Worlds = new WorldProfiles();

	public ChestProfiles Chests = new ChestProfiles();

	public LootLimitProfiles LootLimits = new LootLimitProfiles();

	public LootProfiles Loots = new LootProfiles();

	public SettingsProfile Settings = new SettingsProfile();

	public MuseumProfiles Museum = new MuseumProfiles();

	public MonsterMissionProfiles MonsterMissions = new MonsterMissionProfiles();

	[JsonProperty("annon")]
	public AnnouncementProfile Announcement = new AnnouncementProfile();

	public StatsProfile Stats = new StatsProfile();

	[JsonProperty("rew")]
	public RemoteRewardProfile RemoteRewards = new RemoteRewardProfile();

	private string _uid;

	private readonly string _playerPrefKey;

	public string UID => _uid;

	public PlayerProfile(string playerPrefKey)
	{
		_playerPrefKey = playerPrefKey;
		if (PlayerPrefs.HasKey(_playerPrefKey))
		{
			string @string = PlayerPrefs.GetString(_playerPrefKey);
			JsonConvert.PopulateObject(@string, this);
		}
		else
		{
			Init();
		}
		if (string.IsNullOrEmpty(_uid))
		{
			_uid = Utils.Math.GenerateGUID(8);
		}
	}

	private void Init()
	{
	}

	public void Save()
	{
		PlayerPrefs.SetString(_playerPrefKey, ToJson());
	}

	public string ToJson()
	{
		return JsonConvert.SerializeObject(this);
	}
}