using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class AssetBundleBuilder
{
	public const string AssetBundlesOutputPath = "AssetBundles";

	[MenuItem("Assets/AssetBundles/Build AssetBundles")]
	public static void BuildAssetBundles()
	{
		AssetDatabase.RemoveUnusedAssetBundleNames();

		string outputPath = Path.Combine(AssetBundlesOutputPath, Utility.PlatformName);
		if (!Directory.Exists(outputPath))
			Directory.CreateDirectory(outputPath);

		BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.IgnoreTypeTreeChanges, EditorUserBuildSettings.activeBuildTarget);

		Debug.Log("打包完成.");
	}
}
