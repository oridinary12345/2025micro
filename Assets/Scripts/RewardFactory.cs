using System.Collections.Generic;
using UnityEngine;
using Utils;

public class RewardFactory : MonoBehaviour
{
	private RewardConfigs _configs;

	public const float AdsFreeCoinsMultiplier = 3f;

	public RewardFactory Init(RewardConfigs configs)
	{
		_configs = configs;
		return this;
	}

	public Reward Create(string rewardId, RewardContext rewardContext = null)
	{
		if (rewardContext == null)
		{
			rewardContext = RewardContext.Default;
		}
		RewardConfig config = GetConfig(rewardId);
		if (config != null)
		{
			switch (config.RewardType)
			{
			case RewardType.loot:
				return CreateRewardLoot(config);
			case RewardType.life:
				return CreateRewardLife(config);
			case RewardType.weapon:
				return CreateRewardWeapon(config, rewardContext);
			case RewardType.hero:
				return CreateRewardHero(config, rewardContext);
			case RewardType.chestTimed:
				return CreateChestTimed(config);
			case RewardType.chestPremium1:
				return CreateChestPremium1(config);
			case RewardType.chestPremium2:
				return CreateChestPremium2(config);
			case RewardType.freeCoinsAds:
				return CreateFreeCoinsAds(config);
			case RewardType.freeHeroHeal:
				return CreateHeroHeal(config);
			case RewardType.freeWeaponRepair:
				return CreateFreeWeaponRepair(config, rewardContext);
			}
			UnityEngine.Debug.LogWarning("Reward type not handled: " + config.RewardType);
		}
		return null;
	}

	private RewardFreeCoins CreateFreeCoinsAds(RewardConfig config)
	{
		int amountPerMinuteMax = App.Instance.Player.MuseumManager.GetAmountPerMinuteMax();
		int b = Mathf.RoundToInt((float)amountPerMinuteMax * 3f);
		return new RewardFreeCoins(config.RewardType, Mathf.Max(250, b));
	}

	private RewardFreeCoins CreateChestTimed(RewardConfig config)
	{
		int amountPerMinuteMax = App.Instance.Player.MuseumManager.GetAmountPerMinuteMax();
		int b = Mathf.RoundToInt((float)amountPerMinuteMax * 15f);
		return new RewardFreeCoins(config.RewardType, Mathf.Max(1000, b));
	}

	private RewardFreeCoins CreateChestPremium1(RewardConfig config)
	{
		int amountPerMinuteMax = App.Instance.Player.MuseumManager.GetAmountPerMinuteMax();
		int b = Mathf.RoundToInt((float)amountPerMinuteMax * 60f);
		return new RewardFreeCoins(config.RewardType, Mathf.Max(4000, b));
	}

	private RewardFreeCoins CreateChestPremium2(RewardConfig config)
	{
		int amountPerMinuteMax = App.Instance.Player.MuseumManager.GetAmountPerMinuteMax();
		int b = Mathf.RoundToInt((float)amountPerMinuteMax * 300f);
		return new RewardFreeCoins(config.RewardType, Mathf.Max(10000, b));
	}

	private RewardFreeHeroHeal CreateHeroHeal(RewardConfig config)
	{
		return new RewardFreeHeroHeal();
	}

	private RewardFreeWeaponRepair CreateFreeWeaponRepair(RewardConfig config, RewardContext rewardContext)
	{
		return new RewardFreeWeaponRepair(rewardContext.weapongId);
	}

	private RewardLoot CreateRewardLoot(RewardConfig config)
	{
		LootProfile lootProfile = LootProfile.Create(config.RewardTypeId);
		lootProfile.Amount = UnityEngine.Random.Range(config.AmountMin, config.AmountMax + 1);
		return new RewardLoot(lootProfile, config.Percentage, config.IsCard);
	}

	private RewardLife CreateRewardLife(RewardConfig config)
	{
		return new RewardLife(config.ParamInt1, config.Percentage);
	}

	private Reward CreateRewardWeapon(RewardConfig config, RewardContext rewardContext)
	{
		int num = UnityEngine.Random.Range(config.AmountMin, config.AmountMax + 1);
		string rewardTypeId = config.RewardTypeId;
		WeaponRangeType range = Enum.TryParse(config.RewardSubTypeId, WeaponRangeType.Invalid);
		if (string.IsNullOrEmpty(rewardTypeId))
		{
			List<Reward> list = new List<Reward>();
			for (int i = 0; i < num; i++)
			{
				rewardTypeId = App.Instance.Player.WeaponManager.GetRandomCardId(rewardContext.worldId, range);
				RewardWeapon item = new RewardWeapon(rewardTypeId, 1, config.Percentage);
				list.Add(item);
			}
			RewardList rewardList = new RewardList();
			rewardList.AddRange(Merge(list));
			return rewardList;
		}
		return new RewardWeapon(rewardTypeId, num, config.Percentage);
	}

	private Reward CreateRewardHero(RewardConfig config, RewardContext rewardContext)
	{
		int num = UnityEngine.Random.Range(config.AmountMin, config.AmountMax + 1);
		string rewardTypeId = config.RewardTypeId;
		if (string.IsNullOrEmpty(rewardTypeId))
		{
			List<Reward> list = new List<Reward>();
			for (int i = 0; i < num; i++)
			{
				rewardTypeId = App.Instance.Player.HeroManager.GetRandomCardId(rewardContext.worldId);
				RewardHero item = new RewardHero(rewardTypeId, 1, config.Percentage);
				list.Add(item);
			}
			RewardList rewardList = new RewardList();
			rewardList.AddRange(Merge(list));
			return rewardList;
		}
		return new RewardHero(rewardTypeId, num, config.Percentage);
	}

	public static List<Reward> Merge(List<Reward> rewards)
	{
		rewards = ExplodeRewards(rewards);
		List<Reward> list = new List<Reward>();
		Dictionary<RewardType, List<Reward>> dictionary = new Dictionary<RewardType, List<Reward>>();
		foreach (Reward reward in rewards)
		{
			if (!dictionary.ContainsKey(reward.RewardType))
			{
				dictionary[reward.RewardType] = new List<Reward>();
				dictionary[reward.RewardType].Add(reward);
			}
			else
			{
				bool flag = false;
				foreach (Reward item in dictionary[reward.RewardType])
				{
					if (item.Merge(reward))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					list.Add(reward);
				}
				else
				{
					dictionary[reward.RewardType].Add(reward);
				}
			}
		}
		foreach (Reward item2 in list)
		{
			rewards.Remove(item2);
		}
		return rewards;
	}

	private static List<Reward> ExplodeRewards(List<Reward> rewards)
	{
		List<Reward> list = new List<Reward>();
		List<Reward> list2 = new List<Reward>();
		foreach (Reward reward in rewards)
		{
			RewardList rewardList = reward as RewardList;
			if (rewardList != null)
			{
				list.Add(reward);
				foreach (Reward reward2 in rewardList.Rewards)
				{
					list2.Add(reward2);
				}
			}
		}
		bool flag = list.Count > 0;
		foreach (Reward item in list)
		{
			rewards.Remove(item);
		}
		rewards.AddRange(list2);
		if (flag)
		{
			return ExplodeRewards(rewards);
		}
		return rewards;
	}

	private RewardConfig GetConfig(string id)
	{
		return _configs.GetConfig(id);
	}
}