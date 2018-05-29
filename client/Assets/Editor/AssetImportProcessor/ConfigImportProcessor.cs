using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ConfigImportProcessor : AssetPostprocessor
{
	public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string str in importedAssets)
		{
			var importer = AssetImporter.GetAtPath(str);
			if (importer == null)
				continue;
			
			if (!str.StartsWith("Assets/Config/"))
				continue;

			importer.assetBundleName = "config.bundle";
		}
	}
}
