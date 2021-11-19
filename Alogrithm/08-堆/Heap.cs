using System;

namespace DataStructure.堆 {
	//堆
	public abstract class Heap<T> {
		protected int size;
		public int Size => size;
		public bool IsEmpty => size == 0;
		protected Func<T, T, int> comparer;
		protected abstract void Heapify();//批量建堆
		public abstract void Clear();
		public abstract void Add(T element);
		public abstract T Remove();//只删除堆顶
		public abstract T Get();//只获得堆顶
		public abstract T Replace(T element);//删除堆顶同时插入一个新元素
		public abstract void Traversal(Func<T, bool> func);
		protected int ElementCompare(T e1, T e2) {
			return comparer != null ? comparer(e1, e2) : ((IComparable<T>)e1).CompareTo(e2);
		}
	}
}