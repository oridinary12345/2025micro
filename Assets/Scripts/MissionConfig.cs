using System;

[Serializable]
public class MissionConfig
{
	public string Id;

	public MissionType MissionType;

	public float Objective;

	public string ParamStr1;

	public string ParamStr2;

	public int ParamInt1;

	public static MissionConfig Invalid = new MissionConfig
	{
		Id = "missionInvalid"
	};

	public bool IsValid()
	{
		return Id != Invalid.Id;
	}
}