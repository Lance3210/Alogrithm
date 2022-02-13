using System;
using System.Collections.Generic;

namespace Algorithm.排序
{
	//希尔排序
	//不断将序列分为指定步长的n列，并对n列数据进行排序，直到步长为1时即完成
	//希尔排序可以看成插入排序的升级，因为在每一次列排序后逆序对数量都减少了

	//总结
	//最坏时间复杂度：O(n^(4/3)) -> O(n^2)，与生成步长方式有关
	//平均时间复杂度：取决于步长序列
	//最好时间复杂度：O(n)，完全有序，仅每个元素做一次比较
	//空间复杂度：O(1)
	//稳定性：不稳定
	class ShellSort<T> : SortBase<T>
	{
		//用于装载每一轮步长
		LinkedList<int> stepSequence;
		//程序入口
		public override void Sorting(T[] array)
		{
			base.Sorting(array);
			//先生成每一轮的步长
			SedgwickStepSequence();
			//每一轮都按步长排序
			foreach (int step in stepSequence)
			{
				SortByStep(step);
			}
		}

		//使用希尔给出的步长规则[.....8, 4, 2, 1]，即n/2^k封装生成步长的方法
		//该步长最坏时间复杂度O(n^2)
		private void ShellStepSequence()
		{
			stepSequence = new LinkedList<int>();
			int step = elements.Length;
			while ((step >>= 1) > 0)//即 /= 2
			{
				stepSequence.AddLast(step);
			}
		}
		//优化过的步长规则
		//该步长最坏时间复杂度O(n^(4/3))
		private void SedgwickStepSequence()
		{
			stepSequence = new LinkedList<int>();
			int k = 0, step;
			while (true)
			{
				if (k % 2 == 0)//even number
				{
					int pow = (int)Math.Pow(2, k >> 1);
					step = 1 + 9 * (pow * pow - pow);
				}
				else//odd number
				{
					int pow1 = (int)Math.Pow(2, (k - 1) >> 1);
					int pow2 = (int)Math.Pow(2, (k + 1) >> 1);
					step = 1 + 8 * pow1 * pow2 - 6 * pow2;
				}
				if (step >= elements.Length)
				{
					break;
				}
				stepSequence.AddFirst(step);//倒过来插入
				++k;
			}
		}

		//按指定步长进行排序，底层即插入排序
		private void SortByStep(int step)
		{
			//对每一列进行插入排序，共有step列
			//假设某一个元素要比较的的下一个元素在第row行第col列，即该元素可以表示为 col + row * step
			for (int col = 0; col < step; col++)
			{
				//与插入排序一样，第一个位置的元素已经就绪（row = 0），应该从第二个开始
				for (int begin = col + step; begin < elements.Length; begin += step)//步长顾名思义即下一个元素的距离
				{
					int cur = begin;
					while (cur > col && CompareByIndex(cur, cur - step) < 0)//和上一个比较，直到首元素col
					{
						Swap(cur, cur - step);
						cur -= step;//指针前移
					}
				}
			}
		}
	}
}
