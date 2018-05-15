using UnityEngine;
using System.Collections.Generic;
using System;


public delegate void GameEventCallback(GameEventTypes eventType, object[] args);

public class GameEventManager : ManagerTemplateBase<GameEventManager>
{
	class GameEvent : IPoolableObject
	{
        public GameEventTypes eventType;
		public object[] args;

		public void Reset()
		{
            eventType = 0;
			args = null;
		}
	}

    static Dictionary<GameEventTypes, List<GameEventCallback>> eventMap = new Dictionary<GameEventTypes, List<GameEventCallback>>();
	static Queue<GameEvent> eventQueue = new Queue<GameEvent>();
	static Queue<GameEvent> eventBackQueue = new Queue<GameEvent>();
	static ObjectPool<GameEvent> eventPool = new ObjectPool<GameEvent>(20);
	static object eventLock = new object();

	public static bool EnableEventFiring = true;

	static int mainThreadID;

	protected override void InitManager()
	{
		mainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
	}

	public void Update()
	{
		if (!EnableEventFiring)
			return;

		// 处理上回EnableEventFiring=false引起的未完成任务
		while (eventBackQueue.Count > 0)
		{
			GameEvent ev = eventBackQueue.Dequeue();
            RaiseEventNow(ev.eventType, ev.args);

			if (!EnableEventFiring)
				return;
		}

		lock (this)
		{
			if (eventQueue.Count == 0)
				return;

			var temp = eventQueue;
			eventQueue = eventBackQueue;
			eventBackQueue = temp;
		}

		while (eventBackQueue.Count > 0)
		{
			GameEvent ev = eventBackQueue.Peek();
            RaiseEventNow(ev.eventType, ev.args);
			eventBackQueue.Dequeue();           // 统计数量需要

			if (!EnableEventFiring)
				return;
		}
	}

    private static List<GameEventCallback> GetEventCallbackList(GameEventTypes eventType)
    {
        List<GameEventCallback> callbackList = null;

        if (!eventMap.TryGetValue(eventType, out callbackList))
        {
            callbackList = new List<GameEventCallback>();
            eventMap.Add(eventType, callbackList);
        }
        return callbackList;
    }

    public static void RegisterEvent(GameEventTypes eventType, GameEventCallback callback)
	{
		lock (eventLock)
		{
            List<GameEventCallback> callbackList = GetEventCallbackList(eventType);

			callbackList.Add(callback);
		}
	}

    public static void UnregisterEvent(GameEventTypes eventType, GameEventCallback callback)
	{
		lock (eventLock)
		{
            List<GameEventCallback> callbackList = GetEventCallbackList(eventType);

			for (int i = callbackList.Count - 1; i >= 0; i--)
			{
				if (callbackList[i] == callback)
				{
					callbackList.RemoveAt(i);
					break;
				}
			}
		}
	}

    public static void RaiseEvent(GameEventTypes eventType, params object[] args)
	{
		if (System.Threading.Thread.CurrentThread.ManagedThreadId == mainThreadID && EnableEventFiring)
		{
            RaiseEventNow(eventType, args);
		}
		else
		{
            QueueEvent(eventType, args);
		}
	}

    public static void RaiseEventSync(GameEventTypes eventType, params object[] args)
	{
		if (System.Threading.Thread.CurrentThread.ManagedThreadId == mainThreadID)
		{
            RaiseEventNow(eventType, args);
		}
		else
		{
            QueueEvent(eventType, args);
			WaitQueue();
		}
	}

    private static void RaiseEventNow(GameEventTypes eventType, params object[] args)
	{
        List<GameEventCallback> callbackList = GetEventCallbackList(eventType);

		try
		{
			for (int i = 0; i < callbackList.Count; i++)
			{
                callbackList[i].Invoke(eventType, args);
			}
		}
		catch (Exception ex)
		{
			DebugLogger.LogError(ex);
		}
	}

    private static void QueueEvent(GameEventTypes eventType, params object[] args)
	{
		lock (eventLock)
		{
			GameEvent ev = eventPool.Fetch();
            ev.eventType = eventType;
			ev.args = args;

			eventQueue.Enqueue(ev);
		}
	}

	private static void WaitQueue()
	{
		while (true)
		{
			lock (eventLock)
			{
				if (eventQueue.Count + eventBackQueue.Count == 0)
					break;
			}

			System.Threading.Thread.Sleep(0);
		}
	}
}


