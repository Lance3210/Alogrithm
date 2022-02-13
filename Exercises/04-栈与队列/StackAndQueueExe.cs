using Exercises.Util;
using System.Collections.Generic;


namespace Exercises.栈与队列
{
	class StackAndQueueExe
	{
		/***************************************leetcode***************************************/
		// Title : 739. 每日温度
		// URL :   https://leetcode-cn.com/problems/daily-temperatures/
		// Brief : 
		/***************************************leetcode***************************************/
		//倒着遍历（类似动态规划）
		public int[] DailyTemperatures(int[] temperatures)
		{
			int[] result = new int[temperatures.Length];
			int j;
			for (int i = result.Length - 2; i >= 0; --i)
			{
				j = i + 1;
				while (true)
				{
					if (temperatures[i] < temperatures[j])
					{
						result[i] = j - i;
						break;
					}
					else if (result[j] == 0)
					{
						result[i] = 0;
						break;
					}
					j = result[j] + j;// 需要重新定位
				}
			}
			return result;
		}
		//单调递减栈
		public int[] DailyTemperatures_2(int[] temperatures)
		{
			int[] result = new int[temperatures.Length];
			Stack<int> stack = new Stack<int>();
			for (int i = 0; i < result.Length; i++)
			{
				while (stack.Count != 0 && temperatures[stack.Peek()] < temperatures[i])
				{
					int temp = stack.Pop();
					result[temp] = i - temp;
				}
				stack.Push(i);
			}
			return result;
		}

		/***************************************leetcode***************************************/
		// Title : 最大二叉树（变种：返回每个节点的父节点索引）
		// URL :   
		// Brief : 单调递减栈（从栈底到栈顶）
		/***************************************leetcode***************************************/
		public int[] FindFirst(int[] nums)
		{
			// 用于装载某个节点的左右两边第一个比它大的数的索引
			int[] left_array = new int[nums.Length];
			int[] right_array = new int[nums.Length];
			Stack<int> stack = new Stack<int>();

			for (int i = 0; i < nums.Length; i++)
			{
				left_array[i] = -1;     // 初始化置为-1
				right_array[i] = -1;
				while (stack.Count != 0 && nums[stack.Peek()] < nums[i])
				{
					right_array[stack.Pop()] = i;   // 右边第一个比该节点大
				}
				if (stack.Count != 0)
				{
					left_array[i] = stack.Peek();   // 左边第一个比该节点大
				}
				stack.Push(i);
			}
			// 返回左右中较小的作为父节点（最大二叉树性质）
			int[] result = new int[nums.Length];
			for (int i = 0; i < result.Length; i++)
			{
				if (left_array[i] == -1 && right_array[i] == -1)
				{
					result[i] = -1; // 根节点
					continue;
				}
				if (left_array[i] == -1)
				{
					result[i] = right_array[i];
				}
				else if (right_array[i] == -1)
				{
					result[i] = left_array[i];
				}
				else if (nums[left_array[i]] < nums[right_array[i]])
				{
					result[i] = left_array[i];
				}
				else
				{
					result[i] = right_array[i];
				}
			}
			return result;
		}

		/***************************************leetcode***************************************/
		// Title : 654. 最大二叉树（中等）
		// URL :   https://leetcode-cn.com/problems/maximum-binary-tree/
		// Brief : 类似归并排序的递归操作
		/***************************************leetcode***************************************/
		//递归
		public TreeNode ConstructMaximumBinaryTree(int[] nums)
		{
			return FindRoot(nums, 0, nums.Length);
		}
		//一个在指定范围内找出根节点的方法（左闭右开）
		private TreeNode FindRoot(int[] nums, int left, int right)
		{
			if (left >= right)
			{
				return null;
			}
			int maxIndex = left;
			for (int i = left + 1; i < right; i++)
			{
				if (nums[i] > nums[maxIndex])
				{
					maxIndex = i;
				}
			}
			TreeNode root = new TreeNode(nums[maxIndex]);
			root.left = FindRoot(nums, left, maxIndex);
			root.right = FindRoot(nums, maxIndex + 1, right);
			return root;
		}

		/***************************************leetcode***************************************/
		// Title : 239. 滑动窗口最大值（困难）
		// URL :   https://leetcode-cn.com/problems/sliding-window-maximum/
		// Brief : 双端队列O(n)（单调队列）
		/***************************************leetcode***************************************/
		//使用两个指针表示滑动窗口的头和尾，向右移动
		//使用一个双端队列，存放索引，从头到尾对应的元素逐渐减小
		//当遍历到比队尾大的元素，从队尾出队直到插入元素比队尾小
		//每移动一个需要检测队头元素是否在合法索引范围内[head, tail]，不符合则从队头出队
		//每一轮都完成上面操作后队头即为当前窗口最大值
		public int[] MaxSlidingWindow(int[] nums, int k)
		{
			if (nums == null || nums.Length < 1 || k < 1)
			{
				return null;
			}
			if (nums.Length < 2)
			{
				return nums;
			}
			LinkedList<int> list = new LinkedList<int>();//C#自带的LinkedList就是一个双端队列
			int[] array = new int[nums.Length - k + 1];
			int tail = 0;           //窗口尾部指针
			int head = tail - k + 1;//窗口尾部指针
			while (tail < nums.Length)
			{
				while (list.Count != 0 && nums[list.Last.Value] <= nums[tail])
				{
					list.RemoveLast();//移除所有比插入节点小的元素索引（要判空队列不然报错）
				}
				list.AddLast(tail);//插入窗口尾部索引
				if (list.First.Value < head)
				{
					list.RemoveFirst();//移除不合法的窗口头部索引（不用while的原因是每次只移动一格）
				}
				if (head >= 0)
				{
					array[head] = nums[list.First.Value];//head合法则可以添加进最大值数组
				}
				++head;
				++tail;
			}
			return array;
		}

		/***************************************leetcode***************************************/
		// Title : 20. 有效的括号
		// URL :   https://leetcode-cn.com/problems/valid-parentheses/
		// Brief : 
		/***************************************leetcode***************************************/
		//栈
		public bool IsValid(string s)
		{
			if (s.Length <= 1) return false;
			Stack<char> stack = new Stack<char>();
			for (int i = 0; i < s.Length; i++)
			{
				char ch = s[i];
				if (ch == '(' || ch == '[' || ch == '{')
				{
					stack.Push(ch);
				}
				else
				{
					if (stack.Count == 0) return false;//如果栈中没有就进来了就是错的
					char left = stack.Pop();
					if (left == '(' && ch != ')') return false;
					if (left == '[' && ch != ']') return false;
					if (left == '{' && ch != '}') return false;
				}
			}
			return stack.Count == 0;
		}
		//栈 + 字典
		public bool IsValid2(string s)
		{
			if (s.Length <= 1) return false;
			Dictionary<char, char> dic = new Dictionary<char, char>() {
				{ '(',')'},
				{ '[',']'},
				{ '{','}'}
			};
			Stack<char> stack = new Stack<char>();
			for (int i = 0; i < s.Length; i++)
			{
				char ch = s[i];
				if (dic.ContainsKey(ch))
				{
					stack.Push(ch);
				}
				else if (stack.Count == 0 || ch != dic[stack.Pop()])
				{
					return false;//如果栈中没有且弹出的左不与进来的右对应进就false
				}
			}
			return stack.Count == 0;
		}

		/***************************************leetcode***************************************/
		// Title : 剑指 Offer 30. 包含 min 函数的栈
		// URL :   https://leetcode-cn.com/leetbook/read/illustration-of-algorithm/50bp33/
		// Brief : 
		/***************************************leetcode***************************************/
		//用两个栈实现获取最小值
		public class MinStack
		{
			Stack<int> stack;
			Stack<int> minS;//两个栈的大小会是一致的，最小栈用于过滤

			public MinStack()
			{
				stack = new Stack<int>();
				minS = new Stack<int>();
			}

			public void Push(int x)
			{
				stack.Push(x);
				if (minS.Count == 0)
				{
					minS.Push(x);
				}
				else
				{
					minS.Push(minS.Peek() < x ? minS.Peek() : x);//在Push时，最小栈中的栈也要更新，且栈顶一定是最小的那个数
				}
			}

			public void Pop()
			{
				stack.Pop();
				minS.Pop();//需要一起弹出
			}

			public int Top()
			{
				return stack.Peek();
			}

			public int Min()
			{
				return minS.Peek();
			}
		}

		/***************************************leetcode***************************************/
		// Title : 232. 用栈实现队列
		// URL :   https://leetcode-cn.com/problems/implement-queue-using-stacks/
		// Brief : 
		/***************************************leetcode***************************************/
		public class MyQueue
		{
			Stack<int> inStack;
			Stack<int> outStack;
			/** Initialize your data structure here. */
			public MyQueue()
			{
				inStack = new Stack<int>();
				outStack = new Stack<int>();
			}

			/** Push element x to the back of queue. */
			public void Push(int x)
			{
				inStack.Push(x);
			}

			/** Removes the element from in front of queue and returns that element. */
			public int Pop()
			{
				CheckOutStack();
				return outStack.Pop();//不管怎样outStack只负责弹出队头
			}

			/** Get the front element. */
			public int Peek()
			{
				CheckOutStack();
				return outStack.Peek();
			}

			/** Returns whether the queue is empty. */
			public bool Empty()
			{
				return inStack.Count == 0 && outStack.Count == 0;
			}

			//检查outStack是否满足弹出条件
			public void CheckOutStack()
			{
				if (outStack.Count == 0)//只要outStack为空就从inStack中转移
				{
					while (inStack.Count != 0)
						outStack.Push(inStack.Pop());
				}
			}
		}
	}
}