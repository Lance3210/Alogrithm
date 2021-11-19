using DataStructure.堆;
using DataStructure.堆.二叉堆;
using DataStructure.树.字典树;
using DataStructure.队列.优先级队列;
using System;

namespace DataStructure.测试 {
	class Program4 {
		static void Main0(string[] args) {
			TrieTest();
		}

		#region TrieTest
		private static void TrieTest() {
			Trie<int> trie = new Trie<int>();
			trie.Add("sb", 1000);
			trie.Add("sbs", 2000);
			trie.Add("sbss", 3000);
			trie.Add("s", 8000);
			trie.Add("aa", 666);
			Console.WriteLine(trie.Size);
			Console.WriteLine("-----------------------------------------");
			Console.Write(trie.Get("sb") + " ");
			Console.Write(trie.Get("sbs") + " ");
			Console.Write(trie.Get("sbss") + " ");
			Console.Write(trie.Get("s") + " ");
			Console.Write(trie.Get("aa") + " ");
			Console.WriteLine();

			trie.Remove("sbss");
			trie.Remove("aa");

			Console.WriteLine("-----------------------------------------");
			Console.Write(trie.StartsWith("s") + " ");
			Console.Write(trie.StartsWith("b") + " ");
			Console.WriteLine();

			Console.WriteLine("-----------------------------------------");
			Console.Write(trie.Contains("s") + " ");
			Console.Write(trie.Contains("a") + " ");
			Console.WriteLine();
		}
		#endregion

		#region PriorityQueueTest
		private static void PriorityQueueTest() {
			PriorityQueue<Person> queue = new PriorityQueue<Person>();
			for (int i = 0; i < 10; i++) {
				queue.Enqueue(new Person(i, i, i.ToString()));
			}
			for (int i = 0; i < 10; i++) {
				Console.Write(queue.Dequeue().name + " ");
			}
		}
		#endregion

		#region HeapTest
		private static void HeapTest3() {
			//TopK问题
			//有n个数据，找出其前k个最大的数据
			int[] data = new int[] { 20, 7, 27, 12, 11, 8, 4,
				26, 28, 23, 30, 9, 5, 14, 10, 19, 18, 17, 2,
				13, 6, 16, 15, 1, 25, 21 };
			int k = 5;

			//先实现一个最小堆
			Func<int, int, int> func = (e1, e2) => {
				return e2 - e1;//比较器反过来就是最小堆了
			};
			BinaryHeap<int> heap = new BinaryHeap<int>(func);
			//遍历每一个数据
			for (int i = 0; i < data.Length; i++) {
				if (heap.Size < k) {
					heap.Add(data[i]);//将前k个先添加到堆中
				}
				else if (data[i] > heap.Get())//只要有比堆顶还大的就直接替换
				{
					heap.Replace(data[i]);
				}
			}
			//这样遍历完后剩下的堆中元素就是前k个最大数据
			for (int i = 0; i < data.Length; i++) {
				Console.Write($"{data[i]}" + " ");
				if (((i + 1) % 10) == 0) {
					Console.WriteLine();
				}
			}
			Console.WriteLine();
			Console.WriteLine("-------------------------------------------------");
			HeapTraversalTest(heap);
		}

		private static void HeapTest2() {
			int[] test = new int[] { 1, 2, 3, 4, 5, 6, 76, 0 };
			Func<int, int, int> func = (e1, e2) => {
				return e2 - e1;//比较器反过来就是最小堆了
			};
			BinaryHeap<int> heap = new BinaryHeap<int>(func, test);
			HeapTraversalTest(heap);
			test[0] = 100;
			HeapTraversalTest(heap);
		}


		private static void HeapTest(Heap<int> heap) {
			for (int i = 0; i < 10; i++) {
				heap.Add(i);
			}
			Console.Write("索引遍历：");
			heap.Traversal((e) => {
				Console.Write(e + " ");
				return false;
			});
			Console.WriteLine();
			heap.Replace(11);
			Console.Write("层序遍历：");
			((BinaryHeap<int>)heap).LevelOrderTraversal((e) => {
				Console.Write(e + " ");
				return false;
			});
		}
		#endregion

		#region HeapTraversalTest
		private static void HeapTraversalTest(Heap<int> heap) {
			Console.Write("索引遍历：");
			heap.Traversal((e) => {
				Console.Write(e + " ");
				return false;
			});
			Console.WriteLine();
		}
		#endregion

	}
}