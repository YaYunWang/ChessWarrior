using System.Collections.Generic;
using UnityEngine;


public class AssetBundleLoadTask
{
    public string assetBundleName;
    public string[] dependencies;
    public AssetBundleCreateRequest request;
    public int referencedCount;

    public AssetBundleLoadTask(string assetBundleName, string[] dependencies)
    {
        this.assetBundleName = assetBundleName;
        this.dependencies = dependencies;
        this.referencedCount = 1;
    }
}

public class LoadedAssetBundle
{
    public AssetBundle assetBundle;
    public int referencedCount;

    public LoadedAssetBundle(AssetBundle assetBundle)
    {
        this.assetBundle = assetBundle;
        this.referencedCount = 1;
    }
}

public class AssetLoadManager : ManagerTemplateBase<AssetLoadManager>
{

    static AssetBundleManifest m_AssetBundleManifest = null;
    static bool m_SimulateAssetBundleInEditor = true;

    static Dictionary<string, LoadedAssetBundle> m_LoadedAssetBundles = new Dictionary<string, LoadedAssetBundle>();
    static Dictionary<string, AssetBundleLoadTask> m_LoadAssetBundleTasks = new Dictionary<string, AssetBundleLoadTask>();
    static Dictionary<string, string> m_DownloadingErrors = new Dictionary<string, string>();
    static List<AssetLoadOperation> m_InProgressOperations = new List<AssetLoadOperation>();
    static Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]>();

    static List<string> m_PresaveAssetBundleNames = new List<string>();
    static List<string> m_keysToRemove = new List<string>();

    static float m_gcTimer = 0;
    const float GC_CHECK_TIME = 90;

    public static bool SimulateAssetBundleInEditor
    {
        get
        {
#if !UNITY_EDITOR
            m_SimulateAssetBundleInEditor = false;
#endif
            return m_SimulateAssetBundleInEditor;
        }
        set
        {
            m_SimulateAssetBundleInEditor = value;
        }
    }


    protected override void InitManager()
    {
#if UNITY_EDITOR
        if (SimulateAssetBundleInEditor)
        {

        }
        else
#endif
        {
            m_AssetBundleManifest = LoadAsset(Utility.PlatformName, "AssetBundleManifest", typeof(AssetBundleManifest)) as AssetBundleManifest;
        }
    }

	/// <summary>
	/// PreSave load asset async.
	/// </summary>
	/// <returns>The save load asset async.</returns>
	/// <param name="assetBundleName">Asset bundle name.</param>
	/// <param name="assetName">Asset name.</param>
	/// <param name="type">Type.</param>
	public static AssetBundleLoadAssetOperation PreSaveLoadAssetAsync(string assetBundleName, string assetName, System.Type type)
	{
		PreSaveCollectionDependencies(assetBundleName);
		return LoadAssetAsync(assetBundleName, assetName, type);
	}

	/// <summary>
	/// PreSave load asset.
	/// </summary>
	/// <returns>The save load asset.</returns>
	/// <param name="assetBundleName">Asset bundle name.</param>
	/// <param name="assetName">Asset name.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static T PreSaveLoadAsset<T>(string assetBundleName, string assetName) where T : Object
	{
		PreSaveCollectionDependencies(assetBundleName);
		return LoadAsset<T>(assetBundleName, assetName);
	}

    public static Object PreSaveLoadAsset(string assetBundleName, string assetName, System.Type type)
    {
        PreSaveCollectionDependencies(assetBundleName);
        return LoadAsset(assetBundleName, assetName, type);
    }
	
	/// <summary>
	/// PreSave load assetBundle.
	/// </summary>
	/// <returns>The save load assetBundle.</returns>
	/// <param name="assetBundleName">Asset bundle name.</param>
	public static LoadedAssetBundle PreSaveLoadAssetBundle(string assetBundleName)
    {
        PreSaveCollectionDependencies(assetBundleName);
        return LoadAssetBundle(assetBundleName);
    }
	
	private static void PreSaveCollectionDependencies(string assetBundleName)
    {
        if (!m_PresaveAssetBundleNames.Contains(assetBundleName))
            m_PresaveAssetBundleNames.Add(assetBundleName);

        string[] dependencies = null;
        if (m_AssetBundleManifest != null)
        {
            if (!m_Dependencies.TryGetValue(assetBundleName, out dependencies))
            {
                dependencies = m_AssetBundleManifest.GetDirectDependencies(assetBundleName);
                m_Dependencies.Add(assetBundleName, dependencies);
            }
        }

        if (dependencies != null)
        {
            int length = dependencies.Length;
            for (int i = 0; i < length; i++)
            {
                PreSaveCollectionDependencies(dependencies[i]);
            }
        }
    }
	
	public static LoadedAssetBundle LoadAssetBundle(string assetBundleName)
    {
#if UNITY_EDITOR
        if (SimulateAssetBundleInEditor)
        {
            return null;
        }
        else
#endif
        {
            CollectionDependenciesLoad(assetBundleName);
            string error;
            LoadedAssetBundle loadedAssetBundle = GetLoadedAssetBundle(assetBundleName, out error);
            if (loadedAssetBundle == null || !string.IsNullOrEmpty(error))
            {
                DebugLogger.LogErrorFormat("[AssetLoadManager]:Failed to load assetbundle. {0}", error);
            }
            return loadedAssetBundle;
        }
    }

	/// <summary>
    /// Loads the level async.
    /// </summary>
    /// <returns>The level async.</returns>
    /// <param name="assetBundleName">Asset bundle name.</param>
    /// <param name="levelName">Level name.</param>
    /// <param name="isAdditive">If set to <c>true</c> is additive.</param>
    public static AssetLoadOperation LoadLevelAsync(string assetBundleName, string levelName, bool isAdditive)
	{
		AssetLoadOperation operation = null;
#if UNITY_EDITOR
		if (SimulateAssetBundleInEditor)
		{
			operation = new AssetBundleLoadLevelSimulationOperation(assetBundleName, levelName, isAdditive);
		}
		else
#endif
		{
			CollectionDependenciesLoadTask(assetBundleName);

			operation = new AssetBundleLoadLevelOperation(assetBundleName, levelName, isAdditive);

			m_InProgressOperations.Add(operation);
		}

		return operation;
	}

    /// <summary>
    /// Loads the asset async.
    /// </summary>
    /// <returns>The asset async.</returns>
    /// <param name="assetBundleName">Asset bundle name.</param>
    /// <param name="assetName">Asset name.</param>
    /// <param name="type">Type.</param>
    public static AssetBundleLoadAssetOperation LoadAssetAsync(string assetBundleName, string assetName, System.Type type)
    {
        
        AssetBundleLoadAssetOperation operation = null;
#if UNITY_EDITOR
		if (SimulateAssetBundleInEditor)
		{
			string[] assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, assetName);
			if (assetPaths.Length == 0)
			{
                DebugLogger.LogWarning("[AssetLoadManager]:There is no asset with name \"" + assetName + "\" in " + assetBundleName);
				return null;
			}

			// @TODO: Now we only get the main object from the first asset. Should consider type also.
			Object target = UnityEditor.AssetDatabase.LoadAssetAtPath(assetPaths[0], type);
			operation = new AssetBundleLoadAssetSimulationOperation(target);
		}
		else
#endif
		{
            CollectionDependenciesLoadTask(assetBundleName);

            operation = new AssetBundleLoadAssetFullOperation(assetBundleName, assetName, type);

            m_InProgressOperations.Add(operation);

		}
		return operation;
    }

    /// <summary>
    /// Loads the asset.
    /// </summary>
    /// <returns>The asset.</returns>
    /// <param name="assetBundleName">Asset bundle name.</param>
    /// <param name="assetName">Asset name.</param>
    /// <param name="type">Type.</param>
    public static Object LoadAsset(string assetBundleName, string assetName, System.Type type)
    {
#if UNITY_EDITOR
        if(SimulateAssetBundleInEditor)
        {
			string[] assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, assetName);
			if (assetPaths.Length == 0)
			{
                DebugLogger.LogWarning("[AssetLoadManager]:There is no asset with name \"" + assetName + "\" in " + assetBundleName);
				return null;
			}

			// @TODO: Now we only get the main object from the first asset. Should consider type also.
			Object target = UnityEditor.AssetDatabase.LoadAssetAtPath(assetPaths[0], type);
            return target;
        }
        else
#endif
        {
            CollectionDependenciesLoad(assetBundleName);
            string error;
            LoadedAssetBundle loadedAssetBundle = GetLoadedAssetBundle(assetBundleName, out error);
			if (loadedAssetBundle == null || !string.IsNullOrEmpty(error))
			{
                DebugLogger.LogErrorFormat("[AssetLoadManager]:Failed to load assetbundle. {0}", error);
				return null;
			}

            return loadedAssetBundle.assetBundle.LoadAsset(assetName, type);
        }

    }

    /// <summary>
    /// Loads the asset.
    /// </summary>
    /// <returns>The asset.</returns>
    /// <param name="assetBundleName">Asset bundle name.</param>
    /// <param name="assetName">Asset name.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static T LoadAsset<T>(string assetBundleName, string assetName) where T : Object
    {
        return LoadAsset(assetBundleName, assetName, typeof(T)) as T;
    }

    /// <summary>
    /// Unload asset bundle.
    /// </summary>
    /// <param name="assetBundleName">Asset bundle name.</param>
    public static void UnLoadAssetBundle(string assetBundleName)
    {
#if UNITY_EDITOR
		// If we're in Editor simulation mode, we don't have to load the manifest assetBundle.
		if (SimulateAssetBundleInEditor)
			return;
#endif

        if (m_PresaveAssetBundleNames.Contains(assetBundleName))
            return;

		string[] dependencies = null;

        m_Dependencies.TryGetValue(assetBundleName, out dependencies);

		if (dependencies != null)
		{
			int length = dependencies.Length;
			string depend = "";
			for (int i = 0; i < length; i++)
			{
				depend = dependencies[i];
                UnLoadAssetBundle(depend);
			}
		}

		string error;

		LoadedAssetBundle bundle = GetLoadedAssetBundle(assetBundleName, out error);

        if (bundle != null)
        {
			if (--bundle.referencedCount == 0)
			{
                bundle.assetBundle.Unload(true);
				m_LoadedAssetBundles.Remove(assetBundleName);

				m_Dependencies.Remove(assetBundleName);

				DebugLogger.LogFormat("[AssetLoadManager]:{0} has been unloaded successfully", assetBundleName);
			}
            return;
        }

        AssetBundleLoadTask loadTask = null;
        if (m_LoadAssetBundleTasks.TryGetValue(assetBundleName,out loadTask))
        {
            if(--loadTask.referencedCount == 0)
            {
				m_LoadAssetBundleTasks.Remove(assetBundleName);
            }
        }
	}


    /// <summary>
    /// Gets the loaded asset bundle.
    /// </summary>
    /// <returns>The loaded asset bundle.</returns>
    /// <param name="assetBundleName">Asset bundle name.</param>
    /// <param name="error">Error.</param>
    public static LoadedAssetBundle GetLoadedAssetBundle(string assetBundleName, out string error)
    {
		if (m_DownloadingErrors.TryGetValue(assetBundleName, out error))
			return null;

		LoadedAssetBundle bundle = null;

		if (m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle))
			return bundle;

		return null;
    }

    /// <summary>
    /// Collections the dependencies load.
    /// </summary>
    /// <param name="assetBundleName">Asset bundle name.</param>
    private static void CollectionDependenciesLoad(string assetBundleName)
    {
		if (m_DownloadingErrors.ContainsKey(assetBundleName))
		{
			DebugLogger.LogErrorFormat("[AssetLoadManager]:load {0} error", assetBundleName);
			return;
		}

		string[] dependencies = null;
        if (m_AssetBundleManifest != null)
        {
            if (!m_Dependencies.TryGetValue(assetBundleName, out dependencies))
            {
                dependencies = m_AssetBundleManifest.GetDirectDependencies(assetBundleName);
                m_Dependencies.Add(assetBundleName, dependencies);
            }
        }
		
		if (dependencies != null)
		{
			int length = dependencies.Length;
			for (int i = 0; i < length; i++)
			{
				CollectionDependenciesLoad(dependencies[i]);
			}
		}

		if (m_LoadedAssetBundles.ContainsKey(assetBundleName))
        {
            m_LoadedAssetBundles[assetBundleName].referencedCount++;
            return;
        }

        string url = Utility.GetAssetBundleDiskPath(assetBundleName);
        
        AssetBundle bundle = AssetBundle.LoadFromFile(url);
        if (bundle == null)
        {
            m_DownloadingErrors.Add(assetBundleName, string.Format("[AssetLoadManager]:{0} is not a valid asset bundle.", assetBundleName));
            return;
        }

        m_LoadedAssetBundles.Add(assetBundleName, new LoadedAssetBundle(bundle));

	}

    /// <summary>
    /// Collections the dependencies load task.
    /// </summary>
    /// <param name="assetBundleName">Asset bundle name.</param>
    private static void CollectionDependenciesLoadTask(string assetBundleName)
    {
        if(m_DownloadingErrors.ContainsKey(assetBundleName))
        {
            DebugLogger.LogErrorFormat("[AssetLoadManager]:load {0} error",assetBundleName);
            return;
        }

		string[] dependencies = null;
		if (!m_Dependencies.TryGetValue(assetBundleName, out dependencies))
		{
			dependencies = m_AssetBundleManifest.GetDirectDependencies(assetBundleName);
			m_Dependencies.Add(assetBundleName, dependencies);
		}

		if (dependencies != null)
		{
			int length = dependencies.Length;
			for (int i = 0; i < length; i++)
			{
				CollectionDependenciesLoadTask(dependencies[i]);
			}
		}

		if (m_LoadedAssetBundles.ContainsKey(assetBundleName))
		{
			m_LoadedAssetBundles[assetBundleName].referencedCount++;
			return;
		}

        if(m_LoadAssetBundleTasks.ContainsKey(assetBundleName))
        {
            m_LoadAssetBundleTasks[assetBundleName].referencedCount++;
            return;
        } 
           
		m_LoadAssetBundleTasks.Add(assetBundleName, new AssetBundleLoadTask(assetBundleName, dependencies));
    }

    /// <summary>
    /// Checks all dependencies loaded.
    /// </summary>
    /// <returns><c>true</c>, if all dependencies loaded was checked, <c>false</c> otherwise.</returns>
    /// <param name="task">Task.</param>
	private static bool CheckAllDependenciesLoaded(AssetBundleLoadTask task)
	{
        if (task.dependencies == null || task.dependencies.Length == 0)
			return true;

		bool ok = true;

		for (int i = 0; i < task.dependencies.Length; i++)
		{
			if (m_LoadAssetBundleTasks.ContainsKey(task.dependencies[i]))
			{
				ok = false;
				break;
			}
		}

		return ok;
	}

	void Update()
	{
        // Collect all the finished WWWs.
        m_keysToRemove.Clear();
		foreach (var keyValue in m_LoadAssetBundleTasks)
		{
			AssetBundleLoadTask loadTask = keyValue.Value;

            if (loadTask.request == null)
			{
				if (CheckAllDependenciesLoaded(loadTask))
				{
                    string url = Utility.GetAssetBundleDiskPath(loadTask.assetBundleName);
                    loadTask.request = AssetBundle.LoadFromFileAsync(url);
				}
			}
            else
            {
				// If downloading succeeds.
                if (loadTask.request.isDone)
                {
                    AssetBundle bundle = loadTask.request.assetBundle;
					if (bundle == null)
					{
						m_DownloadingErrors.Add(keyValue.Key, string.Format("[AssetLoadManager]:{0} is not a valid asset bundle.", keyValue.Key));
						m_keysToRemove.Add(keyValue.Key);
						continue;
					}

                    LoadedAssetBundle loadedAssetBundle = new LoadedAssetBundle(bundle);
                    loadedAssetBundle.referencedCount = loadTask.referencedCount;

                    m_LoadedAssetBundles.Add(keyValue.Key, loadedAssetBundle);
					m_keysToRemove.Add(keyValue.Key);
				}
            }
		}

		// Remove the finished WWWs.
		foreach (var key in m_keysToRemove)
		{
			AssetBundleLoadTask loadTask = m_LoadAssetBundleTasks[key];
			m_LoadAssetBundleTasks.Remove(key);
		}

        m_keysToRemove.Clear();

		// Update all in progress operations
		for (int i = 0; i < m_InProgressOperations.Count;)
		{
			if (!m_InProgressOperations[i].Update())
			{
				m_InProgressOperations.RemoveAt(i);
			}
			else
				i++;
		}

        AssetLoadGC();
	}

    public static void OnDispose()
    {
        //m_keysToRemove.Clear();

        //foreach (var pair in m_LoadedAssetBundles)
        //{
        //    if (m_PresaveAssetBundleNames.Contains(pair.Key))
        //        continue;
            
        //    m_keysToRemove.Add(pair.Key);
        //}

        //for (int i = 0; i < m_keysToRemove.Count; i++)
        //{
        //    var bundle = m_LoadedAssetBundles[m_keysToRemove[i]];
        //    m_LoadedAssetBundles.Remove(m_keysToRemove[i]);
        //    bundle.assetBundle.Unload(false);
        //}

        m_LoadAssetBundleTasks.Clear();
        m_InProgressOperations.Clear();
        m_keysToRemove.Clear();

        Resources.UnloadUnusedAssets();
    }

    private static void AssetLoadGC()
    {
        m_gcTimer += Time.deltaTime;
        if (m_gcTimer <= GC_CHECK_TIME)
            return;
        
        m_gcTimer = 0;

        //Resources.UnloadUnusedAssets();
    }

} // End of AssetBundleManager.



