using Algorithm.Util;
using Algorithm.分治;
using Algorithm.动态规划;
using Algorithm.回溯;
using Algorithm.贪心;
using Algorithm.递归;
using System;
using System.Collections.Generic;

namespace Algorithm.测试 {
	static class Program2 {
		static void Main0(string[] args) {
			TimeTestUtil.TimeTest(null, 0);
			DPTest2();
		}

		//动态规划
		private static void DPTest2() {
			string str1 = "123412341234123412341234123412341234";
			string str2 = "23412341234123412341234123412341234";
			DynamicProgramming dp = new DynamicProgramming();
			//最长公共子序列
			TimeTestUtil.TimeTest(dp.LCS_Recursion, str1, str2);
			TimeTestUtil.TimeTest(dp.LCS, str1, str2);
			TimeTestUtil.TimeTest(dp.LCS_Better, str1, str2);
			//最长上升子序列
			int[] nums1 = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
			TimeTestUtil.TimeTest(dp.LengthOfLIS, nums1);
			TimeTestUtil.TimeTest(dp.LengthOfLIS_Better, nums1);
			//最长公共子串
			TimeTestUtil.TimeTest(dp.LCSS, str1, str2);
			TimeTestUtil.TimeTest(dp.LCSS_Better, str1, str2);
			TimeTestUtil.TimeTest(dp.LCSS_Better2, str1, str2);
			//01背包
			int[] values = new int[] { 1, 2, 3, 4 };
			int[] weights = new int[] { 1, 2, 3, 4 };
			int capacity = 10;
			TimeTestUtil.TimeTest(dp.MaxValue, values, weights, capacity);
			TimeTestUtil.TimeTest(dp.MaxValue_Better, values, weights, capacity);
			TimeTestUtil.TimeTest(dp.MaxValueExactly, values, weights, capacity);
		}

		private static void DPTest1() {
			int count = 60;
			int[] coins = new int[] { 5, 20, 25 };
			DynamicProgramming dp = new DynamicProgramming();
			TimeTestUtil.TimeTest(dp.CoinChange_Recursion, count);
			TimeTestUtil.TimeTest(dp.CoinChange_Better, count);
			TimeTestUtil.TimeTest(dp.CoinChange_Iteration, count);
			TimeTestUtil.TimeTest(dp.CoinChange, coins, count);
			List<int> list = dp.CoinChange_Types(coins, count);
			if (list == null) {
				return;
			}
			foreach (var item in list) {
				Console.WriteLine(item);
			}
		}

		//分治
		private static void DivideTest() {
			int[] nums = new int[] { 5, 4, -1, 7, 8 };
			Console.WriteLine(DivideAndConper.MaxSubarry(nums));
		}

		//贪心
		private static void GreedyTest() {
			Article[] articles = new Article[] {
				new Article(35, 10), new Article(30, 40),new Article(60, 30), new Article(50, 50),
				new Article(40, 35), new Article(10, 40),new Article(25, 30)};
			foreach (var item in articles) {
				Console.WriteLine(item.ToString());
			}
			Console.WriteLine("------------------价值主导-----------------------");
			Greedy.Knapsack(articles, 150, (x, y) => {
				return y.Value - x.Value;//价值主导
			});
			Console.WriteLine("------------------重量主导------------------------");
			Greedy.Knapsack(articles, 150, (x, y) => {
				return x.Weight - y.Weight;//重量主导
			});
			Console.WriteLine("-----------------价值密度主导---------------------");
			Greedy.Knapsack(articles, 150, (x, y) => {
				return y.ValueDensity.CompareTo(x.ValueDensity);//价值密度主导
			});


			//Console.WriteLine(贪心.Greedy.Pirate(30, new int[] { 3, 5, 4, 10, 7, 14, 2, 11 }));
			//Console.WriteLine(贪心.Greedy.CoinChange(41, new int[] { 25, 20, 5, 1 }));
			//Console.WriteLine(贪心.Greedy.CoinChange_Better(41, new int[] { 25, 20, 5, 1 }));
		}

		//回溯
		private static void BackTrackingTest() {
			BackTracking backTracking = new BackTracking();
			TimeTestUtil.TimeTest(() => {
				Console.WriteLine("时间测试");
			});
			TimeTestUtil.TimeTest(backTracking.Queens_Base, 8);
			TimeTestUtil.TimeTest(backTracking.Queens_Better, 8);
			TimeTestUtil.TimeTest(backTracking.Queens_Eight);
		}

		//递归转迭代
		private static void ConvertToIteration() {
			int count = 10000;
			TimeTestUtil.TimeTest(Recursion.Log, count);
			TimeTestUtil.TimeTest(Recursion.Log2, count);
		}

		//斐波那契数列
		private static void FibTest() {
			int count = 10;
			TimeTestUtil.TimeTest<int>(Recursion.Fib_Base, count);
			TimeTestUtil.TimeTest<int>(Recursion.Fib_Array, count);
			TimeTestUtil.TimeTest<int>(Recursion.Fib_Iteration_Array, count);
			TimeTestUtil.TimeTest<int>(Recursion.Fib_Iteration_ScrollArray, count);
			TimeTestUtil.TimeTest<int>(Recursion.Fib_Iteration_DoublePointer, count);
			TimeTestUtil.TimeTest<int>(Recursion.Fib_Characteristic_Equation, count);
		}

	}
}
