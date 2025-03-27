using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LootDropManager : MonoBehaviour
{
	private RewardFactory _rewardFactory;

	private MonsterConfigs _monsterConfigs;

	private CharacterEvents _characterEvents;

	private GameEvents _gameEvents;

	private GameState _gameState;

	private RewardSpawner _rewardSpawner;

	private readonly List<LootDrop> _lootDrops = new List<LootDrop>();

	public LootDropManager Init(RewardFactory rewardFactory, MonsterConfigs configs, CharacterEvents characterEvents, GameEvents gameEvents, GameState gameState)
	{
		_rewardSpawner = base.gameObject.AddComponent<RewardSpawner>();
		_monsterConfigs = configs;
		_characterEvents = characterEvents;
		_rewardFactory = rewardFactory;
		_gameEvents = gameEvents;
		_gameState = gameState;
		_characterEvents.DamagedEvent += OnDamaged;
		return this;
	}

	public bool CollectAll()
	{
		if (_lootDrops.Count > 0)
		{
			List<LootDrop> list = new List<LootDrop>(_lootDrops);
			foreach (LootDrop item in list)
			{
				item.ForceCollect();
			}
			return true;
		}
		return false;
	}

	public bool IsThereCardsOnGround()
	{
		foreach (LootDrop lootDrop in _lootDrops)
		{
			if (lootDrop.IsCard)
			{
				return true;
			}
		}
		return false;
	}

	private void OnAnimationStarted(LootDrop loot, GameObject lootShadow)
	{
		UnityEngine.Object.Destroy(lootShadow);
		UnregisterLoot(loot);
	}

	private void UnregisterLoot(LootDrop loot)
	{
		_lootDrops.Remove(loot);
	}

	private void OnDamaged(Character attacker, Character defender, WeaponData weapon, int damage)
	{
		if (defender.IsHero())
		{
			return;
		}
		string id = defender.Id;
		MonsterConfig config = _monsterConfigs.GetConfig(id);
		if (config == null)
		{
			UnityEngine.Debug.LogWarning("Can't spawn any rewards on monster damaged. Config not found for: " + id);
			return;
		}
		Vector3 position = defender.GetPosition();
		Monster monster = defender as Monster;
		int coinAmount = monster.ExpenseCoinsOnHit(damage);
		if (coinAmount > 0)
		{
			Action onCollect = delegate
			{
				_gameState.RedeemLoot("lootCoin", coinAmount, CurrencyReason.inGameDrop);
			};
			Vector3 position2 = GameObject.FindWithTag("LootCoinsTarget").transform.position;
			DropLoot(null, "lootCoin", coinAmount, 5, position, onCollect, position2);
		}
		foreach (string item in config.RewardIdHit)
		{
			if (!defender.IsChest())
			{
				HandleReward(item, position);
			}
		}
		if (defender.IsDead())
		{
			foreach (string item2 in config.RewardIdDead)
			{
				HandleReward(item2, position);
			}
		}
	}

	private void HandleReward(string rewardId, Vector3 spawningPos)
	{
		RewardContext rewardContext = new RewardContext();
		rewardContext.worldId = _gameState.WorldId;
		RewardContext rewardContext2 = rewardContext;
		Reward reward = _rewardFactory.Create(rewardId, rewardContext2);
		if (reward == null)
		{
			return;
		}
		int maxDropSprite = 5;
		if (reward is RewardLoot)
		{
			if (rewardId.StartsWith("rewardChest"))
			{
				maxDropSprite = 12;
				string worldId = _gameState.WorldId;
				WorldData worldData = App.Instance.Player.LevelManager.GetWorldData(worldId);
				LootProfile playPrice = worldData.GetPlayPrice();
				float b = (float)(10 - (worldData.Profile.EndGameChestSpawned - 1)) * 0.1f;
				float num = Mathf.Max(0.25f, b);
				int amount = Mathf.RoundToInt((float)playPrice.Amount * num);
				LootProfile lootProfile = LootProfile.Create("lootCoin");
				lootProfile.Amount = amount;
				reward = new RewardLoot(lootProfile, 1f, reward.IsCard);
			}
			else if ((reward as RewardLoot).LootId == "lootCoin")
			{
				return;
			}
		}
		DropReward(reward, maxDropSprite, spawningPos);
	}

	private void DropReward(Reward reward, int maxDropSprite, Vector3 spawningPos)
	{
		RewardList rewardList = reward as RewardList;
		if (rewardList != null)
		{
			foreach (Reward reward2 in rewardList.Rewards)
			{
				DropReward(reward2, maxDropSprite, spawningPos);
			}
		}
		else
		{
			if (!reward.CheckOdds())
			{
				return;
			}
			RewardLife lifeReward = reward as RewardLife;
			Action onCollect = delegate
			{
				if (lifeReward == null || !_gameState.CurrentHero.IsDead())
				{
					_gameState.Redeem(reward);
				}
			};
			if (lifeReward != null)
			{
				DropLoot(reward, "lootLife", 1, maxDropSprite, spawningPos, onCollect, new Vector3(0f, 0.5f, 0f));
				return;
			}
			RewardWeapon rewardWeapon = reward as RewardWeapon;
			if (rewardWeapon != null)
			{
				DropLoot(reward, "lootWeapon", 1, maxDropSprite, spawningPos, onCollect, new Vector3(0f, 0.5f, 0f));
				return;
			}
			RewardHero rewardHero = reward as RewardHero;
			if (rewardHero != null)
			{
				DropLoot(reward, "lootHero", 1, maxDropSprite, spawningPos, onCollect, new Vector3(0f, 0.5f, 0f));
				return;
			}
			RewardLoot rewardLoot = reward as RewardLoot;
			if (rewardLoot != null)
			{
				Vector3 vector = new Vector3(-2f, 3f, -1f);
				Vector3 collectingTarget = vector;
				if (rewardLoot.LootId == "lootRuby")
				{
					collectingTarget = GameObject.FindWithTag("LootRubiesTarget").transform.position;
				}
				if (rewardLoot.IsCard)
				{
					collectingTarget = new Vector3(0f, 0.5f, 0f);
				}
				DropLoot(reward, rewardLoot.LootId, rewardLoot.LootAmount, maxDropSprite, spawningPos, onCollect, collectingTarget);
			}
			else
			{
				UnityEngine.Debug.LogWarning("The game is not supporting this type of reward drop: " + reward.GetType());
			}
		}
	}

	private void DropLoot(Reward reward, string lootId, int lootAmount, int lootSpriteMax, Vector3 spawningPos, Action onCollect, Vector3 collectingTarget)
	{
		bool flag = reward?.IsCard ?? false;
		float num = 0f;
		float autoCollectDelay = (!flag) ? 2f : 0.25f;
		_gameEvents.OnLootDrop(spawningPos, lootId);
		int num2 = flag ? 1 : Math.Min(lootAmount, lootSpriteMax);
		if (!flag && lootId == "lootCoin")
		{
			_rewardSpawner.Spawn("Loot/" + lootId, num2, spawningPos, GameObject.FindGameObjectWithTag("LootCoinsTarget"), onCollect);
			_gameEvents.OnLootPickUp(lootId, flag);
			return;
		}
		for (int i = 0; i < num2; i++)
		{
			float num3 = UnityEngine.Random.Range(-0.8f, 0.8f);
			float f = UnityEngine.Random.Range(0f, 0.6f);
			float num4 = UnityEngine.Random.Range(-0.4f, 0.1f);
			Vector3 vector = new Vector3(spawningPos.x, spawningPos.y, spawningPos.z);
			Vector2 vector2 = ClampPositionToCircle(position: new Vector2(vector.x + num3, vector.y + num4), center: Vector2.zero, radius: 3.5f);
			float num5 = Mathf.Sign(num3);
			float num6 = Mathf.Sign(num4);
			num3 = Mathf.Min(Mathf.Abs(num3), Mathf.Abs(vector2.x - vector.x));
			num3 *= num5;
			num4 = Mathf.Min(Mathf.Abs(num4), Mathf.Abs(vector2.y - vector.y));
			num4 *= num6;
			Transform transform = new GameObject("Pivot-" + lootId).transform;
			transform.localPosition = vector;
			GameObject loot = GetLootGameObject(reward, lootId);
			if (loot == null)
			{
				break;
			}
			loot.transform.parent = transform;
			loot.transform.position = new Vector3(vector.x, vector.y + num4, (vector.y + num4) * 2f - 3f);
			GameObject lootShadow = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Loot/lootShadow"), vector, Quaternion.identity);
			lootShadow.transform.parent = transform;
			lootShadow.transform.localPosition = Vector3.zero;
			lootShadow.transform.position = new Vector3(vector.x, vector.y + num4 - 0.2f, (vector.y + num4) * 2f - 2.8f);
			Action onCollect2 = (!(lootId == "lootCoin") || flag) ? onCollect : null;
			int roundDuration = (!(lootId == "lootLife")) ? int.MaxValue : 3;
			LootDrop lootDrop = loot.AddComponent<LootDrop>().Init(onCollect2, collectingTarget, roundDuration, _gameEvents, lootId, reward.IsCard);
			lootDrop.AnimationStartedEvent += delegate(LootDrop l)
			{
				OnAnimationStarted(l, lootShadow);
			};
			lootDrop.ExpiredEvent += UnregisterLoot;
			_lootDrops.Add(lootDrop);
			transform.DOMoveX(vector.x + num3, 0.85f).SetDelay(num).SetEase(Ease.OutCirc);
			loot.transform.DOMoveY(vector.y + Mathf.Abs(f) + 0.5f, 0.3f).SetEase(Ease.OutBack).SetDelay(num);
			GameObject sparklingFx = null;
			loot.transform.DOMoveY(vector.y + num4, 0.55f).SetEase(Ease.OutBounce).SetDelay(num + 0.3f)
				.OnComplete(delegate
				{
					sparklingFx = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("GameFX/FXCoinsSparkling"));
					sparklingFx.transform.parent = loot.transform.parent;
					sparklingFx.transform.localPosition = loot.transform.localPosition;
					lootDrop.OnLootDropped();
				});
			if (lootId == "lootCoin" || flag)
			{
				this.Execute(num + 0.3f + 0.55f + autoCollectDelay, delegate
				{
					if (sparklingFx != null)
					{
						UnityEngine.Object.Destroy(sparklingFx);
					}
					GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("GameFX/FXCoinsSparklingCollect"));
					gameObject.transform.parent = loot.transform.parent;
					gameObject.transform.localPosition = loot.transform.localPosition;
					gameObject.AddComponent<AutoDestruct>().Init(autoCollectDelay);
					lootDrop.ForceCollect();
				});
			}
			num += 0.02f;
		}
	}

	private Vector2 ClampPositionToCircle(Vector2 center, float radius, Vector2 position)
	{
		Vector2 a = position - center;
		float magnitude = a.magnitude;
		if (radius < magnitude)
		{
			Vector2 a2 = a / magnitude;
			position = center + a2 * radius;
		}
		return position;
	}

	private GameObject GetLootGameObject(Reward reward, string lootId)
	{
		if (reward != null && reward.IsCard)
		{
			switch (reward.RewardType)
			{
			case RewardType.weapon:
			{
				string weaponId = (reward as RewardWeapon).WeaponId;
				WeaponRangeType rangeType = MonoSingleton<WeaponConfigs>.Instance.GetConfig(weaponId).RangeType;
				lootId = "lootRewardWeapon" + rangeType.ToString();
				break;
			}
			case RewardType.hero:
				lootId = "lootRewardHero";
				break;
			default:
				lootId = "lootReward";
				break;
			}
		}
		GameObject gameObject = Resources.Load<GameObject>("Loot/" + lootId);
		if (gameObject == null)
		{
			return null;
		}
		return UnityEngine.Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity);
	}
}