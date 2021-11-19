namespace Algorithm.排序 {
	//归并排序
	//不断将当前序列平均分割为2个子序列直到不能分割位置
	//不断将两个子序列合并为一个有序序列直到剩只下一个有序序列

	//总结
	//最坏时间复杂度：O(nlogn)，每次都要分割合并，故时间复杂度都一致
	//平均时间复杂度：O(nlogn)
	//最好时间复杂度：O(nlogn)
	//空间复杂度：O(n)，分配了一半的额外的空间n/2 + 递归logn次
	//稳定性：稳定
	class MergeSort<T> : SortBase<T> {
		//排序入口
		//提前分配一半的数组空间用于备份每一次合并操作的左半部分数据
		private T[] leftArray;
		public override void Sorting(T[] array) {
			base.Sorting(array);
			//备份左半部分元素
			leftArray = new T[elements.Length >> 1];
			//将归并排序抽象成不断分割合并的操作
			SplitAndMerge(0, array.Length);
		}

		//对指定范围内数据进行归并排序，内部使用递归
		//将[begin, end)范围内序列分割为[begin, mid)和(mid, end]序列，然后有序合并
		private void SplitAndMerge(int begin, int end)//左闭右开
		{
			if (end - begin < 2)//元素少于2个即不用分割
			{
				return;
			}
			int mid = (begin + end) >> 1;//左闭右开刚好是mid作为右半部分开始
			SplitAndMerge(begin, mid);
			SplitAndMerge(mid, end);
			Merge(begin, mid, end);//然后对分割完毕的数据做合并排序
		}

		//对指定范围内数据进行归并排序的合并操作，内部使用递归
		//将[begin, mid)和(mid, end]范围内序列合并为一个有序序列
		//参数分别为：左半部分头，分割点，有半部分尾
		private void Merge(int begin, int mid, int end) {
			int leftHead = 0;//左半部分头尾标记（leftHead为0是因为操作的是leftArray）
			int leftEnd = mid - begin;
			int rightHead = mid;//右半部分头尾标记
			int rightEnd = end;
			int index = begin;//合并数组的插入索引，要从begin开始

			//备份左边数组
			for (int i = leftHead; i < leftEnd; i++) {
				leftArray[i] = elements[begin + i];//begin才是我们要真正操作的左半部分头
			}

			//左边先结束就无须移动了
			while (leftHead < leftEnd) {
				//两两比较两边数组中的元素，条件顺序不能错，因为可能存在rightHead越界（右边先完成情况）
				if (rightHead < rightEnd && CompareByElement(elements[rightHead], leftArray[leftHead]) < 0) {
					elements[index++] = elements[rightHead++];//将右边的取出后索引后移
				}
				else//要在相同元素时先取出左边的，否则不稳定
				{
					elements[index++] = leftArray[leftHead++];//将左边的取出后索引后移，或者右边先结束则左边直接依次插到后面
				}
			}
		}
	}
}
