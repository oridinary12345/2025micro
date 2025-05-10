using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public abstract class Character : MonoBehaviour
{
	public const float DamagedAnimDuration = 0.4f;

	public const float AfterAttackMovingDuration = 0.25f;

	public const float MinZOffset = 3f;

	public CharacterStateMachine FSM;

	private Vector3 _startingPos;

	private Vector2 _attackingTargetPos;

	private Character _lastCharacterTarget;

	private CharacterEvents _characterEvents;

	protected Vector2 _lastAttackByPos;

	private WeaponData _lastDamageTakenBy;

	private int _lastDamageReceived;

	private float _jumpingMovingSpeed;

	private int _comboDamageBonus;

	private Vector2 _movementDirection;

	private CharacterVisual _visual;

	private MovePatternState _movePatternState;

	private int _currentAttackAttemptCount;

	private Vector3 _lastJumpPosition = Vector3.zero;

	private bool _isCounterAttacking;

	private float _lastDistanceFromTarget = float.MaxValue;

	private Sequence _jumpingAnimation;

	private Tweener _landingScaleTweener;

	public const float ProjectileVisibilityDurationDefault = 0.2f;

	private bool _wasGoingBackward;

	private Cell _incomingCell;

	private List<MoveDirection> _directionToProcess;

	public Cell _lastCellMovedOn;

	public MoveDirection _lastMoveDirection;

	private Tweener _scaleToHideAndKillTweener;

	private Tweener _pushBackTweener;

	private const float _pushBackAnimDurationPerCell = 0.15f;

	private int _petrifiedTurnRemaining;

	private bool _isPetrified;

	protected bool _isPreMoveOver;

	protected bool _isPostMoveOver;

	public Transform FXPivot => _visual.FXPivot;

	public abstract string Id
	{
		get;
	}

	public abstract string Name
	{
		get;
	}

	public abstract float AttackMovementDuration
	{
		get;
	}

	public abstract float RunningSpeed
	{
		get;
	}

	public abstract float MissRate
	{
		get;
	}

	public virtual int HP
	{
		get;
		protected set;
	}

	public virtual int HPMax
	{
		get;
	}

	public float HP01 => (float)HP / (float)HPMax;

	public virtual float AttackPrecision
	{
		get;
	}

	public virtual int TokenValue => 0;

	public virtual int DamageToWeapon => 1;

	public virtual bool CanAttack => true;

	public virtual bool CanLevelUp => true;

	public virtual bool CanJump => false;

	public virtual bool IsStatic => false;

	public virtual bool IsRangedAttack
	{
		get;
	}

	public virtual bool IsMeleeAttack
	{
		get;
	}

	public virtual MovePattern MovePattern
	{
		get;
		protected set;
	}

	public virtual int AttackAttemptCountMax => int.MaxValue;

	public Character CurrentAttackTarget
	{
		get;
		private set;
	}

	public List<Character> PendingTargets
	{
		get;
		private set;
	}

	public bool IsIdle
	{
		get;
		set;
	}

	public bool IsMoving
	{
		get;
		set;
	}

	public bool IsAttacking
	{
		get;
		set;
	}

	public bool IsReceivingDamage
	{
		get;
		set;
	}

	public bool IsReceivingCriticalDamage
	{
		get;
		set;
	}

	public bool IsDamageResistant
	{
		get;
		set;
	}

	public bool HasTargetPos
	{
		get
		{
			Vector3 moveToPos = MoveToPos;
			return moveToPos.x != 1000f;
		}
	}

	public List<MoveDirection> CurrentMoveDirection => MovePattern.GetDirections(_movePatternState == MovePatternState.GoingForward);

	public MovePatternState MovePatternState => _movePatternState;

	public bool IsWaitingToAttack => _movePatternState == MovePatternState.WaitingToAttack;

	public bool HasFinishedLevel
	{
		get;
		private set;
	}

	public Vector3 MoveToPos
	{
		get;
		private set;
	}

	public bool Surprised
	{
		get;
		set;
	}

	public string WeaponName => GetWeaponSpriteName();

	public CharacterVisual Visual => _visual;

	public Cell CurrentCell
	{
		get;
		private set;
	}

	public Cell CurrentMovingCell => _lastCellMovedOn;

	public int TotalLifeTurn
	{
		get;
		private set;
	}

	public CharacterEvents Events => _characterEvents;

	public CharacterAttackingPhase AttackingPhase
	{
		get;
		private set;
	}

	protected abstract int ComputeDamage();

	public abstract Element GetCharacterElement();

	public abstract Element GetWeaponElementWeak();

	public abstract Element GetWeaponElementCritical();

	public abstract Element GetWeaponElementMiss();

	protected Character Init(CharacterVisual characterVisual, CharacterEvents characterEvents)
	{
		_characterEvents = characterEvents;
		_visual = characterVisual;
		PendingTargets = new List<Character>();
		Surprised = false;
		TotalLifeTurn = 0;
		if (IsHero())
		{
			base.gameObject.tag = "Hero";
			_visual.Collider.size = new Vector2(1f, 1f);
			UpdateWeaponSprite();
		}
		else
		{
			_visual.HideWeapon();
			base.gameObject.tag = "Monster";
		}
		FSM = new CharacterStateMachine(this);
		Reset();
		return this;
	}

	private void DecreaseHP(int damage)
	{
		HP = Mathf.Max(0, HP - damage);
	}

	protected virtual void OnAttack(int damageToWeapon)
	{
	}

	protected virtual string GetWeaponSpriteName()
	{
		return string.Empty;
	}

	public virtual List<WeaponData> GetWeapons()
	{
		return new List<WeaponData>();
	}

	public virtual List<WeaponData> GetWeaponItems()
	{
		return new List<WeaponData>();
	}

	public virtual WeaponData GetCurrentWeapon()
	{
		return null;
	}

	public virtual WeaponData GetWeapon(string weaponId)
	{
		return null;
	}

	public virtual WeaponData GetWeaponItem(string weaponId)
	{
		return null;
	}

	protected virtual void UpdateWeapons(List<WeaponData> weapons)
	{
	}

	public void SetComboDamageBonus(int comboCount)
	{
		_comboDamageBonus = comboCount;
	}

	public int GetComboDamageBonus()
	{
		return _comboDamageBonus;
	}

	public void UpdateEquippedWeapons(List<WeaponData> equippedWeapons)
	{
		if (equippedWeapons == null || equippedWeapons.Count == 0)
		{
			Debug.LogWarning("No weapons available in UpdateEquippedWeapons");
			UpdateWeapons(new List<WeaponData>());
			return;
		}

		WeaponData currentWeapon = GetCurrentWeapon();
		string currentWeaponId = currentWeapon != null ? currentWeapon.Id : null;

		UpdateWeapons(equippedWeapons);

		// 如果当前武器不在新的武器列表中，切换到第一个可用武器
		if (string.IsNullOrEmpty(currentWeaponId) || equippedWeapons.Find((WeaponData w) => w.Id == currentWeaponId) == null)
		{
			WeaponData weaponData = equippedWeapons[0];
			_visual.SetWeapon(weaponData.GetSpriteName(), weaponData.IsRangedAttack(), weaponData.Broken);
			if (IsIdle)
			{
				Play(GetIdleAnimId());
			}
		}
	}

	protected virtual void OnDestroy()
	{
		if (_landingScaleTweener != null)
		{
			_landingScaleTweener.Kill();
			_landingScaleTweener = null;
		}
		if (_jumpingAnimation != null)
		{
			_jumpingAnimation.Kill();
			_jumpingAnimation = null;
		}
		if (_pushBackTweener != null)
		{
			_pushBackTweener.Kill();
			_pushBackTweener = null;
		}
	}

	public virtual AnimationState GetIdleAnimId()
	{
		if (IsPetrified())
		{
			return AnimationState.IdlePetrified;
		}
		if (HasWeapon() && GetCurrentWeapon().IsRangedAttack())
		{
			return AnimationState.IdleRanged;
		}
		return AnimationState.Idle;
	}

	private void UpdateWeaponSprite()
	{
		if (HasWeapon())
		{
			_visual.SetWeapon(GetWeaponSpriteName(), IsRangedAttack, GetCurrentWeapon().Broken);
		}
		if (IsIdle)
		{
			Play(GetIdleAnimId());
		}
	}

	public virtual bool IsChest()
	{
		return false;
	}

	public virtual bool IsLockedChest()
	{
		return false;
	}

	protected virtual void OnWeaponBroken(string weaponId)
	{
		UpdateWeaponSprite();
	}

	protected void OnWeaponRepaired(string weaponId, int healedAmount, bool skipFX)
	{
		if (HasWeapon() && GetCurrentWeapon().Id == weaponId)
		{
			UpdateWeaponSprite();
		}
	}

	protected void OnLevelUp(string weaponId)
	{
		if (HasWeapon() && GetCurrentWeapon().Id == weaponId)
		{
			UpdateWeaponSprite();
		}
	}

	protected void OnHeroHealed(HeroData hero, int healAmount, bool skipFx)
	{
		_characterEvents.OnHealed(this, healAmount);
		CharacterHealed message = new CharacterHealed(this, this);
		FSM.OnMessage(message);
	}

	public bool HasWeapon()
	{
		return GetCurrentWeapon() != null;
	}

	public bool IsRangedWeapon()
	{
		return HasWeapon() && GetCurrentWeapon().IsRangedAttack();
	}

	public bool IsMeleeWeapon()
	{
		return HasWeapon() && GetCurrentWeapon().IsMeleeAttack();
	}

	public bool IsBombWeapon()
	{
		return HasWeapon() && GetCurrentWeapon().IsBombAttack();
	}

	public bool IsKunaiWeapon()
	{
		return HasWeapon() && GetCurrentWeapon().Config.Id == "weapon09";
	}

	public virtual bool IsFireballAttack()
	{
		return false;
	}

	public void SetTag(string tag)
	{
		base.gameObject.tag = tag;
		_visual.MovingPivot.tag = tag;
	}

	public void SetCollisionLayer(int layer)
	{
		_visual.MovingPivot.gameObject.layer = layer;
	}

	public void HideTarget()
	{
		_visual.HideTarget();
	}

	private void ChangeWeapon(WeaponData weaponData)
	{
		OnWeaponChanged(weaponData);
	}

	public void OnWeaponChanged(WeaponData weapon)
	{
		OnWeaponChangedImpl(weapon);
		UpdateWeaponSprite();
		_characterEvents.OnWeaponChanged(this, weapon);
	}

	protected virtual void OnWeaponChangedImpl(WeaponData data)
	{
	}

	public void Reset()
	{
		ResetMovingTarget();
	}

	public void IncreaseLifeTurn()
	{
		TotalLifeTurn++;
	}

	public void OnLevelFishined()
	{
		HasFinishedLevel = true;
	}

	public bool IsHero()
	{
		return this is Hero;
	}

	public bool IsMonster()
	{
		return this is Monster;
	}

	public bool IsDead()
	{
		return HP <= 0 || _scaleToHideAndKillTweener != null;
	}

	public bool IsAnimating()
	{
		return _pushBackTweener != null || Visual.IsAnimating() || _scaleToHideAndKillTweener != null;
	}

	public void OnRewardOpened(Reward reward)
	{
		RewardWeapon rewardWeapon = reward as RewardWeapon;
		if (rewardWeapon != null)
		{
			Play(AnimationState.IdleWeaponFound);
		}
		else
		{
			Play(AnimationState.IdleWeaponFound);
		}
	}

	public void SetPosition(Vector2 pos)
	{
		if (_jumpingAnimation == null || !IsKunaiWeapon())
		{
			_visual.MovingPivot.position = new Vector3(pos.x, pos.y, pos.y * 2f - 3f);
		}
	}

	public Vector3 GetLastJumpPosition()
	{
		return _lastJumpPosition;
	}

	public Vector3 GetPosition()
	{
		return _visual.MovingPivot.position;
	}

	public Vector2 GetPositionXY()
	{
		return GetPosition().XY();
	}

	public void Play(AnimationState anim)
	{
		if (HasWeapon() && !IsPetrified())
		{
			anim = GetOverrideAnim(anim.ToString() + GetCurrentWeapon().Config.Id.ToUpperFirst(), anim);
		}
		if (anim != AnimationState.JumpPeakReached)
		{
			_visual.Play(anim.ToString());
		}
	}

	private AnimationState GetOverrideAnim(string animName, AnimationState defautValue)
	{
		AnimationState animationState = Utils.Enum.TryParse(animName, AnimationState.Invalid);
		if (animationState != AnimationState.Invalid)
		{
			return animationState;
		}
		return defautValue;
	}

	public void OnMoving()
	{
		Vector2 position = GetPositionXY() + _movementDirection * RunningSpeed * UnityEngine.Time.deltaTime;
		if (!IsKunaiWeapon())
		{
			SetPosition(position);
		}
	}

	public void SetCurrentTarget(Character monster)
	{
		if (monster == null)
		{
			UnityEngine.Debug.LogWarning("Never use SetCurrentTarget with a NULL monster. Use ResetAttackTarget instead");
		}
		CurrentAttackTarget = monster;
		_lastCharacterTarget = CurrentAttackTarget;
	}

	public void SetNextAttackAsCounter()
	{
		_isCounterAttacking = true;
	}

	public bool IsCounterAttacking()
	{
		return _isCounterAttacking;
	}

	public void SetAttackTargets(List<Character> monsters)
	{
		PendingTargets = monsters;
		_isCounterAttacking = false;
	}

	public void SetNextPendingTarget(Character character)
	{
		PendingTargets.Insert(0, character);
	}

	public void StartAttack()
	{
		_startingPos = _visual.MovingPivot.position;
		AttackBeginPhase(PendingTargets[0]);
	}

	private void AttackBeginPhase(Character target)
	{
		PendingTargets.Remove(target);
		if (target.IsDead())
		{
			HandleAttackState();
			return;
		}
		if (_lastCharacterTarget != target)
		{
			_isCounterAttacking = false;
		}
		else
		{
			Monster monster = target as Monster;
			if (_lastCharacterTarget != null && monster != null && monster.HasTeleported())
			{
				SetNextAttackAsCounter();
			}
		}
		SetCurrentTarget(target);
		_attackingTargetPos = CurrentAttackTarget.GetPosition().XY();
		_movementDirection = (_attackingTargetPos - GetPosition().XY()).normalized;
		Vector2 vector = CurrentAttackTarget.GetPosition().XY() - GetPosition().XY();
		if (vector.magnitude >= 0.45f || Mathf.Approximately(vector.magnitude, 0.45f))
		{
			_attackingTargetPos += -_movementDirection;
		}
		if (Vector2.Distance(CurrentAttackTarget.GetPosition().XY(), GetPosition().XY()) <= 0.45f)
		{
			UnityEngine.Debug.LogWarning("Target is really close! Handle this, Character shouldn't be moving... distance is = " + Vector2.Distance(CurrentAttackTarget.GetPosition().XY(), GetPosition().XY()));
			_movementDirection = Vector3.zero;
		}
		LookAt(target.GetPosition());
		FSM.OnMessage(new CharacterAttackBegin(this));
	}

	public void HandleAttackState()
	{
		if (PendingTargets != null && PendingTargets.Count > 0)
		{
			AttackBeginPhase(PendingTargets[0]);
			return;
		}
		SetHomeTarget();
		if (HasReachTarget())
		{
			ResetMovingTarget();
		}
		Vector3 moveToPos = MoveToPos;
		if (moveToPos.x != 1000f)
		{
			FSM.GoToState(this, StateMoving.Instance);
		}
		else if (!IsIdle)
		{
			FSM.GoToState(this, StateIdle.Instance);
			if (CurrentCell != null && CurrentCell.Back() != null)
			{
				LookAt(CurrentCell.Back().ToMapPos());
			}
		}
	}

	public bool HasReachTarget()
	{
		float num = Vector2.Distance(GetPositionXY(), MoveToPos.XY());
		if (num > _lastDistanceFromTarget)
		{
			return true;
		}
		_lastDistanceFromTarget = num;
		return _lastDistanceFromTarget <= 0.12f;
	}

	public void OnTargetReached()
	{
		_characterEvents.OnTargetReached();
	}

	public void SetPositionAtTarget()
	{
		SetPosition(MoveToPos);
	}

	public void OnAttacking()
	{
		_lastJumpPosition = GetPosition();
		if (IsFireballAttack())
		{
			_jumpingAnimation = DOTween.Sequence();
			_jumpingAnimation.SetDelay(0.3f).OnComplete(OnJumpingCompleted).Play();
		}
		else if (Id == "spider" || Id == "leech")
		{
			AttackingPhase.MoveTowardOpponent();
		}
		else if (Id == "mushroomToxic")
		{
			_jumpingAnimation = DOTween.Sequence();
			_jumpingAnimation.SetDelay(0.1f).OnComplete(OnJumpingCompleted).Play();
		}
		else if (Id == "eye")
		{
			_visual.HideExclamationMark();
			Vector3 localScale = _visual.MovingPivot.localScale;
			RectTransform hpBar = _visual.MovingPivot.GetComponentInChildren<UIStatusBarCharacter>().GetComponent<RectTransform>();
			hpBar.SetParent(null, true);
			_jumpingAnimation = DOTween.Sequence();
			_jumpingAnimation.Append(_visual.MovingPivot.DOScale(localScale * 1.25f, 0.15f));
			_jumpingAnimation.Append(_visual.MovingPivot.DOScale(localScale, 0.5f).SetEase(Ease.OutBounce));
			_jumpingAnimation.SetDelay(0.15f).OnComplete(delegate
			{
				hpBar.SetParent(_visual.MovingPivot, true);
				OnJumpingCompleted();
			}).Play();
		}
		else if (!HasWeapon() || GetCurrentWeapon().IsMeleeAttack() || IsKunaiWeapon())
		{
			Vector3 position = _visual.JumpingPivot.position;
			float y = position.y;
			_jumpingAnimation = DOTween.Sequence();
			float attackMovementDuration = GetAttackMovementDuration();
			_jumpingMovingSpeed = Vector2.Distance(_attackingTargetPos, GetPosition().XY()) / attackMovementDuration;
			_characterEvents.OnAttackMovementStarted(this);
			float duration = attackMovementDuration * 0.5f;
			_jumpingAnimation.Append(_visual.JumpingPivot.DOMoveY(y + GetJumpingOffsetY(), duration).SetEase(Ease.OutCirc).OnUpdate(JumpingMoveXY)
				.OnComplete(OnJumpingPeakReached)
				.SetUpdate( true));
			_jumpingAnimation.Append(_visual.JumpingPivot.DOMoveY(y, duration).SetEase(Ease.InCirc).OnStart(OnJumpLandingStarted)
				.OnComplete(OnJumpingCompleted)
				.OnUpdate(JumpingMoveXY)
				.SetUpdate( true)
				.SetDelay((!CanAttackJumpingMidAir()) ? 0f : 0.4f));
			_jumpingAnimation.SetUpdate( true);
			_jumpingAnimation.Play();
		}
		else
		{
			_jumpingAnimation = DOTween.Sequence();
			_jumpingAnimation.SetDelay(0.1f).OnComplete(OnJumpingCompleted).Play();
		}
	}

	private bool CanAttackJumpingMidAir()
	{
		return IsKunaiWeapon();
	}

	private float GetJumpingOffsetY()
	{
		if (IsKunaiWeapon())
		{
			return 0.4f;
		}
		return (!CanJump) ? 0f : 0.2f;
	}

	private float GetAttackMovementDuration()
	{
		return AttackMovementDuration;
	}

	private void JumpingMoveXY()
	{
		if (!IsKunaiWeapon())
		{
			Vector3 v = GetPosition() + new Vector3(_movementDirection.x, _movementDirection.y, 0f) * _jumpingMovingSpeed * UnityEngine.Time.deltaTime;
			SetPosition(v);
		}
	}

	private void OnJumpLandingStarted()
	{
		if (CanAttackJumpingMidAir())
		{
			Play(AnimationState.Landing);
		}
	}

	private void OnJumpingPeakReached()
	{
		if (CurrentAttackTarget != null)
		{
			CurrentAttackTarget.OnIncomingAttack(this);
			if (IsKunaiWeapon())
			{
				foreach (Character pendingTarget in PendingTargets)
				{
					if (CurrentAttackTarget != pendingTarget)
					{
						pendingTarget.OnIncomingAttack(this);
					}
				}
			}
		}
		if (CanAttackJumpingMidAir())
		{
			AttackEndPhase();
		}
	}

	private void OnJumpingCompleted()
	{
		_jumpingAnimation = null;
		if (!CanAttackJumpingMidAir())
		{
			AttackEndPhase();
		}
	}

	protected virtual void OnIncomingAttack(Character attacker)
	{
	}

	public int GetDamage()
	{
		float num = 1f + (float)Mathf.Min(_comboDamageBonus, 10) * 0.25f;
		return Mathf.CeilToInt((float)ComputeDamage() * num);
	}

	public void SetAttackingPhase(CharacterAttackingPhase attackingPhase)
	{
		if (AttackingPhase != null)
		{
			UnityEngine.Object.Destroy(AttackingPhase);
		}
		AttackingPhase = attackingPhase;
	}

	private void AttackEndPhase()
	{
		if (AttackingPhase == null)
		{
			AttackEndPhaseOldCode();
		}
		else
		{
			AttackingPhase.StartAttackEndPhase();
		}
	}

	private void AttackEndPhaseOldCode()
	{
		UnityEngine.Debug.LogWarning("AttackEndPhaseOldCode(). Something went wrong since we should eventually delete this...");
		if (!(CurrentAttackTarget == null))
		{
			int damage = GetDamage();
			Play(GetAttackAnimation());
			Action action = delegate
			{
				if (CurrentAttackTarget != null)
				{
					if (!IsFireballAttack() && Id != "spider")
					{
						CurrentAttackTarget.TakeDamage(this, GetCurrentWeapon(), damage, GetLastJumpPosition());
					}
					int damageToWeapon = (_comboDamageBonus <= 1) ? CurrentAttackTarget.DamageToWeapon : 0;
					WeaponTakingDamage(damageToWeapon);
					PlayAttackLandingAnimation(OnAttackFinished);
					if (!IsFireballAttack() || _wasGoingBackward)
					{
						SetMovingBackward();
					}
					else
					{
						SetMovingForward();
					}
					ResetAttackAttemptCount();
				}
				else
				{
					UnityEngine.Debug.LogWarning("CurrentAttackTarget is null in AttackEndPhaseOldCode()");
				}
			};
			float delay = (!IsRangedWeapon()) ? 0f : 0.25f;
			this.Execute(delay, action);
			MaybeThrowProjectiles(damage, CurrentAttackTarget, CurrentAttackTarget.GetPosition(), 0.2f);
		}
	}

	public void PlayAttackLandingAnimation(Action onLandingCompleted)
	{
		float duration = (!IsFireballAttack()) ? 0.4f : 1.3f;
		Vector3 localScale = _visual.MovingPivot.localScale;
		Transform movingPivot = _visual.MovingPivot;
		Vector3 localScale2 = _visual.MovingPivot.localScale;
		float x = localScale2.x * 1.15f;
		Vector3 localScale3 = _visual.MovingPivot.localScale;
		float y = localScale3.y * 0.85f;
		Vector3 localScale4 = _visual.MovingPivot.localScale;
		movingPivot.localScale = new Vector3(x, y, localScale4.z);
		_landingScaleTweener = _visual.MovingPivot.DOScale(localScale, duration).OnComplete(delegate
		{
			onLandingCompleted();
		}).SetUpdate( true);
	}

	public void MaybeThrowProjectiles(int damage, Character targetCharacter, Vector3 targetPos, float groundDuration)
	{
		GameObject projectile = _visual.GetWeaponProjectile(GetWeaponSpriteName());
		if (projectile == null && IsFireballAttack())
		{
			Transform transform = Firebreath.Create(this, damage, targetCharacter).transform;
			transform.parent = _visual.MovingPivot;
			Events.OnFirebreath();
		}
		if (projectile != null)
		{
			projectile.transform.position = GetPosition();
			projectile.SetActive( false);
			Action action = delegate
			{
				Vector3 startPos = GetPosition();
				projectile.SetActive( true);
				projectile.transform.DOMove(targetPos, 0.1f).OnComplete(delegate
				{
					Events.OnProjectileLanded(startPos, targetPos, GetCurrentWeapon(), groundDuration);
					this.Execute(0.05f, delegate
					{
						UnityEngine.Object.Destroy(projectile);
					});
				});
			};
			this.Execute(0.05f, action);
		}
	}

	public void WeaponTakingDamage(int damageToWeapon)
	{
		OnAttack(damageToWeapon);
	}

	public AnimationState GetAttackAnimation()
	{
		if (IsFireballAttack())
		{
			return AnimationState.AttackFire;
		}
		if (!HasWeapon() || IsMeleeWeapon())
		{
			return AnimationState.AttackMelee;
		}
		if (IsRangedWeapon())
		{
			return AnimationState.AttackRanged;
		}
		UnityEngine.Debug.LogWarning("Attack animation is missing for this kind of weapons");
		return AnimationState.Idle;
	}

	public bool WasGoingBackward()
	{
		return _wasGoingBackward;
	}

	public void SetWaitingToAttack()
	{
		if (!IsHero())
		{
			_wasGoingBackward = (_movePatternState == MovePatternState.GoingBackward);
			_currentAttackAttemptCount++;
			if (_currentAttackAttemptCount > AttackAttemptCountMax)
			{
				UnityEngine.Debug.LogWarningFormat("SetWaitingToAttack() - {0}: _currentAttackAttemptCount ({1}) should NEVER be bigger than AttackAttemptCountMax ({2})", base.name, _currentAttackAttemptCount, AttackAttemptCountMax);
			}
			if (!IsFireballAttack() && Id != "spider" && Id != "mushroomToxic" && Id != "eye" && _directionToProcess != null)
			{
				_directionToProcess.Clear();
			}
			SetMovePatternState(MovePatternState.WaitingToAttack);
		}
	}

	public void ResetAttackAttemptCount()
	{
		_currentAttackAttemptCount = 0;
	}

	public void SetMovingForward()
	{
		if (_movePatternState == MovePatternState.GoingForward)
		{
		}
		if (!IsHero())
		{
			if (_directionToProcess != null)
			{
				_directionToProcess.Clear();
			}
			_wasGoingBackward = false;
			SetMovePatternState(MovePatternState.GoingForward);
		}
	}

	public void SetMovingBackward()
	{
		if (!IsHero())
		{
			if (_directionToProcess != null)
			{
				_directionToProcess.Clear();
			}
			if (_movePatternState == MovePatternState.GoingBackward)
			{
				UnityEngine.Debug.LogWarning("Monster was already going backard... This shouldn't be called twice");
			}
			SetMovePatternState(MovePatternState.GoingBackward);
		}
	}

	private void SetMovePatternState(MovePatternState state)
	{
		_movePatternState = state;
		bool flag = _movePatternState == MovePatternState.WaitingToAttack;
		if (flag)
		{
			_characterEvents.OnWaitingToAttack(this);
		}
		_visual.SetWaitingToAttack(flag);
	}

	public bool HasReachedAttackAttemptMax()
	{
		if (_currentAttackAttemptCount > AttackAttemptCountMax)
		{
			UnityEngine.Debug.LogWarningFormat("{0}: _currentAttackAttemptCount ({1}) should NEVER be bigger than AttackAttemptCountMax ({2})", base.name, _currentAttackAttemptCount, AttackAttemptCountMax);
		}
		return _currentAttackAttemptCount >= AttackAttemptCountMax;
	}

	public void SetCurrentCell(Cell newCell)
	{
		_lastCellMovedOn = newCell;
		if (CurrentCell == newCell)
		{
			UnityEngine.Debug.LogFormat("Character ({0}) is already on {1}", base.name, newCell);
			return;
		}
		if (CurrentCell != null)
		{
			CurrentCell.RemoveCharacter(this);
		}
		CurrentCell = newCell;
		newCell.AddCharacter(this);
	}

	public void SetIncomingCell(Cell incomingCell)
	{
		if (incomingCell == null)
		{
		}
		_incomingCell = incomingCell;
	}

	public Cell GetIncomingCell()
	{
		return _incomingCell;
	}

	public void SetHomeTarget()
	{
		SetTargetPos(_startingPos);
	}

	private void SetTargetPos(Vector3 pos)
	{
		MoveToPos = pos;
		_lastDistanceFromTarget = float.MaxValue;
		_movementDirection = (MoveToPos.XY() - GetPosition().XY()).normalized;
		if (GetPosition() == pos || Mathf.Approximately(Vector2.Distance(GetPositionXY(), pos.XY()), 0f))
		{
			ResetMovingTarget();
		}
		if (_movementDirection == Vector2.zero)
		{
			ResetMovingTarget();
		}
	}

	public void MoveToPath(List<MoveDirection> directions)
	{
		_directionToProcess = new List<MoveDirection>(directions);
		_lastCellMovedOn = CurrentCell;
		if (_directionToProcess[0] == MoveDirection.Pause || _directionToProcess[0] == MoveDirection.Skip)
		{
			_directionToProcess.RemoveAt(0);
		}
		else
		{
			MaybeProcessNextMovement();
		}
	}

	public bool HasOngoingMovement()
	{
		return _directionToProcess != null && _directionToProcess.Count > 0;
	}

	public void ResumeMovementPause()
	{
		_lastCellMovedOn = CurrentCell;
		if (!HasOngoingMovement())
		{
			return;
		}
		MoveDirection moveDirection = _directionToProcess[0];
		if (moveDirection == MoveDirection.Pause)
		{
			_directionToProcess.RemoveAt(0);
			return;
		}
		if (_incomingCell == null)
		{
			Cell cell = CurrentCell.FindCell(_directionToProcess);
			if (cell == CurrentCell || cell == null)
			{
				_directionToProcess.Clear();
				return;
			}
			if (cell.IsOccupied())
			{
				return;
			}
			cell.AddIncoming(this);
		}
		MaybeProcessNextMovement();
	}

	public bool MaybeProcessNextMovement()
	{
		if (_directionToProcess != null)
		{
			if (_directionToProcess.Count > 0)
			{
				MoveDirection moveDirection = _directionToProcess[0];
				switch (moveDirection)
				{
				case MoveDirection.Pause:
					SetCurrentCell(_lastCellMovedOn);
					return false;
				case MoveDirection.Skip:
					_directionToProcess.RemoveAt(0);
					SetCurrentCell(_lastCellMovedOn);
					return false;
				}
				Cell cellFrom = _lastCellMovedOn.GetCellFrom(moveDirection);
				if (cellFrom != null && cellFrom != _lastCellMovedOn)
				{
					_directionToProcess.RemoveAt(0);
					Vector3 vector = cellFrom.ToMapPos();
					_lastCellMovedOn = cellFrom;
					_lastMoveDirection = moveDirection;
					if (_directionToProcess.Count == 0)
					{
						SetCurrentCell(cellFrom);
					}
					float x = vector.x;
					float y = vector.y;
					Vector3 position = GetPosition();
					SetTargetPos(new Vector3(x, y, position.z));
					LookAt(MoveToPos);
					return true;
				}
				SetCurrentCell(_lastCellMovedOn);
			}
			_directionToProcess.Clear();
		}
		if (_lastCellMovedOn != CurrentCell && _lastCellMovedOn != null)
		{
			UnityEngine.Debug.LogWarning("Problem #1: Current CELL isn't properly set for monster: " + base.name);
			if (_lastCellMovedOn.IsOccupied())
			{
				UnityEngine.Debug.LogWarning("Problem #2: _lastCellMovedOn is already Occupied");
			}
			else
			{
				SetCurrentCell(_lastCellMovedOn);
			}
		}
		return false;
	}

	public void LookAt(Vector3 lookAtPos)
	{
		float x = lookAtPos.x;
		Vector3 position = GetPosition();
		float num = Mathf.Sign(x - position.x);
		Vector3 localScale = _visual.MovingPivot.localScale;
		localScale = new Vector3(num * Mathf.Abs(localScale.x), localScale.y, localScale.z);
		SetLocalScale(localScale);
	}

	public void SetLocalScale(Vector3 scale)
	{
		_visual.MovingPivot.localScale = scale;
		if (_visual.MovingPivot.gameObject.GetChild("BarMonsterCanvas(Clone)") != null)
		{
			RectTransform component = _visual.MovingPivot.gameObject.GetChild("BarMonsterCanvas(Clone)").transform.GetComponent<RectTransform>();
			RectTransform rectTransform = component;
			float num = Mathf.Sign(scale.x);
			Vector3 localScale = component.localScale;
			float x = num * Mathf.Abs(localScale.x);
			Vector3 localScale2 = component.localScale;
			float y = localScale2.y;
			Vector3 localScale3 = component.localScale;
			rectTransform.localScale = new Vector3(x, y, localScale3.z);
		}
	}

	public void OnAttackFinished()
	{
		_landingScaleTweener = null;
		ResetAttackTarget();
	}

	public void ResetMovingTarget()
	{
		MoveToPos = new Vector3(1000f, 1000f, 1000f);
		_movementDirection = Vector2.zero;
	}

	private void ResetAttackTarget()
	{
		CurrentAttackTarget = null;
	}

	public void ResetMovementPattern()
	{
		if (_directionToProcess != null)
		{
			_directionToProcess.Clear();
		}
	}

	private bool IsWeaponWeakAgainstMe(WeaponData weapon)
	{
		return weapon != null && weapon.Config.ElementWeak != 0 && weapon.Config.ElementWeak == GetCharacterElement();
	}

	private bool IsWeaponCriticalAgainstMe(WeaponData weapon)
	{
		return weapon != null && weapon.Config.ElementWeak != 0 && weapon.Config.ElementCritical == GetCharacterElement();
	}

	private bool IsWeaponMissAgainstMe(WeaponData weapon)
	{
		return weapon != null && weapon.Config.ElementWeak != 0 && weapon.Config.ElementMiss == GetCharacterElement();
	}

	public void Heal(int healAmount)
	{
		if (healAmount > 0)
		{
			int num = Mathf.Min(HPMax - HP, healAmount);
			HP += num;
			_characterEvents.OnHealed(this, num);
		}
	}

	public int TakeDamage(Character attacker, WeaponData weapon, int damage, Vector2 contactPoint)
	{
		float num = (!IsWeaponMissAgainstMe(weapon)) ? 0f : 0.5f;
		bool flag = this is Monster && attacker is Monster;
		if (attacker != null && attacker.Id == "salamander")
		{
			num = 0.17f;
		}
		_characterEvents.OnAttackThrown(attacker, this);
		if (flag || attacker == null || IsPetrified() || IsStatic || UnityEngine.Random.value > MissRate + num)
		{
			IsReceivingCriticalDamage = false;
			IsDamageResistant = IsWeaponWeakAgainstMe(weapon);
			if (weapon != null && attacker != null && !IsDamageResistant)
			{
				IsReceivingCriticalDamage = (IsWeaponCriticalAgainstMe(weapon) || UnityEngine.Random.value <= weapon.Config.CriticalChances);
			}
			if (IsDamageResistant)
			{
				damage = Mathf.RoundToInt((float)damage * 0.5f);
			}
			else if (IsReceivingCriticalDamage)
			{
				damage *= 2;
			}
			if (IsLockedChest())
			{
				damage = 0;
			}
			_lastAttackByPos = contactPoint;
			_lastDamageTakenBy = weapon;
			_lastDamageReceived = damage;
			DecreaseHP(damage);
			CharacterDamageTaken message = new CharacterDamageTaken(attacker, this);
			FSM.OnMessage(message);
			_characterEvents.OnDamaged(attacker, this, weapon, damage);
			if (IsDead())
			{
				RemoveFromMap();
			}
		}
		else
		{
			Play(AnimationState.Dodge);
			Vector3 vector = (!(attacker != null)) ? Vector3.zero : attacker.GetLastJumpPosition();
			Vector3 pos = GetPosition();
			Vector2 vector2 = (GetPosition().XY() - vector.XY()).normalized * 0.5f;
			Vector3 endValue = pos + new Vector3(vector2.x, vector2.y, 0f);
			_visual.MovingPivot.DOMove(endValue, 0.18f).SetEase(Ease.OutCubic).SetUpdate( true);
			_visual.MovingPivot.DOMove(pos, 0.18f).SetDelay(0.18f).SetEase(Ease.Linear)
				.SetUpdate( true)
				.OnComplete(delegate
				{
					_visual.MovingPivot.position = pos;
					Play(AnimationState.Idle);
				});
			Vector3 position = GetPosition();
			position.y += base.transform.GetHeight() * 0.75f;
			UIStatusBarCharacter componentInChildren = Visual.transform.GetComponentInChildren<UIStatusBarCharacter>();
			if (componentInChildren != null)
			{
				Vector3 position2 = componentInChildren.transform.position;
				position.y = position2.y;
			}
			_characterEvents.OnDodged(attacker, this, position);
			damage = 0;
		}
		return damage;
	}

	private void RemoveFromMap()
	{
		if (CurrentCell != null)
		{
			CurrentCell.RemoveCharacter(this);
			CurrentCell = null;
		}
		if (_incomingCell != null)
		{
			_incomingCell.ClearIncoming();
			_incomingCell = null;
		}
	}

	public void ScaleToHideAndKill(float delay = 0f)
	{
		_scaleToHideAndKillTweener = _visual.MovingPivot.DOScale(Vector3.zero, 0.2f).SetDelay(delay).SetEase(Ease.InBack)
			.OnComplete(delegate
			{
				Events.OnScaleToDead(this);
				Kill();
				_scaleToHideAndKillTweener = null;
			});
	}

	private void Kill()
	{
		DecreaseHP(HP);
		RemoveFromMap();
	}

	private void Update()
	{
		if (FSM != null)
		{
			FSM.Update(this);
		}
	}

	public void OnDamageTaken(int layerCount = 0)
	{
		if (IsChest())
		{
			Cell currentCell = CurrentCell;
			if (currentCell.IsLastLayer())
			{
				return;
			}
			currentCell = GetBackCell(currentCell);
			if (currentCell == null)
			{
				UnityEngine.Debug.LogWarning("OnDamageTaken() fail to found a proper backCell for CHEST...");
				return;
			}
			Vector3 endValue = currentCell.ToMapPos();
			if (_pushBackTweener != null)
			{
				_pushBackTweener.Complete( true);
				_pushBackTweener = null;
			}
			_pushBackTweener = _visual.MovingPivot.DOMove(endValue, 0.15f).SetEase(Ease.Linear).SetUpdate( true)
				.OnUpdate(OnUpdatePushBack)
				.OnComplete(delegate
				{
					_pushBackTweener = null;
				})
				.SetLoops(2, LoopType.Yoyo);
		}
		if (layerCount == 0 && _lastDamageTakenBy != null && !IsChest())
		{
			layerCount = _lastDamageTakenBy.PushBackForce;
		}
		if (IsHero() || IsDead() || CurrentCell == null)
		{
			return;
		}
		if ((float)layerCount > 0f)
		{
			Cell cell = (CurrentCell == _lastCellMovedOn || _lastCellMovedOn == null) ? CurrentCell : _lastCellMovedOn;
			int i;
			for (i = 0; i < layerCount; i++)
			{
				if (cell != null)
				{
					if (cell.IsLastLayer())
					{
						break;
					}
					cell = GetBackCell(cell);
					if (cell.IsOccupied() && cell.OccupiedBy != this)
					{
						break;
					}
				}
			}
			if (CurrentCell != _lastCellMovedOn && _lastCellMovedOn != null)
			{
				SetCurrentCell(_lastCellMovedOn);
			}
			if (cell != null)
			{
				float animDurationSec = 0.15f * (float)i;
				PushBack(cell.ToMapPos(), cell, null, animDurationSec);
			}
			else
			{
				UnityEngine.Debug.LogWarning("OnDamageTaken() fail to found a proper backCell...");
			}
		}
		if (IsWaitingToAttack && HasReachedAttackAttemptMax())
		{
			if ((!IsFireballAttack() && Id != "spider" && Id != "mushroomToxic" && Id != "eye") || _wasGoingBackward)
			{
				SetMovingBackward();
			}
			else
			{
				SetMovingForward();
			}
			ResetAttackAttemptCount();
		}
	}

	private Cell GetBackCell(Cell backCell)
	{
		if (CurrentCell != _lastCellMovedOn && _lastCellMovedOn != null)
		{
			switch (_lastMoveDirection)
			{
			case MoveDirection.Left:
				return backCell.Right();
			case MoveDirection.Right:
				return backCell.Left();
			}
		}
		return backCell.Back();
	}

	public virtual void OnDead()
	{
	}

	protected void PushBack(Vector3 toPos, Cell nextCell, Action onContinue, float animDurationSec)
	{
		Action onMoveToCompleted = null;
		if (nextCell != null && nextCell != CurrentCell)
		{
			Action setCurrentCell = delegate
			{
				SetCurrentCell(nextCell);
				if (_incomingCell != null)
				{
					_incomingCell.ClearIncoming();
					_incomingCell = null;
				}
				if (onContinue != null)
				{
					onContinue();
				}
			};
			if (nextCell.OccupiedBy != null)
			{
				Character characterToMove = nextCell.OccupiedBy;
				onMoveToCompleted = delegate
				{
					if (characterToMove != null)
					{
						characterToMove.TakeDamage(this, null, Mathf.RoundToInt((float)_lastDamageReceived * 0.5f), GetLastJumpPosition());
					}
					Cell cell = null;
					if (nextCell != null)
					{
						if (nextCell.IsLastLayer())
						{
							if (CurrentCell == null || !CurrentCell.IsLastLayer())
							{
								cell = ((!(UnityEngine.Random.value < 0.5f)) ? nextCell.Right() : nextCell.Left());
							}
							else if (CurrentCell.Left().OccupiedBy == characterToMove && characterToMove != null)
							{
								cell = nextCell.Left();
							}
							else if (CurrentCell.Right().OccupiedBy == characterToMove && characterToMove != null)
							{
								cell = nextCell.Right();
							}
						}
						else
						{
							cell = nextCell.Back();
						}
					}
					if (characterToMove != null && !characterToMove.IsDead() && cell != null)
					{
						if (characterToMove.IsChest())
						{
							if (onContinue != null)
							{
								onContinue();
							}
						}
						else
						{
							characterToMove.PushBack(cell.ToMapPos(), cell, setCurrentCell, 0.15f);
						}
					}
				};
			}
			else
			{
				setCurrentCell();
			}
		}
		if (_pushBackTweener != null)
		{
			_pushBackTweener.Complete( true);
			_pushBackTweener = null;
		}
		_pushBackTweener = _visual.MovingPivot.DOMove(toPos, animDurationSec).SetEase(Ease.Linear).SetUpdate( true)
			.OnUpdate(OnUpdatePushBack)
			.OnComplete(delegate
			{
				_pushBackTweener = null;
			});
		this.Execute(animDurationSec - 0.075f, delegate
		{
			if (onMoveToCompleted != null)
			{
				onMoveToCompleted();
			}
		});
	}

	private void OnUpdatePushBack()
	{
		SetPosition(GetPosition());
	}

	private void OnDrawGizmos()
	{
	}

	public void SetPetrifiedTurn(Character attacker, int turnCount)
	{
		_petrifiedTurnRemaining = turnCount;
		SetPetrified(attacker, turnCount > 0);
	}

	public int GetPetrifiedTurnRemaining()
	{
		return _petrifiedTurnRemaining;
	}

	public bool IsPetrified()
	{
		return _isPetrified;
	}

	public void Unpetrify()
	{
		SetPetrified(null, false);
		_petrifiedTurnRemaining = 0;
	}

	private void SetPetrified(Character attacker, bool isPetrified)
	{
		if (_isPetrified != isPetrified)
		{
			_isPetrified = isPetrified;
			if (_isPetrified)
			{
				Events.OnPetrificationStarted(attacker, this);
			}
			else
			{
				StartCoroutine(PetrificationCR());
			}
		}
	}

	private IEnumerator PetrificationCR()
	{
		Events.OnUnpetrificationStarted(this);
		yield return null;
		Vector3 originLocalScale = Visual.MovingPivot.localScale;
		Visual.HideShadow();
		yield return ScaleTo(Visual.MovingPivot, Vector3.zero).WaitForCompletion();
		Visual.ApplyDefaultSkin();
		Play(GetIdleAnimId());
		yield return ScaleTo(Visual.MovingPivot, originLocalScale).SetEase(Ease.InQuad).WaitForCompletion();
		SetLocalScale(originLocalScale);
		Visual.ShowShadow();
		yield return new WaitForSeconds(0.25f);
	}

	private Tweener ScaleTo(Transform t, Vector3 scaleTarget)
	{
		return t.DOScale(scaleTarget, 0.22f);
	}

	public bool IsPreMoveOver()
	{
		return _isPreMoveOver;
	}

	public void OnPreMove()
	{
		_isPreMoveOver = false;
		StartCoroutine(OnPreMoveCR());
	}

	public bool IsPostMoveOver()
	{
		return _isPostMoveOver;
	}

	public void OnPostMove()
	{
		_isPostMoveOver = false;
		StartCoroutine(OnPostMoveCR());
	}

	protected virtual IEnumerator OnPreMoveCR()
	{
		_isPreMoveOver = true;
		yield return null;
	}

	protected virtual IEnumerator OnPostMoveCR()
	{
		_isPostMoveOver = true;
		yield return null;
	}

	public void OnGameIdle()
	{
		if (_petrifiedTurnRemaining > 0)
		{
			_petrifiedTurnRemaining--;
		}
	}
}