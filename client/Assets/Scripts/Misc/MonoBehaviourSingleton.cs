using UnityEngine;


public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T instance;
	private static bool applicationQuiting = false;
	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				CreateInstance();
			}

			return instance;
		}
	}

	public static bool CreateInstance()
	{
		if (applicationQuiting)
			return false;

		if (instance != null)
			return false;

        GameObject singleton = new GameObject(string.Format("_{0}", typeof(T).Name));
		singleton.AddComponent<T>();

		return true;
	}

	protected virtual void OnCreateInstance()
	{

	}

	protected virtual void Awake()
	{
		if (instance == null)
		{
			instance = this as T;
			DontDestroyOnLoad(this);
			OnCreateInstance();
		}
		else
		{
			DestroyImmediate(this);
			DebugLogger.Log(string.Format("Duplicated singlton({0}) in scene", typeof(T).ToString()));
		}
	}

	protected virtual void OnDestroy()
	{
		if (instance == this)
		{
			instance = null;
			applicationQuiting = true;
		}
	}
}



