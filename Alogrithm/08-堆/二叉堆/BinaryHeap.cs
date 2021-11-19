using System;
using System.Collections.Generic;

namespace DataStructure.堆.二叉堆 {
	/// <summary>
	/// 二叉堆（默认最大堆）
	/// </summary>
	/// <typeparam name="T"></typeparam>
	class BinaryHeap<T> : Heap<T> {
		//用数组实现
		private T[] elements;
		private const int DEFAULT_CAPACITY = 16;

		#region 各种构造方法
		public BinaryHeap() {
			elements = new T[DEFAULT_CAPACITY];
		}
		public BinaryHeap(params T[] elements) {
			if (elements == null || elements.Length == 0) {
				this.elements = new T[DEFAULT_CAPACITY];
			}
			else {
				//深拷贝，以防止外部修改数组
				size = elements.Length;
				int capacity = Math.Max(elements.Length, DEFAULT_CAPACITY);
				this.elements = new T[capacity];
				for (int i = 0; i < elements.Length; i++) {
					this.elements[i] = elements[i];
				}
				//由于可能是乱序，需要批量建堆
				Heapify();
			}
		}
		public BinaryHeap(Func<T, T, int> comparer) {
			this.comparer = comparer;
			elements = new T[DEFAULT_CAPACITY];
		}
		public BinaryHeap(Func<T, T, int> comparer, ICollection<T> elements) {
			this.comparer = comparer;
			T[] array = new T[elements.Count];//转化为数组
			elements.CopyTo(array, 0);
			if (elements == null || array.Length == 0) {
				this.elements = new T[DEFAULT_CAPACITY];
			}
			else {
				//深拷贝，以防止外部修改数组
				size = array.Length;
				int capacity = Math.Max(array.Length, DEFAULT_CAPACITY);
				this.elements = new T[capacity];
				for (int i = 0; i < array.Length; i++) {
					this.elements[i] = array[i];
				}
				//由于可能是乱序，需要批量建堆
				Heapify();
			}
		}
		#endregion

		//清空堆
		public override void Clear() {
			for (int i = 0; i < size; i++) {
				elements[i] = default;
			}
			size = 0;
		}

		//堆空检测
		private void NullCheck() {
			if (size == 0) {
				throw new NullReferenceException("Heap is empty");
			}
		}

		//节点空检测
		private void NullCheck(T element) {
			if (element == null) {
				throw new ArgumentNullException("Element is null");
			}
		}

		//获取根节点
		public override T Get() {
			NullCheck();
			return elements[0];
		}

		//动态扩容
		private void ReSize(int capacity) {
			int oldCapacity = elements.Length;
			if (capacity <= oldCapacity) {
				return;
			}
			int newCapacity = oldCapacity + (oldCapacity >> 1);//扩容1.5倍
			T[] newElements = new T[newCapacity];
			for (int i = 0; i < size; i++) {
				newElements[i] = elements[i];
			}
			elements = newElements;
		}

		//添加节点
		public override void Add(T element) {
			NullCheck(element);
			//检查容量
			ReSize(size + 1);//检查插入后会不会溢出
							 //插入尾部
			elements[size++] = element;
			//上滤该节点
			SiftUp(size - 1);
		}
		//批量添加
		public void AddAll(ICollection<T> elements) {
			if (elements == null) {
				return;
			}
			foreach (var item in elements) {
				Add(item);
			}
		}

		//上滤，传入一个索引让该元素向上移动到合适位置
		private void SiftUp(int index) {
			//前提要有父节点
			T element = elements[index];
			while (index > 0) {
				int parentIndex = (index - 1) >> 1;
				T parentElement = elements[parentIndex];
				if (ElementCompare(element, parentElement) <= 0) {
					break;
				}
				elements[index] = elements[parentIndex];
				//移动指针
				index = parentIndex;
			}
			//交换（类似选择排序）
			elements[index] = element;
		}

		//移除堆顶
		public override T Remove() {
			NullCheck();
			T root = elements[0];
			elements[0] = elements[size - 1];//从最后拿
			elements[size - 1] = default;
			--size;
			SiftDown(0);
			return root;
		}

		//下滤，传入一个索引让该元素下移到合适位置
		private void SiftDown(int index) {
			T element = elements[index];
			int half = size >> 1;
			//非叶子节点才能进来（小于第一个叶子节点的索引，即非叶子节点的数量，完全二叉树的性质）
			while (index < half) {
				//且非叶子节点只有两种情况，只有左子节点，或左右都有子节点
				//默认节点为左子节点
				int childIndex = (index << 1) + 1;
				T betterChild = elements[childIndex];
				//右子节点
				int rightIndex = childIndex + 1;

				//右子节点肯定不超过数组的Length，同时选出左右子节点中最大的（没必要=0）
				if (rightIndex < size && ElementCompare(elements[rightIndex], betterChild) > 0) {
					betterChild = elements[childIndex = rightIndex];//取出右子节点，同时将子节点标记为右子节点（因为上面默认是左子节点）
				}
				//发现需要下滤的节点比自己最大的子节点还要大（或等于）就无须遍历
				if (ElementCompare(element, betterChild) >= 0) {
					break;
				}
				//下滤该节点，将子节点提上去
				elements[index] = betterChild;
				//移动指针
				index = childIndex;
			}
			elements[index] = element;
		}

		//删除堆顶同时插入一个新元素
		public override T Replace(T element) {
			NullCheck(element);
			if (size == 0) {
				elements[0] = element;
				++size;
				return default;
			}
			T oldElement = elements[0];
			elements[0] = element;
			SiftDown(0);
			return oldElement;
		}

		//批量建堆
		protected override void Heapify() {
			//自下而上的下滤
			for (int i = (size >> 1) - 1; i >= 0; i--)//从倒数第一个非叶子节点开始下滤
			{
				SiftDown(i);
			}
		}

		//索引遍历
		public override void Traversal(Func<T, bool> func) {
			for (int i = 0; i < size; i++) {
				if (func(elements[i])) {
					return;
				}
			}
		}

		//层序遍历
		public void LevelOrderTraversal(Func<T, bool> func) {
			Queue<T> queue = new Queue<T>();
			int index = 0;
			int left;
			int right;
			queue.Enqueue(elements[index]);
			T node;
			while (queue.Count != 0) {
				node = queue.Dequeue();
				left = (index << 1) + 1;//获取左右节点索引
				right = (index << 1) + 2;
				if (func(node)) {
					return;
				}
				if (left < size) {
					queue.Enqueue(elements[left]);
				}
				if (right < size) {
					queue.Enqueue(elements[right]);
				}
				++index;
			}
		}
	}
}
