using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public delegate void CallBackObjPool();
/// <summary>
/// 
/// </summary>
public class ObjectPool
{
    private Type m_destType = null;
    private object[] m_ctorArgs = null;
    private int m_minObjCount = 0;
    private int m_maxObjCount = 0;
	private int m_idleObjCount = 0;
    private int m_shrinkPoint = 0;
    private Hashtable m_hashTableObjs = new Hashtable();
    //private Hashtable m_hashTableStatus = new Hashtable();

	private Dictionary<int, bool> m_hashTableStatus = new Dictionary<int,bool>();
	//private ArrayList m_keyList = new ArrayList();
	private QuickList<int> m_keyList = new QuickList<int>();
    private bool m_supportReset = false;

    public event CallBackObjPool PoolShrinked;
    public event CallBackObjPool MemoryUseOut;

    /// <summary>
    /// 初始化对象池
    /// </summary>
    /// <param name="objType">对象类型</param>
    /// <param name="cArgs">构造参数</param>
    /// <param name="minNum">最小值</param>
    /// <param name="maxNum">最大值</param>
    /// <returns></returns>
    public bool Initialize(Type objType, object[] cArgs, int minNum, int maxNum)
    {
        if (minNum < 1)
            minNum = 1;
        if (maxNum < 5)
            maxNum = 5;

        m_destType = objType;
        m_ctorArgs = cArgs;
        m_minObjCount = minNum;
        m_maxObjCount = maxNum;

        double cof = 1 - ((double)minNum / (double)maxNum);
        this.m_shrinkPoint = (int)(cof * minNum);
		this.m_idleObjCount = 0;
        Type supportType = typeof(IPooledObjSupporter);
        if (supportType.IsAssignableFrom(objType))
        {
            this.m_supportReset = true;
        }

        //this.InstanceObjects();

        return true;
    }

    /// <summary>
    /// 批量实例化对象
    /// </summary>
    private void InstanceObjects()
    {
        for (int n = 0; n < m_minObjCount; ++n)
        {
            CreateOneObject();
        }
    }

	/// <summary>
	/// 重置对象池
	/// </summary>
	public void RestPool()
	{
		this.Shrink();
	}

    /// <summary>
    ///  创建一个Object对象
    /// </summary>
    /// <returns></returns>
    private int CreateOneObject()
    {
        object obj = null;

        try
        {
            obj = Activator.CreateInstance(m_destType, m_ctorArgs);
        }
        catch (Exception)
        {
            m_maxObjCount = CurrentObjCount;
            if (m_minObjCount > CurrentObjCount)
                m_minObjCount = CurrentObjCount;
            if (MemoryUseOut != null)
                MemoryUseOut();
            return -1;
        }

        int key = obj.GetHashCode();
        m_hashTableObjs.Add(key, obj);
        m_hashTableStatus.Add(key, true);
        m_keyList.Add(key);
		m_idleObjCount++;
        return key;
    }

    /// <summary>
    /// 销毁一个对象
    /// </summary>
    /// <param name="key"></param>
    private void DestroyOneObject(int key)
    {
        object target = m_hashTableObjs[key];
        IDisposable disposable = target as IDisposable;
        if (disposable != null)
            disposable.Dispose();
		if (this.m_hashTableStatus[key])
		{
			this.m_idleObjCount--;
		}
        m_hashTableObjs.Remove(key);
        m_hashTableStatus.Remove(key);
        m_keyList.Remove(key);
    }

    /// <summary>
    ///  获取一个Object
    /// </summary>
    /// <returns></returns>
    public object GetObject()
    {
		ObjectPool pool = this;
		lock (pool)
		{
			object target = null;
			int num = -1;
			int keysCount = m_keyList.Count;
			for (int n = 0; n < keysCount; ++n)
			{
				num = m_keyList[n];
				if (m_hashTableStatus[num])
				{
					m_hashTableStatus[num] = false;
					this.m_idleObjCount--;
					target = m_hashTableObjs[num];
					break;
				}
			}

			if (target == null)
			{
                if (keysCount < m_maxObjCount)
                {
                    num = this.CreateOneObject();
                    if (num != -1)
                    {
                        m_hashTableStatus[num] = false;
                        this.m_idleObjCount--;
                        target = m_hashTableObjs[num];
                    }
                }
                //else if(keysCount < m_maxObjCount * 1.5)
                //{
                //    num = this.CreateOneObject();
                //    if (num != -1)
                //    {
                //        m_hashTableStatus[num] = false;
                //        this.m_idleObjCount--;
                //        target = m_hashTableObjs[num];
                //    }
                //    Debugging.LogError("Error:Object Pool Above MaxCout." + m_destType.ToString());
                //}
			}

			return target;
		}
    }

    /// <summary>
    /// 将对象退回对象池
    /// </summary>
    /// <param name="objHashCode"></param>
    public void GiveBackObject(int objHashCode)
    {
		if (!m_hashTableStatus.ContainsKey(objHashCode) || m_hashTableStatus[objHashCode])
            return;

		 ObjectPool pool = this;
		 lock (pool)
		 {
			 if (m_hashTableStatus.ContainsKey(objHashCode) && !m_hashTableStatus[objHashCode])
			 {
				 m_hashTableStatus[objHashCode] = true;
				 m_idleObjCount++;
				 if (m_supportReset)
				 {
					 try
					 {
						 IPooledObjSupporter supporter = (IPooledObjSupporter)m_hashTableObjs[objHashCode];
						 supporter.Reset();
					 }
					 catch (Exception ex)
					 {
						 DebugLogger.LogException(ex);
					 }
				 }

				 if (CanShrink())
					 Shrink();
			 }
		 }
    }

    /// <summary>
    /// 是否通收缩
    /// </summary>
    /// <returns></returns>
    private bool CanShrink()
    {
        int idleCount = GetIdleObjCount();
        int busyCount = CurrentObjCount - idleCount;
        return (busyCount < m_shrinkPoint) && (this.CurrentObjCount > (this.MinObjCount + (this.MaxObjCount - this.MinObjCount) / 2));
    }

	/// <summary>
	/// 检查对象状态
	/// </summary>
	/// <param name="hashCode"></param>
	/// <returns></returns>
	public bool CheckObjectStatus(int hashCode)
	{
		if (!this.m_hashTableStatus.ContainsKey(hashCode))
		{
			return false;
		}
		return this.m_hashTableStatus[hashCode];
	}

    /// <summary>
    /// 收缩
    /// </summary>
    private void Shrink()
    {
		int index = 0;
		while (index < this.m_keyList.m_size)
		{
			int key = this.m_keyList[index];
			if (this.m_hashTableStatus[key])
			{
				this.DestroyOneObject(key);
			}
			else
			{
				index++;
			}

			if (this.CurrentObjCount <= this.MinObjCount)
			{
				break;
			}
		}

        if (PoolShrinked != null)
            PoolShrinked();
    }

    /// <summary>
    /// 销毁
    /// </summary>
    public void Dispose()
    {
        Type supType = typeof(System.IDisposable);
        if (supType.IsAssignableFrom(m_destType))
        {
			int num = this.m_keyList.Count - 1;
			for (int n = num; n >= 0; n--)
			{
				this.DestroyOneObject(this.m_keyList[n]);
			}

			//ArrayList list = (ArrayList)this.m_keyList.Clone();
			//for (int n = 0; n < list.Count; ++n)
			//{
			//	this.DestroyOneObject((int)list[n]);
			//}
        }

        m_hashTableStatus.Clear();
        m_hashTableObjs.Clear();
        m_keyList.Clear();
    }

    /// <summary>
    /// 最小对象数
    /// </summary>
    public int MinObjCount
    {
        get{return m_minObjCount;}
    }

    /// <summary>
    /// 最大对象数
    /// </summary>
    public int MaxObjCount
    {
        get{return m_maxObjCount;}
    }

    /// <summary>
    /// 当前对象数
    /// </summary>
    public int CurrentObjCount
    {
        get{return m_keyList.Count;}
    }

    /// <summary>
    /// 空闲对象数
    /// </summary>
    public int IdleObjCount
    {
		get
		{
			ObjectPool pool = this;
			lock (pool)
			{
				return this.GetIdleObjCount();
			}
		}
    }

    /// <summary>
    /// 获取空闲对象数
    /// </summary>
    /// <returns></returns>
    private int GetIdleObjCount()
    {
		return this.m_idleObjCount;
		//int count = 0;
		//for (int n = 0; n < m_keyList.Count; ++n)
		//{
		//	if (m_hashTableStatus[m_keyList[n]])
		//		++count;
		//}

		//return count;
    }
}