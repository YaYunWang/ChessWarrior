using System.Collections.Generic;
using UnityEngine;


public class QuickList<T>
{
	public T[] m_buffer;
	public int m_size;

	public QuickList()
	{
		this.m_buffer = new T[8];
	}

	public QuickList(int capacity)
	{
		this.m_buffer = new T[capacity];
	}

	public void Add(T item)
	{
		if ((this.m_buffer == null) || (this.m_size == this.m_buffer.Length))
		{
			this.AllocateMore();
		}
		this.m_buffer[this.m_size++] = item;
	}

	private void AllocateMore()
	{
		T[] array = (this.m_buffer == null) ? new T[0x20] : new T[Mathf.Max(this.m_buffer.Length << 1, 0x20)];
		if ((this.m_buffer != null) && (this.m_size > 0))
		{
			this.m_buffer.CopyTo(array, 0);
		}
		this.m_buffer = array;
	}

	public void Clear()
	{
		this.m_size = 0;
		int count = m_buffer.Length;

		for (int i = 0; i < count; i++)
		{
			m_buffer[i] = default(T);
		}
	}

	public void Release()
	{
		this.m_size = 0;
		this.m_buffer = null;
	}

	public void Remove(T item)
	{
		if (this.m_buffer != null)
		{
			EqualityComparer<T> comparer = EqualityComparer<T>.Default;
			for (int i = 0; i < this.m_size; i++)
			{
				if (comparer.Equals(this.m_buffer[i], item))
				{
					this.m_size--;
					this.m_buffer[i] = default(T);
					for (int j = i; j < this.m_size; j++)
					{
						this.m_buffer[j] = this.m_buffer[j + 1];
					}
					return;
				}
			}
		}
	}

	public void RemoveAt(int index)
	{
		if ((this.m_buffer != null) && (index < this.m_size))
		{
			this.m_size--;
			this.m_buffer[index] = default(T);
			for (int i = index; i < this.m_size; i++)
			{
				this.m_buffer[i] = this.m_buffer[i + 1];
			}
		}
	}

	public T[] ToArray()
	{
		this.Trim();
		return this.m_buffer;
	}

	private void Trim()
	{
		if (this.m_size > 0)
		{
			if (this.m_size < this.m_buffer.Length)
			{
				T[] localArray = new T[this.m_size];
				for (int i = 0; i < this.m_size; i++)
				{
					localArray[i] = this.m_buffer[i];
				}
				this.m_buffer = localArray;
			}
		}
		else
		{
			this.m_buffer = null;
		}
	}

	public int Count
	{
		get
		{
			return this.m_size;
		}
	}

	public T this[int i]
	{
		get
		{
			return this.m_buffer[i];
		}
		set
		{
			this.m_buffer[i] = value;
		}
	}
}
