namespace Algorithm.排序
{
	//基数排序
	//基于计数排序，对数字依次从个位、十位、百位、千位......即基数进行计数排序
	//缺点：基础形态对正整数排序效果最好，但负数则不可

	//总结
	//最坏时间复杂度：O(d *(n + k))，d最大值的位数，k为进制（十进制即10），即执行了d次计数排序
	//平均时间复杂度：O(d *(n + k))
	//最好时间复杂度：O(d *(n + k))
	//空间复杂度：O(n + k)，k为进制，前提是在循环外声明以避免重复开空间浪费
	//稳定性：稳定，基于计数排序

	class RadixSort<T> : SortBase<int>
	{
		//外部开辟避免浪费
		private int[] counts = new int[10];//直接开辟10空间的数组代表0到9，没必要再算出max和min
		private int[] newArray; //创建一个新数组用来作为最终排序结果

		//程序入口
		public override void Sorting(int[] array)
		{
			base.Sorting(array);
			//先给新数组开辟空间
			newArray = new int[elements.Length];
			//与计数排序一样，先求出最大值
			int max = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] > max)
				{
					max = array[i];
				}
			}
			//对每位进行计数排序，除数不会比最大值还要大（max = 999最大百位所以divisor最大100）
			for (int divisor = 1; divisor <= max; divisor *= 10)
			{
				CountingSort(divisor);
			}
		}

		//对每一位计数排序，根据传入的除数对相应位置的数字进行排序
		private void CountingSort(int divisor)
		{
			//由于外部开辟声明，因此要每轮清0
			for (int i = 0; i < counts.Length; i++)
			{
				counts[i] = 0;
			}
			//统计每个位上的数字出现个数
			for (int i = 0; i < elements.Length; i++)
			{
				counts[elements[i] / divisor % 10]++;//根据传入的除数获取对应位数字
			}
			//对次数进行累加，累加结果（即counts[...]中的值） - 1即数字在新数组中所在索引
			for (int i = 1; i < counts.Length; i++)
			{
				counts[i] += counts[i - 1];//从左边开始，累加前一个即可
			}
			//从右往左，根据旧数组再counts中查找对应位置插入到新数组中去
			for (int i = 0; i < elements.Length; i++)
			{
				newArray[--counts[elements[i] / divisor % 10]] = elements[i];
			}
			//直接覆盖原数组
			for (int i = 0; i < elements.Length; i++)
			{
				elements[i] = newArray[i];
			}
		}
	}
}
