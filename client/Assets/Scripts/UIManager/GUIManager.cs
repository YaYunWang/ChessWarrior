using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FairyGUI;
using System;

public class PackageInfo
{
    public string packageName;
    public int referencedCount;
    public PackageInfo(string name)
    {
        this.packageName = name;
        this.referencedCount = 1;
    }
}

public class GUIManager : ManagerTemplateBase<GUIManager>
{
	private static Dictionary<string, GUIBase> uiViewDic = new Dictionary<string, GUIBase>();
    private static Dictionary<string, PackageInfo> packageDic = new Dictionary<string, PackageInfo>();

    private static string commandBundleName = "ui/command.bundle";
    private static string commandName = "Command";

    protected override void InitManager()
	{
        UIConfig.defaultFont = "SimHei";
        UIConfig.bringWindowToFrontOnClick = false;

		PreLoadAssetBundle(commandName, commandBundleName);
		//PreLoadAssetBundle("ChessInfo", "ui/chessinfo.bundle");

		GameEventManager.RegisterEvent(GameEventTypes.ExitScene, Clear);
    }

    private void Clear(GameEventTypes eventType, object[] args)
    {
        DestroyAll();
    }

	public static void PreLoadAssetBundle(string path, string bundleName)
	{
        UIPackage.AddPackage(path.ToLower(), (string name, string extension, System.Type type) =>{ return AssetLoadManager.LoadAsset(bundleName, name, type); });
	}

	private void Update()
	{
        foreach (var item in uiViewDic)
        {
            item.Value.Update();
        }
	}

	public static GUIBase GetView(string uiName)
	{
        foreach (var item in uiViewDic)
        {
            if (item.Value.uiName.EqualsOrdinal(uiName))
            {
                return item.Value;
            }
        }
		return null;
	}

	public static T GetView<T>(string uiName) where T : GUIBase
	{
        foreach (var item in uiViewDic)
        {
            if (item.Value.uiName.EqualsOrdinal(uiName))
            {
                return item.Value as T;
            }
        }
        return null;
	}

	public static bool Exist(string uiName)
	{
        foreach (var item in uiViewDic)
        {
            if (item.Value.uiName.EqualsOrdinal(uiName))
            {
                return true;
            }
        }
        return false;
	}

    public static T Open<T>(string packageName, string uiName) where T : GUIBase
    {
        GUIBase gui = GetView<T>(uiName);
        if (null != gui)
        {
            gui.Start();
            return gui as T;
        }
        return CreateView<T>(packageName, uiName);
    }

    public static void Close(string uiName)
    {
        GUIBase ui;
        if (uiViewDic.TryGetValue(uiName, out ui))
        {
            ui.Close();
        }
    }
    
    public static void Destroy(string uiName)
	{
        GUIBase ui;
        if (uiViewDic.TryGetValue(uiName,out ui))
        {
            Destroy(ui);
        }
    }

    public static void Destroy(GUIBase ui)
    {
        PackageInfo package = packageDic[ui.packageName];
        package.referencedCount--;

        ui.Destory();
        uiViewDic.Remove(ui.uiName);

        string packageBundleName = string.Format("ui/{0}.bundle", package.packageName.ToLower());
        AssetLoadManager.UnLoadAssetBundle(packageBundleName);

        if (package.referencedCount == 0)
        {
            UIPackage.RemovePackage(package.packageName);
            packageDic.Remove(package.packageName);
        }
    }

    public static void CloseAll()
    {
        foreach (var pair in uiViewDic)
        {
            pair.Value.Close();
        }
    }

	public static void DestroyAll()
	{
		foreach (var pair in uiViewDic)
		{
            PackageInfo package = packageDic[pair.Value.packageName];
            package.referencedCount--;

            pair.Value.Destory();

            string packageBundleName = string.Format("ui/{0}.bundle", package.packageName.ToLower());
            AssetLoadManager.UnLoadAssetBundle(packageBundleName);

            if (package.referencedCount == 0)
            {
                UIPackage.RemovePackage(package.packageName);
                packageDic.Remove(package.packageName);
            }
        }
		uiViewDic.Clear();
        packageDic.Clear();
    }

	private static T CreateView<T>(string packageName, string uiName) where T : GUIBase
	{
		GUIBase view = System.Activator.CreateInstance(typeof(T)) as GUIBase;
		if (view == null)
            return null;

        string packageBundleName = string.Format("ui/{0}.bundle", packageName.ToLower());
        LoadedAssetBundle loadedassetBundle = AssetLoadManager.LoadAssetBundle(packageBundleName);
        if (loadedassetBundle == null && AssetLoadManager.SimulateAssetBundleInEditor == false)
            return null;

        PackageInfo package;
        if (!packageDic.TryGetValue(packageName,out package))
        {
            package = new PackageInfo(packageName);
            packageDic.Add(packageName, package);
        }
        else
            package.referencedCount++;
        
        UIPackage.AddPackage(packageName, (string name, string extension, System.Type type) => 
        {
            if (loadedassetBundle != null)
                return loadedassetBundle.assetBundle.LoadAsset(name, type);
            else if (AssetLoadManager.SimulateAssetBundleInEditor)
                return AssetLoadManager.LoadAsset(packageBundleName, name, type);
            return null;
        });

        view.uiName = uiName;
        view.packageName = packageName;
        view.Start();
        
        uiViewDic.Add(uiName, view);

		return view as T;
	}
}