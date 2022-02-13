using System;
using System.Collections.Generic;

namespace DataStructure.集合与映射.链表集合
{
	//链表实现集合
	class ListSet<T> : Set<T>
	{
		private LinkedList<T> list = new LinkedList<T>();

		protected override int GetSize()
		{
			return list.Count;
		}
		public override void Clear()
		{
			list.Clear();
		}

		public override void Add(T element)
		{
			if (element == null)
			{
				throw new NullReferenceException();
			}
			//集合不包含重复元素
			if (list.Contains(element))
			{
				return;
			}
			list.AddLast(element);
		}

		public override void Remove(T element)
		{
			list.Remove(element);
		}

		public override bool Contains(T element)
		{
			return list.Contains(element);
		}

		public override void Traversal(Func<T, bool> func)
		{
			if (func == null)
			{
				return;
			}
			LinkedListNode<T> node = list.First;
			for (int i = 0; i < Size; i++)
			{
				if (func(node.Value))
				{
					return;
				}
				node = node.Next;
			}
		}
	}
}
