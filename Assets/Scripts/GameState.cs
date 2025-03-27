using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameState : MonoBehaviour
{
	public bool HasGameStarted;

	public bool HasGameEnded;

	private readonly Dictionary<string, int> _lootGathered = new Dictionary<string, int>();

	private readonly List<Reward> _cardCollected = new List<Reward>();

	private Hero _hero;

	private LevelData _level;

	private bool _isChestUnlocked;

	public LevelEvents LevelEvents => _level.Events;

	public Hero CurrentHero => _hero;

	public string WorldId => _level.WorldId;

	public int TotalRoundCount
	{
		get;
		set;
	}

	public int WaveRoundCount
	{
		get;
		set;
	}

	public int LastMonsterSpawnRound
	{
		get;
		set;
	}

	public bool ChestUnlocked => _isChestUnlocked;

	public bool HasUsedChestKey => ChestUnlocked;

	public event Action<string, int, int> LootUpdatedEvent;

	public GameState Init(Hero hero, LevelData level)
	{
		_hero = hero;
		_level = level;
		return this;
	}

	private void OnEnable()
	{
		App.Instance.Player.LootManager.Events.LootUpdatedEvent += OnLootUpdated;
	}

	private void OnDisable()
	{
		if (App.IsCreated())
		{
			App.Instance.Player.LootManager.Events.LootUpdatedEvent -= OnLootUpdated;
		}
	}

	public void UnlockChest()
	{
		_isChestUnlocked = true;
	}

	public int GetWaveMaxCount()
	{
		return _level.GetWaveMaxCount();
	}

	public int GetWaveIndex()
	{
		return _level.GetWaveIndex();
	}

	public int GetWaveRemainingCount()
	{
		return GetWaveMaxCount() - GetWaveIndex();
	}

	public bool IsLevelCompleted()
	{
		return _level.Completed;
	}

	public bool IsObjectiveWaveBased()
	{
		return _level.IsObjectiveWaveBased;
	}

	public bool IsObjectiveMissionBased()
	{
		return _level.IsObjectiveMissionBased;
	}

	public string GetMissionDescription()
	{
		return _level.GetMissionDescription();
	}

	public void Redeem(Reward reward)
	{
		if (reward == null)
		{
			UnityEngine.Debug.LogWarning("Redeem reward failed. The reward is NULL");
			return;
		}
		if (reward.IsCard)
		{
			_cardCollected.Add(reward);
		}
		RewardList rewardList = reward as RewardList;
		if (rewardList != null)
		{
			foreach (Reward reward2 in rewardList.Rewards)
			{
				Redeem(reward2);
			}
			return;
		}
		switch (reward.RewardType)
		{
		case RewardType.loot:
			RedeemLoot(reward as RewardLoot, CurrencyReason.inGameDrop);
			break;
		case RewardType.life:
			RedeemLife(reward as RewardLife);
			break;
		case RewardType.weapon:
			RedeemWeapon(reward as RewardWeapon);
			break;
		case RewardType.hero:
			RedeemHero(reward as RewardHero);
			break;
		default:
			UnityEngine.Debug.LogWarning("Reward type not handled: " + reward.RewardType);
			break;
		}
	}

	public void RedeemLoot(RewardLoot lootReward, CurrencyReason reason)
	{
		RedeemLoot(lootReward.LootId, lootReward.LootAmount, reason);
	}

	public void RedeemLoot(string lootId, int lootAmount, CurrencyReason reason)
	{
		App.Instance.Player.LootManager.Add(lootId, lootAmount, reason);
	}

	private void RedeemLife(RewardLife lifeReward)
	{
		App.Instance.Player.HeroManager.Heal(lifeReward.HealingAmount);
	}

	private void RedeemWeapon(RewardWeapon weaponReward)
	{
		WeaponData weapon = App.Instance.Player.WeaponManager.GetWeapon(weaponReward.WeaponId);
		if (weapon != null)
		{
			bool unlocked = weapon.Unlocked;
			weaponReward.HasUnlockedSomething = !unlocked;
			App.Instance.Player.WeaponManager.AddCards(weaponReward.WeaponId, weaponReward.CardCount);
		}
	}

	private void RedeemHero(RewardHero heroReward)
	{
		bool unlocked = App.Instance.Player.HeroManager.GetHeroData(heroReward.HeroId).Unlocked;
		heroReward.HasUnlockedSomething = !unlocked;
		App.Instance.Player.HeroManager.AddCards(heroReward.HeroId, heroReward.CardCount);
	}

	public int GetLootAmount(string lootId)
	{
		return App.Instance.Player.LootManager.GetLoot(lootId).Amount;
	}

	public Dictionary<string, int> GetLootGathered()
	{
		return _lootGathered;
	}

	public List<Reward> GetCardCollected()
	{
		return _cardCollected;
	}

	private void OnLootUpdated(LootProfile lootProfile, int delta, CurrencyReason reason)
	{
		if (reason == CurrencyReason.inGameDrop)
		{
			if (!_lootGathered.ContainsKey(lootProfile.LootId))
			{
				_lootGathered[lootProfile.LootId] = 0;
			}
			Dictionary<string, int> lootGathered;
			string lootId;
			(lootGathered = _lootGathered)[lootId = lootProfile.LootId] = lootGathered[lootId] + delta;
		}
		if (this.LootUpdatedEvent != null)
		{
			this.LootUpdatedEvent(lootProfile.LootId, lootProfile.Amount, delta);
		}
	}
}