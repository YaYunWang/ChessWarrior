using UnityEngine;
using System.Collections;

public class TrajectoryObject
{
    #region TrajectoryTarget
    struct TrajectoryTarget
    {
        public bool hasTransform;

        public Transform transform;

        public Vector3 position;

        public void Clear()
        {
            hasTransform = false;
            transform = null;
            position = Vector3.zero;
        }

        public void SetTransform(Transform transform)
        {
            hasTransform = true;
            this.transform = transform;
            this.position = transform.position;
        }

        public void SetPosition(Vector3 position)
        {
            hasTransform = false;
            this.position = position;
        }

        public Vector3 GetPosition()
        {
            if (hasTransform && transform != null)
            {
                position = transform.position;
                return position;
            }
            else
            {
                return position;
            }
        }

        public bool IsValid()
        {
            return !hasTransform || transform != null;
        }

    }

    #endregion

    // -------------
    TrajectoryCfg config;
    GameObject gameObject;
    Transform transform;
    Vector3 from;
    TrajectoryTarget target;
    Vector3 to;

    float elapsed = 0;
    float totalTime = 0;

    bool expired = false;
    bool updating = false;

    public float TimeRemain { get { return Mathf.Max(0, totalTime - elapsed); } }

    System.Action onReachTargetCallback = null;

    public bool Expired
    {
        get { return expired; }
    }

    public bool Finished
    {
        get { return expired || !updating; }
    }

    public Vector3 Position
    {
        get { return transform.position; }
    }

    public Quaternion Rotation
    {
        get { return transform.rotation; }
    }

    public Transform Transform
    {
        get { return transform; }
    }
    public TrajectoryObject(Transform root)
    {
        this.gameObject = new GameObject("Trajectory Object");
        this.gameObject.SetActive(false);
        this.transform = this.gameObject.transform;
        this.transform.SetParent(root);
    }

    public void SetConfig(TrajectoryCfg config)
    {
        this.config = config;
    }

    public void Start(Vector3 from, Vector3 to, System.Action onReachTarget = null)
    {
        target.Clear();
        this.from = from;
        target.SetPosition(to);
        this.to = target.GetPosition();
        this.onReachTargetCallback = onReachTarget;
        StartInternal();
    }

    public void Start(Vector3 from, Transform to, System.Action onReachTarget = null)
    {
        target.Clear();
        this.from = from;
        target.SetTransform(to);
        this.to = target.GetPosition();
        this.onReachTargetCallback = onReachTarget;
        StartInternal();
    }

    private void StartInternal()
    {
        expired = false;
        updating = true;
        elapsed = 0;
        gameObject.SetActive(true);
        transform.position = from;

        float distance = Vector3.Distance(from, to);


        switch (config.trajectoryType)
        {
            case TrajectoryTypes.Line:
                var lineParams = (TrajectoryLineParams)config.trajectoryParameters;
                totalTime = distance / lineParams.Speed;
                break;

            case TrajectoryTypes.Parabola:
                Vector3 dir = to - from;
                dir.y = 0;
                var parabolaParams = (TrajectoryParabolaParams)config.trajectoryParameters;
                totalTime = dir.magnitude / parabolaParams.Speed;
                break;

            case TrajectoryTypes.Bezier:
                var bezierParams = (TrajectoryBezierParams)config.trajectoryParameters;
                totalTime = distance / bezierParams.Speed;
                break;

            //case TrajectoryTypes.CatmullRom:
            //    var cutmullRomParams = (TrajectoryCatmullRomParams)config.trajectoryParameters;
            //    totalTime = distance / cutmullRomParams.Speed;
            //    break;
            default:
                Debug.LogErrorFormat("Invalid trajectory type {0} of {1}", config.trajectoryType, config.ID);
                totalTime = 1;
                break;
        }
    }

    // 到达目标后需要调用Stop()
    public void Stop()
    {
        expired = true;
        gameObject.SetActive(false);
        onReachTargetCallback = null;
    }

    public void AlignTransform(Transform trans)
    {
        trans.position = Position;
        trans.rotation = Rotation;
    }

    public void Update()
    {
        if (expired || !updating)
            return;

        if (!target.IsValid())
        {
            updating = false;
            return;
        }

        elapsed += Time.deltaTime;
        if (elapsed >= totalTime)
        {
            if (onReachTargetCallback != null)
            {
                onReachTargetCallback();
                onReachTargetCallback = null;
            }

            elapsed = totalTime;
            updating = false;
            transform.position = target.GetPosition();
            return;
        }

        Vector3 pos = Vector3.zero;

        switch (config.trajectoryType)
        {
            case TrajectoryTypes.Line:
                pos = UpdateLine();
                break;

            case TrajectoryTypes.Parabola:
                pos = UpdateParabola();
                break;

            case TrajectoryTypes.Bezier:
                pos = UpdateBezier();
                break;

            //case TrajectoryTypes.CatmullRom:
            //    pos = UpdateCatmullRom();
                //break;
        }

        Vector3 lastPostion = transform.position;

        if (target.hasTransform)
        {
            float t = elapsed / totalTime;
            pos = Vector3.Lerp(pos, target.GetPosition(), t * t * t);
        }

        transform.position = pos;
        if (pos != lastPostion)
            transform.rotation = Quaternion.LookRotation(pos - lastPostion, Vector3.up);
    }

    private Vector3 UpdateLine()
    {
        return Vector3.Lerp(from, to, elapsed / totalTime);
    }

    private Vector3 UpdateParabola()
    {
        var parabolaParams = (TrajectoryParabolaParams)config.trajectoryParameters;

        return TrajectoryUtil.GetParabolaPoint(from, to, parabolaParams.VAcceleration, elapsed, totalTime);
    }

    private Vector3 UpdateBezier()
    {
        // 在配置空间计算坐标
        float t = elapsed / totalTime;
        var bezierParams = (TrajectoryBezierParams)config.trajectoryParameters;

        return TrajectoryUtil.GetBezierPoint(bezierParams.p0, bezierParams.p1, bezierParams.p2, bezierParams.p3, t, from, to);
    }

    //private Vector3 UpdateCatmullRom()
    //{
    //    float t = elapsed / totalTime;
    //    var catmullRomParams = (TrajectoryCatmullRomParams)config.trajectoryParameters;

    //    return TrajectoryUtil.GetCatmullRomPoint(catmullRomParams.p0, catmullRomParams.p1, catmullRomParams.p2, catmullRomParams.p3, t, catmullRomParams.tension, from, to);
    //}

    public void Destroy()
    {
        UnityEngine.Object.Destroy(gameObject);
        gameObject = null;
    }
}