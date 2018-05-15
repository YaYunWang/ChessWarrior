using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


public class Utility
{
    public const string AssetBundlesOutputPath = "AssetBundles";

    /// <summary>
    /// 获取带本地传输协议的assetbundle文件路径
    /// </summary>
    /// <returns>The asset bundle LPD isk path.</returns>
    /// <param name="assetBundleName">Asset bundle name.</param>
    /// <param name="sync">If set to <c>true</c> sync.</param>
    public static string GetAssetBundleLPDiskPath(string assetBundleName, bool sync = false)
    {
        string url = string.Format("{0}/{1}/{2}", Application.persistentDataPath, PlatformName, assetBundleName);

#if UNITY_ANDROID
        bool inpersistent = false;
#endif

        if (!File.Exists(url))
        {
            url = string.Format("{0}/{1}/{2}", Application.streamingAssetsPath, PlatformName, assetBundleName);
        }
#if UNITY_ANDROID
        else
        {
            inpersistent = true;
        }
#endif

#if UNITY_ANDROID
        if (!sync && inpersistent)
            return "file://" + url;
        return url;
#else
        if (sync)
            return url;
        return "file://" + url;
#endif

    }

    /// <summary>
    /// 获取本地磁盘文件路径
    /// </summary>
    /// <returns>The asset bundle disk path.</returns>
    /// <param name="assetBundleName">Asset bundle name.</param>
    public static string GetAssetBundleDiskPath(string assetBundleName)
    {
        string path = string.Format("{0}/{1}/{2}", Application.persistentDataPath, PlatformName, assetBundleName);

        if (!File.Exists(path))
        {
            path = string.Format("{0}/{1}/{2}", Application.streamingAssetsPath, PlatformName, assetBundleName);
        }

        return path;
    }


	public static string PlatformName
	{
        get
        {
#if UNITY_EDITOR
			return GetPlatformForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
#else
            return GetPlatformForAssetBundles(Application.platform);
#endif
		}

	}

#if UNITY_EDITOR
	private static string GetPlatformForAssetBundles(BuildTarget target)
	{
		switch (target)
		{
			case BuildTarget.Android:
				return "Android";
			case BuildTarget.iOS:
				return "iOS";
			case BuildTarget.WebGL:
				return "WebGL";
			case BuildTarget.StandaloneWindows:
			case BuildTarget.StandaloneWindows64:
				return "Windows";
			case BuildTarget.StandaloneOSX:
				return "OSX";
			// Add more build targets for your own.
			// If you add more targets, don't forget to add the same platforms to GetPlatformForAssetBundles(RuntimePlatform) function.
			default:
				return null;
		}
	}
#endif

	private static string GetPlatformForAssetBundles(RuntimePlatform platform)
	{
		switch (platform)
		{
			case RuntimePlatform.Android:
				return "Android";
			case RuntimePlatform.IPhonePlayer:
				return "iOS";
			case RuntimePlatform.WebGLPlayer:
				return "WebGL";
			case RuntimePlatform.WindowsPlayer:
				return "Windows";
			case RuntimePlatform.OSXPlayer:
				return "OSX";
			// Add more build targets for your own.
			// If you add more targets, don't forget to add the same platforms to GetPlatformForAssetBundles(RuntimePlatform) function.
			default:
				return null;
		}
	}

    /// <summary>
    /// persistentDataPath 持久文件在各平台的路径 没有“File://”
    /// </summary>
    public static string persistentDataPath
    {
        get
        {
            string url = "";
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            url = Application.dataPath + "/../persistentDataPath";
            if (!Directory.Exists(url))
                Directory.CreateDirectory(url);
#else
            url = Application.persistentDataPath;
#endif
            return url + "/";
        }
    }

    //删除这个目录下的所有资源
    public static void DeleteDir(string dir)
    {
        foreach (string var in Directory.GetDirectories(dir))
        {
            Directory.Delete(var, true);
        }
        foreach (string var in Directory.GetFiles(dir))
        {
            File.Delete(var);
        }
    }
}

