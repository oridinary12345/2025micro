public class GameConfigs
{
	public HeroConfigs Heroes;

	public WeaponConfigs Weapons;

	public MonsterConfigs Monsters;

	public LootConfigs Loots;

	public RewardConfigs Rewards;

	public AppConfigs App;

	public MissionConfigs Missions;

	public int Version
	{
		get;
		private set;
	}

	public GameConfigs()
	{
		Heroes = MonoSingleton<HeroConfigs>.Instance;
		Weapons = MonoSingleton<WeaponConfigs>.Instance;
		Monsters = MonoSingleton<MonsterConfigs>.Instance;
		Loots = MonoSingleton<LootConfigs>.Instance;
		Rewards = MonoSingleton<RewardConfigs>.Instance;
		App = MonoSingleton<AppConfigs>.Instance;
		Missions = MonoSingleton<MissionConfigs>.Instance;
	}

	public void Override(ConfigsData data)
	{
		if (data != null)
		{
			Version = data.Version;
			Heroes.LoadFromText(data.HeroConfigs);
			Weapons.LoadFromText(data.WeaponConfigs);
			Monsters.LoadFromText(data.MonsterConfigs);
			Loots.LoadFromText(data.LootConfigs);
			Rewards.LoadFromText(data.RewardConfigs);
			App.LoadFromText(data.AppConfigs);
			Missions.LoadFromText(data.MissionConfigs);
		}
	}
}