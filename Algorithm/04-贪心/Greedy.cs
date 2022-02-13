using Algorithm.Util;
using System;
using System.Collections.Generic;

namespace Algorithm.贪心
{
	static class Greedy
	{
		//贪心算法
		//优点：简单、高效，无须穷举所有解，通常作为其他算法的辅助算法
		//缺点：鼠目寸光，不会回溯，难以得到全局最优解

		//装载问题（海盗问题）
		//求出能装载最多的数量
		public static int Pirate(int capacity, params int[] array)
		{
			Array.Sort(array);//先将数组从小到大排序，然后一件件选即可
			int currentWeight = 0, count = 0;
			for (int i = 0; i < array.Length; i++)
			{
				currentWeight += array[i];
				if (currentWeight > capacity)
				{
					return count;
				}
				Console.WriteLine("选择了" + array[i]);
				++count;
			}
			return count;
		}

		//找零钱
		//假设有许多不同面值的硬币，找零钱，使用到的硬币个数最少
		//但是使用贪心策略并不一定能取得全局最优解
		//如：25，20，5，1的面值，找41元零钱会得到25，5，5，5，1的硬币，但明显最优解是20，20，1
		public static int CoinChange(int money, params int[] values)
		{
			Array.Sort(values);//由于Sort是从小到大，故后续需要从尾部开始
			int count = 0;
			for (int i = values.Length - 1; i >= 0; --i)
			{
				if (money < values[i])
				{
					continue;//如果当前的money比当前面值还要小就到下一轮去找更小的面值试
				}
				Console.WriteLine("选择了" + values[i]);
				money -= values[i++];//要再回来这一层，因为不确定当前面值还能不能使用
				++count;
			}
			return count;
		}
		public static int CoinChange_Better(int money, params int[] values)
		{
			Array.Sort(values);//由于Sort是从小到大，故后续需要从尾部开始
			int i = values.Length - 1, count = 0;
			//当已经没有符合要求的面值（超出索引）时停止遍历
			while (i >= 0)
			{
				if (money < values[i])
				{
					--i;
					continue;//如果当前的money比当前面值还要小就到下一轮去找更小的面值试
				}
				Console.WriteLine("选择了" + values[i]);
				money -= values[i];
				++count;
			}
			return count;
		}

		//0-1背包
		//n件物件和一个最大承重为W的背包，每件物品重量为wi、价值为vi，每个物件只有一件
		//用贪心策略有三种方案，但其实都不太靠谱
		//①价值主导；②重量主导；③价值密度主导（价值/重量）
		public static void Knapsack(Article[] articles, int capacity, Comparison<Article> comparison)
		{
			Array.Sort(articles, comparison);//按价值规则先排好序
			List<Article> list = new List<Article>();
			int currentWeight, weight = 0, value = 0;
			for (int i = 0; i < articles.Length && weight < capacity; i++)
			{
				currentWeight = weight + articles[i].Weight;
				if (currentWeight <= capacity)
				{
					weight = currentWeight;
					value += articles[i].Value;
					list.Add(articles[i]);
				}
			}
			Console.WriteLine("总重量：" + weight + "  总价值：" + value);
			foreach (var item in list)
			{
				Console.WriteLine(item.ToString());
			}
		}
	}
}