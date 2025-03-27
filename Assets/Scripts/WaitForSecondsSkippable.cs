using System;
using UnityEngine;

public class WaitForSecondsSkippable : CustomYieldInstruction
{
	private readonly Func<bool> _skip;

	private readonly float _waitUntil;

	public override bool keepWaiting => !_skip() && Time.time < _waitUntil;

	public WaitForSecondsSkippable(float waitingTime, Func<bool> skip)
	{
		_waitUntil = Time.time + waitingTime;
		_skip = skip;
	}
}