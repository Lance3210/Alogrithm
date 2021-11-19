using Algorithm.排序;
using System;
using System.Collections.Generic;

namespace Algorithm.测试 {
	static class Program1 {
		static void Main1(string[] args) {
			int[] array = new int[count];
			RandomNums(array);
			Test[] tests = new Test[count];
			for (int i = 0; i < tests.Length; i++) {
				tests[i] = new Test(array[i], array[i]);
			}

			Sorting(tests);
			Sorting2(array);
		}

		//数组大小
		static int count = 6000;

		//创建数组与算法
		static void Sorting(Test[] tests) {
			SortTest(tests, new SortBase<Test>[]
			{
				new BubbleSort<Test>(),
				new SelectionSort<Test>(),
				new HeapSort<Test>(),
				new InsertionSort<Test>(),
				new MergeSort<Test>(),
				new QuickSort<Test>(),
				new ShellSort<Test>()
			});
			if (count <= 10) {
				Traversal(tests);
			}
		}
		//计算排序测试
		static void Sorting2(int[] array) {
			SortTest(array, new SortBase<int>[]{
				new CountingSort<int>(),
				new RadixSort<int>()
			});
			if (count <= 10) {
				Traversal(array);
			}
		}
		//排序测试
		static void SortTest(Test[] array, params SortBase<Test>[] sorts) {
			//创建数个相同数组用于独立算法测试
			List<Test[]> lists = new List<Test[]>(sorts.Length);
			lists.Add(array);//先进入一个，后面少copy一份
			for (int i = 0; i < sorts.Length - 1; i++) {
				Test[] copy = new Test[array.Length];
				array.CopyTo(copy, 0);
				lists.Add(copy);
			}
			//用于记录排序性能
			PrintTest[] prints = new PrintTest[sorts.Length];
			DateTime time;
			//依次进行排序
			for (int i = 0; i < sorts.Length; i++) {
				time = DateTime.Now;
				sorts[i].Sorting(lists[i]);
				prints[i] = new PrintTest(sorts[i].GetType().Name,
					DateTime.Now - time,
					sorts[i].CompareCount,
					sorts[i].SwapCount);
				prints[i].isStable = IsStable(sorts[i]);
			}
			//对算法效率进行排序
			SortBase<PrintTest> sort = new MergeSort<PrintTest>();
			sort.Sorting(prints);
			//从时间快到慢遍历打印
			foreach (var item in prints) {
				Console.WriteLine(item.name + "\t\t" + item.endTime.TotalMilliseconds + " ms");
				Console.Write($"CompareCount: {item.compareCount}\t\tSwapCount: {item.swapCount}\t\tIsStable: {item.isStable}\t\t");
				Console.WriteLine();
				Console.WriteLine("-----------------------------------------------------------------------------------------");
			}
			//比较排序后是否一致
			bool isEqual = true;
			for (int i = 0; i < lists.Count; i++) {
				for (int j = i + 1; j < lists.Count; j++) {
					if (!EqualsArray(lists[i], lists[j])) {
						isEqual = false;
						break;
					}
				}
			}
			Console.WriteLine("SortsCount: " + lists.Count + "\tWhether all arrays are Equal: " + isEqual + "\tEqualCount: " + equalCount);
			Console.WriteLine("-----------------------------------------------------------------------------------------");
		}

		//整数排序测试
		static void SortTest(int[] array, params SortBase<int>[] sorts) {
			//创建数个相同数组用于独立算法测试
			List<int[]> lists = new List<int[]>(sorts.Length);
			lists.Add(array);//先进入一个，后面少copy一份
			for (int i = 0; i < sorts.Length - 1; i++) {
				int[] copy = new int[array.Length];
				array.CopyTo(copy, 0);
				lists.Add(copy);
			}
			//用于记录排序性能
			PrintTest[] prints = new PrintTest[sorts.Length];
			DateTime time;
			//依次进行排序
			for (int i = 0; i < sorts.Length; i++) {
				time = DateTime.Now;
				sorts[i].Sorting(lists[i]);
				prints[i] = new PrintTest(sorts[i].GetType().Name,
					DateTime.Now - time,
					sorts[i].CompareCount,
					sorts[i].SwapCount);
				prints[i].isStable = IsStable(sorts[i]);
			}
			//对算法效率进行排序
			SortBase<PrintTest> sort = new MergeSort<PrintTest>();
			sort.Sorting(prints);
			//从时间快到慢遍历打印
			foreach (var item in prints) {
				Console.WriteLine(item.name + "\t\t" + item.endTime.TotalMilliseconds + " ms");
				Console.Write($"CompareCount: {item.compareCount}\t\tSwapCount: {item.swapCount}\t\tIsStable: {item.isStable}\t\t");
				Console.WriteLine();
				Console.WriteLine("-----------------------------------------------------------------------------------------");
			}
			//比较排序后是否一致
			bool isEqual = true;
			for (int i = 0; i < lists.Count; i++) {
				for (int j = i + 1; j < lists.Count; j++) {
					if (!EqualsArray(lists[i], lists[j])) {
						isEqual = false;
						break;
					}
				}
			}
			Console.WriteLine("SortsCount: " + lists.Count + "\tWhether all arrays are Equal: " + isEqual + "\tEqualCount: " + equalCount);
			Console.WriteLine("-----------------------------------------------------------------------------------------");
		}


		#region 旧测试
		//private static void SortTest1()
		//{
		//	//创建数组
		//	Test[] array1, array2, array3, array4;
		//	array1 = new Test[Count];
		//	RandomNums(array1);
		//	array2 = new Test[array1.Length];
		//	array1.CopyTo(array2, 0);
		//	array3 = new Test[array1.Length];
		//	array1.CopyTo(array3, 0);
		//	array4 = new Test[array1.Length];
		//	array1.CopyTo(array4, 0);
		//	//排序测试
		//	SortTest(array1, new BubbleSort<Test>());
		//	SortTest(array2, new SelectionSort<Test>());
		//	SortTest(array3, new HeapSort<Test>());
		//	SortTest(array4, new InsertionSort<Test>());
		//	//遍历打印
		//	Traversal(array1);
		//	Traversal(array2);
		//	Traversal(array3);
		//	Traversal(array4);
		//	结果比较
		//	Console.WriteLine();
		//	Console.WriteLine("Is Compare: " + EqualsArray(array1, array4));
		//	Console.WriteLine("-----------------------------------------------------------------------------------------");
		//}

		//排序测试
		//static void SortTest(Test[] array, SortBase<Test> sort)
		//{
		//	DateTime time = DateTime.Now;
		//	sort.Sorting(array);
		//	TimeSpan endTime = DateTime.Now - time;

		//	Console.WriteLine(sort.GetType().Name + "\t\t\t" + endTime.TotalMilliseconds + " ms");
		//	Console.WriteLine();

		//	Console.Write("CompareCount: " + sort.CompareCount + "\t\t" + "SwapCount: " + sort.SwapCount);
		//	Console.WriteLine("\t\t" + "IsStable: " + IsStable(sort));
		//	Console.WriteLine("-----------------------------------------------------------------------------------------");
		//}
		#endregion

		//稳定性测试
		public static bool IsStable(SortBase<Test> sort) {
			if (sort is ShellSort<Test>)//希尔排序本质是不稳定的
			{
				return false;
			}

			Test[] tests = new Test[10];
			for (int i = 0; i < tests.Length; i++) {
				tests[i] = new Test(i * 10, 1);
			}
			Test[] tests2 = new Test[tests.Length];
			tests.CopyTo(tests2, 0);

			sort.Sorting(tests);

			for (int i = 0; i < tests.Length; i++) {
				if (tests[i].height.CompareTo(tests2[i].height) != 0) {
					return false;
				}
			}
			return true;
		}
		//整数稳定性测试
		public static bool IsStable(SortBase<int> sort) {
			int[] tests = new int[10];
			for (int i = 0; i < tests.Length; i++) {
				tests[i] = i * 10;
			}
			int[] tests2 = new int[tests.Length];
			tests.CopyTo(tests2, 0);

			sort.Sorting(tests);

			for (int i = 0; i < tests.Length; i++) {
				if (tests[i].CompareTo(tests2[i]) != 0) {
					return false;
				}
			}
			return true;
		}

		//随机元素生成
		static void RandomNums(Test[] array) {
			Random random = new Random();
			for (int i = 0; i < array.Length; i++) {
				array[i] = new Test(random.Next(0, array.Length + 1), random.Next(0, array.Length + 1));
			}
			if (count <= 10) {
				Console.Write("原始数据： ");
				for (int i = 0; i < count; i++) {
					Console.Write(array[i].age + " ");
				}
				Console.WriteLine();
				Console.WriteLine("-----------------------------------------------------------------------------------------");
			}
		}
		//随机整数生成
		static void RandomNums(int[] array) {
			Random random = new Random();
			for (int i = 0; i < array.Length; i++) {
				array[i] = random.Next(0, array.Length + 1);
			}
			if (count <= 10) {
				Console.Write("原始数据： ");
				for (int i = 0; i < count; i++) {
					Console.Write(array[i] + " ");
				}
				Console.WriteLine();
				Console.WriteLine("-----------------------------------------------------------------------------------------");
			}
		}

		//数组比较
		static int equalCount = 0;
		static bool EqualsArray<T>(T[] array1, T[] array2) where T : IComparable<T> {
			++equalCount;
			for (int i = 0; i < array1.Length; i++) {
				if (array1[i].CompareTo(array2[i]) != 0) {
					return false;
				}
			}
			return true;
		}

		//元素遍历打印
		static void Traversal(Test[] array) {
			for (int i = 0; i < array.Length; i++) {
				Console.Write(array[i].age + " ");
			}
			Console.WriteLine();
			Console.WriteLine("-----------------------------------------------------");
		}
		//整数遍历打印
		public static void Traversal(int[] array) {
			for (int i = 0; i < array.Length; i++) {
				Console.Write(array[i] + " ");
			}
			Console.WriteLine();
			Console.WriteLine("-----------------------------------------------------");
		}
	}

	//测试用类
	class Test : IComparable<Test> {
		public int age;
		public int height;
		public Test(int height, int age) {
			this.height = height;
			this.age = age;
		}
		public int CompareTo(Test other)//以age作为比较媒介
		{
			return age - other.age;
		}
	}
	class PrintTest : IComparable<PrintTest> {
		public string name;
		public TimeSpan endTime;
		public int compareCount;
		public int swapCount;
		public bool isStable;

		public PrintTest(string name, TimeSpan endTime, int compareCount, int swapCount) {
			this.name = name;
			this.endTime = endTime;
			this.compareCount = compareCount;
			this.swapCount = swapCount;
		}

		public int CompareTo(PrintTest other) {
			if ((endTime - other.endTime).TotalMilliseconds > 0) {
				return 1;
			}
			else if ((endTime - other.endTime).TotalMilliseconds < 0) {
				return -1;
			}
			else {
				return 0;
			}
		}
	}
}
