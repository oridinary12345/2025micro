using System;
using UnityEngine;

public abstract class Mission
{
	private MissionProfile _profile;

	public bool Completed => Progress01 >= 1f;

	public float Progress01 => Progress / Objective;

	public float Progress => _profile.Progress;

	public float Objective => _profile.Objective;

	public MissionType MissionType => _profile.MissionType;

	public event Action<Mission> CompletedEvent;

	protected Mission(MissionProfile profile)
	{
		_profile = profile;
	}

	protected void SetObjective(float objective)
	{
		_profile.Objective = objective;
	}

	protected void AddProgress(float progressToAdd)
	{
		if (!Completed)
		{
			_profile.Progress = Mathf.Min(Objective, Progress + progressToAdd);
			if (Completed && this.CompletedEvent != null)
			{
				this.CompletedEvent(this);
			}
		}
	}

	public abstract string GetDescription();

	public virtual string GetDescriptionCompact()
	{
		return GetDescription();
	}

	public string GetProgressionText()
	{
		return $"{Progress}/{Objective}";
	}

	public virtual void Register(GameEvents gameEvents)
	{
	}

	public virtual void UnRegister(GameEvents gameEvents)
	{
	}

	public void ForceIncreasePogress()
	{
		AddProgress(Mathf.RoundToInt(Objective * 0.25f));
	}
}