public class RewardConfig
{
	public string Id;

	public RewardType RewardType;

	public string RewardTypeId;

	public string RewardSubTypeId;

	public int AmountMin;

	public int AmountMax;

	public int Weight;

	public float Percentage;

	public int ParamInt1;

	public bool IsCard;

	public override bool Equals(object obj)
	{
		RewardConfig rewardConfig = obj as RewardConfig;
		return rewardConfig.Id == Id;
	}

	public override int GetHashCode()
	{
		return $"{Id}".GetHashCode();
	}
}