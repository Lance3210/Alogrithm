using DataStructure.哈希表.有序哈希表;
using System;

namespace DataStructure.集合与映射.哈希集合
{
	class MyLinkedHashSet<T> : Set<T>
	{
		//利用HashMap实现的Set
		LinkedHashMap<T, object> map = new LinkedHashMap<T, object>();
		protected override int GetSize()
		{
			return map.Size();
		}

		public override void Clear()
		{
			map.Clear();
		}

		public override void Add(T element)
		{
			map.Put(element, null);
		}

		public override void Remove(T element)
		{
			map.Remove(element);
		}

		public override bool Contains(T element)
		{
			return map.ContainsKey(element);
		}

		//遍历只需要key
		public override void Traversal(Func<T, bool> func)
		{
			LinkedHashNode<T, object> node = map.First;
			while (node != null)
			{
				func(node.key);
				node = node.next;
			}
		}
	}
}
