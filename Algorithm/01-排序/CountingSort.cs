namespace Algorithm.排序 {
	//计数排序
	//不基于比较的排序，对一定范围内的整数进行排序（自定义类型中可以利用整数进行排序也可以）

	//总结
	//最坏时间复杂度：O(n + k)，有些时候比nlogn的比较型排序会好（取决于k）
	//平均时间复杂度：O(n + k)
	//最好时间复杂度：O(n + k)
	//空间复杂度：O(n + k)，k为 max - min
	//稳定性：稳定，前提是从右往左插入新数组

	class CountingSort<T> : SortBase<int> {
		//优化
		//同时求出min和max，缩短counts大小
		//将counts记录出现次数转变为累加出现次数，并从右向左遍历插入，以达到稳定排序
		public override void Sorting(int[] array) {
			base.Sorting(array);

			//先找出最大值和最小值，用来作为计数排序数组的范围
			int max = 0, min = 0;
			for (int i = 0; i < array.Length; i++) {
				if (max < array[i]) {
					max = array[i];
				}
				if (min > array[i])//不用else if是因为min和max有可能是同一个数
				{
					min = array[i];
				}
			}

			//创建计数排序用数组，范围直接变为[min, max]
			int[] counts = new int[max - min + 1];
			//统计array中每个数字出现的次数
			for (int i = 0; i < array.Length; i++) {
				counts[array[i] - min]++;//对应的数字在counts中的索引应该变为该数字 - min
			}
			//还需要对次数进行累加，累加结果（即counts[...]中的值） - 1即数字在新数组中所在索引
			for (int i = 1; i < counts.Length; i++) {
				counts[i] += counts[i - 1];//从左边开始，累加前一个即可
			}

			//再创建一个新数组用来作为最终排序结果
			int[] newArray = new int[array.Length];
			//从右往左，根据旧数组在counts中查找对应位置插入到新数组中去
			for (int i = array.Length - 1; i >= 0; --i) {
				//array[i] - min即找出counts中对应索引，--是在取出前就获取新数组中对应位置
				newArray[--counts[array[i] - min]] = array[i];//累加结果（即counts[...]中的值） - 1即数字在新数组中所在索引
			}
			//直接覆盖原数组
			for (int i = 0; i < array.Length; i++) {
				array[i] = newArray[i];
			}
		}

		//基础形态
		//利用一个数组计算一定范围数字出现的次数
		//缺点：无法排序负整数，内存消耗极大，不稳定排序，且只能对正数排序
		private void Sorting0(int[] array) {
			base.Sorting(array);

			//先找出最大值，用来作为计数排序数组的范围
			int max = 0;
			for (int i = 0; i < array.Length; i++) {
				if (max < array[i]) {
					max = array[i];
				}
			}

			//创建计数排序用数组，长度需要 + 1，因为还有索引0
			int[] counts = new int[max + 1];
			//统计array中每个数字出现的次数
			for (int i = 0; i < array.Length; i++) {
				counts[array[i]]++;//array[i]即对应一个counts中的位置，其值只要出现一次便会+1
			}

			//插入array时用的指针
			int index = 0;
			//遍历counts，对array中的元素排序
			for (int i = 0; i < counts.Length; i++) {
				//按每个数字出现的频率取出数字
				while (counts[i]-- > 0) {
					array[index++] = i;//i即对应数字
				}
			}
		}
	}
}
