using System;

public class TimeEvents
{
	public event Action<Timer> TimerFinishedEvent;

	public event Action<Timer> TimerTickedEvent;

	public void OnTimerFinished(Timer timer)
	{
		if (this.TimerFinishedEvent != null)
		{
			this.TimerFinishedEvent(timer);
		}
	}

	public void OnTimerTicked(Timer timer)
	{
		if (this.TimerTickedEvent != null)
		{
			this.TimerTickedEvent(timer);
		}
	}
}