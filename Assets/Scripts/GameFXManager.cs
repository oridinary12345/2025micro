using DG.Tweening;
using UnityEngine;

public class GameFXManager : MonoBehaviour
{
	private CommonFXManager _commonFXManager;

	private GameEvents _gameEvents;

	private AudioClip _clipDodgeAttack;

	private Camera _camera;

	private FX FX;

	private Tweener _monsterCamShakeTween;

	private Tweener _damageCamShakeTween;

	public GameFXManager Setup(CommonFXManager commonFxManager, GameEvents gameEvents, CharacterEvents characterEvents, Camera camera)
	{
		_commonFXManager = commonFxManager;
		_gameEvents = gameEvents;
		_camera = camera;
		FX = new FX();
		_gameEvents.LevelFinishedEvent += OnLevelFinished;
		_gameEvents.LootDropEvent += OnLootDrop;
		_gameEvents.LootPickupEvent += OnLootPickup;
		_gameEvents.WeaponRotationStartedEvent += OnWeaponRotationStarted;
		_gameEvents.WeaponRotationStoppedEvent += OnWeaponRotationStopped;
		_gameEvents.MonsterSpawnedEvent += OnMonsterSpawned;
		_gameEvents.MonsterMovedEvent += OnMonsterMoved;
		_gameEvents.MonsterExpulsedEvent += OnMonsterExpulsed;
		_gameEvents.BombExplosionEvent += OnBombExplosion;
		_gameEvents.BombDropEvent += OnBombDrop;
		characterEvents.ProjectileLandedEvent += OnProjectileLanded;
		characterEvents.DamagedEvent += OnDamaged;
		characterEvents.HealedEvent += OnHealed;
		characterEvents.SippedEvent += OnSipped;
		characterEvents.AttackThrownEvent += OnAttackThrown;
		characterEvents.UnpetrificationStartedEvent += OnUnpetrificationStarted;
		characterEvents.PetrificationStartedEvent += OnPetrificationStarted;
		characterEvents.PetrificationEndedEvent += OnPetrificationEnded;
		characterEvents.DodgedEvent += OnDodged;
		characterEvents.AttackMovementStartedEvent += OnAttackMovementStarted;
		characterEvents.FirebreathEvent += OnFirebreath;
		characterEvents.WaitingToAttackEvent += OnWaitingToAttack;
		characterEvents.DivedInEvent += OnDivedIn;
		characterEvents.DivedOutEvent += OnDivedOut;
		characterEvents.ScaleToDeadEvent += OnScaleToDead;
		_clipDodgeAttack = Resources.Load<AudioClip>("hitDodge");
		return this;
	}

	private void OnDestroy()
	{
		if (_monsterCamShakeTween != null)
		{
			_monsterCamShakeTween.Kill();
			_monsterCamShakeTween = null;
		}
		if (_damageCamShakeTween != null)
		{
			_damageCamShakeTween.Kill();
			_damageCamShakeTween = null;
		}
	}

	private void OnDodged(Character attacker, Character defender, Vector3 defaultPos)
	{
		FX.CreateStatusMiss(null, GetDodgeStatusPos(defender, defaultPos));
		MonoSingleton<AudioManager>.Instance.PlaySound(_clipDodgeAttack);
	}

	private Vector3 GetHitStatusPos(Character defender)
	{
		Vector3 position = defender.GetPosition();
		float y = position.y + defender.transform.GetHeight() * 0.75f;
		if (defender.IsHero())
		{
			string skinPath = CharacterVisual.GetSkinPath(defender.Id);
			CharacterSkin characterSkin = Resources.Load<CharacterSkin>(skinPath);
			y = characterSkin.StatusBarPosY + 0.25f;
		}
		else
		{
			UIStatusBarCharacter componentInChildren = defender.Visual.transform.GetComponentInChildren<UIStatusBarCharacter>();
			if (componentInChildren != null)
			{
				Vector3 position2 = componentInChildren.transform.position;
				y = position2.y + 0.42f;
			}
		}
		return new Vector3(position.x + 0.1f, y, position.z - 3f);
	}

	private Vector3 GetDodgeStatusPos(Character defender, Vector3 defaultPos)
	{
		float num = 0f;
		if (defender.IsHero())
		{
			string skinPath = CharacterVisual.GetSkinPath(defender.Id);
			CharacterSkin characterSkin = Resources.Load<CharacterSkin>(skinPath);
			num = characterSkin.StatusBarPosY + 0.25f;
		}
		else
		{
			num = defaultPos.y + 0.42f;
		}
		return new Vector3(defaultPos.x + 0.1f, num, defaultPos.z - 3f);
	}

	private void OnLootDrop(Vector3 spawningPos, string lootId)
	{
		if (lootId == "lootCoin")
		{
			FX.CreateStarFX(spawningPos);
		}
	}

	private void OnLootPickup(string lootId, bool isCard)
	{
		if (isCard)
		{
			AudioClip audioClip = _commonFXManager.PlaySoundFX("card_collect1");
			if (audioClip != null)
			{
				this.Execute(audioClip.length * 2f, delegate
				{
					_commonFXManager.PlaySoundFX("card_collect2");
				});
			}
		}
		else if (!(lootId == "lootCoin") && lootId == "lootLife")
		{
			_commonFXManager.PlaySoundFX("life");
		}
	}

	private void OnFirebreath()
	{
		_commonFXManager.PlaySoundFX("firebreath");
	}

	private void OnAttackMovementStarted(Character attacker)
	{
		if (!attacker.IsHero())
		{
			_commonFXManager.PlaySoundFX("heroAttack");
		}
	}

	private void OnWaitingToAttack(Character character)
	{
		_commonFXManager.PlaySoundFX("exclamation");
	}

	private void OnDivedIn(Character character)
	{
		_commonFXManager.PlaySoundFX("dive_in");
		Vector3 position = character.Visual.MovingPivot.position;
		FX.CreateDiveSplash(null, position);
	}

	private void OnDivedOut(Character character)
	{
		_commonFXManager.PlaySoundFX("dive_out");
		Vector3 position = character.Visual.MovingPivot.position;
		FX.CreateDiveSplash(null, position);
	}

	private void OnScaleToDead(Character character)
	{
		Transform transform = UnityEngine.Object.Instantiate(Resources.Load<Transform>("CFXM3_Hit_SmokePuff"));
		Vector3 position = character.Visual.MovingPivot.position;
		position.z += -0.1f;
		transform.position = position;
		_commonFXManager.PlaySoundFX("smoke");
	}

	private void OnLevelFinished(LevelData levelData)
	{
	}

	private void OnWeaponRotationStarted()
	{
		_commonFXManager.PlaySoundFX("rotationStart");
	}

	private void OnWeaponRotationStopped()
	{
		_commonFXManager.PlaySoundFX("rotationStop");
	}

	private void OnMonsterSpawned()
	{
		_commonFXManager.PlaySoundFX("enemySpawn");
	}

	private void OnMonsterMoved()
	{
		_commonFXManager.PlaySoundFX("enemyWalking");
	}

	private void OnMonsterExpulsed()
	{
		_monsterCamShakeTween = _camera.DOShakePosition(0.1f, 0.075f, 25, 35f).SetUpdate( true).OnKill(delegate
		{
			_monsterCamShakeTween = null;
		});
	}

	private void OnAttackThrown(Character attacker, Character defender)
	{
		if (!(attacker == null))
		{
			GameObject gameObject = FX.CreateAttackFX(attacker.FXPivot, attacker.Id);
			if (gameObject != null && attacker.Id == "salamander")
			{
				Vector3 localPosition = gameObject.transform.localPosition;
				localPosition.x = 0.4f;
				localPosition.y = -0.23f;
				localPosition.z += -5f;
				gameObject.transform.localPosition = localPosition;
			}
		}
	}

	private void OnHealed(Character healedCharacter, int healedAmount)
	{
		if (healedCharacter is Monster)
		{
			_commonFXManager.PlaySoundFX("heal");
			Vector3 position = healedCharacter.Visual.MovingPivot.position;
			FX.CreateHealing(null, position);
		}
	}

	private void OnSipped()
	{
		_commonFXManager.PlaySoundFX("sip");
	}

	private void OnDamaged(Character attacker, Character defender, WeaponData weapon, int damage)
	{
		_damageCamShakeTween = _camera.DOShakePosition(0.1f, 0.05f, 20, 30f).SetUpdate( true).OnKill(delegate
		{
			_damageCamShakeTween = null;
		});
		FX.CreateImpact(null, defender.FXPivot.position + new Vector3(0f, 0.35f, -1f), defender.IsReceivingCriticalDamage);
		if (attacker != null)
		{
			PlayDamagedGivenSoundFX(attacker, weapon);
			if (attacker.Id == "mushroomToxic")
			{
				Vector3 position = defender.Visual.MovingPivot.position;
				GameObject gameObject = FX.CreateToxicGaz(null, position);
				gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
				gameObject.transform.localScale = Vector3.one;
				Vector3 position2 = attacker.Visual.MovingPivot.position;
				GameObject gameObject2 = FX.CreateToxicGaz(null, position2 + new Vector3(-0.5f, 0.45f, 0f));
				gameObject2.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 45f));
				gameObject2.transform.localScale = Vector3.one * 0.4f;
				GameObject gameObject3 = FX.CreateToxicGaz(null, position2 + new Vector3(0.5f, 0.45f, 0f));
				gameObject3.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -45f));
				gameObject3.transform.localScale = Vector3.one * 0.4f;
			}
		}
		if (defender.IsDamageResistant)
		{
			FX.CreateStatusWeak(null, GetHitStatusPos(defender));
		}
		else if (defender.IsReceivingCriticalDamage)
		{
			FX.CreateStatusCritical(null, GetHitStatusPos(defender));
		}
		PlayDamagedReceivedSoundFX(attacker, defender, weapon);
	}

	private void OnPetrificationStarted(Character attacker, Character defender)
	{
		_commonFXManager.PlaySoundFX("ray");
		Vector3 position = defender.Visual.MovingPivot.position;
		FX.CreatePetrifyTarget(null, position);
		Vector3 position2 = attacker.Visual.MovingPivot.position;
		GameObject gameObject = FX.CreatePetrifyRay(null, position2);
		Vector3 localScale = attacker.Visual.MovingPivot.localScale;
		float num = Mathf.Sign(localScale.x);
		Vector3 localScale2 = gameObject.transform.localScale;
		localScale2.x = (0f - num) * Mathf.Abs(localScale2.x);
		gameObject.transform.localScale = localScale2;
	}

	private void OnUnpetrificationStarted(Character petrifiedCharacter)
	{
		_commonFXManager.PlaySoundFX("ray");
		Vector3 position = petrifiedCharacter.Visual.MovingPivot.position;
		FX.CreatePetrifyTarget(null, position);
	}

	private void OnPetrificationEnded(Character defender)
	{
		_commonFXManager.PlaySoundFX("stone");
	}

	private void OnProjectileLanded(Vector3 fromPos, Vector3 toPos, WeaponData weapon, float groundDuration)
	{
		float sign = Mathf.Sign(toPos.x - fromPos.x);
		FX.CreateProjectileLanded(null, toPos + new Vector3(0f, 0f, -2f), weapon, sign, groundDuration);
	}

	private void OnBombDrop()
	{
		_commonFXManager.PlaySoundFX("bomb_drop");
	}

	private void OnBombExplosion(Vector3 position)
	{
		_commonFXManager.PlaySoundFX("bomb_explosion");
		FX.CreateBombExplosion(null, position + new Vector3(0.154f, 0.684f, -1f));
	}

	private void PlayDamagedGivenSoundFX(Character attacker, WeaponData weapon)
	{
		if (attacker != null && attacker.IsHero())
		{
			string path = "attack" + weapon.Id;
			AudioClip audioClip = Resources.Load<AudioClip>(path);
			if (audioClip != null)
			{
				_commonFXManager.PlaySoundFX(audioClip);
			}
			else
			{
				_commonFXManager.PlaySoundFX("heroAttack");
			}
		}
	}

	private void PlayDamagedReceivedSoundFX(Character attacker, Character defender, WeaponData damagedFrom)
	{
		string name = "hitImpact" + (UnityEngine.Random.Range(0, 3) + 1);
		if (attacker != null && attacker.IsFireballAttack())
		{
			name = "hitImpactFirebreath";
		}
		else if (attacker != null && attacker.Id == "mushroomToxic")
		{
			name = "gaz";
		}
		else if (defender.IsChest())
		{
			name = "hitImpactChest";
		}
		else if (damagedFrom != null && !defender.IsReceivingCriticalDamage && damagedFrom != null && damagedFrom.IsRangedAttack() && attacker is Hero)
		{
			name = "arrow_impact";
		}
		_commonFXManager.PlaySoundFX(name);
		if (defender.IsReceivingCriticalDamage)
		{
			_commonFXManager.PlaySoundFX("critical_hit");
		}
	}
}