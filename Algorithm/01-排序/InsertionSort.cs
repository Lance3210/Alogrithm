namespace Algorithm.排序
{
	//插入排序
	//类似于扑克牌的排序，元素分为已排序，和待排序两个部分，每轮循环将待排序元素插入已排序部分
	//逆序对：(2, 1)，(5, 3)等类型排布
	//数组中逆序对越少性能越高，甚至可能比O(nlogn)级别的快速排序还要快，数量适中时其效率客观

	//总结
	//最坏时间复杂度：O(n^2)，即逆序对最多形成完全逆序时
	//平均时间复杂度：O(n^2)，即使减少了交换代码和比较次数，遍历和挪动依然是n
	//最好时间复杂度：O(n)，完全有序，仅每个元素做一次比较
	//空间复杂度：O(1)
	//稳定性：稳定
	//原地算法；
	class InsertionSort<T> : SortBase<T>
	{
		//优化2：在优化1的基础上，将交换改为挪动减少代码
		//使用二分搜索优化比较逻辑，大大减少减少比较次数
		public override void Sorting(T[] array)
		{
			base.Sorting(array);
			T insert;
			int insertIndex;
			for (int i = 1; i < array.Length; i++)//每一个元素都要做一次插入遍历（i = 1）
			{
				insert = array[i];//备份待插入元素
				insertIndex = GetInsertionIndex(i);//获取合适插入位置
				for (int j = i; j > insertIndex; j--)
				{
					array[j] = array[j - 1];//insertIndex到i - 1的元素全部后移一位
				}
				array[insertIndex] = insert;
			}
		}
		private int GetInsertionIndex(int index)//二分查找法查找插入位置
		{
			int begin = 0;//第一个有序元素位置
			int end = index;//前半部分有序元素的末尾索引（开区间）
			int mid;
			while (begin < end)
			{
				mid = begin + ((end - begin) >> 1);//防止直接begin + end造成的溢出，位移运算符低于加减

				//这里是只要>=就往右边找，这样可以保证插入位置必定在第一个比element大的元素的位置
				if (CompareByElement(elements[index], elements[mid]) >= 0)
				{
					begin = mid + 1;
				}
				else
				{
					end = mid;
				}
			}
			return begin;//end也可以，因为循环完毕后这两个肯定相等，该位置就是第一个比element大的元素的位置
		}

		//优化1：将交换转为挪动
		//备份待插入元素，找到合适位置，将合适位置后面（到begin）所有元素后移一位，空出位置再插入
		//减少交换操作代码，换成只用一行的覆盖
		//public override void Sorting(T[] array)
		//{
		//	base.Sorting(array);
		//	int current;//用于记录插入遍历时的索引
		//	T insert;//备份待插入元素							
		//	for (int begin = 1; begin < array.Length; begin++)//每一个元素都要做一次插入遍历（与自己前一个比故begin = 1）
		//	{
		//		current = begin;//记录每一个元素最初位置
		//		insert = array[begin];//备份待插入元素

		//		//循环条件：current最多到第二个，因为要和前一个比较
		//		//比前一个还要小，即将前一个后移一位
		//		while (current > 0 && CompareByElement(insert, array[current - 1]) < 0)//相同不交换以保持稳定
		//		{
		//			array[current] = array[current - 1];//直接覆盖
		//			++swapCount;//算作一次交换
		//			--current;//指针前移
		//		}
		//		array[current] = insert;//遍历完后找到合适位置插入
		//	}
		//}

		//基础形态
		//public override void Sorting(T[] array)
		//{
		//	base.Sorting(array);
		//	int current;//用于记录插入遍历时的索引
		//	for (int begin = 1; begin < array.Length; begin++)//每一个元素都要做一次插入遍历（与自己前一个比故begin = 1）
		//	{
		//		current = begin;//记录每一个元素最初位置
		//
		//		//循环条件：current最多到第二个，因为要和前一个比较
		//		//比前一个还要小，就要进行交换
		//		while (current > 0 && CompareByIndex(current, current - 1) < 0)//相同不交换以保持稳定
		//		{
		//			Swap(current, current - 1);
		//			--current;//指针前移
		//		}
		//	}
		//}
	}
}
