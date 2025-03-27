using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class MonsterManager
{
	private readonly MonsterConfigs _monsterConfigs;

	private readonly RewardFactory _rewardFactory;

	private readonly CharacterEvents _characterEvents;

	private readonly GameEvents _gameEvents;

	private readonly List<Monster> _monsters = new List<Monster>();

	private readonly List<Monster> _movingMonsters = new List<Monster>();

	private readonly List<Monster> _attackingMonsters = new List<Monster>();

	private readonly List<Monster> _waitingMonsters = new List<Monster>();

	private Character _hero;

	private GridCircle _grid;

	private bool _isChestUnlocked;

	private int _spawnCount;

	public MonsterManager(MonsterConfigs monsterConfigs, RewardFactory rewardFactory, CharacterEvents characterEvents, GameEvents gameEvents, Character hero, GridCircle grid, bool isChestUnlocked)
	{
		_monsterConfigs = monsterConfigs;
		_rewardFactory = rewardFactory;
		_characterEvents = characterEvents;
		_gameEvents = gameEvents;
		_hero = hero;
		_grid = grid;
		_isChestUnlocked = isChestUnlocked;
		_gameEvents.RoundEndedEvent += OnRoundEnded;
		_gameEvents.RoundStartedEvent += OnRoundStarted;
		characterEvents.WaitingToAttackEvent += OnWaitingToAttack;
	}

	private void OnRoundStarted(int roundCount)
	{
		_monsters.ForEach(delegate(Monster c)
		{
			if (c != null)
			{
				c.OnRoundStarted();
			}
		});
	}

	private void OnRoundEnded()
	{
		foreach (Monster monster in _monsters)
		{
			monster.IncreaseLifeTurn();
			if (monster.TotalLifeTurn > 0)
			{
				if (monster.AttackEachRoundCount > 0 && monster.TotalLifeTurn % monster.AttackEachRoundCount == 0 && (!monster.IsTheEye() || !_hero.IsPetrified()))
				{
					monster.SetWaitingToAttack();
				}
				if (monster.LifeDurationRoundCount > 0 && !monster.IsDead() && monster.TotalLifeTurn != monster.LifeDurationRoundCount - 1 && monster.TotalLifeTurn >= monster.LifeDurationRoundCount)
				{
					monster.ScaleToHideAndKill();
				}
			}
		}
	}

	private void OnWaitingToAttack(Character character)
	{
		if (character.IsMonster())
		{
			character.LookAt(_hero.GetPosition());
		}
	}

	private void OnMonsterMoved()
	{
		_gameEvents.OnMonsterMoved();
	}

	public void CleanMonsters()
	{
		_monsters.ForEach(delegate(Monster m)
		{
			if ((m.IsDead() || m.CurrentCell == null) && !m.IsAnimating())
			{
				UnityEngine.Object.Destroy(m.gameObject);
			}
		});
		_monsters.RemoveAll((Monster m) => m == null || ((m.IsDead() || m.CurrentCell == null) && !m.IsAnimating()));
	}

	public bool IsThereNonIdleMonsters()
	{
		foreach (Monster monster in _monsters)
		{
			if (!monster.IsDead() && !monster.IsIdle)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsThereChest()
	{
		foreach (Monster monster in _monsters)
		{
			if (monster != null && monster.IsChest() && monster.CurrentCell != null && !monster.IsDead())
			{
				return true;
			}
		}
		return false;
	}

	public void HandleMonstersAttack()
	{
		foreach (Monster monster in _monsters)
		{
			if (!monster.IsDead() && monster.IsAttacking && monster.CurrentAttackTarget == null)
			{
				monster.HandleAttackState();
			}
		}
	}

	public void HideAllTargetsOverMonsters()
	{
		foreach (Monster monster in _monsters)
		{
			if (monster != null && !monster.IsDead() && monster.CurrentCell != null)
			{
				monster.HideTarget();
			}
		}
	}

	public void SpawnMonster(float delay, Cell cell, MonsterConfig monsterConfig, int monsterLevel, Action after = null, int i = 0)
	{
		SpawnMonster(monsterConfig, monsterLevel, delay, cell, after);
	}

	public void SpawnChest(int monsterLevel, float delay, Cell cell, Action after = null)
	{
		string id = (!_isChestUnlocked) ? "chestLocked" : "chest";
		MonsterConfig config = _monsterConfigs.GetConfig(id);
		SpawnMonster(config, monsterLevel, delay, cell, after);
	}

	private void SpawnMonster(MonsterConfig monsterConfig, int monsterLevel, float delay, Cell spawnCell, Action after)
	{
		List<Reward> list = new List<Reward>();
		foreach (string item in monsterConfig.RewardIdHit)
		{
			if (!string.IsNullOrEmpty(item))
			{
				Reward reward = _rewardFactory.Create(item);
				if (reward != null)
				{
					list.Add(reward);
				}
			}
		}
		int damageToHeroWeapon = Mathf.CeilToInt((float)(monsterLevel + 1) / 2f);
		if (monsterConfig.IsChest())
		{
			damageToHeroWeapon = 1;
		}
		float statBoost = 1f + 0.2f * (float)monsterLevel;
		Monster monster = Monster.Create(monsterConfig, list, _characterEvents, statBoost, damageToHeroWeapon);
		monster.transform.FindChildComponent<WeaponShape>("WeaponShape").gameObject.SetActive( false);
		monster.transform.position = spawnCell.ToMapPos();
		monster.SetPosition(monster.transform.position);
		monster.SetTag("Monster");
		monster.LookAt(_hero.GetPosition());
		monster.SetCollisionLayer(10);
		monster.name = monsterConfig.Id + "_" + _spawnCount;
		monster.SetCurrentCell(spawnCell);
		string skinPath = CharacterVisual.GetSkinPath(monsterConfig.Id);
		CharacterSkin characterSkin = Resources.Load<CharacterSkin>(skinPath);
		UIStatusBarCharacter uIStatusBarCharacter = UnityEngine.Object.Instantiate(Resources.Load<UIStatusBarCharacter>("BarMonsterCanvas"));
		uIStatusBarCharacter.transform.SetParent(monster.Visual.MovingPivot);
		uIStatusBarCharacter.transform.localPosition = new Vector3(0f, characterSkin.StatusBarPosY, 0f);
		uIStatusBarCharacter.Init(monster, _characterEvents, false, false);
		uIStatusBarCharacter.gameObject.SetActive(monsterConfig.Id != "chestLocked");
		monster.SetStatusBar(uIStatusBarCharacter);
		Transform transform = monster.Visual.MovingPivot.gameObject.GetChild("target_triangle").transform;
		Transform transform2 = transform;
		Vector3 localPosition = transform.localPosition;
		float x = localPosition.x;
		float targetArrowPosY = characterSkin.TargetArrowPosY;
		Vector3 localPosition2 = transform.localPosition;
		transform2.localPosition = new Vector3(x, targetArrowPosY, localPosition2.z);
		transform = monster.Visual.MovingPivot.gameObject.GetChild("target_weak").transform;
		Transform transform3 = transform;
		Vector3 localPosition3 = transform.localPosition;
		float x2 = localPosition3.x;
		float targetArrowPosY2 = characterSkin.TargetArrowPosY;
		Vector3 localPosition4 = transform.localPosition;
		transform3.localPosition = new Vector3(x2, targetArrowPosY2, localPosition4.z);
		transform = monster.Visual.MovingPivot.gameObject.GetChild("target_critical").transform;
		Transform transform4 = transform;
		Vector3 localPosition5 = transform.localPosition;
		float x3 = localPosition5.x;
		float targetArrowPosY3 = characterSkin.TargetArrowPosY;
		Vector3 localPosition6 = transform.localPosition;
		transform4.localPosition = new Vector3(x3, targetArrowPosY3, localPosition6.z);
		Transform transform5 = monster.Visual.MovingPivot.gameObject.GetChild("ExclamationAnim").transform;
		Transform transform6 = transform5;
		Vector3 localPosition7 = transform5.localPosition;
		float x4 = localPosition7.x;
		float exclamationPosY = characterSkin.ExclamationPosY;
		Vector3 localPosition8 = transform5.localPosition;
		transform6.localPosition = new Vector3(x4, exclamationPosY, localPosition8.z);
		_spawnCount++;
		_monsters.Add(monster);
		monster.transform.localScale = Vector3.zero;
		monster.transform.DOScale(Vector3.one, 0.25f).SetDelay(delay).OnComplete(delegate
		{
			_gameEvents.OnMonsterSpawned();
			if (after != null)
			{
				after();
			}
		});
	}

	public void PrepareMonsterTurn()
	{
		_waitingMonsters.Clear();
		_movingMonsters.Clear();
		_attackingMonsters.Clear();
		CleanMonsters();
		_waitingMonsters.AddRange(_monsters);
		foreach (Monster waitingMonster in _waitingMonsters)
		{
			if (!waitingMonster.IsDead())
			{
				if (waitingMonster.IsWaitingToAttack)
				{
					if (waitingMonster.CurrentCell.Layer == 0 || waitingMonster.IsRangedAttack)
					{
						_attackingMonsters.Add(waitingMonster);
						continue;
					}
					if (waitingMonster.HasReachedAttackAttemptMax())
					{
						waitingMonster.SetMovingBackward();
						waitingMonster.ResetAttackAttemptCount();
					}
					else
					{
						waitingMonster.SetMovingForward();
					}
				}
				_movingMonsters.Add(waitingMonster);
			}
		}
	}

	public void StartMonsterMovingPhase()
	{
		int num = 0;
		for (int num2 = _movingMonsters.Count - 1; num2 >= 0; num2--)
		{
			Monster monster = _movingMonsters[num2];
			if (monster.IsDead() || monster.CurrentCell == null)
			{
				_movingMonsters.RemoveAt(num2);
				continue;
			}
			if (monster.HasOngoingMovement())
			{
				monster.ResumeMovementPause();
			}
			else
			{
				List<MoveDirection> currentMoveDirection = monster.CurrentMoveDirection;
				Cell cell = _grid.FindCell(monster.CurrentCell, currentMoveDirection);
				if (cell == null)
				{
					UnityEngine.Debug.LogWarning(monster.name + " is trying to move on an invalid cell.");
					continue;
				}
				if (cell == monster.CurrentCell)
				{
					if (currentMoveDirection[0] != MoveDirection.Pause && currentMoveDirection[0] != MoveDirection.Skip)
					{
						if (!monster.IsChest())
						{
							UnityEngine.Debug.Log(monster.name + " is trying to move on the same cell, just wait here.");
						}
						continue;
					}
				}
				else if (cell.IsOccupied())
				{
					if (!(cell.OccupiedBy != null))
					{
					}
					continue;
				}
				cell.AddIncoming(monster);
				monster.MoveToPath(currentMoveDirection);
			}
			MonoExtensions.Execute((float)num * 0.1f, OnMonsterMoved);
			num++;
		}
	}

	public void StartMonsterAttackingPhase()
	{
		_attackingMonsters.Shuffle();
		if (_attackingMonsters.Count <= 0)
		{
			return;
		}
		Monster monster = _attackingMonsters[0];
		if (monster.IsDead() || monster.CurrentCell == null)
		{
			_attackingMonsters.RemoveAt(0);
			StartMonsterAttackingPhase();
			return;
		}
		if (monster.IsTheEye() && _hero.IsPetrified())
		{
			_attackingMonsters.RemoveAt(0);
			monster.ResetAttackAttemptCount();
			if (monster.WasGoingBackward())
			{
				monster.SetMovingBackward();
			}
			else
			{
				monster.SetMovingForward();
			}
			return;
		}
		if (monster.IsLeech())
		{
			monster.SetAttackingPhase(monster.gameObject.AddComponent<CharacterLeechAttackingPhase>().Init(monster));
		}
		else if (monster.IsTheEye())
		{
			monster.SetAttackingPhase(monster.gameObject.AddComponent<CharacterEyeAttackingPhase>().Init(monster));
		}
		else if (monster.IsSpider())
		{
			monster.SetAttackingPhase(monster.gameObject.AddComponent<CharacterSpiderAttackingPhase>().Init(monster));
		}
		else
		{
			monster.SetAttackingPhase(monster.gameObject.AddComponent<CharacterAttackingPhase>().Init(monster));
		}
		List<Character> list = new List<Character>();
		list.Add(_hero);
		List<Character> list2 = list;
		if (monster.IsSalamander())
		{
			list2.Add(_hero);
		}
		monster.SetAttackTargets(list2);
		monster.StartAttack();
	}

	public void UpdateAttackingMonsters()
	{
		if (AreMonstersAttacking() && _attackingMonsters[0].IsIdle && _attackingMonsters[0].CurrentAttackTarget == null)
		{
			_attackingMonsters.RemoveAt(0);
			StartMonsterAttackingPhase();
		}
	}

	public bool AreMonstersMoving()
	{
		return _movingMonsters.Count > 0 && !AreMonstersAttacking();
	}

	public bool AreMonstersAttacking()
	{
		return _attackingMonsters.Count > 0;
	}

	public bool AreMonstersAllDead()
	{
		foreach (Monster monster in _monsters)
		{
			if (monster != null && !monster.IsDead() && monster.CurrentCell != null && !monster.IsLockedChest())
			{
				return false;
			}
		}
		return true;
	}

	public int GetAliveMonsterCount()
	{
		return _monsters.FindAll((Monster m) => m != null && !m.IsDead() && m.CurrentCell != null).Count;
	}

	public void UpdateMovingMonsters()
	{
		for (int num = _movingMonsters.Count - 1; num >= 0; num--)
		{
			Monster monster = _movingMonsters[num];
			if (monster.IsDead() || monster.CurrentCell == null)
			{
				_movingMonsters.RemoveAt(num);
			}
			else if (monster.IsIdle)
			{
				int layer = monster.CurrentCell.Layer;
				if (layer == 0 && monster.AttackEachRoundCount <= 0 && monster.CanAttack)
				{
					monster.SetWaitingToAttack();
				}
				else if (layer == 5 && monster.MovePatternState != 0 && !monster.IsWaitingToAttack)
				{
					monster.SetMovingForward();
				}
				_movingMonsters.RemoveAt(num);
			}
		}
	}

	public void KillAndExpulseAllMonsters()
	{
		_gameEvents.OnMonsterExpulsed();
		foreach (Monster monster in _monsters)
		{
			if (!monster.IsDead())
			{
				monster.TakeDamage(null, null, Mathf.RoundToInt(monster.HP), Vector2.zero);
			}
		}
	}

	public void PushAllMonstersBack(int layerCount)
	{
		foreach (Monster monster in _monsters)
		{
			if (!monster.IsDead())
			{
				monster.OnDamageTaken(layerCount);
			}
		}
	}

	public void HideChests()
	{
		foreach (Monster monster in _monsters)
		{
			if (!monster.IsDead())
			{
				monster.ScaleToHideAndKill();
			}
		}
	}

	public IEnumerable<Monster> GetMonsters()
	{
		return _monsters;
	}
}