public class NoMission : Mission
{
	public NoMission(MissionProfile profile)
		: base(profile)
	{
	}

	public override string GetDescription()
	{
		return string.Empty;
	}
}