using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoSingleton<TimeManager>
{
	private List<Timer> _timers = new List<Timer>();

	private static YieldInstruction _wait = new WaitForEndOfFrame();

	public TimeEvents Events
	{
		get;
		private set;
	}

	protected override void Init()
	{
		Events = new TimeEvents();
		StartCoroutine(ClockCR());
	}

	public void Register(Timer timer)
	{
		if (!_timers.Contains(timer))
		{
			_timers.Add(timer);
		}
		else
		{
			UnityEngine.Debug.LogWarning("Trying to register the same timer multiple time...");
		}
	}

	public void Unregister(Timer timer)
	{
		_timers.Remove(timer);
	}

	private IEnumerator ClockCR()
	{
		while (true)
		{
			for (int num = _timers.Count - 1; num >= 0; num--)
			{
				Timer timer = _timers[num];
				OnTimerTick(timer);
				if (timer.IsFinished())
				{
					OnTimerFinished(timer);
				}
			}
			yield return _wait;
		}
	}

	private void OnTimerFinished(Timer timer)
	{
		Events.OnTimerFinished(timer);
		Unregister(timer);
	}

	private void OnTimerTick(Timer timer)
	{
		Events.OnTimerTicked(timer);
	}
}