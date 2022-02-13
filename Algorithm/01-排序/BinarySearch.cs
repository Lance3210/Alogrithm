using System;

namespace Algorithm.搜索
{
	//二分搜索
	//前提是数组要有序排列
	static class BinarySearch<T>
	{
		//查询插入下标，用于查询插入位置
		//在基础形态上，将=的情况也纳入搜索，这样遍历到最后的就是插入位置
		//存在相同元素情况下该插入不会破坏排序稳定性
		public static int GetInsertionIndex(T[] array, T element)
		{
			if (array == null || array.Length == 0)
			{
				return -1;
			}
			int begin = 0;//左闭
			int end = array.Length;//右开
			int mid;
			int result;
			while (begin < end)
			{
				mid = begin + ((end - begin) >> 1);//防止直接begin + end造成的溢出，位移运算符低于加减	
				result = ((IComparable<T>)element).CompareTo(array[mid]);
				if (result >= 0)//这里是只要>=就往右边找，这样可以保证插入位置必定在第一个比element大的元素的位置
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

		//基础形态，返回相同元素的索引
		//但这种查询在存在多个相同元素时不稳定，这也是二分法在查找时的缺点
		public static int IndexOf(T[] array, T element)
		{
			if (array == null || array.Length == 0)
			{
				return -1;
			}
			int begin = 0;//左闭
			int end = array.Length;//右开
			int mid;
			int result;
			while (begin < end)
			{
				mid = begin + ((end - begin) >> 1);//防止直接begin + end造成的溢出，位移运算符低于加减	
				result = ((IComparable<T>)element).CompareTo(array[mid]);
				if (result > 0)
				{
					begin = mid + 1;
				}
				else if (result < 0)
				{
					end = mid;
				}
				else
				{
					return mid;
				}
			}
			return -1;
		}
	}
}
