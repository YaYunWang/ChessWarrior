using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum EffectPriority
{
    Low = 0,        // 常驻低优先级特效，达到限制时会被顶替， 小怪技能特效
    Meidum = 1,     // 参与同屏限制，会顶替Low， 主角技能特效
    High = 2,       // High不参与同屏限制，不计入特效总数. 武器，场景特效
}

[Serializable]
public class EffectCfg
{
    // 配置表数据
    public int ID;
    public string OnBeginPlay;
    public string OnEndPlay;
    public string AssetName;
    public string BindPoint;
    public int Delay;
    public int Lifetime;
    public bool FollowPosition;
    public bool FollowRotation;
    public bool FollowScale;
    public string LocalPosition;
    public string LocalRotation;
    public string LocalScale;
    public int FadeOutTime;
    public int Audio;
    public int Priority;

    public bool InitFollowRotation;

    // 运行时数据
    public int[] OnBeginPlayArray;
    public int[] OnEndPlayArray;
    public Vector3 LocalPositionVec3;
    public Vector3 LocalRotationVec3;
    public Quaternion LocalRotationQuaternion;
    public Vector3 LocalScaleVec3;
}

public class EffectCfgLoader : CfgLoaderBase
{
    private Dictionary<int, EffectCfg> m_data = new Dictionary<int, EffectCfg>();
    protected override void OnLoad()
    {
        ReadConfig<EffectCfg>("Config/effect_cfg.xml", OnReadRow);
    }

    private void OnReadRow(EffectCfg obj)
    {
        InitRuntimeData(obj);
        m_data[obj.ID] = obj;
    }

    protected override void OnUnload()
    {
        m_data.Clear();
    }

    private void InitRuntimeData(EffectCfg row)
    {
        row.OnBeginPlayArray = ConfigParseUtil.ParseIntArray(row.OnBeginPlay);
        row.OnEndPlayArray = ConfigParseUtil.ParseIntArray(row.OnEndPlay);
        row.LocalPositionVec3 = ConfigParseUtil.ParseVec3(row.LocalPosition);
        row.LocalRotationVec3 = ConfigParseUtil.ParseVec3(row.LocalRotation);
        row.LocalRotationQuaternion = Quaternion.Euler(row.LocalRotationVec3);

        row.LocalScaleVec3 = ConfigParseUtil.ParseVec3(row.LocalScale);
        if (row.LocalScaleVec3 == Vector3.zero)
            row.LocalScaleVec3 = Vector3.one;
    }

    public EffectCfg GetConfig(int id)
    {
        EffectCfg config;
        m_data.TryGetValue(id, out config);
        return config;
    }
}