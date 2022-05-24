using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


// ------------------------------------------------------------------------------------------------------------------
// 1. 作为底层容器，只支持"增删"功能，不支持"改查"功能
// 2. "增删"功能用于遍历，"改查"功能属于上层业务，且对象的InnerIndex和业务没有具体的对应关系，因此不提供"改查"接口
// 3. 备注：Index的存储为什么使用Stack，而不使用Queue？回答：都可以，使用Stack感觉Index分配更紧凑而已。
// 4. 调整了入Stack的顺序，倒序入栈。最终会导致在获取的时候是正序的，从而使得MemoryDetect更加清晰。
// 5. 侵入式设计改为了抽象函数，即abstract的SetInnerIndex和GetInnerIndex接口。当继承类指定T类型时，就可以自行绑定字段或属性访问器
// ------------------------------------------------------------------------------------------------------------------

//IIndexContainer接口，提供容器的增删改查操作
public interface IIndexContainer<T>
{
	//获取容器的迭代器
	IEnumerator<T> GetEnumerator();

	//添加一个对象
	void Add(T t);

	//移除一个对象
	void Remove(T t);

	//获取容器中的对象数量
	int Count { get; }
}

public abstract class IndexContainer<T> : IIndexContainer<T>
{
	protected int m_curCapacity;
	protected T[] m_array;
	private Stack<int> m_stack = new Stack<int>();
	private int m_count = 0;
	public int Count { get { return m_count; } }

	//必须实现SetInnerIndex
	protected abstract void SetInnerIndex(T obj, int index);
	protected abstract int GetInnerIndex(T obj);

	//构造函数
	public IndexContainer(int _capacity = 64)
	{
		m_curCapacity = _capacity;
		m_array = new T[m_curCapacity];
		for (int i = m_curCapacity - 1; i >= 0; --i)
		{
			m_stack.Push(i);
		}
	}

	//获取迭代器
	public IEnumerator<T> GetEnumerator()
	{
		int i = 0;
		while (i < m_array.Length)
		{
			if (m_array[i] != null)
			{
				yield return m_array[i];
			}
			++i;
		}
	}

	//添加GameObject
	public void Add(T t)
	{
		//扩容
		if (m_stack.Count == 0)
		{
			Expand();
		}

		//得到空闲位置
		int index = m_stack.Pop();

		//在空闲位置插入
		m_array[index] = t;

		//记录空闲位置到对象的InnerIndex
		SetInnerIndex(t, index);

		//计数器加1
		++m_count;
	}

	//删除GameObject
	public void Remove(T t)
	{
		int index = GetInnerIndex(t);

		//对应Index置空，断掉引用
		m_array[index] = default(T);

		//空闲位置记录
		m_stack.Push(index);

		//计数器减1
		--m_count;
	}

	//扩容
	private void Expand()
	{
		Debug.LogFormat("{0} Expand, Now Capacity : {1}", this.GetType().Name, m_curCapacity * 2);
		T[] tempArr = new T[m_curCapacity * 2];
		for (int i = 0; i < m_curCapacity; ++i)
		{
			tempArr[i] = m_array[i];
		}
		for (int i = m_curCapacity - 1; i >= 0; --i)
		{
			m_stack.Push(i + m_curCapacity);
		}
		m_array = tempArr;
		m_curCapacity = m_curCapacity * 2;
	}
}

public class ItemContainer<T> : IndexContainer<T> where T : IIndexItem
{
	public ItemContainer(int _capacity = 64) : base(_capacity) { }

	public T this[int index]
	{
		get { return m_array[index]; }
		set { m_array[index] = value; }
	}

	protected override int GetInnerIndex(T obj)
	{
		return obj.InnerIndex;
	}

	protected override void SetInnerIndex(T obj, int index)
	{
		obj.InnerIndex = index;
	}
}

