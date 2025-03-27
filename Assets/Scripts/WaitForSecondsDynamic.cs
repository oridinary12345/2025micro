using System;
using UnityEngine;

public class WaitForSecondsDynamic : CustomYieldInstruction
{
	private readonly Func<float> _getTimeScale;

	private readonly float _startedWaitingAt;

	private readonly float _waitingTime;

	public override bool keepWaiting => Time.realtimeSinceStartup < _startedWaitingAt + _waitingTime / _getTimeScale();

	public WaitForSecondsDynamic(float waitingTime, Func<float> getTimeScale)
	{
		_waitingTime = waitingTime;
		_startedWaitingAt = Time.realtimeSinceStartup;
		_getTimeScale = getTimeScale;
	}
}