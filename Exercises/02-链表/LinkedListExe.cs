using Exercises.Util.LeetCode;
using System.Collections.Generic;

namespace Exercises.链表
{
	class LinkedListExe
	{
		/***************************************leetcode***************************************/
		// Title : 86. 分隔链表（中等）
		// URL :   https://leetcode-cn.com/problems/partition-list/
		// Brief : 模拟双链表，小于哨兵节点的分到一个链表，大于等于的分到另一个链表
		/***************************************leetcode***************************************/
		public ListNode Partition(ListNode head, int x)
		{
			if (head == null || head.next == null)
			{
				return head;
			}
			ListNode lHead = new ListNode();//两个dummy，方便操作
			ListNode lTail = lHead;
			ListNode rHead = new ListNode();
			ListNode rTail = rHead;
			while (head != null)
			{
				if (head.val < x)
				{
					lTail = lTail.next = head;
				}
				else
				{
					rTail = rTail.next = head;
				}
				head = head.next;
			}
			rTail.next = null;//rTail的next必须指向空
			lTail.next = rHead.next;//这里为什么是rHead.next，那是因为rHead是一个dummy
			return lHead.next;
		}

		/***************************************leetcode***************************************/
		// Title : 160. 相交链表（简单）
		// URL :   https://leetcode-cn.com/problems/intersection-of-two-linked-lists/
		// Brief : 两个链表首尾互相拼接，在相同长度的条件下遍历
		/***************************************leetcode***************************************/
		public ListNode GetIntersectionNode(ListNode headA, ListNode headB)
		{
			if (headA == null || headB == null)
			{
				return null;
			}
			ListNode node1 = headA, node2 = headB;
			//巧妙的将链表A接到链表B尾部，链表B接到链表A尾部，使两个链表长度相等
			//总会遍历到相等的条件，不会死循环（因为没有相交时两个都为null，也是相等）
			while (node1 != node2)
			{
				node1 = node1 == null ? headB : node1.next;
				node2 = node2 == null ? headA : node2.next;
			}
			return node1;
		}

		/***************************************leetcode***************************************/
		// Title : 203. 移除链表元素（简单）
		// URL :   https://leetcode-cn.com/problems/remove-linked-list-elements/
		// Brief : 虚拟头结点，尾指针遍历拼接
		/***************************************leetcode***************************************/
		public ListNode RemoveElements(ListNode head, int val)
		{
			ListNode newHead = new ListNode();//虚拟头结点
			ListNode newTail = newHead;//先赋值保证后面不会报空错
			while (head != null)
			{
				if (head.val != val)
				{
					newTail = newTail.next = head;//不符合条件的节点就拼接到新链表尾部
				}
				head = head.next;
			}
			newTail.next = null;//尾结点的next要置空，因为有可能最后一个符合条件的没有被去掉
			return newHead.next;//虚拟头结点的next即为新链表的head
		}

		/***************************************leetcode***************************************/
		// Title : 206. 反转链表
		// URL :   https://leetcode-cn.com/problems/reverse-linked-list/
		// Brief : 
		/***************************************leetcode***************************************/
		//递归
		ListNode ReverseList1(ListNode head)
		{
			if (head == null || head.next == null) return head;//如果不加上head.next，下面的head.next.next就会是空，因为head指向最后一个了
			ListNode newHead = ReverseList1(head.next);//递归到最后返回的就是新的头结点
			head.next.next = head;//对这一层的前后两个节点翻转
			head.next = null;
			return newHead;//每次翻转完都返回头结点
		}
		//迭代
		ListNode ReverseList2(ListNode head)
		{
			if (head == null || head.next == null) return head;
			ListNode newHead = null;
			ListNode temp;
			while (head != null)
			{
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
		// Brief : 鸠占鹊巢
		/***************************************leetcode***************************************/
		public void DeleteNode(ListNode node)
		{
			if (node == null) return;
			if (node.next == null)
			{
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
		public ListNode RemoveNthFromEnd1(ListNode head, int n)
		{
			if (head == null) return head;
			head.next = RemoveNthFromEnd1(head.next, n);
			return ++cur == n ? head.next : head;
		}

		//递归2
		public ListNode RemoveNthFromEnd2(ListNode head, int n)
		{
			if (head == null) return head;
			int pos = GetPos(head, n);
			if (pos == n)//遍历完长度与n相同那就是删除头节点
				head = head.next;
			return head;
		}
		public int GetPos(ListNode node, int n)
		{
			if (node == null) return 0;
			int pos = GetPos(node.next, n) + 1;//从最后面开始算
			if (pos == n + 1)//获取到目标位置的前一个，然后进行覆盖
				node.next = node.next.next;
			return pos;
		}

		//迭代
		public int GetLength(ListNode node)
		{
			int length = 0;
			while (node != null)
			{
				++length;
				node = node.next;
			}
			return length;
		}
		public ListNode RemoveNthFromEnd3(ListNode head, int n)
		{
			if (head == null) return head;
			int length = GetLength(head);
			if (n == length) return head.next;
			ListNode curr = head;
			for (int i = 0; i < length - n - 1; i++)
			{
				curr = curr.next;
			}
			curr.next = curr.next.next;
			return head;
		}

		//双指针
		public ListNode RemoveNthFromEnd4(ListNode head, int n)
		{
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
		public ListNode MergeTwoLists(ListNode l1, ListNode l2)
		{
			if (l1 == null)
				return l2;
			if (l2 == null)
				return l1;

			ListNode p1 = l1;
			ListNode p2 = l2;
			ListNode head = new ListNode();
			ListNode temp = head;
			while (p1 != null && p2 != null)
			{
				if (p1.val <= p2.val)//较小或相等的值先进入
				{
					temp.next = p1;
					p1 = p1.next;
				}
				else
				{
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
		public bool HasCycle(ListNode head)
		{
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
		// Title : 234. 回文链表（简单）
		// URL :   https://leetcode-cn.com/problems/palindrome-linked-list/
		// Brief : 
		/***************************************leetcode***************************************/
		//双指针+反转链表
		public bool IsPalindrome(ListNode head)
		{
			if (head == null || head.next == null)
			{
				return true;
			}
			if (head.next.next == null)
			{
				return head.val == head.next.val;
			}

			//快慢指针找出mid节点
			ListNode slow = head;
			ListNode quick = head;
			while (quick != null && quick.next != null)
			{
				slow = slow.next;
				quick = quick.next.next;
			}

			//根据奇偶判断中间节点是哪一个
			quick = quick == null ? slow : slow.next;//quick指向mid
			slow = head;//slow指向头

			//下面是对后半部分反转
			ListNode newHead = ReverseList2(quick);//保留反转后的head，复原链表时需要使用
			quick = newHead;

			//开始一一对比，如果quick能走完就证明是回文
			while (quick != null)
			{
				if (slow.val != quick.val)
				{
					ReverseList2(newHead);//即使不符合也需要恢复
					return false;
				}
				slow = slow.next;
				quick = quick.next;
			}

			//链表复原
			ReverseList2(newHead);
			return true;
		}

		//栈
		public bool IsPalindrome2(ListNode head)
		{
			if (head == null)
				return false;
			if (head.next == null)
				return true;
			Stack<int> stack = new Stack<int>();
			ListNode temp = head;
			while (temp != null)
			{
				stack.Push(temp.val);
				temp = temp.next;
			}
			temp = head;
			while (temp != null)
			{
				if (stack.Pop() != temp.val)
					return false;
				temp = temp.next;
			}
			return true;
		}

		//递归+短路
		ListNode temp;
		public bool IsPalindrome3(ListNode head)
		{
			if (head == null)
				return false;
			if (head.next == null)
				return true;
			temp = head;
			return Check(head);
		}
		private bool Check(ListNode head)
		{
			if (head == null)
				return true;
			bool result = Check(head.next) && (temp.val == head.val);//前面的条件就是遍历到最后一个，然后头尾比较
			temp = temp.next;//头下一个，尾也跟着回一层
			return result;
		}

		/***************************************leetcode***************************************/
		// Title : 2. 两数相加
		// URL :   https://leetcode-cn.com/problems/add-two-numbers/
		// Brief : 使用一个flag，在每一轮计算时考虑是否加上前一轮的进位
		/***************************************leetcode***************************************/
		public ListNode AddTwoNumbers(ListNode l1, ListNode l2)
		{
			ListNode head = new ListNode();//用于返回的虚拟头指针
			ListNode newNode = head;
			int carry = 0;
			int sum;
			while (l1 != null || l2 != null)
			{
				sum = (l1 == null ? 0 : l1.val) + (l2 == null ? 0 : l2.val) + carry;//对应位相加
				if (sum > 9)
				{
					newNode.next = new ListNode(sum % 10);//需要进位，flag标记为1
					carry = 1;
				}
				else
				{
					newNode.next = new ListNode(sum);//不用进位，flag标记为0
					carry = 0;
				}
				newNode = newNode.next;//指针后移
				if (l1 != null)
				{
					l1 = l1.next;//空则不用后移以免报错
				}
				if (l2 != null)
				{
					l2 = l2.next;
				}
			}
			if (carry == 1)
			{
				newNode.next = new ListNode(1);//完成后flag依然为1说明还需要再进位
			}
			return head.next;
		}

		/***************************************leetcode***************************************/
		// Title : 从尾到头打印链表（返回数组）
		// URL :   https://leetcode-cn.com/leetbook/read/illustration-of-algorithm/5dt66m/
		// Brief : 
		/***************************************leetcode***************************************/
		List<int> list = new List<int>();
		public int[] ReversePrint(ListNode head)
		{
			Rev(head);
			return list.ToArray();
		}
		//利用递归
		public void Rev(ListNode head)
		{
			if (head == null)
				return;
			Rev(head.next);
			list.Add(head.val);
		}
	}
}
