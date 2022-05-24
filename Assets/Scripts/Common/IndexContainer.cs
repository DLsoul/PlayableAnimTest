using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


// ------------------------------------------------------------------------------------------------------------------
// 1. ��Ϊ�ײ�������ֻ֧��"��ɾ"���ܣ���֧��"�Ĳ�"����
// 2. "��ɾ"�������ڱ�����"�Ĳ�"���������ϲ�ҵ���Ҷ����InnerIndex��ҵ��û�о���Ķ�Ӧ��ϵ����˲��ṩ"�Ĳ�"�ӿ�
// 3. ��ע��Index�Ĵ洢Ϊʲôʹ��Stack������ʹ��Queue���ش𣺶����ԣ�ʹ��Stack�о�Index��������ն��ѡ�
// 4. ��������Stack��˳�򣬵�����ջ�����ջᵼ���ڻ�ȡ��ʱ��������ģ��Ӷ�ʹ��MemoryDetect����������
// 5. ����ʽ��Ƹ�Ϊ�˳���������abstract��SetInnerIndex��GetInnerIndex�ӿڡ����̳���ָ��T����ʱ���Ϳ������а��ֶλ����Է�����
// ------------------------------------------------------------------------------------------------------------------

//IIndexContainer�ӿڣ��ṩ��������ɾ�Ĳ����
public interface IIndexContainer<T>
{
	//��ȡ�����ĵ�����
	IEnumerator<T> GetEnumerator();

	//���һ������
	void Add(T t);

	//�Ƴ�һ������
	void Remove(T t);

	//��ȡ�����еĶ�������
	int Count { get; }
}

public abstract class IndexContainer<T> : IIndexContainer<T>
{
	protected int m_curCapacity;
	protected T[] m_array;
	private Stack<int> m_stack = new Stack<int>();
	private int m_count = 0;
	public int Count { get { return m_count; } }

	//����ʵ��SetInnerIndex
	protected abstract void SetInnerIndex(T obj, int index);
	protected abstract int GetInnerIndex(T obj);

	//���캯��
	public IndexContainer(int _capacity = 64)
	{
		m_curCapacity = _capacity;
		m_array = new T[m_curCapacity];
		for (int i = m_curCapacity - 1; i >= 0; --i)
		{
			m_stack.Push(i);
		}
	}

	//��ȡ������
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

	//���GameObject
	public void Add(T t)
	{
		//����
		if (m_stack.Count == 0)
		{
			Expand();
		}

		//�õ�����λ��
		int index = m_stack.Pop();

		//�ڿ���λ�ò���
		m_array[index] = t;

		//��¼����λ�õ������InnerIndex
		SetInnerIndex(t, index);

		//��������1
		++m_count;
	}

	//ɾ��GameObject
	public void Remove(T t)
	{
		int index = GetInnerIndex(t);

		//��ӦIndex�ÿգ��ϵ�����
		m_array[index] = default(T);

		//����λ�ü�¼
		m_stack.Push(index);

		//��������1
		--m_count;
	}

	//����
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

