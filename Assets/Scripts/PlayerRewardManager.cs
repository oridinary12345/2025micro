using UnityEngine;

public class PlayerRewardManager
{
	private readonly Player _player;

	public PlayerRewardManager(Player player)
	{
		_player = player;
	}

	public void Redeem(Reward reward, CurrencyReason reason)
	{
		if (reward == null)
		{
			UnityEngine.Debug.LogWarning("Redeem reward failed. The reward is NULL");
			return;
		}
		RewardList rewardList = reward as RewardList;
		if (rewardList != null)
		{
			foreach (Reward reward2 in rewardList.Rewards)
			{
				Redeem(reward2, reason);
			}
			return;
		}
		RewardLoot rewardLoot = reward as RewardLoot;
		if (rewardLoot != null)
		{
			RedeemLoot(rewardLoot, reason);
			return;
		}
		RewardWeapon rewardWeapon = reward as RewardWeapon;
		if (rewardWeapon != null)
		{
			RedeemWeapon(rewardWeapon);
			return;
		}
		RewardHero rewardHero = reward as RewardHero;
		if (rewardHero != null)
		{
			RedeemHero(rewardHero);
			return;
		}
		RewardFreeCoins rewardFreeCoins = reward as RewardFreeCoins;
		if (rewardFreeCoins != null)
		{
			RedeemFreeCoins(rewardFreeCoins, reason);
			return;
		}
		RewardFreeHeroHeal rewardFreeHeroHeal = reward as RewardFreeHeroHeal;
		if (rewardFreeHeroHeal != null)
		{
			RedeemHeroHeal(rewardFreeHeroHeal);
			return;
		}
		RewardFreeWeaponRepair rewardFreeWeaponRepair = reward as RewardFreeWeaponRepair;
		if (rewardFreeWeaponRepair != null)
		{
			RedeemWeaponRepair(rewardFreeWeaponRepair);
		}
		else
		{
			UnityEngine.Debug.LogWarning("Reward type not handled: " + reward.RewardType);
		}
	}

	private void RedeemLoot(RewardLoot lootReward, CurrencyReason reason)
	{
		if (lootReward == null)
		{
			UnityEngine.Debug.LogWarning("You tried to redeemed a loot reward, but the class type wasn't a RewardLoot");
		}
		else
		{
			_player.LootManager.Add(lootReward, reason);
		}
	}

	private void RedeemWeapon(RewardWeapon weaponReward)
	{
		if (weaponReward == null)
		{
			UnityEngine.Debug.LogWarning("You tried to redeemed a weapon reward, but the class type wasn't a RewardWeapon");
			return;
		}
		WeaponData weapon = App.Instance.Player.WeaponManager.GetWeapon(weaponReward.WeaponId);
		if (weapon != null)
		{
			bool unlocked = weapon.Unlocked;
			weaponReward.HasUnlockedSomething = !unlocked;
			_player.WeaponManager.AddCards(weaponReward.WeaponId, weaponReward.CardCount);
		}
	}

	private void RedeemHero(RewardHero heroReward)
	{
		if (heroReward == null)
		{
			UnityEngine.Debug.LogWarning("You tried to redeemed a hero reward, but the class type wasn't a RewardHero");
			return;
		}
		bool unlocked = App.Instance.Player.HeroManager.GetHeroData(heroReward.HeroId).Unlocked;
		heroReward.HasUnlockedSomething = !unlocked;
		_player.HeroManager.AddCards(heroReward.HeroId, heroReward.CardCount);
	}

	private void RedeemFreeCoins(RewardFreeCoins reward, CurrencyReason reason)
	{
		_player.LootManager.Add("lootCoin", reward.CoinsAmount, reason);
	}

	private void RedeemHeroHeal(RewardFreeHeroHeal reward)
	{
		_player.HeroManager.InstantHeal(_player.HeroManager.GetCurrentHeroId());
	}

	private void RedeemWeaponRepair(RewardFreeWeaponRepair reward)
	{
		_player.WeaponManager.FullInstantRepair(reward.WeaponId);
	}
}