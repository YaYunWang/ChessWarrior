using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[Serializable]
public class AudioCfg
{
    public int ID;
    public string AssetName;
    public bool Loop;
    public float Volume;
    public int Priority;
    public int FadeIn;
    public int FadeOut;
}

public class AudioCfgLoader : CfgLoaderBase
{
    private Dictionary<int, AudioCfg> m_data = new Dictionary<int, AudioCfg>();
    protected override void OnLoad()
    {
        ReadConfig<AudioCfg>("Config/audio_cfg.xml", OnReadRow);
    }

    private void OnReadRow(AudioCfg obj)
    {
        m_data[obj.ID] = obj;
    }

    protected override void OnUnload()
    {
        m_data.Clear();
    }

    public AudioCfg GetConfig(int id)
    {
        AudioCfg config;
        m_data.TryGetValue(id, out config);
        return config;
    }
}