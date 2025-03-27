using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
	private MonsterConfig _config;

	private int _coinsRewardOnHit;

	private int _coinsRewardOnHitRemaining;

	private int _damageToHeroWeapon;

	private int _lifeDurationRoundCount;

	private float _statBoost;

	private UIStatusBarCharacter _statusBar;

	private bool _hasTeleported;

	private Sequence _teleportSequence;

	private int _movementCount;

	private bool _isUnderwater;

	private GameObject _underwaterShadowGO;

	public override string Id => _config.Id;

	public override string Name => _config.Name;

	public override int HPMax => Mathf.RoundToInt((float)_config.HP * _statBoost);

	public override int TokenValue => _config.TokenValue;

	public override float AttackPrecision => 1f;

	public override bool CanAttack => _config.DamageMin > 0 && _config.WeaponType != WeaponType.None;

	public override bool CanLevelUp => false;

	public override bool IsStatic => _config.MonsterType == MonsterType.monsterStatic;

	public override bool IsRangedAttack => _config.WeaponType == WeaponType.Fireball || _config.WeaponType == WeaponType.Ranged;

	public override bool IsMeleeAttack => _config.WeaponType == WeaponType.Melee;

	public bool IsBoss
	{
		get;
		private set;
	}

	public bool IsMidBoss
	{
		get;
		private set;
	}

	public int AttackEachRoundCount => _config.AttackEachRoundCount;

	public override MovePattern MovePattern
	{
		get;
		protected set;
	}

	public override float AttackMovementDuration => 0.2f;

	public override float RunningSpeed => 2.25f;

	public override float MissRate => _config.MissRate;

	public override int DamageToWeapon => _damageToHeroWeapon;

	public override int AttackAttemptCountMax => _config.AttackAttemptCountMax;

	public int LifeDurationRoundCount => _lifeDurationRoundCount;

	public static Monster Create(MonsterConfig monsterConfig, List<Reward> rewards, CharacterEvents characterEvents, float statBoost, int damageToHeroWeapon)
	{
		CharacterVisual characterVisual = CharacterVisual.Create(monsterConfig.Id, monsterConfig.Element);
		Monster monster = characterVisual.gameObject.AddComponent<Monster>();
		return monster.Init(monsterConfig, false, false, rewards, characterVisual, characterEvents, statBoost, damageToHeroWeapon);
	}

	private Monster Init(MonsterConfig config, bool isBoss, bool isMidBoss, List<Reward> rewardsOnHit, CharacterVisual characterVisual, CharacterEvents characterEvents, float statBoost, int damageToHeroWeapon)
	{
		Init(characterVisual, characterEvents);
		_statBoost = statBoost;
		_config = config;
		HP = HPMax;
		IsBoss = isBoss;
		IsMidBoss = isMidBoss;
		_damageToHeroWeapon = damageToHeroWeapon;
		_lifeDurationRoundCount = _config.LifeDurationRoundCount;
		MovePattern = MovePattern.FromString(config.MovePatternForward, config.MovePatternBackward);
		foreach (Reward item in rewardsOnHit)
		{
			RewardLoot rewardLoot = item as RewardLoot;
			if (rewardLoot != null && rewardLoot.LootId == "lootCoin")
			{
				_coinsRewardOnHit = Mathf.RoundToInt(rewardLoot.LootAmount);
				_coinsRewardOnHitRemaining = _coinsRewardOnHit;
				return this;
			}
		}
		return this;
	}

	protected override void OnDestroy()
	{
		DestroyTeleportSequence();
		base.OnDestroy();
	}

	public void SetStatusBar(UIStatusBarCharacter statusBar)
	{
		_statusBar = statusBar;
		UpdateRemainingTurnText();
	}

	public override Element GetCharacterElement()
	{
		return _config.Element;
	}

	public override Element GetWeaponElementWeak()
	{
		return Element.None;
	}

	public override Element GetWeaponElementCritical()
	{
		return Element.None;
	}

	public override Element GetWeaponElementMiss()
	{
		return Element.None;
	}

	public int ExpenseCoinsOnHit(int damage)
	{
		if (_coinsRewardOnHitRemaining > 0)
		{
			if (IsDead())
			{
				return _coinsRewardOnHitRemaining;
			}
			float num = Mathf.Clamp01((float)damage / (float)HPMax);
			int a = Mathf.FloorToInt(num * (float)_coinsRewardOnHit);
			int num2 = Mathf.Min(a, _coinsRewardOnHitRemaining);
			_coinsRewardOnHitRemaining -= num2;
			return num2;
		}
		return 0;
	}

	protected override int ComputeDamage()
	{
		return Mathf.RoundToInt((float)UnityEngine.Random.Range(_config.DamageMin, _config.DamageMax + 1) * _statBoost);
	}

	public override void OnDead()
	{
		base.Visual.StopFlash();
		if (!IsBoss)
		{
			Vector3 position = GetPosition();
			Vector2 normalized = (GetPosition().XY() - _lastAttackByPos).normalized;
			PushBack(position + 12f * new Vector3(normalized.x, normalized.y), null, null, 0.5f);
		}
	}

	public void OnRoundStarted()
	{
		_hasTeleported = false;
		UpdateRemainingTurnText();
	}

	private void UpdateRemainingTurnText()
	{
		if (_statusBar != null)
		{
			_statusBar.SetRemainingTurnText(_lifeDurationRoundCount - base.TotalLifeTurn);
		}
	}

	private void DestroyTeleportSequence()
	{
		if (_teleportSequence != null)
		{
			_teleportSequence.Complete( true);
			_teleportSequence = null;
		}
	}

	public bool HasTeleported()
	{
		return _hasTeleported;
	}

	protected override void OnIncomingAttack(Character attacker)
	{
		if (!IsGhost() || attacker.IsCounterAttacking())
		{
			return;
		}
		int column = (base.CurrentCell.Column + Mathf.RoundToInt(12f)) % 24;
		Cell cell = base.CurrentCell.GetCell(base.CurrentCell.Layer, column);
		if (UnityEngine.Random.value >= 0.5f && TeleportTo(cell, delegate
		{
			LookAt(attacker.GetPosition());
		}))
		{
			Vector3 fxPos = GetPosition();
			fxPos.y += base.transform.GetHeight() * 0.75f;
			UIStatusBarCharacter componentInChildren = base.Visual.transform.GetComponentInChildren<UIStatusBarCharacter>();
			if (componentInChildren != null)
			{
				ref Vector3 reference = ref fxPos;
				Vector3 position = componentInChildren.transform.position;
				reference.y = position.y;
			}
			this.Execute(0.1f, delegate
			{
				base.Events.OnDodged(attacker, this, fxPos);
			});
			WeaponData currentWeapon = attacker.GetCurrentWeapon();
			if (currentWeapon.Config.CanCounterGhost)
			{
				attacker.SetNextPendingTarget(this);
			}
		}
	}

	private bool TeleportTo(Cell targetCell, Action onComplete)
	{
		if (targetCell != null && !targetCell.IsOccupied())
		{
			_hasTeleported = true;
			DestroyTeleportSequence();
			_teleportSequence = DOTween.Sequence();
			_teleportSequence.OnStart(delegate
			{
				SetCurrentCell(targetCell);
			});
			_teleportSequence.Append(base.Visual.FadeOut().OnComplete(delegate
			{
				base.transform.position = targetCell.ToMapPos();
				SetPosition(base.transform.position);
			}));
			_teleportSequence.Append(base.Visual.FadeIn().SetDelay(0.1f));
			_teleportSequence.OnComplete(delegate
			{
				onComplete();
			});
			OnTargetReached();
			return true;
		}
		return false;
	}

	public override AnimationState GetIdleAnimId()
	{
		if (IsUnderwater())
		{
			return AnimationState.IdleUnderwater;
		}
		return base.GetIdleAnimId();
	}

	protected override IEnumerator OnPreMoveCR()
	{
		yield return null;
		if (IsFrog())
		{
			if (IsUnderwater())
			{
				yield return StartCoroutine(DiveOutCR());
			}
			else
			{
				yield return StartCoroutine(DiveInCR());
			}
		}
		_isPreMoveOver = true;
	}

	protected override IEnumerator OnPostMoveCR()
	{
		yield return null;
		if (IsFrog() && base.CurrentCell.Layer == 0 && _isUnderwater)
		{
			yield return StartCoroutine(DiveOutCR());
		}
		_movementCount++;
		_isPostMoveOver = true;
	}

	public bool IsUnderwater()
	{
		return _isUnderwater;
	}

	private IEnumerator DiveInCR()
	{
		_isUnderwater = true;
		Collider2D collider = base.Visual.MovingPivot.GetComponent<Collider2D>();
		if (collider != null)
		{
			collider.enabled = false;
		}
		base.Visual.HideUI();
		Vector3 currentLocalScale = base.Visual.MovingPivot.localScale;
		Sequence sequence = DOTween.Sequence();
		Transform t = base.Visual.MovingPivot;
		sequence.Append(t.DOScale(new Vector3(currentLocalScale.x * 0.7f, currentLocalScale.y * 1.2f, currentLocalScale.z), 0.08f));
		sequence.Append(t.DOScale(new Vector3(currentLocalScale.x * 0.7f * 0.05f, currentLocalScale.y * 1.2f * 0.05f, currentLocalScale.z), 0.2f));
		sequence.Append(t.DOScale(Vector3.zero, 0.05f));
		Transform ripple = UnityEngine.Object.Instantiate(Resources.Load<Transform>("CFXM_Ripple"));
		Vector3 ripplePos = base.Visual.MovingPivot.position;
		ripplePos.z += 0.15f;
		ripple.position = ripplePos;
		yield return sequence.WaitForCompletion();
		base.Events.OnDivedIn(this);
		base.Visual.HideShadow();
		if (_underwaterShadowGO == null)
		{
			_underwaterShadowGO = FX.CreateUnderwaterShadow(base.Visual.MovingPivot, Vector3.zero);
			_underwaterShadowGO.AddComponent<Follow>().Init(base.Visual.MovingPivot);
		}
		if (_underwaterShadowGO != null)
		{
			_underwaterShadowGO.transform.localScale = Vector3.one;
			_underwaterShadowGO.SetActive( true);
		}
		string skinPath = CharacterVisual.GetSkinPath("Underwater");
		base.Visual.ApplySkin(skinPath);
		Play(GetIdleAnimId());
		base.Visual.MovingPivot.localScale = currentLocalScale;
	}

	private IEnumerator DiveOutCR()
	{
		yield return new WaitForSeconds(0.05f);
		_isUnderwater = false;
		Collider2D collider = base.Visual.MovingPivot.GetComponent<Collider2D>();
		if (collider != null)
		{
			collider.enabled = true;
		}
		Vector3 currentLocalScale = base.Visual.MovingPivot.localScale;
		base.Visual.MovingPivot.localScale = Vector3.zero;
		base.Visual.ApplyDefaultSkin();
		Play(AnimationState.Run);
		base.Events.OnDivedOut(this);
		Transform ripple = UnityEngine.Object.Instantiate(Resources.Load<Transform>("CFXM_Ripple"));
		Vector3 ripplePos = base.Visual.MovingPivot.position;
		ripplePos.z += 0.15f;
		ripple.position = ripplePos;
		Sequence sequence = DOTween.Sequence();
		Transform t = base.Visual.MovingPivot;
		sequence.Append(t.DOScale(new Vector3(currentLocalScale.x * 0.7f * 0.05f, currentLocalScale.y * 1.2f * 0.05f, currentLocalScale.z), 0.05f));
		sequence.Append(t.DOScale(new Vector3(currentLocalScale.x * 0.7f, currentLocalScale.y * 1.2f, currentLocalScale.z), 0.2f));
		sequence.Append(t.DOScale(currentLocalScale, 0.08f));
		yield return sequence.WaitForCompletion();
		if (_underwaterShadowGO != null)
		{
			_underwaterShadowGO.SetActive( false);
		}
		SetPosition(base.CurrentCell.ToMapPos());
		base.Visual.MovingPivot.localScale = currentLocalScale;
		base.Visual.ShowShadow();
		base.Visual.ShowUI();
	}

	public override bool IsFireballAttack()
	{
		return _config.WeaponType == WeaponType.Fireball;
	}

	public override bool IsChest()
	{
		return _config.IsChest();
	}

	public override bool IsLockedChest()
	{
		return _config.IsLockedChest();
	}

	private bool IsGhost()
	{
		return _config.Id == "ghost";
	}

	public bool IsSpider()
	{
		return _config.Id == "spider";
	}

	public bool IsTheEye()
	{
		return _config.Id == "eye";
	}

	public bool IsSalamander()
	{
		return _config.Id == "salamander";
	}

	public bool IsLeech()
	{
		return _config.Id == "leech";
	}

	public bool IsFrog()
	{
		return _config.Id == "frog";
	}
}