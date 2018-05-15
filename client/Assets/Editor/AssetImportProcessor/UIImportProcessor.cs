using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class UIImportProcessor : AssetPostprocessor
{
	public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		const string PANEL_PATH = "Assets/UI/";

		foreach (string str in importedAssets)
		{
			string ext = Path.GetExtension(str).ToLower();
			var importer = AssetImporter.GetAtPath(str);
			if (importer == null)
				continue;

			if (str.Contains(PANEL_PATH) && (ext == ".bytes" || ext == ".png"))
			{
				string uiResName = Path.GetFileNameWithoutExtension(str);
				string[] uiResNameAry = uiResName.Split('@');

				importer.assetBundleName = string.Format("ui/{0}.bundle", uiResNameAry[0]);
			}
		}
	}
}
