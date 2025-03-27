using System;
using UnityEngine;
using Utils;

public class ChestData
{
	private readonly ChestConfig _config;

	private readonly ChestProfile _profile;

	private readonly ChestEvents _chestEvents;

	private readonly TimeManager _timeManager;

	private Timer _timer;

	public ChestConfig Config => _config;

	public int OpenedCount => _profile.ChestOpenedCount;

	public ChestEvents Events => _chestEvents;

	public event Action ChestUpdatedEvent;

	public ChestData(ChestConfig config, ChestProfile profile, ChestEvents events, TimeManager timeManager)
	{
		_config = config;
		_profile = profile;
		_chestEvents = events;
		_timeManager = timeManager;
		UpdateTimer();
		_timeManager.Events.TimerFinishedEvent += OnTimerFinished;
	}

	private void OnTimerFinished(Timer timer)
	{
		if (_timer == timer)
		{
			_profile.ChestStartedDate.Time = DateTime.UtcNow;
			_profile.ChestOpenedCount = 0;
			UpdateTimer();
		}
	}

	public void OnRedeemed()
	{
		if (HasRedeemedAll() && !Config.IsContinuousTimer)
		{
			_profile.ChestStartedDate.Time = DateTime.UtcNow;
			UpdateTimer();
		}
	}

	private void UpdateTimer()
	{
		if ((Config.IsContinuousTimer || HasRedeemedAll()) && IsTimedLimited())
		{
			_timer = Timer.Create(_profile.ChestStartedDate.Time, (float)Config.WaitingTimeHour * 60f * 60f);
			_timeManager.Register(_timer);
			OnChestUpdated();
		}
		else if (_timer != null)
		{
			_timeManager.Unregister(_timer);
			_timer = null;
			OnChestUpdated();
		}
	}

	public bool IsReadyForRedeem()
	{
		if (Config.IsContinuousTimer)
		{
			return !HasRedeemedAll();
		}
		return _timer == null || _timer.IsFinished();
	}

	public string GetTimeBeforeRedeem()
	{
		if (_timer == null || _timer.IsFinished())
		{
			return string.Empty;
		}
		return Utils.Time.SecondToLongString(Mathf.RoundToInt(_timer.GetRemainingTimeSec()));
	}

	public bool IsTimedLimited()
	{
		return _config.WaitingTimeHour > 0;
	}

	public bool IsTimerActive()
	{
		return Config.IsContinuousTimer || IsSleeping();
	}

	public bool IsSleeping()
	{
		if (Config.IsContinuousTimer)
		{
			return HasRedeemedAll();
		}
		return IsTimedLimited() && !IsReadyForRedeem();
	}

	public bool HasRedeemedAll()
	{
		return OpenedCount >= Config.Max;
	}

	public LootProfile GetPrice()
	{
		if (!string.IsNullOrEmpty(Config.PriceLootId))
		{
			return LootProfile.Create(Config.PriceLootId, Config.PriceLootAmout);
		}
		return LootProfile.Create(LootConfig.Invalid.Id, 1);
	}

	private void OnChestUpdated()
	{
		if (this.ChestUpdatedEvent != null)
		{
			this.ChestUpdatedEvent();
		}
	}
}