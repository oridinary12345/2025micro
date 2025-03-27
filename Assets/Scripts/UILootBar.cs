using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILootBar : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _lootAmountText;

	[SerializeField]
	private TextMeshProUGUI _lootGenerationAmountText;

	[SerializeField]
	private Image _lootIcon;

	private string _lootId;

	private Sequence _animationTween;

	private Sequence _amountTextSequence;

	private int _lastTargetAmount;

	public void Init(string lootId)
	{
		_lootId = lootId;
		_lootIcon.sprite = Resources.Load<Sprite>("UI/" + lootId);
		int amount = App.Instance.Player.LootManager.GetLoot(_lootId).Amount;
		_lootAmountText.text = ((amount != 0) ? amount.ToString("### ### ###") : "0");
		_lastTargetAmount = amount;
		UpdateLootGenerator();
		RegisterEvents();
		if (_lootGenerationAmountText != null)
		{
			_lootGenerationAmountText.gameObject.SetActive( false);
		}
	}

	private void OnEnable()
	{
		RegisterEvents();
		UpdateLootGenerator();
	}

	private void OnDisable()
	{
		UnRegisterEvents();
	}

	private void RegisterEvents()
	{
		UnRegisterEvents();
		App.Instance.Player.LootManager.Events.LootUpdatedEvent += OnLootUpdated;
		if (_lootGenerationAmountText != null)
		{
			MuseumEvents events = App.Instance.Player.MuseumManager.Events;
			events.MonsterUnlockedEvent += OnMonsterUnlocked;
			events.MonsterLevelUpEvent += OnMonsterLevelUp;
			events.MonsterAwakenEvent += OnMonsterAwaken;
			events.MonsterFallAsleepEvent += OnMonsterFallAsleep;
		}
	}

	private void UnRegisterEvents()
	{
		if (App.IsCreated())
		{
			App.Instance.Player.LootManager.Events.LootUpdatedEvent -= OnLootUpdated;
			if (_lootGenerationAmountText != null)
			{
				MuseumEvents events = App.Instance.Player.MuseumManager.Events;
				events.MonsterUnlockedEvent -= OnMonsterUnlocked;
				events.MonsterLevelUpEvent -= OnMonsterLevelUp;
				events.MonsterAwakenEvent -= OnMonsterAwaken;
				events.MonsterFallAsleepEvent -= OnMonsterFallAsleep;
			}
		}
		if (_animationTween != null)
		{
			_animationTween.Kill();
			_animationTween = null;
		}
	}

	private void OnLootUpdated(LootProfile loot, int delta, CurrencyReason reason)
	{
		string lootId = loot.LootId;
		int amount2 = loot.Amount;
		if (lootId == _lootId && delta != 0)
		{
			int amount = _lastTargetAmount;
			Tweener t = DOTween.To(() => amount, delegate(int x)
			{
				amount = x;
			}, amount2, 0.2f).OnUpdate(delegate
			{
				_lootAmountText.text = ((amount != 0) ? amount.ToString("### ### ###") : "0");
			});
			if (_amountTextSequence == null)
			{
				_amountTextSequence = DOTween.Sequence();
				_amountTextSequence.OnComplete(delegate
				{
					_amountTextSequence = null;
				});
			}
			_amountTextSequence.Append(t);
			_lastTargetAmount = amount2;
			if (_animationTween == null)
			{
				_animationTween = DOTween.Sequence();
				_animationTween.Insert(0f, _lootIcon.rectTransform.DOPunchScale(Vector3.one * 0.3f, 0.2f));
				_animationTween.Insert(0f, _lootAmountText.rectTransform.DOPunchScale(Vector3.one * 0.3f, 0.2f));
				_animationTween.SetDelay(0.3f);
				_animationTween.OnComplete(delegate
				{
					_lootIcon.rectTransform.localScale = Vector3.one;
					_lootAmountText.rectTransform.localScale = Vector3.one;
					_animationTween = null;
				});
			}
		}
	}

	private void OnMonsterUnlocked(MuseumData data)
	{
		UpdateLootGenerator();
	}

	private void OnMonsterLevelUp(MuseumData data)
	{
		UpdateLootGenerator();
	}

	private void OnMonsterAwaken(MuseumData data)
	{
		UpdateLootGenerator();
	}

	private void OnMonsterFallAsleep(MuseumData data)
	{
		UpdateLootGenerator();
	}

	private void UpdateLootGenerator()
	{
		if (_lootGenerationAmountText != null && !(_lootId == "lootCoin"))
		{
		}
	}
}