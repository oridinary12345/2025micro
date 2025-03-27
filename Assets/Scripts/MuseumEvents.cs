using System;

public class MuseumEvents
{
	public event Action<MuseumData> MonsterLevelUpEvent;

	public event Action<MuseumData> MonsterKillChangedEvent;

	public event Action<MuseumData> MonsterUnlockedEvent;

	public event Action<MuseumData> MonsterAwakenEvent;

	public event Action<MuseumData> MonsterFallAsleepEvent;

	public void OnMonsterLevelUp(MuseumData data)
	{
		if (this.MonsterLevelUpEvent != null)
		{
			this.MonsterLevelUpEvent(data);
		}
	}

	public void OnMonsterKillChanged(MuseumData data)
	{
		if (this.MonsterKillChangedEvent != null)
		{
			this.MonsterKillChangedEvent(data);
		}
	}

	public void OnMonsterUnlocked(MuseumData data)
	{
		if (this.MonsterUnlockedEvent != null)
		{
			this.MonsterUnlockedEvent(data);
		}
	}

	public void OnMonsterAwaken(MuseumData data)
	{
		if (this.MonsterAwakenEvent != null)
		{
			this.MonsterAwakenEvent(data);
		}
	}

	public void OnMonsterFallAsleep(MuseumData data)
	{
		if (this.MonsterFallAsleepEvent != null)
		{
			this.MonsterFallAsleepEvent(data);
		}
	}
}