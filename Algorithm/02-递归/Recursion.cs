using System;
using System.Collections.Generic;
using System.Numerics;

namespace Algorithm.递归
{
	static class Recursion
	{
		#region 斐波那契数列
		//递归实现
		//时间复杂度：O(2^n)，T(n) = T(n - 1) + T(n - 2) + 1
		//空间复杂度：O(n)，即递归深度*每次调用的辅助空间
		//出现过多的重复计算
		public static int Fib_Base(int n)
		{
			if (n <= 2)
			{
				return 1;
			}
			return Fib_Base(n - 1) + Fib_Base(n - 2);
		}

		//优化1：递归，使用数组
		//时间复杂度：O(n)
		//空间复杂度：O(n)
		public static int Fib_Array(int n)
		{
			if (n <= 2)
			{
				return 1;//小于3个都是1
			}
			int[] array = new int[n + 1];//+1是为了让1号位置刚好对应下标1，以此类推，方便后续操作
			array[1] = array[2] = 1;//初始化
			return FibRecursion(n, array);//数组用来装载已经算出结果的数据
		}
		private static int FibRecursion(int i, int[] array)
		{
			if (array[i] == 0)
			{
				array[i] = FibRecursion(i - 1, array) + FibRecursion(i - 2, array);//i位置上的数还未计算则递归计算出
			}
			return array[i];//直接返回对应的值即可
		}

		//优化2：非递归，使用数组
		public static int Fib_Iteration_Array(int n)
		{
			if (n <= 2)
			{
				return 1;
			}
			int[] array = new int[n + 1];
			array[1] = array[2] = 1;//初始化
			for (int i = 3; i <= n; i++)
			{
				array[i] = array[i - 1] + array[i - 2];
			}
			return array[n];
		}

		//优化3：非递归，使用滚动数组
		public static int Fib_Iteration_ScrollArray(int n)
		{
			if (n <= 2)
			{
				return 1;
			}
			int[] array = new int[2];//使用只有两个元素数组
			array[0] = array[1] = 1;
			for (int i = 3; i <= n; i++)
			{
				//array[i % 2] = array[(i - 1) % 2] + array[(i - 2) % 2];//%2会得到0或1
				array[i & 1] = array[(i - 1) & 1] + array[(i - 2) & 1];//用&1代替%2
			}
			return array[n & 1];
		}

		//优化4：非递归，使用两个指针
		//时间复杂度：O(n)
		//空间复杂度：O(1)
		public static int Fib_Iteration_DoublePointer(int n)
		{
			if (n <= 2)
			{
				return 1;
			}
			int first = 1;//只需要两个指针即可
			int second = 1;
			for (int i = 3; i <= n; i++)
			{
				second = first + second;
				first = second - first;//不断覆盖前后指针值即可
			}
			return second;
		}

		//优化5：特征方程，直接使用线性代数解法
		public static int Fib_Characteristic_Equation(int n)
		{
			double c = Math.Sqrt(5);
			return (int)((Math.Pow((1 + c) / 2, n) - Math.Pow((1 - c) / 2, n)) / c);
		}
		#endregion

		#region 爬楼梯
		//有n阶台阶，一步可以上1阶或2阶，走完n阶台阶有多少种解法
		//将问题拆分为子问题：即假设在第一步有多少种走法
		//若第一步为1阶，则剩下n- 1阶，即剩下f(n - 1)种走法
		//若第一步为2阶，则剩下n- 2阶，即剩下f(n - 2)种走法
		//所以f(n) = f(n - 1) + f(n - 2)
		public static int ClimbStairs(int n)
		{
			if (n <= 2)
			{
				return n;//n小于2有两种走法
			}
			return ClimbStairs(n - 1) + ClimbStairs(n - 2);
		}
		//迭代优化，与斐波那契数列思路一致
		public static int ClimbStairs_Iteration(int n)
		{
			if (n <= 2)
			{
				return n;
			}
			int first = 1;
			int second = 2;
			for (int i = 3; i <= n; i++)
			{
				second = first + second;
				first = second - first;
			}
			return second;
		}
		#endregion

		#region 汉诺塔
		//两种情况：
		//n = 1，直接从A移动到C
		//n > 1，可以拆分为3大步骤
		//①：将n - 1个盘子从A移动到B
		//②：将第n个盘子从A移动到C
		//③：将n - 1个盘子从B移动到C
		//该函数代表n个盘子，从p1移动到p3，p2作为中间柱子
		public static void Hanoi(int n, string p1, string p2, string p3)
		{
			if (n == 1)
			{
				Move(n, p1, p3);//递归基
				return;
			}
			Hanoi(n - 1, p1, p3, p2);//即①
			Move(n, p1, p3);//即②
			Hanoi(n - 1, p2, p1, p3);//即③
		}
		private static void Move(int no, string from, string to)
		{
			Console.WriteLine($"将 {no} 号盘子从 {from} 移动到 {to}");
		}
		#endregion

		#region 递归转非递归
		//万能方法：自己维护一个栈，用来保存参数、变量
		//但空间复杂度并没有得到优化
		//递归实现的函数
		public static void Log(int n)
		{
			if (n < 1)
			{
				return;
			}
			Log(n - 1);
			int v = n + 10;
			Console.WriteLine(v);
		}
		//用栈模拟栈帧
		public static void Log2(int n)
		{
			Stack<Vector2> stack = new();
			while (n > 0)
			{
				stack.Push(new Vector2(n, n + 10));
				--n;
			}
			while (stack.Count != 0)
			{
				Vector2 frame = stack.Pop();
				Console.WriteLine(frame.Y);
			}
		}
		//重复利用一组相同的变量来保存栈帧中的内容
		public static void Log3(int n)
		{
			for (int i = 0; i <= n; i++)
			{
				Console.WriteLine(i + 10);
			}
		}
		#endregion

		#region 尾递归
		//尾调用Tail Call：一个函数最后一个动作是调用自身，就是尾调用
		//尾递归Tail Recursion：特殊的尾调用，在最后调用自身，一些编译器能对尾调用进行优化以优化栈空间
		//尾调用优化Tail Call Optimization：也叫Tail Call Elimination尾调用消除
		//如果当前栈帧上的局部变量等内容都不需要再使用了，适当改变后可以直接当做尾调用函数的栈帧，然后程序可以直接jump到该函数
		//尾递归的优化比一般尾调用会简单，因为递归栈大小是不变的

		//递归实现阶乘
		public static int Factorial(int n)
		{
			if (n <= 1)
			{
				return n;
			}
			return n * Factorial(n - 1);
		}
		//阶乘尾递归优化
		public static int Factorial2(int n)
		{
			return Factorial2(n, 1);
		}
		private static int Factorial2(int n, int result)
		{
			if (n <= 1)
			{
				return result;
			}
			return Factorial2(n - 1, result * n);
		}

		//斐波那契数列尾递归优化
		public static int Fib_Tail(int n)
		{
			return Fib_Tail(n, 1, 1);
		}
		private static int Fib_Tail(int n, int first, int second)
		{
			if (n <= 1)
			{
				return first;
			}
			return Fib_Tail(n - 1, second, second + first);
		}
		#endregion


	}
}
