using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoSingleton<SceneSwitcher>
{
	protected override void Init()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		base.Init();
	}

	public void LoadScene(string sceneName)
	{
		LoadSceneNoFade(sceneName);
	}

	public void LoadSceneNoFade(string sceneName)
	{
		DOTween.KillAll();
		SceneManager.LoadScene(sceneName);
	}
}