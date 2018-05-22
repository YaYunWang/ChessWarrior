using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ModelImportProcessor : AssetPostprocessor
{
	public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		const string MODEL_PATH = "Assets/Art/Model/Chess/";
		foreach (string str in importedAssets)
		{
			var importer = AssetImporter.GetAtPath(str);
			if (importer == null)
				continue;

			string ext = Path.GetExtension(str).ToLower();
			string assetName = Path.GetFileNameWithoutExtension(str);

			if (ext.EqualsOrdinal(".fbx") && assetName.Contains("@"))
			{
				assetName = assetName.Replace("@", "_"); 
				importer.assetBundleName = string.Format("animations/anim_{0}.bundle", assetName);
			}
			else if (ext.EqualsOrdinal(".prefab") && str.StartsWith(MODEL_PATH))
			{
				importer.assetBundleName = string.Format("model/{0}.bundle", assetName);
			}

			if (importer is ModelImporter)
			{
				var modelImporter = importer as ModelImporter;
				modelImporter.isReadable = false;
			}
		}

	}
}
