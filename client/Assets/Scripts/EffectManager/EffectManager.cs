using UnityEngine;
using UnityEngine.Profiling;
using System.Collections.Generic;
using System;

public enum EffectRuntimeQuality
{
    Low,
    High,
}

public class EffectManager : ManagerTemplateBase<EffectManager>
{

    public static int MAX_EFFECT_COUNT
    {
        get { return s_maxEffectCount; }
        set { s_maxEffectCount = Mathf.Max(5, value); }
    }

    public static int EFFECT_POOL_SIZE = 30;

    static EffectCfgLoader effectConfigLoader;

    static LinkedList<EffectObject> pool = new LinkedList<EffectObject>();
    static LinkedList<EffectObject> activeEffects = new LinkedList<EffectObject>();
    static List<EffectObject> pending = new List<EffectObject>();
    static bool updating = false;
    static int unimportantEffectsCount = 0;

    static int s_maxEffectCount = 50;



    protected override void InitManager()
    {
        effectConfigLoader = ConfigManager.Get<EffectCfgLoader>();

        GameEventManager.RegisterEvent(GameEventTypes.ExitScene, Clear);
    }

    private static void Clear(GameEventTypes eventType, object[] args)
    {
        LinkedListNode<EffectObject> curNode = activeEffects.First;
        int count = activeEffects.Count;
        for (int i = 0; i < count; i++)
        {
            curNode.Value.Destroy();
            curNode = curNode.Next;
        }

        curNode = pool.First;
        count = pool.Count;
        for (int i = 0; i < count; i++)
        {
            curNode.Value.Destroy();
            curNode = curNode.Next;
        }

        count = pending.Count;
        for (int i = 0; i < count; i++)
        {
            curNode.Value.Destroy();
            curNode = curNode.Next;
        }

        activeEffects.Clear();
        pool.Clear();
        pending.Clear();
        unimportantEffectsCount = 0;
    }

    /// <summary>
    /// 在指定位置播放特效
    /// </summary>
    /// <param name="id"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public static EffectObject Play(int id, Vector3 position, Quaternion rotation, System.Action<EffectObject> onLoad, EffectRuntimeQuality quality = EffectRuntimeQuality.High)
    {
        EffectObject effectObj = FetchEffectObject(id);
        if (effectObj == null)
            return null;

        if (updating)
            pending.Add(effectObj);
        else
            activeEffects.AddLast(effectObj);

        effectObj.Start(position, rotation, onLoad, quality);

        return effectObj;
    }

    public static EffectObject Play(int id, Vector3 position, Quaternion rotation, EffectRuntimeQuality quality = EffectRuntimeQuality.High)
    {
        return Play(id, position, rotation, null, quality);
    }

    /// <summary>
    /// 在指定Transform上播放特效
    /// </summary>
    /// <param name="id"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static EffectObject Play(int id, Transform target, System.Action<EffectObject> onLoad, EffectRuntimeQuality quality = EffectRuntimeQuality.High)
    {
        EffectObject effectObj = FetchEffectObject(id);
        if (effectObj == null)
            return null;

        if (updating)
            pending.Add(effectObj);
        else
            activeEffects.AddLast(effectObj);

        effectObj.Start(target, onLoad, quality);

        return effectObj;
    }

    public static EffectObject Play(int id, Transform target, EffectRuntimeQuality quality = EffectRuntimeQuality.High)
    {
        return Play(id, target, null, quality);
    }

    private static EffectObject FetchEffectObject(int id)
    {
        // 检测特效限制
        EffectCfg config = effectConfigLoader.GetConfig(id);
        if (config == null)
        {
            DebugLogger.LogErrorFormat("[EffectManager]:Failed to find config of effect {0}", id);
            return null;
        }

        if (config.Priority != (int)EffectPriority.High && unimportantEffectsCount >= s_maxEffectCount)
        {
            if (config.Priority == (int)EffectPriority.Meidum)
            {
                if (RemoveALowPriorityEffect() == false)
                    return null;
            }
            else
            {
                // 达到同屏限制
                return null;
            }
        }

        EffectObject effectObj = FetchFromPool(id);

        if (effectObj == null)
            effectObj = CreateEffectObject(config);


        if (config.Priority != (int)EffectPriority.High)
        {
            unimportantEffectsCount++;
        }

        return effectObj;
    }

    private static bool RemoveALowPriorityEffect()
    {
        LinkedListNode<EffectObject> curNode = activeEffects.First;
        int count = activeEffects.Count;

        for (int i = 0; i < count; i++)
        {
            LinkedListNode<EffectObject> next = curNode.Next;

            if (curNode.Value.Config.Priority == (int)EffectPriority.Low && !curNode.Value.IsLoop)
            {
                curNode.Value.StopImmediate();
                return true;
            }

            curNode = next;
        }

        return false;
    }

    private static EffectObject CreateEffectObject(EffectCfg config)
    {
        if (config == null)
            return null;

        Profiler.BeginSample("new EffectObject");
        EffectObject effectObj = new EffectObject(config, Instance.transform);
        Profiler.EndSample();

        return effectObj;
    }

    private static EffectObject FetchFromPool(int id)
    {
        LinkedListNode<EffectObject> curNode = pool.First;

        int count = pool.Count;
        for (int i = 0; i < count; i++)
        {
            LinkedListNode<EffectObject> next = curNode.Next;

            if (curNode.Value.EffectID == id)
            {
                pool.Remove(curNode);
                return curNode.Value;
            }

            curNode = next;
        }

        return null;
    }

    private static void ProcessExpiredEffect(EffectObject effectObj)
    {
        if (effectObj.Config.Priority != (int)EffectPriority.High)
        {
            unimportantEffectsCount--;
        }

        if (pool.Count < EFFECT_POOL_SIZE)
        {
            pool.AddLast(effectObj);
        }
        else
        {
            pool.AddLast(effectObj);
            var first = pool.First;
            pool.RemoveFirst();
            first.Value.Destroy();
        }
    }

    void Update()
    {
        if (activeEffects.Count == 0)
            return;

        updating = true;

        LinkedListNode<EffectObject> curNode = activeEffects.First;

        int count = activeEffects.Count;
        for (int i = 0; i < count; i++)
        {
            Profiler.BeginSample("curNode.Next");
            LinkedListNode<EffectObject> next = curNode.Next;
            Profiler.EndSample();

            Profiler.BeginSample("EffectObjectUpdate");
            curNode.Value.Update();
            Profiler.EndSample();

            if (curNode.Value.Expired)
            {
                Profiler.BeginSample("ProcessExpiredEffect");
                ProcessExpiredEffect(curNode.Value);
                Profiler.EndSample();
                activeEffects.Remove(curNode);
            }

            curNode = next;
        }

        updating = false;

        count = pending.Count;

        while (count-- > 0)
        {
            activeEffects.AddLast(pending[pending.Count - 1]);
            pending.RemoveAt(pending.Count - 1);
        }
    }

    private void LateUpdate()
    {
        if (activeEffects.Count == 0)
            return;

        LinkedListNode<EffectObject> curNode = activeEffects.First;

        int count = activeEffects.Count;
        for (int i = 0; i < count; i++)
        {
            LinkedListNode<EffectObject> next = curNode.Next;
            curNode.Value.LateUpdate();
            curNode = next;
        }
    }

    public static EffectObject Find(int effectID)
    {
        LinkedListNode<EffectObject> curNode = activeEffects.First;

        int count = activeEffects.Count;
        for (int i = 0; i < count; i++)
        {
            if (curNode.Value.EffectID == effectID)
            {
                return curNode.Value;
            }

            curNode = curNode.Next;
        }

        return null;
    }

    public static void SetMaxEffectCount(int count)
    {
        s_maxEffectCount = count;
    }

    private static List<ParticleSystem> activeParticleSystems = new List<ParticleSystem>(100);
    private static int particleSystemCount = 0;

    public static void AddParticleSystems(ParticleSystem[] psArray)
    {
        if (psArray == null || psArray.Length == 0)
            return;

        int index = activeParticleSystems.IndexOf(psArray[0]);
        if (index != -1)
            return;

        if (particleSystemCount > 50)
        {
            for (int i = 0; i < psArray.Length; i++)
            {
                psArray[i].Stop();
            }
            return;
        }

        particleSystemCount += psArray.Length;
        activeParticleSystems.AddRange(psArray);
    }

    public static void RemoveParticleSystem(ParticleSystem[] psArray)
    {
        if (psArray == null || psArray.Length == 0)
            return;

        int index = activeParticleSystems.IndexOf(psArray[0]);
        if (index != -1)
        {
            activeParticleSystems.RemoveRange(index, psArray.Length);
            particleSystemCount -= psArray.Length;
        }
    }
}



