using System.Collections.Generic;

public class AdventureLevelConfig
{
	public string Id;

	public string WorldId;

	public string Title;

	public int Index;

	public List<WaveConfig> Waves = new List<WaveConfig>();

	public override bool Equals(object obj)
	{
		HeroConfig heroConfig = obj as HeroConfig;
		return heroConfig.Id == Id;
	}

	public override int GetHashCode()
	{
		return $"{Id}".GetHashCode();
	}

	public int GetLastBossIndex()
	{
		return Waves.FindLastIndex((WaveConfig e) => e.Type == WaveType.Boss);
	}
}
