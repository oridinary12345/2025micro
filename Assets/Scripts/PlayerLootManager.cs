using System.Collections.Generic;
using UnityEngine;

public class PlayerLootManager
{
	public LootEvents Events = new LootEvents();

	private readonly LootProfiles _profiles;

	private readonly LootConfigs _configs;

	private readonly Dictionary<string, LootData> _cacheData = new Dictionary<string, LootData>();

	public PlayerLootManager(LootProfiles profiles, LootConfigs configs)
	{
		_profiles = profiles;
		_configs = configs;
		if (!_profiles.Loots.ContainsKey("lootRuby"))
		{
			Add("lootRuby", 100, CurrencyReason.newAccount);
		}
		if (!_profiles.Loots.ContainsKey("lootCoin"))
		{
			Add("lootCoin", 0, CurrencyReason.newAccount);
		}
		if (!_profiles.Loots.ContainsKey("lootKey"))
		{
			Add("lootKey", 12, CurrencyReason.newAccount);
		}
	}

	public LootData GetLoot(string lootId)
	{
 LootData value;		if (!_cacheData.TryGetValue(lootId, out value))
		{
			LootConfig config = _configs.GetConfig(lootId);
 LootProfile value2;			if (!_profiles.Loots.TryGetValue(lootId, out value2))
			{
				value2 = LootProfile.Create(config.Id);
				_profiles.Loots.Add(lootId, value2);
			}
			value = new LootData(config, value2);
			_cacheData[lootId] = value;
		}
		return value;
	}

	public bool CanAfford(string lootId, int amount)
	{
		return GetLoot(lootId).Amount >= amount;
	}

	public bool TryExpense(string lootId, int amount, CurrencyReason reason)
	{
		if (CanAfford(lootId, amount))
		{
			Expense(lootId, amount, reason);
			return true;
		}
		return false;
	}

	public void Add(LootProfiles profiles, CurrencyReason reason)
	{
		foreach (LootProfile value in profiles.Loots.Values)
		{
			Add(value, reason);
		}
	}

	public void Add(LootProfile profile, CurrencyReason reason)
	{
		Add(profile.LootId, profile.Amount, reason);
	}

	public void Add(RewardLoot reward, CurrencyReason reason)
	{
		Add(reward.LootId, reward.LootAmount, reason);
	}

	public void Add(string lootId, int amount, CurrencyReason reason)
	{
		int num = int.MaxValue;
		LootConfig config = MonoSingleton<LootConfigs>.Instance.GetConfig(lootId);
		if (config != null)
		{
			num = config.MaxCapacity;
		}
		amount = Mathf.Min(amount, num - GetLoot(lootId).Amount);
		if (amount > 0)
		{
			LootHelper.Add(_profiles, lootId, amount);
			Events.OnLootUpdated(_profiles.Loots[lootId], amount, reason);
		}
	}

	private void Expense(string lootId, int amount, CurrencyReason reason)
	{
		LootConfig config = _configs.GetConfig(lootId);
 LootProfile value;		if (!_profiles.Loots.TryGetValue(lootId, out value))
		{
			value = LootProfile.Create(config.Id);
			_profiles.Loots.Add(lootId, value);
		}
		value.Amount = Mathf.Max(0, value.Amount - amount);
		Events.OnLootUpdated(value, -amount, reason);
	}
}