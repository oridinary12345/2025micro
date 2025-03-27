using System;
using UnityEngine;
using Utils;

public class LootKeyManager
{
	private const float WaitingTimeHour = 1f;

	private const float KeyGenerationTimerSec = 3600f;

	private PlayerLootManager _lootManager;

	private TimeManager _timeManager;

	private KeyProfile _keyProfile;

	private Timer _keyGenerationTimer;

	private bool _isPaused;

	public LootKeyManager(KeyProfile keyProfile, PlayerLootManager lootManager, TimeManager timeManager)
	{
		_keyProfile = keyProfile;
		_lootManager = lootManager;
		_timeManager = timeManager;
		UpdateTimer();
		lootManager.Events.LootUpdatedEvent += OnLootUpdated;
		timeManager.Events.TimerFinishedEvent += OnTimerFinished;
	}

	public string GetRemainingTimeBeforeNextKey()
	{
		if (_keyGenerationTimer == null || _keyGenerationTimer.IsFinished())
		{
			return string.Empty;
		}
		int num = Mathf.RoundToInt(_keyGenerationTimer.GetRemainingTimeSec());
		return $"+{1} in {Utils.Time.SecondToLongString(num)}";
	}

	public bool IsFull()
	{
		return GetCurrentAmount() >= GetMaxCapacitySoft();
	}

	public int GetCurrentAmount()
	{
		return _lootManager.GetLoot("lootKey").Amount;
	}

	public int GetMaxCapacityHard()
	{
		return _lootManager.GetLoot("lootKey").AmountMax;
	}

	public int GetMaxCapacitySoft()
	{
		return 12;
	}

	public int GetMissingAmount()
	{
		return Mathf.Max(0, GetMaxCapacitySoft() - GetCurrentAmount());
	}

	public bool CanExpense()
	{
		return GetCurrentAmount() > 0;
	}

	public bool ExpenseKey()
	{
		return _lootManager.TryExpense("lootKey", 1, CurrencyReason.play);
	}

	public void RefillKeys()
	{
		AddGeneratedKey(GetMissingAmount());
	}

	public void OnAppQuit()
	{
	}

	public void OnAppPause()
	{
		if (_isPaused)
		{
			UnityEngine.Debug.LogWarning("Game is already paused!?");
		}
		else
		{
			_isPaused = true;
		}
	}

	public void OnAppResume()
	{
		if (_isPaused)
		{
			_isPaused = false;
			UpdateTimer();
		}
	}

	private void OnLootUpdated(LootProfile loot, int delta, CurrencyReason reason)
	{
		if (loot.LootId == "lootKey")
		{
			if (!IsFull() && _keyGenerationTimer == null)
			{
				_keyProfile.NextKeyStartDate.Time = DateTime.UtcNow;
			}
			UpdateTimer();
		}
	}

	private void UpdateTimer()
	{
		if (!IsFull())
		{
			if (_keyGenerationTimer != null)
			{
				return;
			}
			if (_keyProfile.NextKeyStartDate.Time == DateTime.MinValue)
			{
				UnityEngine.Debug.LogWarning("_keyProfile.NextKeyStartDate is set to min value.. issue here!");
				return;
			}
			float num = (float)(DateTime.UtcNow - _keyProfile.NextKeyStartDate.Time).TotalSeconds;
			int num2 = Mathf.FloorToInt(num / 3600f);
			int amount = Mathf.Min(num2, GetMissingAmount());
			AddGeneratedKey(amount);
			if (!IsFull())
			{
				DateTime time = _keyProfile.NextKeyStartDate.Time.AddSeconds((float)num2 * 3600f);
				_keyProfile.NextKeyStartDate.Time = time;
				_keyGenerationTimer = Timer.Create(_keyProfile.NextKeyStartDate.Time, 3600f);
				_timeManager.Register(_keyGenerationTimer);
			}
		}
		else if (_keyGenerationTimer != null)
		{
			_timeManager.Unregister(_keyGenerationTimer);
			_keyGenerationTimer = null;
		}
	}

	private void OnTimerFinished(Timer timer)
	{
		if (_keyGenerationTimer == timer)
		{
			_keyGenerationTimer = null;
			_keyProfile.NextKeyStartDate.Time = DateTime.MinValue;
			AddGeneratedKey(1);
			UpdateTimer();
		}
	}

	private void AddGeneratedKey(int amount)
	{
		if (amount > 0)
		{
			_lootManager.Add("lootKey", amount, CurrencyReason.keyGeneration);
		}
	}
}