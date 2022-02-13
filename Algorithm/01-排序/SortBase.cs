using System;

namespace Algorithm.排序
{
	//排序基类
	abstract class SortBase<T>
	{
		//添加一个指针方便比较和交换
		protected T[] elements;
		//记录比较次数
		protected int compareCount;
		public int CompareCount => compareCount;
		//记录交换次数
		protected int swapCount;
		public int SwapCount => swapCount;

		//通过索引比较
		protected int CompareByIndex(int i1, int i2)
		{
			++compareCount;
			return ((IComparable<T>)elements[i1]).CompareTo(elements[i2]);
		}

		//通过元素比较
		protected int CompareByElement(T e1, T e2)
		{
			++compareCount;
			return ((IComparable<T>)e1).CompareTo(e2);
		}

		//交换元素
		protected void Swap(int i1, int i2)
		{
			++swapCount;
			T temp = elements[i1];
			elements[i1] = elements[i2];
			elements[i2] = temp;
		}

		//数组检测
		protected void ArrayCheck(T[] array)
		{
			if (array == null || array.Length < 1)
			{
				throw new Exception("Array does not need to be sorted");
			}
			elements = array;//添加一个指针方便比较和交换
		}

		//排序方法主体
		public virtual void Sorting(T[] array)
		{
			ArrayCheck(array);
		}
	}
}
