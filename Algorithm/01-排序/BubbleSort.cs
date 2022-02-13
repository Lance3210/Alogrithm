namespace Algorithm.排序
{
	//冒泡排序

	//总结
	//最坏时间复杂度：O(n^2)，即完全反过来，[n * (n + 1)] / 2次
	//最好时间复杂度：O(n)，即完全有序
	//空间复杂度：O(1)
	//稳定性：稳定，前提是比较时不能将相等的也交换
	//原地算法；
	class BubbleSort<T> : SortBase<T>
	{
		//优化2：添加一个记录最后一次交换位置的标记，因为最后一次交换位置在之后的所有元素必定已经有序
		//下一轮开始只需要遍历到标记位置即可，另外将i、j替换为end和begin方便赋值
		public override void Sorting(T[] array)
		{
			base.Sorting(array);
			int lastChangeIndex;
			for (int end = array.Length - 1; end > 0; end--)//提取结束位置
			{
				lastChangeIndex = 0;//0可以在第一轮时检测到完全有序后退出
				for (int begin = 1; begin <= end; begin++)//改为从1开始
				{
					if (CompareByIndex(begin - 1, begin) > 0)
					{
						Swap(begin, begin - 1);
						lastChangeIndex = begin;//标记交换的后者位置
					}
				}
				end = lastChangeIndex;//下次遍历只需到最后一次交换位置即可
			}
		}

		//基础形态：相邻的元素两两互换，每次能将最大的一个排到最后，算作一轮
		//进行多轮互换，每次少换一个元素，因为在前一轮中已经确定好在最后了，故要 - i
		//public static void BubbleSort0(T[] array)
		//{
		//	T temp;
		//	for (int i = 0; i < array.Length - 1; i++)
		//	{
		//		for (int j = 0; j < array.Length - 1 - i; j++)
		//		{
		//			if (array[j].CompareTo(array[j + 1]) > 0)
		//			{
		//				temp = array[j];
		//				array[j] = array[j + 1];
		//				array[j + 1] = temp;
		//			}
		//		}
		//	}
		//}
		//优化1：存在完全有序的情况，添加一个每一轮true的标记，本轮有交换即标记为false，当一轮结束后还是true则无须遍历了
		//实际上完全有序情况较少，由于多了逻辑判断，性能反而可能更低
		//public static void BubbleSort1(T[] array)
		//{
		//	bool sorted;
		//	T temp;
		//	for (int i = 0; i < array.Length - 1; i++)
		//	{
		//		sorted = true;//是否已经有序的标记
		//		for (int j = 0; j < array.Length - 1 - i; j++)
		//		{
		//			if (array[j].CompareTo(array[j + 1]) > 0)
		//			{
		//				temp = array[j];
		//				array[j] = array[j + 1];
		//				array[j + 1] = temp;
		//				sorted = false;//本轮进行过交换
		//			}
		//		}
		//		if (sorted)//若该轮不存在交换即无须再遍历
		//		{
		//			break;
		//		}
		//	}
		//}
	}
}
