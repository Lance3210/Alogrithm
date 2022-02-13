using System;

namespace Algorithm.分治
{
	static class DivideAndConper
	{
		//分治

		//最大连续子序列
		//暴力遍历法 O(n^3)
		public static int MaxSubarry0(int[] nums)
		{
			if (nums == null || nums.Length == 0)
			{
				return 0;
			}
			int sum = 0, max = int.MinValue;//初值要最小，不然没法比
			for (int begin = 0; begin < nums.Length; begin++)
			{
				for (int end = begin; end < nums.Length; end++)
				{
					for (int i = begin; i <= end; i++)
					{
						sum += nums[i];
					}
					max = Math.Max(sum, max);
					sum = 0;
				}
			}
			return max;
		}
		//暴力遍历法优化 O(n^2)
		public static int MaxSubarry1(int[] nums)
		{
			if (nums == null || nums.Length == 0)
			{
				return 0;
			}
			int sum = 0, max = int.MinValue;//初值要最小，不然没法比
			for (int begin = 0; begin < nums.Length; begin++)
			{
				for (int end = begin; end < nums.Length; end++)
				{
					sum += nums[end];
					max = Math.Max(sum, max);
				}
				sum = 0;
			}
			return max;
		}

		//分治法 T(n) = 2 * T(n/2) + O(n) = O(nlogn)
		//假设问题的解为S[i, j)，则有3种情况
		//[i, j)存在于[begin, mid)中
		//[i, j)存在于[mid, end)中
		//[i, j)存在于[i, mid) + [mid, j)中
		public static int MaxSubarry(int[] nums)
		{
			if (nums == null || nums.Length == 0)
			{
				return 0;
			}
			return MaxSubarry_Divide(nums, 0, nums.Length);//左闭右开
		}
		private static int MaxSubarry_Divide(int[] nums, int begin, int end)
		{
			if (end - begin < 2)
			{
				return nums[begin];
			}
			int mid = begin + ((end - begin) >> 1);
			//从mid开始，分别往两边一个个相加直到算出最大值，最后将两边相加即为第三种情况
			int leftSum = 0, leftMax = int.MinValue;//初值要最小，不然没法比
			for (int i = mid - 1; i >= begin; i--)
			{
				leftSum += nums[i];
				leftMax = Math.Max(leftMax, leftSum);
			}
			int rightSum = 0, rightMax = int.MinValue;
			for (int i = mid; i < end; i++)
			{
				rightSum += nums[i];
				rightMax = Math.Max(rightMax, rightSum);
			}
			return Math.Max(leftMax + rightMax,
				Math.Max(MaxSubarry_Divide(nums, begin, mid),
				MaxSubarry_Divide(nums, mid, end)));//只需求出以上3种情况中最大的即可
		}

	}
}
