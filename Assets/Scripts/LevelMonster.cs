public class LevelMonster
{
	public string MonsterId;

	public int SpawnLayerMin;

	public int SpawnLayerMax;

	public LevelMonster(string monsterId, int layerMin, int layerMax)
	{
		MonsterId = monsterId;
		SpawnLayerMin = layerMin;
		SpawnLayerMax = layerMax;
	}

	public LevelMonster(string id)
		: this(id, -1, -1)
	{
	}
}