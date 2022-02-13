using System;

namespace Exercises.动态规划
{
	class DPExe
	{
		/***************************************leetcode***************************************/
		// Title : 122. 买卖股票的最佳时机 II
		// URL :   https://leetcode-cn.com/problems/best-time-to-buy-and-sell-stock-ii/
		// Brief : 贪心算法
		/***************************************leetcode***************************************/
		public int MaxProfit(int[] prices)
		{
			if (prices == null) return default;
			int profit = 0;
			for (int i = 1; i < prices.Length; i++)//i = 1，避免多次 - 1运算
			{
				if (prices[i - 1] < prices[i])
				{
					profit += prices[i] - prices[i - 1];//只要今天比明天低就立刻买抛
				}
			}
			return profit;
		}

		/***************************************leetcode***************************************/
		// Title : 11. 盛最多水的容器
		// URL :   https://leetcode-cn.com/problems/container-with-most-water/
		// Brief : 
		/***************************************leetcode***************************************/
		//双指针
		public int MaxArea(int[] height)
		{
			int left = 0, right = height.Length - 1;
			int min, result = 0;
			while (left != right)
			{
				min = Math.Min(height[left], height[right]) * (right - left);//取出两边最低的用于计算面积
				result = Math.Max(result, min);//与上一轮比较，选出最大的面积
				if (height[left] < height[right])
				{
					++left;//哪边最小就移动哪边
				}
				else
				{
					--right;
				}
			}
			return result;
		}
	}
}
