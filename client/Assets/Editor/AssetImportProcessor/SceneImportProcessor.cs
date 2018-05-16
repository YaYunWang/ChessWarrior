using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SceneImportProcessor : AssetPostprocessor 
{
    public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {

        const string SCENE_PATH = "Assets/Scenes/";
        foreach (string str in importedAssets)
        {
            string ext = Path.GetExtension(str).ToLower();
            var importer = AssetImporter.GetAtPath(str);
            if (importer == null)
                continue;

            if (str.StartsWithNonAlloc(SCENE_PATH) && ext.EqualsOrdinal(".unity"))
            {
                importer.assetBundleName = string.Format("scene/scene_{0}.bundle", Path.GetFileNameWithoutExtension(str).ToLower());
            }
        }

    }
}
