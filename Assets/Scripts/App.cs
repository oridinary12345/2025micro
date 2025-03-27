using UnityEngine;

public class App : MonoBehaviour
{
	private static App _instance;

	public MenuType NextMenuType;

	public static App Instance
	{
		get
		{
			if (_instance == null)
			{
				Universe.Create();
			}
			return _instance;
		}
	}

	public Player Player
	{
		get;
		private set;
	}

	public RewardFactory RewardFactory
	{
		get;
		private set;
	}

	public AppEvents Events
	{
		get;
		private set;
	}

	public MenuEvents MenuEvents
	{
		get;
		private set;
	}

	public CommonFXManager FXManager
	{
		get;
		private set;
	}

	public Analytics Analytics
	{
		get;
		private set;
	}

	public GameConfigs Configs
	{
		get;
		private set;
	}

	public static bool IsCreated()
	{
		return _instance;
	}

	public static App Create()
	{
		return new GameObject("App").AddComponent<App>();
	}

	private void Awake()
	{
		Screen.fullScreen = true;
		_instance = this;
		Events = new AppEvents();
		MenuEvents = new MenuEvents();
		MonoSingleton<InAppManager>.Instance.Create();
		Analytics = base.gameObject.AddComponent<Analytics>();
		new GameObject("Configs").transform.parent = base.transform;
		Configs = new GameConfigs();
		base.gameObject.AddComponent<AudioListener>();
		MonoSingleton<GameMusicManager>.Instance.Create();
		MonoSingleton<GameMusicManager>.Instance.transform.parent = base.transform;
		MonoSingleton<TimeManager>.Instance.Create();
		MonoSingleton<TimeManager>.Instance.transform.parent = base.transform;
		MonoSingleton<GameAdsController>.Instance.Create();
		MonoSingleton<GameAdsController>.Instance.transform.parent = base.transform;
		MonoSingleton<Cloud>.Instance.Create();
		MonoSingleton<Cloud>.Instance.transform.parent = base.transform;
		RewardFactory = base.gameObject.AddComponent<RewardFactory>().Init(MonoSingleton<RewardConfigs>.Instance);
		InitPlayer();
		FXManager = base.gameObject.AddComponent<CommonFXManager>().Setup(Player);
	}

	private void Start()
	{
		Analytics.Setup(Player.LootManager.Events, Player.LevelManager.Events);
	}

	private void InitPlayer()
	{
		Player = base.gameObject.AddComponent<Player>().Init();
		Application.targetFrameRate = ((!Player.SettingsManager.BatterySavingEnabled) ? 60 : 30);
	}

	public void LoadMainMenu()
	{
		Player.HeroManager.InstantHealAllHeroes();
		Player.WeaponManager.FullInstantRepairAllWeapons();
		MonoSingleton<SceneSwitcher>.Instance.LoadSceneNoFade("Game");
		MonoSingleton<GameMusicManager>.Instance.PlayMusicMenu();
	}

	public void LoadMainMenuFromGame(MenuType menuType = MenuType.Default)
	{
		NextMenuType = menuType;
		Player.HeroManager.InstantHealAllHeroes();
		Player.WeaponManager.FullInstantRepairAllWeapons();
		MonoSingleton<SceneSwitcher>.Instance.LoadScene("Game");
		MonoSingleton<GameMusicManager>.Instance.PlayMusicMenu();
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			Events.OnAppPaused();
			Instance.Player.Save();
		}
		else
		{
			Events.OnAppResumed();
		}
	}

	private void Update()
	{
		if (!Input.GetKeyDown(KeyCode.C))
		{
		}
	}
}