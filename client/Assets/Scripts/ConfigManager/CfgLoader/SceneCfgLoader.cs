using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[Serializable]
public class SceneCfg
{
    public int ID;
    public string Name;
    public string AssetPath;
    public int BGM;
    public int SceneType;

    public float GridWidth;
    public string BornGrid;
    public string BossBornGrid;
}

public class SceneCfgLoader : CfgLoaderBase
{
    private Dictionary<int, SceneCfg> m_data = new Dictionary<int, SceneCfg>();
    protected override void OnLoad()
    {
        ReadConfig<SceneCfg>("Config/scene_cfg.xml", OnReadRow);
    }

    private void OnReadRow(SceneCfg obj)
    {
        m_data[obj.ID] = obj;
    }

    protected override void OnUnload()
    {
        m_data.Clear();
    }

    public SceneCfg GetConfig(int configID)
    {
        SceneCfg config = null;
        m_data.TryGetValue(configID, out config);
        return config;
    }
}