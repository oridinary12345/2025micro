using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopPanelBoxChest : MonoBehaviour
{
	[SerializeField]
	private Text _cardAmountText;

	[SerializeField]
	private Text _reward1Text;

	[SerializeField]
	private Text _reward2Text;

	[SerializeField]
	private Text _priceText;

	[SerializeField]
	private Text _claimText;

	[SerializeField]
	private Text _timerText;

	[SerializeField]
	private Image _productImage;

	[SerializeField]
	private Image _backgroundImage;

	[SerializeField]
	private UIBadge _badge;

	private ChestData _chestData;

	private readonly WaitForSeconds _wait = new WaitForSeconds(0.4f);

	private Coroutine _timerCR;

	public UIGameButton Button
	{
		get;
		private set;
	}

	public UIShopPanelBoxChest Init(ChestData chestData, string imagePath)
	{
		Button = base.gameObject.GetComponent<UIGameButton>();
		_chestData = chestData;
		_chestData.Events.ChestRedeemedEvent += OnChestRedeemed;
		_chestData.ChestUpdatedEvent += OnChestUpdated;
		App.Instance.Player.LootManager.Events.LootUpdatedEvent += OnLootUpdated;
		_productImage.sprite = Resources.Load<Sprite>(imagePath);
		UpdateContent();
		UpdateRewardText();
		return this;
	}

	private void OnDestroy()
	{
		if (_chestData != null)
		{
			if (_chestData.Events != null)
			{
				_chestData.Events.ChestRedeemedEvent -= OnChestRedeemed;
			}
			_chestData.ChestUpdatedEvent -= OnChestUpdated;
		}
		if (App.IsCreated())
		{
			App.Instance.Player.LootManager.Events.LootUpdatedEvent -= OnLootUpdated;
		}
	}

	public void UpdateContent()
	{
		LootProfile price = _chestData.GetPrice();
		if (_claimText != null)
		{
			_claimText.text = "CLAIM!";
		}
		if (_claimText != null)
		{
			_claimText.enabled = (!price.IsValid() && _chestData.IsReadyForRedeem());
		}
		if (_priceText != null)
		{
			_priceText.text = ((price.Amount > 0) ? (price.Amount + InlineSprites.GetLootInlineSprite(price.LootId)) : string.Empty);
		}
		if (_priceText != null)
		{
			_priceText.enabled = price.IsValid();
		}
		_badge.SetVisible(!price.IsValid() && _chestData.IsReadyForRedeem(), true);
		if (!price.IsValid() && !_chestData.IsReadyForRedeem())
		{
			_backgroundImage.color = new Color(0.8f, 0.8f, 0.8f, 0.8f);
		}
		else
		{
			_backgroundImage.color = Color.white;
		}
		_timerText.enabled = _chestData.IsTimerActive();
		UpdateTimer();
		UpdatePrice();
		UpdateButton();
	}

	private void UpdatePrice()
	{
		LootProfile price = _chestData.GetPrice();
		if (price.IsValid())
		{
			Button.interactable = App.Instance.Player.LootManager.CanAfford(price.LootId, price.Amount);
			Button.SetDisabledExplanation("You don't have enough rubies");
		}
	}

	private void UpdateButton()
	{
		LootProfile price = _chestData.GetPrice();
		if (!price.IsValid())
		{
			Button.interactable = !_chestData.IsSleeping();
			Button.SetDisabledExplanation("Come back later!");
		}
	}

	private void UpdateRewardText()
	{
		// 检查所有文本组件
		if (_cardAmountText == null || _reward1Text == null || _reward2Text == null)
		{
			Debug.LogWarning("UpdateRewardText: Some text components are missing");
			return;
		}

		_cardAmountText.text = string.Empty;
		_reward1Text.text = string.Empty;
		_reward2Text.text = string.Empty;
		Dictionary<string, int> mergedRewards = new Dictionary<string, int>();
		foreach (string rewardId in _chestData.Config.RewardIds)
		{
			Reward reward = App.Instance.RewardFactory.Create(rewardId);
			if (reward != null)
			{
				Action<Reward> action = delegate(Reward r)
				{
					RewardLoot rewardLoot = r as RewardLoot;
					if (rewardLoot != null)
					{
						if (!mergedRewards.ContainsKey(rewardLoot.LootId))
						{
							mergedRewards[rewardLoot.LootId] = 0;
						}
						Dictionary<string, int> dictionary;
						string lootId;
						(dictionary = mergedRewards)[lootId = rewardLoot.LootId] = dictionary[lootId] + rewardLoot.LootAmount;
					}
					RewardFreeCoins rewardFreeCoins = r as RewardFreeCoins;
					if (rewardFreeCoins != null)
					{
						if (!mergedRewards.ContainsKey("lootCoin"))
						{
							mergedRewards["lootCoin"] = 0;
						}
						Dictionary<string, int> dictionary;
						(dictionary = mergedRewards)["lootCoin"] = dictionary["lootCoin"] + rewardFreeCoins.CoinsAmount;
					}
					RewardHero rewardHero = r as RewardHero;
					if (rewardHero != null)
					{
						if (!mergedRewards.ContainsKey("cards"))
						{
							mergedRewards["cards"] = 0;
						}
						Dictionary<string, int> dictionary;
						(dictionary = mergedRewards)["cards"] = dictionary["cards"] + rewardHero.CardCount;
					}
					RewardWeapon rewardWeapon = r as RewardWeapon;
					if (rewardWeapon != null)
					{
						if (!mergedRewards.ContainsKey("cards"))
						{
							mergedRewards["cards"] = 0;
						}
						Dictionary<string, int> dictionary;
						(dictionary = mergedRewards)["cards"] = dictionary["cards"] + rewardWeapon.CardCount;
					}
				};
				RewardList rewardList = reward as RewardList;
				if (rewardList != null)
				{
					foreach (Reward reward2 in rewardList.Rewards)
					{
						action(reward2);
					}
				}
				else
				{
					action(reward);
				}
			}
		}
		if (mergedRewards.ContainsKey("cards"))
		{
			if (_cardAmountText != null)
			{
				_cardAmountText.text = string.Format("×{0}", mergedRewards["cards"].ToString());
			}
		}
		if (mergedRewards.ContainsKey("lootCoin"))
		{
			if (_reward1Text != null)
			{
				_reward1Text.text = InlineSprites.GetLootInlineSprite("lootCoin") + " " + mergedRewards["lootCoin"].ToString("### ### ###").Trim();
			}
		}
		if (mergedRewards.ContainsKey("lootRuby"))
		{
			if (_reward2Text != null)
			{
				_reward2Text.text = InlineSprites.GetLootInlineSprite("lootRuby") + " " + mergedRewards["lootRuby"].ToString("### ### ###").Trim();
			}
		}
	}

	private void OnLootUpdated(LootProfile loot, int delta, CurrencyReason reason)
	{
		if (loot.LootId == _chestData.GetPrice().LootId)
		{
			UpdatePrice();
		}
	}

	private void OnChestRedeemed(string chestId)
	{
		if (_chestData.Config.Id == chestId)
		{
			UpdateContent();
		}
	}

	private void OnChestUpdated()
	{
		UpdateContent();
	}

	private void UpdateTimer()
	{
		if (_timerCR != null)
		{
			StopCoroutine(_timerCR);
			_timerCR = null;
		}
		if (_chestData.IsTimerActive() && base.gameObject.activeInHierarchy)
		{
			_timerCR = StartCoroutine(UpdateTimerCR());
		}
	}

	private IEnumerator UpdateTimerCR()
	{
		while (_chestData.IsTimerActive())
		{
			if (_timerText != null)
			{
				_timerText.text = _chestData.GetTimeBeforeRedeem();
			}
			yield return _wait;
		}
		if (_timerText != null)
		{
			_timerText.text = string.Empty;
		}
		_timerCR = null;
		yield return null;
		UpdateContent();
	}
}