using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 定时器模式
/// </summary>
public class Timer : System.IDisposable, IPooledObjSupporter
{
	//  名称
	private string m_name = string.Empty;
	public string Name { get { return m_name; } }

	//  模式
	private int m_count = 0;

	//  定时器时长
	private float m_duration = 0.0f;

    //  剩余时间
	private float m_leftTime = 0.0f;

	//  定时器委托
	private Callback<object[]> m_callback = null;

    //  参数列表
	private object[] m_args = null;

    /// <summary>
    /// 初始化函数
    /// </summary>
	public void Initialize(string name, int count, float duration, Callback<object[]> handler, params object[] args)
	{
		//Debugging.Log("add time " + name);
		m_name = name;
		m_count = count;
		m_duration = duration;
		m_leftTime = duration;
		m_callback = handler;
		m_args = args;
	}

	/// <summary>
	/// 运行事件
	/// </summary>
	public bool Run(float delta)
	{
		if (null == m_callback)
		{
			DebugLogger.LogError("time null == m_callback " + m_name);
			return false;
		}

		m_leftTime -= delta;
		if (m_leftTime > 0.0f)
			return true;

		if (m_count >= 0)
		{
			if ((1 == m_count) || (0 == m_count))
			{
				m_callback(m_args);

				// 通知删除定时器
				return false;
			}
			--m_count;
		}

		m_callback(m_args);
		m_leftTime += m_duration;
		return true;
	}

    /// <summary>
    /// 重置
    /// </summary>
    public void Reset()
    {
		//Debugging.Log("del time " + m_name);
		m_name = string.Empty;
        m_count = 0;
        m_duration = 0.0f;
        m_leftTime = 0.0f;
        m_callback = null;
        m_args = null;
    }

    /// <summary>
    /// 释放
    /// </summary>
    public void Dispose()
    {
        TimerManager.Instance.DestroyTimeObj(this);
    }
}

/// <summary>
/// 定时管理器
/// </summary>
public class TimerManager : ManagerTemplateBase<TimerManager>
{
	private ObjectPool m_objectPool = null;
	private LinkedList<Timer> m_TimerList = new LinkedList<Timer>();
	private LinkedList<Timer> m_TimerPool = new LinkedList<Timer>();

    /// <summary>
    /// 初始化
    /// </summary>
	protected override void InitManager()
    {
        m_objectPool = new ObjectPool();
        m_objectPool.Initialize(typeof(Timer), null, 20, 2000);
	}

	protected LinkedListNode<Timer> m_curTimer = null;

    /// <summary>
    /// 更新定时器
    /// </summary>
	private void Update()
	{
		m_curTimer = m_TimerList.First;
		while (null != m_curTimer)
		{
            var cur_timer = m_curTimer;
            m_curTimer = m_curTimer.Next;

            try
            {
                if (!cur_timer.Value.Run(Time.deltaTime))
                {
                    if (m_TimerList == cur_timer.List)
                    {
						Internal_RemoveTimer(cur_timer);
                    }
                }
            }
            catch (System.Exception ex)
            {
				DebugLogger.LogException(ex);

                // 回调对象被删除
                if (m_TimerList == cur_timer.List)
                {
					Internal_RemoveTimer(cur_timer);
                }
            }
        }
	}

	/// <summary>
	/// 增加定时器
	/// </summary>
	public bool AddOnceTimer(string key, float duration, Callback<object[]> handler, params object[] args)
	{
		return Internal_AddTimer(key, 1, duration, handler, args);
	}

    /// <summary>
    /// 增加计数定时器
    /// </summary>
	public bool AddCountTimer(string key, float duration, Callback<object[]> handler, uint count, params object[] args)
	{
		return Internal_AddTimer(key, (int)count, duration, handler, args);
	}

	/// <summary>
	/// 增加持续定时器
	/// </summary>
	public bool AddRepeatTimer(string key, float duration, Callback<object[]> handler, params object[] args)
	{
		return Internal_AddTimer(key, -1, duration, handler, args);
	}

	/// <summary>
	/// 销毁带有前缀的所有定时器
	/// </summary>
	public void ClearTimerWithPrefix(string prefix)
	{
		var timerNode = m_TimerList.First;
		while (null != timerNode)
		{
			var curTimerNode = timerNode;
			timerNode = timerNode.Next;

			if (curTimerNode.Value.Name.StartsWith(prefix))
			{
				Internal_RemoveTimer(curTimerNode);
			}
		}
	}
    /// <summary>
    /// 重置指定定时器
    /// </summary>
    /// <param name="key"></param>
    public bool ResetTimer(string key)
    {
        bool isReset = false;
        var timerNode = m_TimerList.First;
        while (null != timerNode)
        {
            var curTimerNode = timerNode;
            timerNode = timerNode.Next;
            if (curTimerNode.Value.Name == key)
            {
                curTimerNode.Value.Reset();
                return true;
            }
        }
        return isReset;
    }

	/// <summary>
	/// 销毁指定定时器
	/// </summary>
    public bool RemoveTimer(string key)
	{
		bool isDestroy = false;
		var timerNode = m_TimerList.First;
		while (null != timerNode)
		{
			var curTimerNode = timerNode;
			timerNode = timerNode.Next;

			if (curTimerNode.Value.Name == key)
			{
				if (m_curTimer == curTimerNode)
					m_curTimer = m_curTimer.Next;

				Internal_RemoveTimer(curTimerNode);
				isDestroy = true;
			}
		}
		return isDestroy;
	}

	public void RemoveTimer(Timer timer)
	{
		var node = m_TimerList.Find(timer);
		Internal_RemoveTimer(node);
	}

	private void Internal_RemoveTimer(LinkedListNode<Timer> node)
	{
		node.Value.Dispose();
		node.Value = null;

		m_TimerList.Remove(node);

		if (m_TimerPool.Count < 50)
			m_TimerPool.AddLast(node);
	}

    /// <summary>
    /// 销毁时间对象
    /// </summary>
    public void DestroyTimeObj(Timer timer)
    {
		if (null == timer)
			return;

        m_objectPool.GiveBackObject(timer.GetHashCode());
    }

    /// <summary>
    /// 从对象池创建时间对象
    /// </summary>
    private Timer CreateObj()
    {
        Timer newObj = (m_objectPool.GetObject() as Timer);
        return newObj;
    }

	/// <summary>
	/// 增加定时器
	/// </summary>
	private bool Internal_AddTimer(string key, int count, float duration, Callback<object[]> handler, params object[] args)
	{
		if (string.IsNullOrEmpty(key))
			return false;

		if (duration < 0.0f)
			return false;

		Timer timer = CreateObj();
        if (timer == null)
        {
            DebugLogger.Log("Create Timer is Null!");
            return false;
        }

		timer.Initialize(key, count, duration, handler, args);

		if (m_TimerPool.Count > 0)
		{
			var node = m_TimerPool.First;
			m_TimerPool.Remove(node);
			node.Value = timer;
			m_TimerList.AddFirst(node);
		}
		else
		{
			m_TimerList.AddFirst(timer);
		}
		return true;
	}

    /// <summary>
    /// 是否在运行
    /// </summary>
	public bool IsRunning(string key)
	{
		var timerNode = m_TimerList.First;
		while (null != timerNode)
		{
			var curTimerNode = timerNode;
			timerNode = timerNode.Next;

			if (curTimerNode.Value.Name == key)
			{
				return true;
			}
		}
		return false;
	}
}
