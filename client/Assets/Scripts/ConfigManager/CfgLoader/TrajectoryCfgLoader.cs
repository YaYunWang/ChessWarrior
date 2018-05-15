using UnityEngine;
using System.Collections.Generic;

public enum TrajectoryTypes
{
    Invalid,
    Line = 1,       // 直线
    Parabola,   // 抛物线
    Bezier,     // 贝塞尔
    //CatmullRom, // Catmull–Rom 样条
}

public class TrajectoryLineParams
{
    public float Speed = 1;
}

public class TrajectoryParabolaParams
{
    public float Speed = 1;
    public float VAcceleration = -10;
}

public class TrajectoryBezierParams
{
    public float Speed = 1;
    public Vector3 p0;
    public Vector3 p1 = Vector3.one;
    public Vector3 p2 = Vector3.one;
    public Vector3 p3;
}

public class TrajectoryCatmullRomParams
{
    public float Speed = 1;
    public Vector3 p0;
    public Vector3 p1 = Vector3.one;
    public Vector3 p2 = Vector3.one;
    public Vector3 p3;
    public float tension = 0.5f;
}

public class TrajectoryCfg
{
    // 配置表数据
    public int ID;
    public string Data;

    // 运行时数据
    public TrajectoryTypes trajectoryType;
    public object trajectoryParameters;
}

public class TrajectoryCfgLoader : CfgLoaderBase
{
    private Dictionary<int, TrajectoryCfg> m_data = new Dictionary<int, TrajectoryCfg>();
    private Dictionary<int, TrajectoryCfg> m_tempData = new Dictionary<int, TrajectoryCfg>();

    protected override void OnLoad()
    {
		ReadConfig<TrajectoryCfg>("Config/trajectory_cfg.xml", OnReadRow);
    }

    protected override void OnUnload()
    {
        m_data.Clear();
        m_tempData.Clear();
    }

    private void OnReadRow(TrajectoryCfg row)
    {
        InitRuntimeData(row);
        m_data[row.ID] = row;
    }

    public static void InitRuntimeData(TrajectoryCfg row)
    {
        row.trajectoryType = TrajectoryTypes.Invalid;
        row.trajectoryParameters = null;

        if (!string.IsNullOrEmpty(row.Data))
        {
            int i = row.Data.IndexOf("=");
            if (i == -1)
                return;

            string typeStr = row.Data.Substring(0, i);
            string paramStr = row.Data.Substring(i + 1);

            int typeInt;
            if (!int.TryParse(typeStr, out typeInt))
                return;

            row.trajectoryType = (TrajectoryTypes)typeInt;
            switch (row.trajectoryType)
            {
                case TrajectoryTypes.Line:
                    var lineParams = JsonUtility.FromJson<TrajectoryLineParams>(paramStr);
                    row.trajectoryParameters = lineParams;
                    if (lineParams.Speed <= 0)
                    {
                        lineParams.Speed = 1;
                        Debug.LogErrorFormat("Invalid trajectory speed {0} of {1}", lineParams.Speed, row.ID);
                    }
                    break;
                case TrajectoryTypes.Parabola:
                    var parabolaParams = JsonUtility.FromJson<TrajectoryParabolaParams>(paramStr);
                    row.trajectoryParameters = parabolaParams;
                    if (parabolaParams.Speed <= 0)
                    {
                        parabolaParams.Speed = 1;
                        Debug.LogErrorFormat("Invalid trajectory speed {0} of {1}", parabolaParams.Speed, row.ID);
                    }
                    if (parabolaParams.VAcceleration >= 0)
                    {
                        parabolaParams.VAcceleration = -9.8f;
                        Debug.LogErrorFormat("Invalid trajectory acceleration {0} of {1}", parabolaParams.VAcceleration, row.ID);
                    }
                    break;
                case TrajectoryTypes.Bezier:
                    var bezierParams = JsonUtility.FromJson<TrajectoryBezierParams>(paramStr);
                    row.trajectoryParameters = bezierParams;
                    if (bezierParams.Speed <= 0)
                    {
                        bezierParams.Speed = 1;
                        Debug.LogErrorFormat("Invalid trajectory speed {0} of {1}", bezierParams.Speed, row.ID);
                    }
                    break;
                //case TrajectoryTypes.CatmullRom:
                //    var catmullRomParams = JsonUtility.FromJson<TrajectoryCatmullRomParams>(paramStr);
                //    row.trajectoryParameters = catmullRomParams;
                //    if (catmullRomParams.Speed <= 0)
                //    {
                //        catmullRomParams.Speed = 1;
                //        Debug.LogErrorFormat("Invalid trajectory speed {0} of {1}", catmullRomParams.Speed, row.ID);
                //    }
                //    break;
                default:
                    row.trajectoryType = TrajectoryTypes.Invalid;
                    row.trajectoryParameters = null;
                    break;
            }
        }
    }

    public TrajectoryCfg GetConfig(int id)
    {
        TrajectoryCfg config;
        if (m_data.TryGetValue(id, out config))
            return config;

        m_tempData.TryGetValue(id, out config);

        return config;
    }

    public bool SetTempConfig(int id, TrajectoryCfg config)
    {
        if (m_data.ContainsKey(id))
        {
            Debug.LogErrorFormat("Can not add trajectory config because of duplicated key {0}", id);
            return false;
        }

        InitRuntimeData(config);
        m_tempData[id] = config;

        return true;
    }
}
