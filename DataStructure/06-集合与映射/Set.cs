using System;

namespace DataStructure.集合与映射
{
	//集合基类
	public abstract class Set<T>
	{
		public int Size => GetSize();
		public bool IsEmpty => Size == 0;
		protected abstract int GetSize();

		public abstract void Clear();

		public abstract void Add(T element);

		public abstract void Remove(T element);

		public abstract bool Contains(T element);

		public abstract void Traversal(Func<T, bool> func);
	}
}
