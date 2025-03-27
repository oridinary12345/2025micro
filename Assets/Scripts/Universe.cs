using DG.Tweening;
using UnityEngine;

public class Universe : MonoBehaviour
{
	private static Universe _instance;

	private App _app;

	public static Universe Instance => _instance;

	public static Universe Create()
	{
		if (_instance != null)
		{
			return _instance;
		}
		return new GameObject("Universe").AddComponent<Universe>();
	}

	private void Awake()
	{
		_instance = this;
		QualitySettings.vSyncCount = 0;
		Object.DontDestroyOnLoad(base.gameObject);
		InitApp();
	}

	private void InitApp()
	{
		_app = App.Create();
		_app.transform.parent = base.transform;
	}

	public void ResetSave()
	{
		GameObject gameObject = GameObject.Find("IronSourceEvents");
		if (gameObject != null)
		{
			UnityEngine.Object.DestroyImmediate(gameObject);
		}
		DOTween.KillAll();
		UnityEngine.Object.DestroyImmediate(_app.gameObject);
		PlayerPrefs.DeleteAll();
		Cloud.Reset();
		UnityEngine.Object.DestroyImmediate(base.gameObject);
		MonoSingleton<SceneSwitcher>.Instance.LoadSceneNoFade("Boot");
	}
}