using DataStructure.树.二叉树;
using DataStructure.链表.单链表;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithm.算法题目 {
	class LeetCode {
		/***************************************leetcode***************************************/
		// Title : 26. 删除有序数组中的重复项
		// URL :   https://leetcode-cn.com/problems/remove-duplicates-from-sorted-array/
		// Brief : 
		/***************************************leetcode***************************************/
		//双指针
		public int RemoveDuplicates(int[] nums) {
			if (nums == null || nums.Length == 0) return 0;
			int left = 0;
			int right = 1;
			while (right < nums.Length) {
				if (nums[left] != nums[right])//一旦发现两个不同，则左指针向前一步，然后把右指针值给左指针
				{
					nums[++left] = nums[right];//这样从第一个开始到左指针就不会有重复的了
				}
				++right;
			}
			return ++left;//返回的就是修改后原数组的“length”，但实际上length后面不一定都给改了
		}

		/***************************************leetcode***************************************/
		// Title : 189. 旋转数组
		// URL :   https://leetcode-cn.com/problems/rotate-array/
		// Brief : 
		/***************************************leetcode***************************************/
		public void Rotate(int[] nums, int k) {
			if (nums == null || nums.Length <= 1) return;
			k %= nums.Length;//有可能k会超过数组长
			RotateBase(nums, 0, nums.Length - 1);
			RotateBase(nums, 0, k - 1);
			RotateBase(nums, k, nums.Length - 1);
		}
		public void RotateBase(int[] nums, int start, int end) {
			int temp;
			while (start < end) {
				temp = nums[start];
				nums[start++] = nums[end];
				nums[end--] = temp;
			}
		}

		/***************************************leetcode***************************************/
		// Title : 136. 只出现一次的数字
		// URL :   https://leetcode-cn.com/problems/single-number/
		// Brief :
		// 异或运算
		// 交换律：a ^ b ^ c <=> a ^ c ^ b
		// 任何数与0异或为本身 0 ^ n => n
		// 相同的数异或为0: n ^ n => 0
		// var a = [2, 3, 2, 4, 4]
		// 2 ^ 3 ^ 2 ^ 4 ^ 4等价于 2 ^ 2 ^ 4 ^ 4 ^ 3 => 0 ^ 0 ^3 => 3
		/***************************************leetcode***************************************/
		public int SingleNumber(int[] nums) {
			if (nums == null) return default;
			int result = 0;
			for (int i = 0; i < nums.Length; i++) {
				result ^= nums[i];
			}
			return result;
		}

		/***************************************leetcode***************************************/
		// Title : 217. 存在重复元素
		// URL :   https://leetcode-cn.com/problems/contains-duplicate/
		// Brief : 
		/***************************************leetcode***************************************/
		//先排序然后遍历
		public bool ContainsDuplicate(int[] nums) {
			Array.Sort(nums);
			for (int i = 1; i < nums.Length; i++)//i = 1，避免多次 - 1运算
			{
				if (nums[i - 1] == nums[i])
					return true;
			}
			return false;
		}

		/***************************************leetcode***************************************/
		// Title : 122. 买卖股票的最佳时机 II
		// URL :   https://leetcode-cn.com/problems/best-time-to-buy-and-sell-stock-ii/
		// Brief : 
		/***************************************leetcode***************************************/
		//贪心算法
		public int MaxProfit(int[] prices) {
			if (prices == null) return default;
			int profit = 0;
			for (int i = 1; i < prices.Length; i++)//i = 1，避免多次 - 1运算
			{
				if (prices[i - 1] < prices[i]) {
					profit += prices[i] - prices[i - 1];//只要今天比明天低就立刻买抛
				}
			}
			return profit;
		}

		/***************************************leetcode***************************************/
		// Title : 350. 两个数组的交集 II
		// URL :   https://leetcode-cn.com/problems/intersection-of-two-arrays-ii/
		// Brief : 
		/***************************************leetcode***************************************/
		//排序加双指针
		public int[] Intersect(int[] nums1, int[] nums2) {
			if (nums2 == null || nums2 == null) return default;
			List<int> list = new List<int>();//用列表装载后转成数组
			Array.Sort(nums1);
			Array.Sort(nums2);
			int left = 0;
			int right = 0;
			while (left < nums1.Length && right < nums2.Length) {
				if (nums1[left] < nums2[right]) {
					++left;
				}
				else if (nums1[left] > nums2[right]) {
					++right;
				}
				else {
					list.Add(nums1[left++]);//出现相同则加入
					++right;
				}
			}
			return list.ToArray();
		}

		/***************************************leetcode***************************************/
		// Title : 88. 合并两个有序数组
		// URL :   https://leetcode-cn.com/problems/merge-sorted-array/
		// Brief : 
		/***************************************leetcode***************************************/
		//双指针
		public void Merge(int[] nums1, int m, int[] nums2, int n) {
			int left = m - 1;
			int right = n - 1;
			int tail = m + n - 1;
			while (right >= 0) {
				if (left < 0 || nums1[left] <= nums2[right]) {
					nums1[tail--] = nums2[right--];
				}
				else {
					nums1[tail--] = nums1[left--];
				}
			}
		}

		/***************************************leetcode***************************************/
		// Title : 206. 反转链表
		// URL :   https://leetcode-cn.com/problems/reverse-linked-list/
		// Brief : 
		/***************************************leetcode***************************************/
		//递归
		ListNode ReverseList1(ListNode head) {
			if (head == null || head.next == null) return head;//如果不加上head.next，下面的head.next.next就会是空，因为head指向最后一个了
			ListNode newHead = ReverseList1(head.next);//递归到最后返回的就是新的头结点
			head.next.next = head;//对这一层的前后两个节点翻转
			head.next = null;
			return newHead;//每次翻转完都返回头结点
		}

		//迭代
		ListNode ReverseList2(ListNode head) {
			if (head == null || head.next == null) return head;
			ListNode newHead = null;
			ListNode temp;
			while (head != null) {
				temp = head.next;
				head.next = newHead;
				newHead = head;
				head = temp;
			}
			return newHead;
		}

		/***************************************leetcode***************************************/
		// Title : 237. 删除链表节点（只有目标节点）
		// URL :   https://leetcode-cn.com/problems/delete-node-in-a-linked-list/
		// Brief : 
		/***************************************leetcode***************************************/
		public void DeleteNode(ListNode node) {
			if (node == null) return;
			if (node.next == null) {
				node = null;
				return;
			}
			node.val = node.next.val;
			node.next = node.next.next;
		}

		/***************************************leetcode***************************************/
		// Title : 19. 删除链表倒数第 n 个节点
		// URL :   https://leetcode-cn.com/problems/remove-nth-node-from-end-of-list/
		// Brief : 
		/***************************************leetcode***************************************/
		//递归1
		int cur = 0;//这里从0开始会让整个循环多出一次，而多出来的一次就是要实现删除该节点（next指针覆盖）
		public ListNode RemoveNthFromEnd1(ListNode head, int n) {
			if (head == null) return head;
			head.next = RemoveNthFromEnd1(head.next, n);
			return ++cur == n ? head.next : head;
		}

		//递归2
		public ListNode RemoveNthFromEnd2(ListNode head, int n) {
			if (head == null) return head;
			int pos = GetPos(head, n);
			if (pos == n)//遍历完长度与n相同那就是删除头节点
				head = head.next;
			return head;
		}
		public int GetPos(ListNode node, int n) {
			if (node == null) return 0;
			int pos = GetPos(node.next, n) + 1;//从最后面开始算
			if (pos == n + 1)//获取到目标位置的前一个，然后进行覆盖
				node.next = node.next.next;
			return pos;
		}

		//迭代
		public int GetLength(ListNode node) {
			int length = 0;
			while (node != null) {
				++length;
				node = node.next;
			}
			return length;
		}
		public ListNode RemoveNthFromEnd3(ListNode head, int n) {
			if (head == null) return head;
			int length = GetLength(head);
			if (n == length) return head.next;
			ListNode curr = head;
			for (int i = 0; i < length - n - 1; i++) {
				curr = curr.next;
			}
			curr.next = curr.next.next;
			return head;
		}

		//双指针
		public ListNode RemoveNthFromEnd4(ListNode head, int n) {
			if (head == null) return head;
			if (head.next == null) return head.next;
			ListNode left = head;
			ListNode right = head;
			for (int i = 0; i < n; ++i)//先让右指针走n步
			{
				right = right.next;
			}
			if (right == null) return head.next;//左指针都还没开始右指针就为空说明目标位置就是顺数第一个，返回的节点直接next即可
			while (right.next != null)//等右指针为null即左指针指向目标位置
			{
				left = left.next;
				right = right.next;
			}
			left.next = left.next.next;//删除
			return head;
		}

		/***************************************leetcode***************************************/
		// Title : 21. 合并两个有序链表
		// URL :   https://leetcode-cn.com/problems/merge-two-sorted-lists/
		// Brief : 
		/***************************************leetcode***************************************/
		//双指针
		public ListNode MergeTwoLists(ListNode l1, ListNode l2) {
			if (l1 == null)
				return l2;
			if (l2 == null)
				return l1;

			ListNode p1 = l1;
			ListNode p2 = l2;
			ListNode head = new ListNode();
			ListNode temp = head;
			while (p1 != null && p2 != null) {
				if (p1.val <= p2.val)//较小或相等的值先进入
				{
					temp.next = p1;
					p1 = p1.next;
				}
				else {
					temp.next = p2;
					p2 = p2.next;
				}
				temp = temp.next;
			}
			temp.next = p2 == null ? p1 : p2;//谁不为空就直接接入新链表尾部
			return head.next;
		}

		/***************************************leetcode***************************************/
		// Title : 141. 环形链表
		// URL :   https://leetcode-cn.com/problems/linked-list-cycle/
		// Brief : 
		/***************************************leetcode***************************************/
		//快慢指针
		public bool HasCycle(ListNode head) {
			if (head == null)
				return false;
			ListNode slow = head;
			ListNode fast = head;
			while (fast != null && fast.next != null)//判断快指针是否会碰到尾部
			{
				slow = slow.next;
				fast = fast.next.next;
				if (slow == fast)
					return true;
			}
			return false;
		}

		/***************************************leetcode***************************************/
		// Title : 234. 回文链表
		// URL :   https://leetcode-cn.com/problems/palindrome-linked-list/
		// Brief : 
		/***************************************leetcode***************************************/
		//双指针反转链表
		public bool IsPalindrome(ListNode head) {
			if (head == null)
				return false;
			if (head.next == null)
				return true;
			ListNode slow = head;
			ListNode fast = head;
			while (fast != null && fast.next != null) {
				fast = fast.next.next;
				slow = slow.next;
			}
			if (fast != null) slow = slow.next;//奇数判断
			slow = ReverseList1(slow);  //反转后半段
			fast = head;
			while (slow != null) {
				if (slow.val != fast.val)
					return false;
				slow = slow.next;
				fast = fast.next;
			}
			return true;
		}

		//栈
		public bool IsPalindrome2(ListNode head) {
			if (head == null)
				return false;
			if (head.next == null)
				return true;
			Stack<int> stack = new Stack<int>();
			ListNode temp = head;
			while (temp != null) {
				stack.Push(temp.val);
				temp = temp.next;
			}
			temp = head;
			while (temp != null) {
				if (stack.Pop() != temp.val)
					return false;
				temp = temp.next;
			}
			return true;
		}

		//递归
		ListNode temp;
		public bool IsPalindrome3(ListNode head) {
			if (head == null)
				return false;
			if (head.next == null)
				return true;
			temp = head;
			return Check(head);
		}
		private bool Check(ListNode head) {
			if (head == null)
				return true;
			bool result = Check(head.next) && (temp.val == head.val);//前面的条件就是遍历到最后一个，然后头尾比较
			temp = temp.next;//头下一个，尾也跟着回一层
			return result;
		}

		/***************************************leetcode***************************************/
		// Title : 2. 两数相加
		// URL :   https://leetcode-cn.com/problems/add-two-numbers/
		// Brief : 
		/***************************************leetcode***************************************/
		//迭代
		public ListNode AddTwoNumbers(ListNode l1, ListNode l2) {
			ListNode head = new ListNode(0);
			ListNode cur = head;//用于返回的新链表（带头指针）
			int carry = 0;//两位相加大于9，用于给下一位补上

			while (l1 != null || l2 != null) {
				int sum = (l1 == null ? 0 : l1.val) + (l2 == null ? 0 : l2.val) + carry;//对应位空则补0

				carry = sum > 9 ? 1 : 0;//大于9就要补
				sum %= 10;//取出个位
				cur.next = new ListNode(sum);//不直接给cur赋值，因为下面cur要向后移动

				cur = cur.next;//都后移
				if (l1 != null)
					l1 = l1.next;
				if (l2 != null)
					l2 = l2.next;
			}

			if (carry == 1)//遍历完全部还是1说明最高位要补一位
			{
				cur.next = new ListNode(carry);
			}
			return head.next;//head相当于头指针而非头结点指向cur，因此head.next才是算出的数	
		}

		/***************************************leetcode***************************************/
		// Title : 从尾到头打印链表（返回数组）
		// URL :   https://leetcode-cn.com/leetbook/read/illustration-of-algorithm/5dt66m/
		// Brief : 
		/***************************************leetcode***************************************/
		List<int> list = new List<int>();
		public int[] ReversePrint(ListNode head) {
			Rev(head);
			return list.ToArray();
		}
		//利用递归
		public void Rev(ListNode head) {
			if (head == null)
				return;
			Rev(head.next);
			list.Add(head.val);
		}

		/***************************************leetcode***************************************/
		// Title : 20. 有效的括号
		// URL :   https://leetcode-cn.com/problems/valid-parentheses/
		// Brief : 
		/***************************************leetcode***************************************/
		//栈
		public bool IsValid(string s) {
			if (s.Length <= 1) return false;
			Stack<char> stack = new Stack<char>();
			for (int i = 0; i < s.Length; i++) {
				char ch = s[i];
				if (ch == '(' || ch == '[' || ch == '{') {
					stack.Push(ch);
				}
				else {
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
		public bool IsValid2(string s) {
			if (s.Length <= 1) return false;
			Dictionary<char, char> dic = new Dictionary<char, char>() {
				{ '(',')'},
				{ '[',']'},
				{ '{','}'}
			};
			Stack<char> stack = new Stack<char>();
			for (int i = 0; i < s.Length; i++) {
				char ch = s[i];
				if (dic.ContainsKey(ch)) {
					stack.Push(ch);
				}
				else if (stack.Count == 0 || ch != dic[stack.Pop()]) {
					return false;//如果栈中没有且弹出的左不与进来的右对应进就false
				}
			}
			return stack.Count == 0;
		}
	}

	/***************************************leetcode***************************************/
	// Title : 剑指 Offer 30. 包含 min 函数的栈
	// URL :   https://leetcode-cn.com/leetbook/read/illustration-of-algorithm/50bp33/
	// Brief : 
	/***************************************leetcode***************************************/
	//用两个栈实现获取最小值
	public class MinStack {
		Stack<int> stack;
		Stack<int> minS;

		public MinStack() {
			stack = new Stack<int>();
			minS = new Stack<int>();
		}

		public void Push(int x) {
			stack.Push(x);
			if (minS.Count == 0) {
				minS.Push(x);
			}
			else {
				minS.Push(minS.Peek() < x ? minS.Peek() : x);//最小栈中的栈顶一定是最小的那个数
			}
		}

		public void Pop() {
			stack.Pop();
			minS.Pop();//需要一起弹出
		}

		public int Top() {
			return stack.Peek();
		}

		public int Min() {
			return minS.Peek();
		}
		/***************************************leetcode***************************************/
		// Title : 232. 用栈实现队列
		// URL :   https://leetcode-cn.com/problems/implement-queue-using-stacks/
		// Brief : 
		/***************************************leetcode***************************************/
		public class MyQueue {
			Stack<int> inStack;
			Stack<int> outStack;
			/** Initialize your data structure here. */
			public MyQueue() {
				inStack = new Stack<int>();
				outStack = new Stack<int>();
			}

			/** Push element x to the back of queue. */
			public void Push(int x) {
				inStack.Push(x);
			}

			/** Removes the element from in front of queue and returns that element. */
			public int Pop() {
				CheckOutStack();
				return outStack.Pop();//不管怎样outStack只负责弹出队头
			}

			/** Get the front element. */
			public int Peek() {
				CheckOutStack();
				return outStack.Peek();
			}

			/** Returns whether the queue is empty. */
			public bool Empty() {
				return inStack.Count == 0 && outStack.Count == 0;
			}

			//检查outStack是否满足弹出条件
			public void CheckOutStack() {
				if (outStack.Count == 0)//只要outStack为空就从inStack中转移
				{
					while (inStack.Count != 0)
						outStack.Push(inStack.Pop());
				}
			}
		}
		/***************************************leetcode***************************************/
		// Title : 53. 最大子序和
		// URL :   https://leetcode-cn.com/problems/maximum-subarray/
		// Brief : 
		/***************************************leetcode***************************************/
		public int MaxSubArray(int[] nums) {
			if (nums == null || nums.Length == 0) {
				return 0;
			}
			return MaxSubarry_Divide(nums, 0, nums.Length);//左闭右开
		}
		private static int MaxSubarry_Divide(int[] nums, int begin, int end) {
			if (end - begin < 2) {
				return nums[begin];
			}
			int mid = begin + ((end - begin) >> 1);
			//从mid开始，分别往两边一个个相加直到算出最大值，最后将两边相加即为第三种情况
			int leftSum = 0, leftMax = int.MinValue;
			for (int i = mid - 1; i >= begin; i--) {
				leftSum += nums[i];
				leftMax = Math.Max(leftMax, leftSum);
			}
			int rightSum = 0, rightMax = int.MinValue;
			for (int i = mid; i < end; i++) {
				rightSum += nums[i];
				rightMax = Math.Max(rightMax, rightSum);
			}
			return Math.Max(leftMax + rightMax,
				Math.Max(MaxSubarry_Divide(nums, begin, mid),
				MaxSubarry_Divide(nums, mid, end)));//只需求出以上3种情况中最大的即可
		}

		/***************************************leetcode***************************************/
		// Title : 476. 数字的补数
		// URL :   https://leetcode-cn.com/problems/number-complement/
		// Brief : 
		/***************************************leetcode***************************************/
		public int FindComplement(int num) {
			int bit = 1;
			while (bit < num) {
				bit |= bit << 1;//num有多少位就创建一个有多少位全是1的数
			}
			return bit ^ num;//最后另这个数与num异或，则能达到取反位效果
		}

		/***************************************leetcode***************************************/
		// Title : 22. 括号生成
		// URL :   https://leetcode-cn.com/problems/generate-parentheses/
		// Brief : 
		/***************************************leetcode***************************************/
		List<string> result = new List<string>();
		public IList<string> GenerateParenthesis(int n) {
			if (n < 1) {
				return result;
			}
			Operate("", n, n);//传入n个left和right，每拼接一个对应的括号就要-1
			return result;
		}
		public void Operate(string curStr, int left, int right) {
			if (left == 0 && right == 0) {
				result.Add(curStr);
				return;
			}
			if (left > right) {
				return;//剪枝（回溯时将已经拼接的去掉了），只有left的个数比right多时才可以拼接
			}
			if (left > 0) {
				Operate(curStr + "(", left - 1, right);//left还剩，放入一个left
			}
			if (right > 0) {
				Operate(curStr + ")", left, right - 1);//right还剩，放入一个right
			}
			//执行到这里会自动返回上一层
		}


		/***************************************leetcode***************************************/
		// Title : 11. 盛最多水的容器
		// URL :   https://leetcode-cn.com/problems/container-with-most-water/
		// Brief : 
		/***************************************leetcode***************************************/
		//双指针
		public int MaxArea(int[] height) {
			int left = 0, right = height.Length - 1;
			int min, result = 0;
			while (left != right) {
				min = Math.Min(height[left], height[right]) * (right - left);//取出两边最低的用于计算面积
				result = Math.Max(result, min);//与上一轮比较，选出最大的面积
				if (height[left] < height[right]) {
					++left;//哪边最小就移动哪边
				}
				else {
					--right;
				}
			}
			return result;
		}

		/***************************************leetcode***************************************/
		// Title : 9. 回文数
		// URL :   https://leetcode-cn.com/problems/palindrome-number/
		// Brief : 
		/***************************************leetcode***************************************/
		public bool IsPalindrome(int x) {
			if (x < 0 || (x != 0 && x % 10 == 0)) {
				return false;
			}

			int halfReverse = 0;
			//只要后半段反转的结果大于或等于前半段则刚好到中间了
			while (halfReverse < x) {
				halfReverse = halfReverse * 10 + x % 10;
				x /= 10;
			}
			//奇数则另外/10
			return x == halfReverse || x == halfReverse / 10;
		}

		/***************************************leetcode***************************************/
		// Title : Z 字形变换（明明是N型）
		// URL :   https://leetcode-cn.com/problems/zigzag-conversion/
		// Brief : 
		/***************************************leetcode***************************************/
		public string Convert(string s, int numRows) {
			if (numRows < 2) {
				return s;
			}
			List<StringBuilder> list = new List<StringBuilder>();
			//创建numsRows个行来存储
			for (int i = 0; i < numRows; ++i) {
				list.Add(new StringBuilder());
			}

			int flag = -1;//转折标记
			int index = 0;//index的区间为[0, numRows - 1]

			//遍历每一个字符
			for (int i = 0; i < s.Length; ++i) {
				list[index].Append(s[i]);
				if (index == 0 || index == numRows - 1)//到达转折点
				{
					flag = -flag;//直接反转
				}
				index += flag;//一行行插入
			}
			StringBuilder sb = new StringBuilder();
			//拼接所有行
			for (int i = 0; i < list.Count; ++i) {
				sb.Append(list[i]);
			}
			return sb.ToString();
		}

		/***************************************leetcode***************************************/
		// Title : 7. 整数反转
		// URL :   https://leetcode-cn.com/problems/reverse-integer/
		// Brief : 如果反转后整数超过 32 位的有符号整数的范围 [−231,  231 − 1] ，就返回 0。
		// 假设环境不允许存储 64 位整数（有符号或无符号）。
		/***************************************leetcode***************************************/
		public int Reverse(int x) {
			int result = 0;
			while (x != 0) {
				//就是每次将最高位放到个位时都要判断是否溢出
				//而且要在最大位前一个就开始判断了
				if (result > int.MaxValue / 10 || result < int.MinValue / 10) {
					return 0;
				}
				int digit = x % 10;
				x /= 10;
				result = result * 10 + digit;//这里是从0位一个个向前补的
			}
			return result;
		}

		/***************************************leetcode***************************************/
		// Title : 75. 颜色分类
		// URL :   https://leetcode-cn.com/problems/sort-colors/
		// Brief : 
		/***************************************leetcode***************************************/
		//双指针
		public void SortColors(int[] nums) {
			int p0 = 0;
			int p2 = nums.Length - 1;
			for (int i = 0; i <= p2; ++i)//直到i与右指针相交
			{
				//使用while是因为要避免2与2的调换如[2,1,2]，只要num[i]（在调换后仍然还）是2就一直不停调换到右指针p2去
				while (i <= p2 && nums[i] == 2) {
					nums[i] = nums[p2];
					nums[p2] = 2;
					--p2;//右指针前移
				}
				if (nums[i] == 0)//发现0就调换到左指针p0去
				{
					nums[i] = nums[p0];
					nums[p0] = 0;
					++p0;//左指针后移
				}
			}
		}

		//面试题：一颗完全二叉树有768个节点，求子节点个数
		//假设叶子节点个数为n0个，度为1节点的有n1个，度为2节点的右n2个
		//则节点总数：n = n0 + n1 + n2
		//又因为：n0 = n2 + 1
		//则：n = 2n0 + n1 - 1
		//又因为：完全二叉树中，度为1的节点要不为0，要不为1
		//故：n1要被整除只能是1了，结果n0则为384

		//总结点为n
		//n为偶数：叶子节点n0 = n/2
		//n为奇数：叶子节点n0 = (n + 1)/2
		//编程中写成一句即可 n0 = (n + 1) >> 2（默认就是floor的）
		//或n0 = ceiling(n >> 2)

		/***************************************leetcode***************************************/
		// Title : 226. 翻转二叉树
		// URL :   https://leetcode-cn.com/problems/invert-binary-tree/
		// Brief : 因为必须遍历到所有节点，故每一种遍历方法都可以
		/***************************************leetcode***************************************/
		//递归
		//前序
		//后序也差不多
		public Node<int> InvertTree(Node<int> root) {
			if (root == null) return root;

			Node<int> temp = root.left;
			root.left = root.right;
			root.right = temp;

			InvertTree(root.left);
			InvertTree(root.right);
			return root;
		}
		//中序就不同了
		public Node<int> InvertTree2(Node<int> root) {
			if (root == null) return root;
			InvertTree2(root.left);

			Node<int> temp = root.left;
			root.left = root.right;
			root.right = temp;

			InvertTree2(root.left);//注意：因为上面以及将左右调换，故应遍历原本的左子树
			return root;
		}
		//迭代
		//层序遍历
		public Node<int> InvertTree3(Node<int> root) {
			if (root == null) return root;
			Queue<Node<int>> queue = new Queue<Node<int>>();
			queue.Enqueue(root);
			while (queue.Count != 0) {
				Node<int> node = queue.Dequeue();
				Node<int> temp = node.left;
				node.left = node.right;
				node.right = temp;

				if (node.left != null) {
					queue.Enqueue(node.left);
				}
				if (node.right != null) {
					queue.Enqueue(node.right);
				}
			}
			return root;
		}
	}
}
