using UnityEngine;

public abstract class Configs<T> : MonoSingleton<T> where T : Configs<T>
{
	public abstract ConfigType ConfigType
	{
		get;
	}

	protected override void Init()
	{
		base.transform.parent = GameObject.Find("Configs").transform;
	}

	public virtual void LoadFromCSV(CSVFile file)
	{
		UnityEngine.Debug.LogWarning(ConfigType.ToString() + " update ignored...");
	}

	public void LoadFromText(string data)
	{
		CSVFile cSVFile = new CSVFile(data);
		if (cSVFile.IsValid)
		{
			LoadFromCSV(cSVFile);
		}
	}

	protected void LogDiff(object oldValue, object newValue)
	{
		if (oldValue != null && !oldValue.Equals(newValue))
		{
			UnityEngine.Debug.Log("<color=blue>New Value detected! Old value = [" + oldValue + "], new value = [" + newValue + "]</color>");
		}
	}
}