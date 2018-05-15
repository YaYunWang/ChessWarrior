using UnityEngine;
using System.Collections.Generic;

public class TrajectoryManager : ManagerTemplateBase<TrajectoryManager>
{
    static TrajectoryCfgLoader trajectoryConfigLoader;

    static List<TrajectoryObject> pool = new List<TrajectoryObject>();
    static LinkedList<TrajectoryObject> activeTrajectories = new LinkedList<TrajectoryObject>();

    public static int TRAJECTORY_POOL_SIZE = 10;

    protected override void InitManager()
    {
        trajectoryConfigLoader = ConfigManager.Get<TrajectoryCfgLoader>();
    }

    private static void Clear(GameEventTypes eventType, object[] args)
    {
        if (activeTrajectories.Count > 0)
        {
            var node = activeTrajectories.First.Value;
            node.Stop();
            ProcessExpiredEffect(node);
            activeTrajectories.RemoveFirst();
        }
    }

    public static TrajectoryObject Play(int id, Vector3 from, Vector3 to, System.Action onReachTarget = null)
    {
        TrajectoryObject trajectoryObj = FetchTrajectoryObject(id);
        if (trajectoryObj == null)
            return null;

        activeTrajectories.AddLast(trajectoryObj);

        trajectoryObj.Start(from, to, onReachTarget);

        return trajectoryObj;
    }

    public static TrajectoryObject Play(int id, Vector3 from, Transform to, System.Action onReachTarget = null)
    {
        TrajectoryObject trajectoryObj = FetchTrajectoryObject(id);
        if (trajectoryObj == null)
            return null;

        activeTrajectories.AddLast(trajectoryObj);

        trajectoryObj.Start(from, to, onReachTarget);

        return trajectoryObj;
    }

    private static TrajectoryObject FetchTrajectoryObject(int id)
    {
        TrajectoryCfg config = trajectoryConfigLoader.GetConfig(id);
        if (config == null)
        {
            Debug.LogErrorFormat("Failed to find config of trajectory {0}", id);
            return null;
        }

        TrajectoryObject trajectoryObj = FetchFromPool();

        if (trajectoryObj == null)
            trajectoryObj = CreateTrajectoryObject();

        trajectoryObj.SetConfig(config);

        return trajectoryObj;
    }

    private static TrajectoryObject FetchFromPool()
    {
        if (pool.Count > 0)
        {
            TrajectoryObject last = pool[pool.Count - 1];
            pool.RemoveAt(pool.Count - 1);

            return last;
        }

        return null;
    }

    private static TrajectoryObject CreateTrajectoryObject()
    {
        return new TrajectoryObject(Instance.transform);
    }

    private static void ProcessExpiredEffect(TrajectoryObject trajectoryObj)
    {
        if (pool.Count < TRAJECTORY_POOL_SIZE)
        {
            pool.Add(trajectoryObj);
        }
        else
        {
            trajectoryObj.Destroy();
        }
    }

    void Update()
    {
        if (activeTrajectories.Count == 0)
            return;

        LinkedListNode<TrajectoryObject> curNode = activeTrajectories.First;

        int count = activeTrajectories.Count;

        for (int i = 0; i < count; i++)
        {
            LinkedListNode<TrajectoryObject> next = curNode.Next;

            curNode.Value.Update();

            if (curNode.Value.Expired)
            {
                ProcessExpiredEffect(curNode.Value);
                activeTrajectories.Remove(curNode);
            }

            curNode = next;
        }
    }
}
