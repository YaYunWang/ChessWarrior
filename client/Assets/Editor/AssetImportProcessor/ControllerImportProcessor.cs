using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ControllerImportProcessor : AssetPostprocessor
{
	public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{

		const string CONTROLLER_PATH = "Assets/Art/Controller/";
		foreach (string str in importedAssets)
		{
			string ext = Path.GetExtension(str).ToLower();
			var importer = AssetImporter.GetAtPath(str);
			if (importer == null)
				continue;

			if (str.StartsWithNonAlloc(CONTROLLER_PATH) && ext.EqualsOrdinal(".controller"))
			{
				importer.assetBundleName = string.Format("controller/controller_{0}.bundle", Path.GetFileNameWithoutExtension(str).ToLower());
			}
		}

	}
}
