using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
		if (_priceText != null)
		{
			LootProfile price = _chestData.GetPrice();
			_priceText.text = ((price.Amount > 0) ? (price.Amount + InlineSprites.GetLootInlineSprite(price.LootId)) : string.Empty);
			UpdatePrice();
			Button.SetDisabledExplanation("You don't have enough rubies");
		}
		if (_countText != null)
		{
			_countText.text = $"{_chestData.Config.Max - _chestData.OpenedCount}/{_chestData.Config.Max}";
		}
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
			_timerCR = StartCoroutine(UpdateTimerCR());
		}
	}

	private IEnumerator UpdateTimerCR()
	{
		while (_chestData.IsTimerActive())
		{
			_timerText.text = _chestData.GetTimeBeforeRedeem();
			yield return wait;
		}
		_timerText.SetText(string.Empty);
		_timerCR = null;
		yield return null;
		UpdateContent();
	}
}