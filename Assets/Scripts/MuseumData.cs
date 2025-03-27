using System;
using UnityEngine;
using Utils;

public class MuseumData
{
	private readonly MuseumConfig _config;

	private readonly MuseumMonsterProfile _profile;

	private readonly MuseumEvents _museumEvents;

	private readonly TimeManager _timeManager;

	private Timer _awakeTimer;

	private Timer _paymentTimer;

	private bool _isPaused;

	public MuseumConfig Config => _config;

	public MuseumEvents Events => _museumEvents;

	public int CurrentMonsterKilledCount => _config.GetCurrentMonsterKilled(_profile.MonsterCollectedCount);

	public float MonsterKilledObjective01 => Mathf.Clamp01((float)CurrentMonsterKilledCount / (float)_config.GetKillAmountNeeded(Level));

	public int Level => _config.GetLevel(_profile.MonsterCollectedCount);

	public MuseumData(MuseumConfig config, MuseumMonsterProfile profile, MuseumEvents events, TimeManager timeManager)
	{
		_config = config;
		_profile = profile;
		_museumEvents = events;
		_timeManager = timeManager;
		UpdateTimer();
		_timeManager.Events.TimerFinishedEvent += OnTimerFinished;
	}

	public void Register(GameEvents gameEvents)
	{
		gameEvents.MonsterKilledEvent += OnMonsterKilled;
	}

	public void Unregister(GameEvents gameEvents)
	{
		gameEvents.MonsterKilledEvent -= OnMonsterKilled;
	}

	public void WakeUp()
	{
		if (!IsSleeping())
		{
			UnityEngine.Debug.LogWarning("You can't wake up a monster that's not sleeping");
			return;
		}
		ResetWakeUpTime();
		_museumEvents.OnMonsterAwaken(this);
	}

	public int GetAmountPerMinute()
	{
		if (IsSleeping())
		{
			return 0;
		}
		return GetAmountPerMinuteMax();
	}

	public int GetAmountPerMinuteMax()
	{
		if (!IsUnlocked())
		{
			return 0;
		}
		float num = 60f / (float)_config.PaymentDelaySec;
		return Mathf.FloorToInt((float)GetPaymentAmout() * num);
	}

	public bool IsUnlocked()
	{
		return _profile.MonsterCollectedCount > 0;
	}

	public bool IsSleeping()
	{
		return _awakeTimer == null;
	}

	public float GetPaymentProgress01()
	{
		if (_paymentTimer == null)
		{
			return 0f;
		}
		return 1f - _paymentTimer.GetRemainingTimeSec() / (float)_config.PaymentDelaySec;
	}

	public int GetPaymentAmout()
	{
		return _config.GetPaymentAmount(Level);
	}

	private void ResetWakeUpTime()
	{
		_profile.LastWakeUpTime.Time = DateTime.UtcNow;
		_profile.LastTickTime.Time = DateTime.UtcNow;
		UpdateTimer();
	}

	public bool HasReachLevelMax()
	{
		return Level == 5;
	}

	private void OnMonsterKilled(string monsterId, string killedWithWeaponId)
	{
		if (!(_config.Id != monsterId) && !HasReachLevelMax())
		{
			int level = Level;
			_profile.MonsterCollectedCount++;
			if (_profile.MonsterCollectedCount == 1)
			{
				UnlockMonsterCard();
			}
			_museumEvents.OnMonsterKillChanged(this);
			if (Level > level && Level > 1)
			{
				_museumEvents.OnMonsterLevelUp(this);
				UpdateTimer();
			}
		}
	}

	private void OnMonsterFallAsleep()
	{
		_museumEvents.OnMonsterFallAsleep(this);
	}

	private void OnPaymentReady()
	{
		Pay(GetPaymentAmout());
		UpdatePaymentTimer();
	}

	private void Pay(int coinsAmount)
	{
	}

	private void UnlockMonsterCard()
	{
		ResetWakeUpTime();
		_museumEvents.OnMonsterUnlocked(this);
	}

	private void OnTimerFinished(Timer timer)
	{
		if (_awakeTimer == timer)
		{
			EndAwakeTimer();
			OnMonsterFallAsleep();
		}
		else if (_paymentTimer == timer)
		{
			_profile.LastTickTime.Time = DateTime.UtcNow;
			EndPaymentTimer();
			OnPaymentReady();
		}
	}

	public void OnAppPause()
	{
		if (IsUnlocked() && !IsSleeping() && !_isPaused)
		{
			_isPaused = true;
			_profile.LastTickTime.Time = DateTime.UtcNow;
		}
	}

	public bool OnAppResume()
	{
		if (!IsUnlocked())
		{
			return false;
		}
		if (IsSleeping())
		{
			return false;
		}
		if (!_isPaused)
		{
			return false;
		}
		_isPaused = false;
		return true;
	}

	public void UpdateTimer()
	{
		if (IsUnlocked())
		{
			bool flag = EndAwakeTimer();
			EndPaymentTimer();
			DateTime time = _profile.LastWakeUpTime.Time;
			int maxAwakeTimeMin = _config.GetMaxAwakeTimeMin(_config.GetLevel(_profile.MonsterCollectedCount));
			DateTime t = time.AddMinutes(maxAwakeTimeMin);
			if (t > DateTime.UtcNow)
			{
				_awakeTimer = Timer.Create(time, (float)maxAwakeTimeMin * 60f);
				_timeManager.Register(_awakeTimer);
				UpdatePaymentTimer();
			}
			else if (flag)
			{
				OnMonsterFallAsleep();
			}
		}
	}

	private void UpdatePaymentTimer()
	{
		if (IsUnlocked() && !IsSleeping())
		{
			double num = (DateTime.UtcNow - _profile.LastTickTime.Time).TotalSeconds;
			if (num >= (double)_config.PaymentDelaySec)
			{
				num %= (double)_config.PaymentDelaySec;
			}
			_paymentTimer = Timer.Create(DateTime.UtcNow.AddSeconds(0.0 - num), _config.PaymentDelaySec);
			_timeManager.Register(_paymentTimer);
			_profile.LastTickTime.Time = DateTime.UtcNow;
		}
	}

	private bool EndAwakeTimer()
	{
		if (_awakeTimer != null)
		{
			_timeManager.Unregister(_awakeTimer);
			_awakeTimer = null;
			return true;
		}
		return false;
	}

	private void EndPaymentTimer()
	{
		if (_paymentTimer != null)
		{
			_timeManager.Unregister(_paymentTimer);
			_paymentTimer = null;
		}
	}

	public bool IsReadyToWakeUp()
	{
		return IsUnlocked() && IsSleeping();
	}

	public string GetTimeBeforeSleep()
	{
		if (IsSleeping())
		{
			return string.Empty;
		}
		return Utils.Time.SecondToLongString(Mathf.RoundToInt(_awakeTimer.GetRemainingTimeSec()));
	}
}