
public class ObjectPool<T> where T : IPoolableObject, new()
{
	public int PoolSize
	{
		get
		{
			return poolSize;
		}

		set
		{
			if (value <= 0)
				value = 1;

			while (pool.Count > value)
			{
				pool.RemoveAt(pool.Count - 1);
			}

			poolSize = value;
		}
	}

	private int poolSize;

	private QuickList<T> pool = new QuickList<T>();

	public ObjectPool()
	{
		poolSize = 10;
	}

	public ObjectPool(int size)
	{
		PoolSize = size;
	}

	public T Fetch()
	{
		if (pool.Count > 0)
		{
			var t = pool[pool.Count - 1];
			pool.RemoveAt(pool.Count - 1);

			return t;
		}
		else
		{
			return new T();
		}
	}

	public void Put(T t)
	{
		t.Reset();

		if (pool.Count < poolSize)
		{
			pool.Add(t);
		}
	}

	public void Clear()
	{
		pool.Clear();
	}
}

