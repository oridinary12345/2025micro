public static class MissionFactory
{
	public static Mission Create(MissionProfile profile, MissionConfig config)
	{
		if (profile != null && config.IsValid())
		{
			profile.Objective = config.Objective;
			switch (profile.MissionType)
			{
			case MissionType.MonsterKill:
				return new MissionKillMonsters(profile, config);
			case MissionType.None:
				return new NoMission(profile);
			}
		}
		return new NoMission(new MissionProfile());
	}
}