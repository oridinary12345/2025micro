using System;
using UnityEngine;

public class Timer
{
	private readonly bool _isUtc;

	private DateTime _startTime;

	private DateTime _endTime;

	private float _durationSec;

	public Timer(DateTime startTime, float durationSec, bool isUtc)
	{
		_durationSec = durationSec;
		_startTime = startTime;
		_endTime = startTime.AddSeconds(_durationSec);
		_isUtc = isUtc;
	}

	public static Timer Create(DateTime startTime, float durationSec)
	{
		return new Timer(startTime, durationSec, true);
	}

	public float GetRemainingTimeSec()
	{
		return Mathf.Max(0f, (float)(_endTime - Now()).TotalSeconds);
	}

	public float GetRemainingTime01()
	{
		return Mathf.Clamp01((float)(Now() - _startTime).TotalSeconds / _durationSec);
	}

	public bool IsFinished()
	{
		return Now() >= _endTime;
	}

	private DateTime Now()
	{
		return (!_isUtc) ? DateTime.Now : DateTime.UtcNow;
	}
}