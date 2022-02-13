using DataStructure.堆.二叉堆;
using System;

namespace DataStructure.队列.优先级队列
{
	//优先级队列，使用二叉堆实现
	class PriorityQueue<T> : MyQueue<T>
	{
		private BinaryHeap<T> heap;

		public PriorityQueue()
		{
			heap = new BinaryHeap<T>();
		}

		public PriorityQueue(Func<T, T, int> comparer)
		{
			heap = new BinaryHeap<T>(comparer);
		}

		public override bool IsEmpty()
		{
			return heap.IsEmpty;
		}

		public override void Clear()
		{
			heap.Clear();
		}

		public override T Dequeue()
		{
			return heap.Remove();
		}

		public override void Enqueue(T element)
		{
			heap.Add(element);
		}

		public override T GetFront()
		{
			return heap.Get();
		}
	}
}
