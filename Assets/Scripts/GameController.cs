using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	[SerializeField]
	private Camera _camera;

	[SerializeField]
	private GameObject _gamePanel;

	[SerializeField]
	private UIGameHUD _hud;

	[SerializeField]
	private SpriteRenderer _mapBackground;

	[SerializeField]
	private UIWeaponUnlockedPanel _unlockedPanel;

	[SerializeField]
	private WorldRenderer _worldRenderer;

	[SerializeField]
	private UIMenuGameOverContinue _gameOverContinueMenu;

	[SerializeField]
	private UIEndGamePanel _endGamePanel;

	public GameStateMachine FSM;

	private const int InstructionDisplayCountMax = 5;

	private CharacterEvents _characterEvents;

	private WeaponShape _weaponShape;

	private MonsterManager _monsterManager;

	private GameStats _stats;

	private GridCircle _grid;

	private LevelData _level;

	private GameState _gameState;

	private LootDropManager _lootDropManager;

	private IGameAnimationFlow _endGameFlow;

	private IGameAnimationFlow _gameIntroFlow;

	private Hero _hero;

	private int _comboCountCurrent;

	private int _comboCountTotal;

	private Player _player;

	private bool _isReadyToStart;

	private bool _isNewRoundReady;

	private readonly List<Bomb> _bombs = new List<Bomb>();

	private bool _endGameChestSpawned;

	public GameEvents Events
	{
		get;
		private set;
	}

	public CharacterEvents CharacterEvents => _characterEvents;

	public GameState State => _gameState;

	public LootDropManager LootDropManager => _lootDropManager;

	public int RoundCountBetweenWave => _level.RoundCountBetweenWave;

	public GameOverContinueManager GameOverManager
	{
		get;
		private set;
	}

	public bool ChestPhaseStarted
	{
		get;
		set;
	}

	public void Create()
	{
		Init(App.Instance.Player);
	}

	private void Init(Player player)
	{
		_player = player;
		Events = new GameEvents();
		_characterEvents = new CharacterEvents();
		_stats = new GameStats(Events);
		_grid = new GridCircle();
		FSM = new GameStateMachine(this);
		App.Instance.Player.WeaponManager.RegisterGameEvents(Events);
		App.Instance.Player.MonsterMissions.RegisterGameEvents(Events);
		HeroData currentHeroData = _player.HeroManager.GetCurrentHeroData();
		SetHero(currentHeroData);
		_gameState = base.gameObject.AddComponent<GameState>().Init(_hero, _level);
		base.gameObject.AddComponent<FloatingTextManager>().Init(Events, _characterEvents, _gameState);
		base.gameObject.AddComponent<GameFXManager>().Setup(App.Instance.FXManager, Events, _characterEvents, _camera);
		_lootDropManager = base.gameObject.AddComponent<LootDropManager>().Init(App.Instance.RewardFactory, MonoSingleton<MonsterConfigs>.Instance, _characterEvents, Events, _gameState);
		App.Instance.Events.AppPausedEvent += OnAppPaused;
		_player.HeroManager.Events.WeaponsUpdatedEvent += OnWeaponsUpdated;
		_player.HeroManager.Events.HeroSelectedEvent += OnHeroSelected;
	}

	public void SetReadyToStart()
	{
		_isReadyToStart = true;
	}

	public bool IsReadyToStart()
	{
		return _isReadyToStart;
	}

	public void SetLevel(LevelData level)
	{
		if (_level != null)
		{
			_level.UnRegisterGameEvents(Events);
		}
		_level = level;
		_level.RegisterGameEvents(Events);
		UpdateWorld(level.WorldId);
	}

	private void UpdateWorld(string worldId)
	{
		_worldRenderer.Init(worldId);
	}

	private void OnAppPaused()
	{
	}

	private void SetHero(HeroData heroData)
	{
		if (_hero != null)
		{
			UnityEngine.Object.Destroy(_hero.gameObject);
		}
		_hero = Hero.Create(heroData, _player.WeaponManager.Events, _characterEvents);
		_hero.transform.position = Vector3.zero;
		_hero.SetPosition(_hero.transform.position);
		_hero.SetTag("Hero");
		_hero.SetCollisionLayer(9);
	}

	public void StartGame()
	{
		MonoSingleton<GameMusicManager>.Instance.PlayMusicBattle();
		App.Instance.Analytics.RegisterGameEvents(Events);
		App.Instance.Player.LevelManager.RegisterGameEvents(Events);
		_monsterManager = new MonsterManager(MonoSingleton<MonsterConfigs>.Instance, App.Instance.RewardFactory, _characterEvents, Events, _hero, _grid, _gameState.ChestUnlocked);
		_hud.Init(_hero, Events, _characterEvents, _stats, _gameState, _level);
		OnWeaponsUpdated(_player.WeaponManager.GetWeapons(_player.HeroManager.EquippedWeaponIds));
		base.gameObject.AddComponent<GameInputController>().Init(this);
		_weaponShape = _hero.transform.FindChildComponent<WeaponShape>("WeaponShape");
		_weaponShape.SetShape(_hero.GetCurrentWeapon());
		_characterEvents.WeaponChangedEvent += OnWeaponChanged;
		_characterEvents.DamagedEvent += OnDamaged;
		_characterEvents.TargetReachedEvent += OnTargetReached;
		_grid.CharacterEnteredCellEvent += OnCharacterEnteredCell;
		Events.MonsterKilledEvent += OnMonsterKilled;
		Events.GameAbandonedEvent += OnGameAbandoned;
		_stats.OnGameStarted();
		Events.OnGameStarted(_level);
		_gameState.Init(_hero, _level);
		if (_gameState.HasUsedChestKey)
		{
			App.Instance.Player.MuseumManager.RegisterGameEvents(Events);
		}
		GameOverManager = base.gameObject.AddComponent<GameOverContinueManager>().Init(Events, _hero);
		_gameOverContinueMenu.Init(GameOverManager);
		FSM.OnReady();
		_gameState.HasGameStarted = true;
	}

	private void OnDestroy()
	{
		if (_level != null && Events != null)
		{
			_level.UnRegisterGameEvents(Events);
		}
		if (App.IsCreated())
		{
			App.Instance.Player.HeroManager.Events.WeaponsUpdatedEvent -= OnWeaponsUpdated;
			App.Instance.Player.HeroManager.Events.HeroSelectedEvent -= OnHeroSelected;
			App.Instance.Player.WeaponManager.UnRegisterGameEvents(Events);
			App.Instance.Player.LevelManager.UnRegisterGameEvents(Events);
			App.Instance.Player.MonsterMissions.UnRegisterGameEvents(Events);
			App.Instance.Analytics.UnregisterGameEvents(Events);
			if (_gameState != null && _gameState.HasUsedChestKey)
			{
				App.Instance.Player.MuseumManager.UnRegisterGameEvents(Events);
			}
		}
		if (_characterEvents != null)
		{
			_characterEvents.WeaponChangedEvent -= OnWeaponChanged;
			_characterEvents.DamagedEvent -= OnDamaged;
			_characterEvents.TargetReachedEvent -= OnTargetReached;
		}
		if (_grid != null)
		{
			_grid.CharacterEnteredCellEvent -= OnCharacterEnteredCell;
		}
	}

	private void OnWeaponsUpdated(List<WeaponData> equippedWeapons)
	{
		_hero.UpdateEquippedWeapons(equippedWeapons);
		if (_gameState.HasGameStarted)
		{
			_hud.UpdateWeapons(equippedWeapons);
		}
	}

	private void OnHeroSelected(HeroData heroData)
	{
		SetHero(heroData);
		_hud.UpdateHero(_hero, _gameState, Events);
		if (_weaponShape != null)
		{
			UnityEngine.Object.Destroy(_weaponShape.gameObject);
		}
		_weaponShape = _hero.transform.FindChildComponent<WeaponShape>("WeaponShape");
		_weaponShape.SetShape(_hero.GetCurrentWeapon());
	}

	private void OnDrawGizmos()
	{
		for (int i = 0; i < 7; i++)
		{
		}
		if (_grid != null)
		{
			_grid.OnDrawGizmos();
		}
		foreach (Bomb bomb in _bombs)
		{
			foreach (Cell affectedCell in bomb.GetAffectedCells())
			{
			}
		}
	}

	private void DrawGizmosCircle(float size)
	{
	}

	public void OnIdle()
	{
		_hero.OnGameIdle();
	}

	public void StartWave()
	{
		State.WaveRoundCount = 0;
		State.LastMonsterSpawnRound = 0;
		_level.StartWave();
		Events.OnWaveStarted();
	}

	public void EndWave()
	{
		_level.EndWave();
	}

	public void StartRound()
	{
		_weaponShape.ResetTouchedObjects();
		_monsterManager.CleanMonsters();
		_isNewRoundReady = true;
		State.TotalRoundCount++;
		State.WaveRoundCount++;
		if (State.TotalRoundCount == 1)
		{
			_hud.ShowWeapons();
		}
		Events.OnRoundStarted(State.TotalRoundCount);
		HandleBombsOnRoundStarted();
		SetComboCountCurrent(0);
	}

	public void EndRound()
	{
		Events.OnRoundEnded();
	}

	public bool IsCurrentWaveCompleted()
	{
		if (_level.IsObjectiveMissionBased)
		{
			return _level.IsMissionCompleted();
		}
		if (!_level.IsObjectiveWaveBased)
		{
			UnityEngine.Debug.LogWarning("Check if IsCurrentWaveCompleted() condition is okay for this mode...");
		}
		return (!_level.IsLastWaveReached() && IsAboutToSpawnMonsters()) || AreMonstersAllDead();
	}

	public bool IsThereCardsOnGround()
	{
		return _lootDropManager.IsThereCardsOnGround();
	}

	private bool IsAboutToSpawnMonsters()
	{
		return !IsMonsterCountLimitReached() && State.WaveRoundCount + 1 - State.LastMonsterSpawnRound >= RoundCountBetweenWave;
	}

	public bool AreAllWaveCompleted()
	{
		return _level.Completed;
	}

	public bool IsNewRoundReady()
	{
		return _isNewRoundReady;
	}

	private void SetComboCountCurrent(int amount)
	{
		_comboCountCurrent = amount;
		Events.OnComboUpdated(_comboCountCurrent);
		_hero.SetComboDamageBonus(_comboCountCurrent);
	}

	public bool IsMonsterCountLimitReached()
	{
		return _level.GetMaxMonsters() - GetAliveMonsterCount() <= 0;
	}

	public bool SpawnMonsters()
	{
		int monsterPerWaveMinCount = _level.GetMonsterPerWaveMinCount();
		int monsterPerWaveMaxCount = _level.GetMonsterPerWaveMaxCount();
		int minLayer = _level.GetMonsterSpawnLayerMin();
		int maxLayer = _level.GetMonsterSpawnLayerMax();
		int maxMonsters = _level.GetMaxMonsters();
		int num = Math.Min(UnityEngine.Random.Range(monsterPerWaveMinCount, monsterPerWaveMaxCount + 1), maxMonsters - GetAliveMonsterCount());
		for (int i = 0; i < num; i++)
		{
			Action after = null;
			if (i == num - 1)
			{
				after = delegate
				{
					Events.OnMonsterWaveSpawned();
					Events.OnGameStateMessage(new MonsterSpawnedMessage());
				};
			}
			LevelMonster levelMonster = _level.GetMonsterIds().Pick();
			MonsterConfig config = MonoSingleton<MonsterConfigs>.Instance.GetConfig(levelMonster.MonsterId);
			int monsterLevel = App.Instance.Player.MuseumManager.GetMonsterLevel(levelMonster.MonsterId);
			if (levelMonster.SpawnLayerMin != -1)
			{
				minLayer = levelMonster.SpawnLayerMin;
			}
			if (levelMonster.SpawnLayerMax != -1)
			{
				maxLayer = levelMonster.SpawnLayerMax;
			}
			Cell monsterSpawnCell = GetMonsterSpawnCell(minLayer, maxLayer);
			SpawnMonster((float)i * 0.1f, monsterSpawnCell, config, monsterLevel, after, i);
		}
		State.LastMonsterSpawnRound = State.WaveRoundCount;
		return num > 0;
	}

	public void SpawnChests()
	{
		App.Instance.Player.LevelManager.OnEndGameChestSpawned(_level);
		int minLayer = _level.GetMonsterSpawnLayerMin();
		int maxLayer = _level.GetMonsterSpawnLayerMax();
		if (_level.WorldId == "w00" && _level.WorldData.Profile.EndGameChestSpawned <= 3)
		{
			minLayer = 0;
			maxLayer = 1;
		}
		for (int i = 0; i < 1; i++)
		{
			Action after = null;
			if (i == 0)
			{
				after = delegate
				{
					Events.OnMonsterWaveSpawned();
					Events.OnGameStateMessage(new MonsterSpawnedMessage());
				};
			}
			Cell monsterSpawnCell = GetMonsterSpawnCell(minLayer, maxLayer);
			SpawnChest((float)i * 0.1f, monsterSpawnCell, after);
		}
	}

	private Cell GetMonsterSpawnCell(int minLayer, int maxLayer)
	{
		minLayer = Mathf.Clamp(minLayer, 0, 5);
		maxLayer = Mathf.Clamp(maxLayer, 0, 5);
		Cell cell;
		do
		{
			cell = _grid.GetCell(UnityEngine.Random.Range(minLayer, maxLayer + 1), UnityEngine.Random.Range(0, 24));
		}
		while (cell != null && cell.IsOccupied());
		return cell;
	}

	private float GetCirclePos(int circleIndex)
	{
		float num = 0f;
		if (circleIndex == 0)
		{
			return GetCenterCircleSize();
		}
		return (float)circleIndex * 0.45f + GetCenterCircleSize();
	}

	private float GetCenterCircleSize()
	{
		return 0.799999952f;
	}

	private void SpawnMonster(float delay, Cell cell, MonsterConfig monsterConfig, int monsterLevel, Action after = null, int i = 0)
	{
		_monsterManager.SpawnMonster(delay, cell, monsterConfig, monsterLevel, after, i);
	}

	private void SpawnChest(float delay, Cell cell, Action after = null)
	{
		WorldData worldData = App.Instance.Player.LevelManager.GetWorldData(_level.WorldId);
		int endGameChestSpawned = worldData.Profile.EndGameChestSpawned;
		int a = Mathf.FloorToInt((float)endGameChestSpawned / 5f);
		a = Mathf.Min(a, 50);
		_monsterManager.SpawnChest(a, delay, cell, after);
	}

	private void Update()
	{
		if (!(_gameState == null) && _gameState.HasGameStarted)
		{
			if (FSM != null)
			{
				FSM.Update(this);
			}
			HandleHeroAttack();
			if (_monsterManager != null)
			{
				_monsterManager.HandleMonstersAttack();
			}
		}
	}

	private void HandleHeroAttack()
	{
		if (_hero != null && _hero.IsAttacking && _hero.CurrentAttackTarget == null && !_monsterManager.IsThereNonIdleMonsters())
		{
			_hero.HandleAttackState();
		}
	}

	private void OnWeaponChanged(Character character, WeaponData weapon)
	{
		if (character == _hero)
		{
			_weaponShape.SetShape(weapon);
			_weaponShape.StartRotation();
			Events.OnGameStateMessage(new HeroWeaponChange());
		}
	}

	private void OnDamaged(Character attacker, Character defender, WeaponData weaponUsed, int damage)
	{
		if (!defender.IsMonster())
		{
			return;
		}
		string killedWithWeaponId = string.Empty;
		if (weaponUsed != null)
		{
			killedWithWeaponId = weaponUsed.Config.Id;
			Events.OnWeaponUsed(weaponUsed.GetSpriteName());
		}
		if (defender.IsDead())
		{
			Events.OnMonsterKilled(defender.Id, killedWithWeaponId);
		}
		if (attacker == _hero && !ChestPhaseStarted)
		{
			SetComboCountCurrent(_comboCountCurrent + 1);
			if (_hero.PendingTargets.Count == 0 && _comboCountCurrent > 1)
			{
				_comboCountTotal += _comboCountCurrent;
				Events.OnComboFinished(_comboCountCurrent);
				this.Execute(1f, delegate
				{
					SetComboCountCurrent(0);
				});
			}
		}
	}

	public void OnTargetReached()
	{
		CheckBombCollision();
	}

	public void OnTouch()
	{
		if (Events != null)
		{
			Events.OnGameStateMessage(new UserTapMessage());
		}
	}

	public bool CollectLoot()
	{
		bool result = false;
		List<GameObject> lastTouchedObjects = _weaponShape.GetLastTouchedObjects("LootLife");
		foreach (GameObject item in lastTouchedObjects)
		{
			LootDrop component = item.transform.GetComponent<LootDrop>();
			if (component != null)
			{
				component.ForceCollect();
				result = true;
			}
		}
		return result;
	}

	public void StartRotation()
	{
		_weaponShape.StartRotation();
	}

	public void StopRotation()
	{
		_weaponShape.StopRotation();
		_weaponShape.Flash();
	}

	public void HideWeaponShape()
	{
		_weaponShape.Hide();
		_monsterManager.HideAllTargetsOverMonsters();
	}

	public void HideShapeField()
	{
		_weaponShape.HideShapeField();
	}

	public void ShowShapeField()
	{
		_weaponShape.ShowShapeField();
	}

	public void OnScreenTapWhileWeaponRotating()
	{
		_hud.OnScreenTapWhileWeaponRotating();
	}

	public void OnLevelCompleted()
	{
		Events.OnLevelFinished(_level);
		_hero.OnLevelFishined();
	}

	public void OnHeroDead()
	{
		MonoSingleton<GameMusicManager>.Instance.PlayBattleLost();
	}

	public void PrepareMonsterTurn()
	{
		_monsterManager.PrepareMonsterTurn();
	}

	public void StartHeroAttackPhase()
	{
		CharacterAttackingPhase characterAttackingPhase = null;
		if (_hero.IsKunaiWeapon())
		{
			characterAttackingPhase = _hero.gameObject.AddComponent<CharacterAttackingPhaseKunai>().Init(_hero, _weaponShape);
		}
		else if (_hero.IsMeleeWeapon() || _hero.IsRangedWeapon())
		{
			characterAttackingPhase = _hero.gameObject.AddComponent<CharacterAttackingPhaseWeapon>().Init(_hero, _weaponShape);
		}
		else if (_hero.IsBombWeapon())
		{
			characterAttackingPhase = _hero.gameObject.AddComponent<CharacterAttackingPhaseBomb>().Init(this, _hero, _weaponShape, _grid);
		}
		if (characterAttackingPhase != null)
		{
			_hero.SetAttackingPhase(characterAttackingPhase);
		}
		else
		{
			UnityEngine.Debug.LogWarning("This weapon isn't handled");
		}
	}

	public void EndHeroAttackPhase()
	{
		_hero.SetAttackingPhase(null);
	}

	public bool IsHeroAttackPhaseOver()
	{
		if (_hero.AttackingPhase != null)
		{
			return _hero.AttackingPhase.IsOver();
		}
		return true;
	}

	public bool IsHeroIdle()
	{
		return _hero.IsIdle;
	}

	public bool IsHeroMoving()
	{
		return _hero.HasTargetPos;
	}

	public bool IsHeroDead()
	{
		return _hero.IsDead();
	}

	public int GetPetrifiedTurnRemaining()
	{
		return _hero.GetPetrifiedTurnRemaining();
	}

	public bool IsHeroPetrified()
	{
		return _hero.IsPetrified();
	}

	public void UnpetrifyHero()
	{
		_hero.Unpetrify();
	}

	public void MoveHeroToHome()
	{
		_hero.SetHomeTarget();
	}

	public void StartMonsterMovingPhase()
	{
		_monsterManager.StartMonsterMovingPhase();
	}

	public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
	{
		return Quaternion.Euler(angles) * (point - pivot) + pivot;
	}

	public void StartMonsterAttackingPhase()
	{
		_monsterManager.StartMonsterAttackingPhase();
	}

	public void UpdateAttackingMonsters()
	{
		_monsterManager.UpdateAttackingMonsters();
	}

	public bool AreMonstersMoving()
	{
		return _monsterManager.AreMonstersMoving();
	}

	public bool AreMonstersAttacking()
	{
		return _monsterManager.AreMonstersAttacking();
	}

	public bool AreMonstersAllDead()
	{
		return _monsterManager.AreMonstersAllDead();
	}

	public int GetAliveMonsterCount()
	{
		return _monsterManager.GetAliveMonsterCount();
	}

	public bool MustSpawnMoreMonsters()
	{
		if (IsMonsterCountLimitReached())
		{
			return false;
		}
		bool flag = State.WaveRoundCount == 1;
		int num = State.WaveRoundCount - State.LastMonsterSpawnRound;
		bool flag2 = num >= RoundCountBetweenWave;
		return (flag | (flag2 && (!_level.IsObjectiveWaveBased || !_level.IsLastWaveReached()))) || AreMonstersAllDead();
	}

	public void UpdateMovingMonsters()
	{
		_monsterManager.UpdateMovingMonsters();
	}

	public void PushAllMonstersBack(int layerCount)
	{
		_monsterManager.PushAllMonstersBack(layerCount);
	}

	public void ShowWeapons()
	{
		_hud.ShowWeapons();
	}

	public void KillAndExpulseAllMonsters()
	{
		_monsterManager.KillAndExpulseAllMonsters();
		while (_bombs.Count > 0)
		{
			BombExplode(0);
		}
		_bombs.Clear();
	}

	public void HideChests()
	{
		_monsterManager.HideChests();
	}

	public void StartGameLevelStartedAnim()
	{
		_hud.StartGameLevelStartedAnim();
	}

	public void OnWeaponShapeAnim()
	{
	}

	public void HideWeaponsMenu()
	{
		_hud.HideWeapons();
	}

	private void OnGameAbandoned(GameStats stats, GameState gameState)
	{
		App.Instance.Player.StatsManager.MergeGameStats(_stats);
	}

	public void OnEndGame()
	{
		if (_level != null && Events != null)
		{
			_level.UnRegisterGameEvents(Events);
		}
		_gameState.HasGameEnded = true;
		_gameState.HasGameStarted = false;
		if (IsHeroDead())
		{
			App.Instance.Player.LevelManager.OnLevelLost(_level.BaseId);
		}
		_stats.OnGameEnded();
		App.Instance.Player.StatsManager.MergeGameStats(_stats);
		Events.OnGameEnded(_stats, _gameState, _level);
		App.Instance.Player.Save();
		ShowEndGame();
	}

	private void ShowEndGame()
	{
		_gamePanel.SetActive( false);
		CheckForNewWeapon();
	}

	private void CheckForNewWeapon()
	{
		Events.OnGameQuit();
		List<Reward> cardCollected = _gameState.GetCardCollected();
		if (cardCollected.Count == 0)
		{
			App.Instance.LoadMainMenuFromGame();
			return;
		}
		_endGamePanel.Init(cardCollected);
		_endGamePanel.Show();
	}

	public void AddBomb(Bomb bomb)
	{
		_bombs.Add(bomb);
	}

	public bool HandleMapObjects()
	{
		if (_hero.IsBombWeapon())
		{
			CheckBombCollision();
			int count = _bombs.Count;
			for (int num = count - 1; num >= 0; num--)
			{
				BombExplode(num);
			}
			return count > 0;
		}
		return false;
	}

	public void CheckBombCollision()
	{
		HashSet<Character> hashSet = new HashSet<Character>();
		List<int> list = new List<int>();
		foreach (Monster monster in _monsterManager.GetMonsters())
		{
			if (monster != null && !monster.IsDead() && monster.CurrentCell != null)
			{
				for (int num = _bombs.Count - 1; num >= 0; num--)
				{
					Bomb bomb = _bombs[num];
					foreach (Cell affectedCell in bomb.GetAffectedCells())
					{
						if (monster.CurrentCell == affectedCell || monster.CurrentMovingCell == affectedCell)
						{
							hashSet.Add(monster);
							if (!list.Contains(num))
							{
								list.Add(num);
							}
						}
					}
				}
			}
		}
		list.Sort();
		foreach (int item in list)
		{
			BombExplode(item);
		}
		foreach (Character item2 in hashSet)
		{
			item2.ResetMovementPattern();
			WeaponData weaponItem = _hero.GetWeaponItem("weapon04Bomb");
			if (weaponItem != null)
			{
				int damage = Mathf.CeilToInt(weaponItem.GetDamage());
				item2.TakeDamage(_hero, weaponItem, damage, _hero.GetLastJumpPosition());
			}
		}
	}

	private void HandleBombsOnRoundStarted()
	{
		bool flag = false;
		for (int num = _bombs.Count - 1; num >= 0; num--)
		{
			if (_bombs[num].IsReadyToExplode())
			{
				BombExplode(num);
				flag = true;
			}
			else
			{
				_bombs[num].OnRoundStarted();
			}
		}
		if (flag)
		{
			_isNewRoundReady = false;
			this.Execute(0.25f, delegate
			{
				_isNewRoundReady = true;
			});
		}
	}

	private void OnCharacterEnteredCell(Cell cell)
	{
	}

	private void BombExplode(int index)
	{
		if (index >= _bombs.Count)
		{
			UnityEngine.Debug.LogWarning("Bomb Explose index out of bound. index = " + index + ", bomb count = " + _bombs.Count);
			return;
		}
		Events.OnBombExplosion(_bombs[index].transform.position);
		UnityEngine.Object.Destroy(_bombs[index].gameObject);
		_bombs.RemoveAt(index);
	}

	public void OnPetrifiedHeroRoundStarted()
	{
		Vector3 startPosition = new Vector3(0f, 3f, -8f);
		FloatingText.CreateScaleUp("TURN LOST!", startPosition, Color.white);
	}

	public void StartIntroGameFlow()
	{
		_gameIntroFlow = IntroGameFlow.Create(this, base.transform);
		_gameIntroFlow.StartFlow(null);
	}

	public bool IsIntroGameFlowFinished()
	{
		if (_gameIntroFlow == null)
		{
			UnityEngine.Debug.LogWarning("_endGameFlow should not be null while calling IsEndGameFlowFinished()");
			return false;
		}
		return _gameIntroFlow.Finished;
	}

	public void StartEndGameFlow()
	{
		_endGameFlow = EndGameFlow.Create(this, base.transform);
		_endGameFlow.StartFlow(new List<Monster>(_monsterManager.GetMonsters()));
	}

	public bool IsEndGameFlowFinished()
	{
		if (_endGameFlow == null)
		{
			UnityEngine.Debug.LogWarning("_endGameFlow should not be null while calling IsEndGameFlowFinished()");
			return false;
		}
		return _endGameFlow.Finished;
	}

	private void OnMonsterKilled(string defenderId, string weaponName)
	{
		if (!(defenderId == "chest"))
		{
		}
	}

	public bool CanSpawnEndGameChest()
	{
		if (!_level.IsLastWaveReached())
		{
			return false;
		}
		bool result = !_endGameChestSpawned;
		_endGameChestSpawned = true;
		return result;
	}

	public void ShowGameOverContinue()
	{
		_gameOverContinueMenu.Show();
	}
}