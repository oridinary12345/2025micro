using System;

public class LevelEvents
{
	public event Action WaveStartedEvent;

	public event Action WaveCompletedEvent;

	public event Action AllWaveCompletedEvent;

	public event Action<string> LevelUnlockedEvent;

	public event Action<string> LastSelectedWorldIdChangedEvent;

	public event Action<string> WorldUnlockedEvent;

	public void OnWaveStarted()
	{
		if (this.WaveStartedEvent != null)
		{
			this.WaveStartedEvent();
		}
	}

	public void OnWaveCompleted()
	{
		if (this.WaveCompletedEvent != null)
		{
			this.WaveCompletedEvent();
		}
	}

	public void OnAllWaveCompleted()
	{
		if (this.AllWaveCompletedEvent != null)
		{
			this.AllWaveCompletedEvent();
		}
	}

	public void OnLevelUnlocked(string levelId)
	{
		if (this.LevelUnlockedEvent != null)
		{
			this.LevelUnlockedEvent(levelId);
		}
	}

	public void OnLastSelectedWorldIdChanged(string worldId)
	{
		if (this.LastSelectedWorldIdChangedEvent != null)
		{
			this.LastSelectedWorldIdChangedEvent(worldId);
		}
	}

	public void OnWorldUnlocked(string worldId)
	{
		if (this.WorldUnlockedEvent != null)
		{
			this.WorldUnlockedEvent(worldId);
		}
	}
}