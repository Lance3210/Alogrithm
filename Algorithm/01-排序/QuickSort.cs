using System;

namespace Algorithm.排序
{
	class QuickSort<T> : SortBase<T>
	{
		//快速排序
		//将序列的首元素作为轴点，将小于等于轴点的放轴点左边，大于的放右边
		//以轴点为分割点，左右序列继续进行上一步操作，即递归调用

		//总结
		//最坏时间复杂度：O(n^2)，排列为7 1 2 3 4 5 6，每轮都有一边为0，即T(n) = T(n - 1) + O(n) = O(n^2)
		//平均时间复杂度：O(nlogn)
		//最好时间复杂度：O(nlogn)，划分轴点时左右分布数量相近时，即T(n) = 2 * T(n/2) + O(n) = O(nlogn)
		//空间复杂度：O(logn)
		//稳定性：不稳定
		public override void Sorting(T[] array)
		{
			base.Sorting(array);
			Quick(0, array.Length - 1);
		}

		//将[begin, end)范围内元素快速排序，递归调用
		public void Quick(int begin, int end)
		{
			if (end - begin < 2)
			{
				return;
			}
			int pivot = GetPivotIndex(begin, end);//获取轴点位置
			Quick(begin, pivot);//左边快速排序
			Quick(pivot + 1, end);//右边快速排序，不包含轴点
		}

		private Random random = new Random();
		//传入[begin, end)范围内元素获取其轴点应在的位置
		public int GetPivotIndex(int begin, int end)
		{
			//随机指定轴点，可以降低最坏情况出现的几率
			Swap(begin, random.Next(begin, end));//直接交换随机到的实际轴点到头部

			//备份首元素（即轴点）
			T pivot = elements[begin];

			//开始左右交替比较
			while (begin < end)//到最后begin必然等于end，因为都是移动1
			{
				//三个while可以达到左右交替执行的效果
				while (begin < end)
				{
					if (CompareByElement(pivot, elements[end]) < 0)//右边大于轴点
					{
						--end;
					}
					else//之所以=也要交换是为了更好分散左右元素，降低最坏复杂度概率
					{
						elements[begin++] = elements[end];//右边小于轴点则覆盖begin
						break;//掉头从左边开始
					}
				}
				while (begin < end)
				{
					if (CompareByElement(pivot, elements[begin]) > 0)//左边小于轴点
					{
						++begin;
					}
					else
					{
						elements[end--] = elements[begin];//左边大于轴点则覆盖begin
						break;//掉头从右边开始
					}
				}
			}
			//最后插入轴点元素并返回其下标
			elements[begin] = pivot;
			return begin;
		}
	}
}
