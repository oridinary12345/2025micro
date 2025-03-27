using UnityEngine;

public class GameLoader : MonoSingleton<GameLoader>
{
	protected override void Init()
	{
		base.Init();
		Object.DontDestroyOnLoad(base.gameObject);
	}
}