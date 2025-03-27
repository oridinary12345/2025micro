using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
	private static T _instance;

	public static T Instance
	{
		get
		{
			if ((Object)_instance == (Object)null)
			{
				_instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
			}
			return _instance;
		}
	}

	private void Awake()
	{
		if ((Object)_instance == (Object)null)
		{
			_instance = (this as T);
			_instance.Init();
		}
	}

	public T Create()
	{
		return _instance;
	}

	protected virtual void Init()
	{
	}

	public virtual void Destroy()
	{
		_instance = (T)null;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public static bool IsCreated()
	{
		return (Object)_instance != (Object)null;
	}

	private void OnApplicationQuit()
	{
		_instance = (T)null;
	}
}