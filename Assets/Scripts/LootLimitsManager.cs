using System;
using System.Collections.Generic;

public class LootLimitsManager
{
	private readonly Dictionary<string, int> Limits = new Dictionary<string, int>
	{
		{
			"lootRuby",
			50
		}
	};

	private readonly LootLimitProfiles _profiles;

	public LootLimitsManager(LootLimitProfiles profile)
	{
		_profiles = profile;
		Init();
	}

	private void Init()
	{
		foreach (KeyValuePair<string, int> limit in Limits)
		{
			LootLimitProfile profile = GetProfile(limit.Key);
			if (profile == null)
			{
				LootLimitProfile lootLimitProfile = new LootLimitProfile();
				lootLimitProfile.Id = limit.Key;
				lootLimitProfile.Amount = 0;
				lootLimitProfile.TimeStart = new DateTimeJson
				{
					Time = DateTime.UtcNow.Date
				};
				profile = lootLimitProfile;
				_profiles.Loots.Add(profile);
			}
		}
		for (int num = _profiles.Loots.Count - 1; num >= 0; num--)
		{
			if (!Limits.ContainsKey(_profiles.Loots[num].Id))
			{
				_profiles.Loots.RemoveAt(num);
			}
		}
	}

	private LootLimitProfile GetProfile(string lootId)
	{
		return _profiles.Loots.Find((LootLimitProfile p) => p.Id == lootId);
	}

	public bool IsLimitReached(string lootId)
	{
		if (!Limits.ContainsKey(lootId))
		{
			return false;
		}
		LootLimitProfile profile = GetProfile(lootId);
		if (profile == null)
		{
			return false;
		}
		UpdateTime(profile);
		return profile.Amount >= Limits[lootId];
	}

	private void UpdateTime(LootLimitProfile profile)
	{
		DateTime time = profile.TimeStart.Time;
		if (time.AddDays(1.0) <= DateTime.UtcNow.Date)
		{
			Reset(profile);
		}
	}

	private void Reset(LootLimitProfile profile)
	{
		profile.Amount = 0;
		profile.TimeStart.Time = DateTime.UtcNow.Date;
	}
}