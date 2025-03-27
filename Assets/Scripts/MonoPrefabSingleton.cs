using UnityEngine;

public abstract class MonoPrefabSingleton<T> : MonoBehaviour where T : MonoPrefabSingleton<T>
{
	private static T mInstance;

	public static T Instance
	{
		get
		{
			if ((Object)mInstance == (Object)null)
			{
				GameObject gameObject = GameObject.Find(typeof(T).ToString());
				if (gameObject == null)
				{
					gameObject = (UnityEngine.Object.Instantiate(Resources.Load(typeof(T).ToString())) as GameObject);
				}
				mInstance = gameObject.GetComponent<T>();
				mInstance.Init();
			}
			return mInstance;
		}
	}

	private void Awake()
	{
		T instance = Instance;
		instance.Create();
	}

	public void Create()
	{
	}

	protected virtual void Init()
	{
	}

	private void OnApplicationQuit()
	{
		if ((Object)mInstance != (Object)null)
		{
			mInstance = (T)null;
		}
	}
}