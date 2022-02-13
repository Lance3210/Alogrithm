using System;
using System.Collections.Generic;

namespace Algorithm.动态规划
{
	class DynamicProgramming
	{
		//动态规划

		//找零钱
		//假设有许多不同面值的硬币，找零钱，使用到的硬币个数最少
		//但是使用贪心策略并不一定能取得全局最优解
		//如：25，20，5，1的面值，找41元零钱会得到25，5，5，5，1的硬币，但明显最优解是20，20，1

		//暴力递归
		//类似斐波那契数列，存在大量重复计算
		//规模为n的问题，可以分解为以下问题
		//dp(n) = dp(n - i) + 1;（i为面值，+1相当于使用了一个）
		public int CoinChange_Recursion(int n)
		{
			if (n < 1)
			{
				return int.MaxValue;//只要当前面值不能用，则返回一个max以便于上一层Min函数中选择最小的
			}
			if (n == 25 || n == 20 || n == 5 || n == 1)
			{
				return 1;//只要刚好够对应面值，只需要一个即可
			}
			int min1 = Math.Min(CoinChange_Recursion(n - 25), CoinChange_Recursion(n - 20));
			int min2 = Math.Min(CoinChange_Recursion(n - 5), CoinChange_Recursion(n - 1));
			return Math.Min(min1, min2) + 1;//从四种结果中选择所需数量最少的
		}

		//记忆化搜索
		//使用数组来存储之前已经计算过的硬币数
		public int CoinChange_Better(int n)
		{
			if (n < 1)
			{
				return -1;//只要当前面值不能用，则返回一个-1以表示不合理
			}
			int[] faces = new int[] { 1, 5, 20, 25 };
			int[] dp = new int[n + 1];//使用一个数组，记录计算对应数量零钱的硬币个数（索引为n，需要数为值）
			for (int i = 0; i < faces.Length; i++)
			{
				if (n < faces[i])
				{
					break;//连前面的都不能找，后面无须找了
				}
				dp[faces[i]] = 1;//初始化对应零钱需要的个数
			}
			return CoinChange_Memory(dp, n);
		}
		public int CoinChange_Memory(int[] dp, int n)
		{
			if (n < 1)
			{
				return int.MaxValue;//只要当前面值不能用，则返回一个max以便于上一层Min函数中选择最小的
			}
			//没有求过n所需硬币个数
			if (dp[n] == 0)
			{
				int min1 = Math.Min(CoinChange_Memory(dp, n - 25), CoinChange_Memory(dp, n - 20));
				int min2 = Math.Min(CoinChange_Memory(dp, n - 5), CoinChange_Memory(dp, n - 1));
				dp[n] = Math.Min(min1, min2) + 1;//一旦发现更小的，更新该位置对应的个数
			}
			return dp[n];//存在n对应的个数
		}

		//递推
		//自底向上，从较小的值推出较大的值
		public int CoinChange_Iteration(int n)
		{
			if (n < 1)
			{
				return -1;
			}
			int[] dp = new int[n + 1];//使用一个数组，记录计算对应数量零钱的硬币个数（索引为n，需要数为值）
			int min;
			//计算出1到n所有结果
			for (int i = 1; i <= n; i++)
			{
				min = int.MaxValue;
				if (i >= 1)
				{
					min = Math.Min(dp[i - 1], min);
				}
				if (i >= 5)
				{
					min = Math.Min(dp[i - 5], min);
				}
				if (i >= 20)
				{
					min = Math.Min(dp[i - 20], min);
				}
				if (i >= 25)
				{
					min = Math.Min(dp[i - 25], min);
				}
				dp[i] = min + 1;//min就是上面中的最小值
			}
			return dp[n];//直接返回对应值即可
		}

		//优化
		public int CoinChange(int[] coins, int amount)
		{
			int[] dp = new int[amount + 1];
			dp[0] = 0;//最小值初始化为0即可
			int max = int.MaxValue - 1;//-1是不让元素超限，以方便return

			//计算出1到n所有结果
			for (int i = 1; i <= amount; i++)
			{
				dp[i] = max;
				//每一轮循环都会检测选择用哪一张面值的情况下会最少
				for (int j = 0; j < coins.Length; j++)
				{
					if (i >= coins[j] && dp[i - coins[j]] < dp[i])
					{
						dp[i] = dp[i - coins[j]] + 1;//只要发现能使用更大面值的就更新				
					}
				}
			}
			return dp[amount] >= max ? -1 : dp[amount];//若dp[amount]不符合要求，其值无论如何会 >= max
		}

		//返回用了那几张
		public List<int> CoinChange_Types(int[] coins, int amount)
		{
			int[] dp = new int[amount + 1];
			int[] faces = new int[dp.Length];//记录i钱时最后选择的面值
			dp[0] = 0;//最小值初始化为0即可
			int max = int.MaxValue - 1;//-1是不让元素超限，以方便return

			//计算出1到n所有结果
			for (int i = 1; i <= amount; i++)
			{
				dp[i] = max;
				//每一轮循环都会检测选择用哪一张面值的情况下会最少
				for (int j = 0; j < coins.Length; j++)
				{
					if (i >= coins[j] && dp[i - coins[j]] < dp[i])
					{
						dp[i] = dp[i - coins[j]] + 1;//发现更小的就更新		
						faces[i] = coins[j];//用于记录每一次的选择		
					}
				}
			}
			return dp[amount] >= max ? null : Count(faces, amount);//若dp[amount]不符合要求，其值无论如何会 >= max
		}
		private List<int> Count(int[] faces, int amount)
		{
			List<int> list = new List<int>();
			while (amount > 0)
			{
				list.Add(faces[amount]);
				amount -= faces[amount];
			}
			return list;
		}

		//最大连续子序列和
		//定义状态：令dp(i)为第i下标元素的最大子序列和，初始dp(0)即为nums[0]
		//dp[i]的值有两种情况
		//1. 要不就是在dp(i - 1) + nums[i]
		//2. 要不是就是nums[i]（即如果加上nums[i]还没有直接取nums[i]大，即dp(i - 1)负数）
		public int MaxSubArray(int[] nums)
		{
			int[] dp = new int[nums.Length];
			dp[0] = nums[0];
			int max = dp[0];
			for (int i = 1; i < nums.Length; i++)
			{
				dp[i] = Math.Max(nums[i], dp[i - 1] + nums[i]);
				max = Math.Max(max, dp[i]);
			}
			return max;
		}

		//优化，实际上无须使用dp[i]数组，使用一个前缀记录即可
		public int MaxSubArray_Better(int[] nums)
		{
			int pre = nums[0], max = nums[0];
			for (int i = 1; i < nums.Length; i++)
			{
				pre = Math.Max(nums[i], pre + nums[i]);
				max = Math.Max(max, pre);
			}
			return max;
		}

		//最长上升（递增）子序列
		//定义状态：令dp(i)为第i下标元素的最长上升子序列，初始dp(0)即为1（只有一个）
		//动态规划
		//时间复杂度为O(n*m)，空间复杂度为O(n*m)
		public int LengthOfLIS(int[] nums)
		{
			int[] dp = new int[nums.Length];
			int length = dp[0] = 1;
			for (int i = 1; i < dp.Length; ++i)
			{
				dp[i] = 1;//每一个元素开始检测前都默认最长为1
				for (int j = 0; j < i; ++j)
				{
					//遍历其之前的每一个子序列长度，只要发现还能另其变得更长就更新
					if (nums[i] > nums[j])
					{
						dp[i] = Math.Max(dp[i], dp[j] + 1);
					}
				}
				length = Math.Max(length, dp[i]);
			}
			return length;
		}

		//二分搜索解法
		//时间复杂度为O(nlogn)，空间复杂度为O(n)
		//把每个数字看做一张扑克牌，从左到右按顺序处理每一个扑克牌
		//每遍历到一张牌就拿该牌与一个牌堆的牌顶比较，若小于则放在该牌堆上（压住牌顶）
		//若找不到比它大的牌堆，则在右边新建一个牌堆，将这张牌放入
		public int LengthOfLIS_Better(int[] nums)
		{
			int[] top = new int[nums.Length];//表示牌顶的数组
			int length = 0, begin, end, mid;
			for (int i = 0; i < nums.Length; i++)
			{
				begin = 0;
				end = length;
				while (begin < end)
				{
					mid = begin + ((end - begin) >> 1);
					if (nums[i] <= top[mid])
					{
						end = mid;
					}
					else
					{
						begin = mid + 1;
					}
				}
				if (begin == length)
				{
					++length;//检查是否需要新建堆
				}
				top[begin] = nums[i];//改变堆顶或新建堆
			}
			return length;
		}


		//最长公共子序列
		//假设两个序列分别是str1，str2，i、j 范围皆为[1, strX.Length]
		//定义状态：令dp(i, j)为str1[i - 1]和str2[j - 1]元素结尾的最长公共子序列
		//初始dp(i, 0)，dp(0, j)均为0

		//将两个数组拆分看做前i（j）个元素，与当前的str1[i - 1]（和str2[j - 1]）
		//如果str1[i - 1] == str2[j - 1]，那么dp(i, j) = dp(i - 1, j - 1) + 1
		//也就是在前面已经存在的最长公共子序列长度基础上 + 1
		//如果str1[i - 1] != str2[j - 1]，那么dp(i, j) = Max(dp(i - 1 j),dp(i, j - 1))
		//也就是在前面已经存在的最长公共子序列长度基础上 + 1

		//动态规划
		//时间复杂度为O(n*m)，空间复杂度为O(n*m)
		public int LCS(string text1, string text2)
		{
			//使用一个二维数组，表示第i和j位置对应长度 (i, j 范围皆为[0, strX.Length])
			int[,] dp = new int[text1.Length + 1, text2.Length + 1];//故这里容量要+1
			for (int i = 1; i <= text1.Length; i++)
			{
				//dp(i, 0)和dp(0, j)都默认初始值都为0
				for (int j = 1; j <= text2.Length; j++)
				{
					if (text1[i - 1] == text2[j - 1])
					{
						dp[i, j] = dp[i - 1, j - 1] + 1;//当两个序列的末端相等，相当于找到一个符合的
					}
					else
					{
						dp[i, j] = Math.Max(dp[i - 1, j], dp[i, j - 1]);//取两者最大
					}
				}
			}
			return dp[text1.Length, text2.Length];//表示为算出两个数组的最大公共子序列长度
		}

		//动态规划，优化二维数组为一维数组
		//仔细观察，每次确定dp(i, j)时，只会用到dp(i - 1, j)，dp(i, j - 1)和dp(i - 1, j - 1)
		//故只需要记录一行即可，可以实现在一行中(j - 1)和(j)刚好就是(i, j)位置的左边和上边
		//时间复杂度为O(n*m)，空间复杂度为O(m)（m < n）
		public int LCS_Better(string text1, string text2)
		{
			//包装为行列，这样可以使访问的时候无须再转化，自动选择填充最短的作为列
			string rows = text1, cols = text2;//设置默认值
			if (text1.Length < text2.Length)
			{
				cols = text1;//选择最短的作为列
				rows = text2;
			}
			//使用一个一维数组保存上一行变量，而且只需要选择上面确定为最短的列作为数组长即可
			int[] dp = new int[cols.Length + 1];
			int leftTop, current;//使用一个变量保存左上角，相当于dp(i - 1, j - 1)，current作为中介
			for (int row = 1; row <= rows.Length; row++)
			{
				current = 0;//每次开始j从1开始，左上角必然为0
				for (int col = 1; col <= cols.Length; col++)
				{
					leftTop = current;//指向左上角（上一轮的上边）
					current = dp[col];//游标后移，指向当前的上边
					if (rows[row - 1] == cols[col - 1])
					{
						dp[col] = leftTop + 1;//相当于dp[i - 1, j - 1] + 1
					}
					else
					{
						dp[col] = Math.Max(dp[col - 1], dp[col]); //相当于Max(dp[i - 1, j], dp[i, j - 1]);
					}
				}
			}
			return dp[cols.Length];
		}

		//递归实现
		//当两个数组长度n相等，则时间复杂度为O(2^n)，空间复杂度为O(n)
		public int LCS_Recursion(string text1, string text2)
		{
			return LCS(text1, text1.Length, text2, text2.Length);
		}
		private int LCS(string text1, int i, string text2, int j)
		{
			if (i == 0 || j == 0)
			{
				return 0;
			}
			if (text1[i - 1] == text2[j - 1])
			{
				return LCS(text1, i - 1, text2, j - 1) + 1;
			}
			return Math.Max(LCS(text1, i - 1, text2, j), LCS(text1, i, text2, j - 1));
		}

		//最长公共子串
		//假设两个串分别是str1，str2，i、j 范围皆为[1, strX.Length]
		//定义状态：令dp(i, j)为str1[i - 1]和str2[j - 1]元素结尾的最长公共子串
		//初始dp(i, 0)，dp(0, j)均为0
		//如果str[i - 1] == str2[j - 1]，那么dp(i, j) = dp(i - 1, j - 1) + 1
		//如果str[i - 1] != str2[j - 1]，那么dp(i, j) = 0
		public int LCSS(string text1, string text2)
		{
			//使用一个二维数组，表示第i和j位置对应长度 (i, j 范围皆为[0, strX.Length])
			int[,] dp = new int[text1.Length + 1, text2.Length + 1];//故这里容量要+1
			int max = 0;
			for (int i = 1; i <= text1.Length; i++)
			{
				//dp(i, 0)和dp(0, j)都默认初始值都为0
				for (int j = 1; j <= text2.Length; j++)
				{
					//当两个序列的末端相等，就在前面已找到的最长子串基础上+1
					if (text1[i - 1] == text2[j - 1])
					{
						dp[i, j] = dp[i - 1, j - 1] + 1;
						max = Math.Max(max, dp[i, j]);
					}
				}
			}
			return max;
		}

		//动态规划，优化二维数组为一维数组
		//时间复杂度为O(n*m)，空间复杂度为O(m)（m < n）
		public int LCSS_Better(string text1, string text2)
		{
			//包装为行列，这样可以使访问的时候无须再转化，自动选择填充最短的作为列
			string rows = text1, cols = text2;//设置默认值
			if (text1.Length < text2.Length)
			{
				cols = text1;//选择最短的作为列
				rows = text2;
			}
			//使用一个一维数组保存上一行变量，而且只需要选择上面确定为最短的列作为数组长即可
			int[] dp = new int[cols.Length + 1];
			int max = 0, leftTop, current;//使用一个变量保存左上角，相当于dp(i - 1, j - 1)，current作为中介
			for (int row = 1; row <= rows.Length; row++)
			{
				current = 0;
				for (int col = 1; col <= cols.Length; col++)
				{
					leftTop = current;
					current = dp[col];
					if (rows[row - 1] == cols[col - 1])
					{
						dp[col] = leftTop + 1;//相当于dp[i - 1, j - 1] + 1
						max = Math.Max(max, dp[col]);
					}
					else
					{
						dp[col] = 0;//因为要重复使用一维数组，所以要主动置为0
					}
				}
			}
			return max;
		}

		//继续优化，即倒过来遍历，与下面背包问题的优化一致
		public int LCSS_Better2(string text1, string text2)
		{
			string rows = text1, cols = text2;
			if (text1.Length < text2.Length)
			{
				cols = text1;
				rows = text2;
			}
			int[] dp = new int[cols.Length + 1];
			int max = 0;
			for (int row = 1; row <= rows.Length; row++)
			{
				//每一轮开始，从右往左遍历，这样就不需要多余的变量去保存了
				for (int col = cols.Length; col >= 1; --col)
				{
					if (rows[row - 1] == cols[col - 1])
					{
						dp[col] = dp[col - 1] + 1;
						max = Math.Max(max, dp[col]);
					}
					else
					{
						dp[col] = 0;
					}
				}
			}
			return max;
		}

		//0 - 1背包
		//定义状态：令dp(i, j)为当前最大承重为j，有前i个元素可供选择时的最大价值
		//i, j 范围 [1, n], [1, c]，初始dp(i, 0)，dp(0, j)均为0，最终解为dp(n, capacity)
		//如果当前物件重量比容量大，就不选这个物件
		//即dp(i, j) = dp(i - 1, j)
		//如果当前物件重量比容量小，就要判断是选择这个物件还是不选择这个物件的价值更高
		//即dp(i, j) = Max(dp(i - 1, j - weights[i - 1]) + values[i - 1], dp(i - 1, j))
		public int MaxValue(int[] values, int[] weights, int capacity)
		{
			int[,] dp = new int[values.Length + 1, capacity + 1];
			for (int i = 1; i <= values.Length; i++)
			{
				for (int j = 1; j <= capacity; j++)
				{
					if (j < weights[i - 1])
					{
						dp[i, j] = dp[i - 1, j];//不选择该物件，最大价值延续上一个
					}
					else
					{
						//不选择该物件的价值时与剔除相应容量后剩下的最大价值加上当前物件价值，两者取最大
						dp[i, j] = Math.Max(
							dp[i - 1, j],
							dp[i - 1, j - weights[i - 1]] + values[i - 1]);
					}
				}
			}
			return dp[values.Length, capacity];//最终解为当有n件物品时，最大容量为capacity时的解
		}

		//动态规划，优化二维数组为一维数组
		//仔细观察，每次确定dp(i, j)时，只会用到dp(i - 1, j)和dp(i - 1, j - weights[i - 1])
		//上一行可以反过来计算下一行
		public int MaxValue_Better(int[] values, int[] weights, int capacity)
		{
			int[] dp = new int[capacity + 1];
			for (int i = 1; i <= values.Length; i++)
			{
				//每一轮开始，j从右往左开始，一旦发现是有机会选择就更新最大值
				//另外，j如果比weights[i - 1]还小，就无须再判断了，因为都等于对应的上一行的值
				for (int j = capacity; j >= weights[i - 1]; --j)
				{
					//不选择该物件的价值时与剔除相应容量后剩下的最大价值加上当前物件价值，两者取最大
					dp[j] = Math.Max(
						dp[j],
						dp[j - weights[i - 1]] + values[i - 1]);
				}
			}
			return dp[capacity];//最终解为当有n件物品时，最大容量为capacity时的解
		}

		//变形题，求刚好装满时的最大价值
		//只需要初始值定为负无穷即可，因为只要j重量时无法刚好装满，最大价值也就不存在
		public int MaxValueExactly(int[] values, int[] weights, int capacity)
		{
			int[] dp = new int[capacity + 1];
			for (int j = 1; j <= capacity; j++)
			{
				dp[j] = int.MinValue;//用负无穷的原因是，后面计算时要加上value，有可能会超过0
			}
			for (int i = 1; i <= values.Length; i++)
			{
				for (int j = capacity; j >= weights[i - 1]; --j)
				{
					dp[j] = Math.Max(
						dp[j],
						dp[j - weights[i - 1]] + values[i - 1]);
				}
			}
			return dp[capacity];//负数表示无法刚好装满这个容量
		}
	}
}