public class MonsterMissionData
{
	private Mission _mission;

	public float MissionObjective01 => (_mission == null) ? 0f : _mission.Progress01;

	public string MissionProgress => (_mission == null) ? string.Empty : _mission.GetProgressionText();

	public int CurrentChestLevelProgress => Profile.MissionsCompletedCount % 7;

	public Mission Mission => _mission;

	public string MonsterId => Profile.MonsterId;

	public string MissionDescription => (_mission == null) ? string.Empty : _mission.GetDescriptionCompact();

	public MonsterMissionProfile Profile
	{
		get;
		private set;
	}

	public string WorldId
	{
		get;
		private set;
	}

	public MonsterMissionData(MonsterMissionProfile profile, Mission mission, string worldId)
	{
		Profile = profile;
		WorldId = worldId;
		_mission = mission;
	}

	public void UpdateMission(Mission mission)
	{
		_mission = mission;
	}
}