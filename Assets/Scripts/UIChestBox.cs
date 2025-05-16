using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIChestBox : MonoBehaviour
{
	public UIGameButton Button;

	[SerializeField]
	private Image _chestImage;

	[SerializeField]
	private TextMeshProUGUI _priceText;

	[SerializeField]
	private TextMeshProUGUI _timerText;

	[SerializeField]
	private TextMeshProUGUI _countText;

	[SerializeField]
	private GameObject _timerPanel;

	[SerializeField]
	private GameObject _readyPanel;

	[SerializeField]
	private Image _shadow;

	[SerializeField]
	private Image _adsIcon;

	[SerializeField]
	private Animator _animator;

	private ChestData _chestData;

	private readonly WaitForSeconds wait = new WaitForSeconds(0.4f);

	private const string TimerString = "+{0} <sprite={1}> in {2}";
	
	[SerializeField]
	private ResourceDisplay _timerResourceDisplay; // 用于显示计时器中的资源
	
	[SerializeField]
	private ResourceDisplay _priceResourceDisplay; // 用于显示价格

	private Coroutine _timerCR;

	public void Init(ChestData chestData)
	{
		Button = base.gameObject.GetComponent<UIGameButton>();
		_chestData = chestData;
		_chestData.Events.ChestRedeemedEvent += OnChestRedeemed;
		_chestData.ChestUpdatedEvent += OnChestUpdated;
		App.Instance.Player.LootManager.Events.LootUpdatedEvent += OnLootUpdated;
		UpdateContent();
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

	private void UpdateContent()
	{
		_chestImage.sprite = Resources.Load<Sprite>("UI/" + _chestData.Config.Id);
		if (_shadow != null)
		{
			_shadow.enabled = !_chestData.IsSleeping();
		}
		if (_adsIcon != null)
		{
			_adsIcon.enabled = !_chestData.IsSleeping();
		}
		if (_timerPanel != null)
		{
			_timerPanel.SetActive(_chestData.IsTimerActive());
			UpdateButton();
			UpdateTimer();
		}
		if (_readyPanel != null)
		{
			_readyPanel.SetActive(!_chestData.IsSleeping());
		}
		
		// 更新价格显示
		LootProfile price = _chestData.GetPrice();
		
		// 优先使用ResourceDisplay
		if (_priceResourceDisplay != null && price.Amount > 0)
		{
			// 获取对应的图标
			Sprite priceIcon = null;
			
			// 尝试从SpriteAssetManager获取图标
			if (SpriteAssetManager.Instance != null)
			{
				// 根据资源类型获取对应的图标
				int spriteIndex = GetSpriteIndexForLoot(price.LootId);
				priceIcon = SpriteAssetManager.Instance.GetSprite(spriteIndex);
			}
			// 如果SpriteAssetManager不可用，尝试使用ResourceManager
			else if (ResourceManager.Instance != null)
			{
				// 根据资源类型获取对应的ResourceType
				ResourceType resourceType = GetResourceTypeForLoot(price.LootId);
				priceIcon = ResourceManager.Instance.GetResourceIcon(resourceType);
			}
			
			_priceResourceDisplay.SetValue(priceIcon, price.Amount);
		}
		// 兼容旧版本
		else if (_priceText != null)
		{
			_priceText.text = ((price.Amount > 0) ? (price.Amount + InlineSprites.GetLootInlineSprite(price.LootId)) : string.Empty);
		}
		
		if (_countText != null)
		{
			_countText.text = $"{_chestData.Config.Max - _chestData.OpenedCount}/{_chestData.Config.Max}";
		}
		
		UpdatePrice();
		Button.SetDisabledExplanation("You don't have enough rubies");
	}

	private void OnLootUpdated(LootProfile loot, int delta, CurrencyReason reason)
	{
		if (loot.LootId == _chestData.GetPrice().LootId)
		{
			UpdatePrice();
		}
	}

	private void UpdatePrice()
	{
		LootProfile price = _chestData.GetPrice();
		if (price.IsValid())
		{
			Button.interactable = App.Instance.Player.LootManager.CanAfford(price.LootId, price.Amount);
		}
	}

	public void UpdateButton()
	{
		Button.interactable = !_chestData.IsSleeping();
		Button.SetDisabledExplanation("You have reached the limit for now.");
	}

	private void UpdateTimer()
	{
		if (_timerCR != null)
		{
			StopCoroutine(_timerCR);
			_timerCR = null;
		}
		
		if (_chestData.IsTimerActive())
		{
			// 获取奖励和时间信息
			LootProfile reward = _chestData.GetPrice(); // 使用GetPrice方法获取奖励信息
			string timeString = _chestData.GetTimeBeforeRedeem(); // 使用GetTimeBeforeRedeem获取时间信息
			
			// 优先使用ResourceDisplay
			if (_timerResourceDisplay != null)
			{
				// 显示ResourceDisplay
				_timerResourceDisplay.gameObject.SetActive(true);
				
				// 获取对应的图标
				Sprite rewardIcon = null;
				
				// 尝试从SpriteAssetManager获取图标
				if (SpriteAssetManager.Instance != null)
				{
					// 根据资源类型获取对应的图标
					int spriteIndex = GetSpriteIndexForLoot(reward.LootId);
					rewardIcon = SpriteAssetManager.Instance.GetSprite(spriteIndex);
				}
				// 如果SpriteAssetManager不可用，尝试使用ResourceManager
				else if (ResourceManager.Instance != null)
				{
					// 根据资源类型获取对应的ResourceType
					ResourceType resourceType = GetResourceTypeForLoot(reward.LootId);
					rewardIcon = ResourceManager.Instance.GetResourceIcon(resourceType);
				}
				
				_timerResourceDisplay.SetValue(rewardIcon, reward.Amount);
				
				// 设置时间文本
				if (_timerText != null)
				{
					_timerText.text = $"in {timeString}";
				}
			}
			// 兼容旧版本
			else if (_timerText != null)
			{
				_timerText.text = string.Format(TimerString, reward.Amount, InlineSprites.GetLootInlineSprite(reward.LootId), timeString);
			}
			
			_timerCR = StartCoroutine(UpdateTimerCR());
		}
		else
		{
			// 隐藏ResourceDisplay
			if (_timerResourceDisplay != null)
			{
				_timerResourceDisplay.gameObject.SetActive(false);
			}
			
			// 清空时间文本
			if (_timerText != null)
			{
				_timerText.text = string.Empty;
			}
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
			yield return wait;
		}
		UpdateContent();
	}

	/// <summary>
	/// 根据资源ID获取对应的sprite索引
	/// </summary>
	private int GetSpriteIndexForLoot(string lootId)
	{
		switch (lootId)
		{
			case "lootCoin": return 3; // 金币对应sprite=3
			case "lootRuby": return 2; // 宝石对应sprite=2
			case "lootEnergy": return 4; // 能量对应sprite=4
			default: return 0;
		}
	}

	/// <summary>
	/// 根据资源ID获取对应的ResourceType
	/// </summary>
	private ResourceType GetResourceTypeForLoot(string lootId)
	{
		switch (lootId)
		{
			case "lootCoin": return ResourceType.Coin;
			case "lootRuby": return ResourceType.Gem;
			case "lootEnergy": return ResourceType.Energy;
			default: return ResourceType.Coin;
		}
	}
}