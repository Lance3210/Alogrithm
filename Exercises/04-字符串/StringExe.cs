using System.Collections.Generic;
using System.Text;
using Exercises.Util.LeetCode;

namespace Exercises.字符串
{
	class StringExe
	{
		/***************************************leetcode***************************************/
		// Title : 572. 另一棵树的子树（简单）
		// URL :   https://leetcode-cn.com/problems/subtree-of-another-tree/
		// Brief : 前或后序遍历序列化比较，但要注意不同的遍历方式时分隔符的位置
		/***************************************leetcode***************************************/
		public bool IsSubtree(TreeNode root, TreeNode subRoot)
		{
			StringBuilder sb1 = new StringBuilder();
			StringBuilder sb2 = new StringBuilder();
			PostSerialize(root, sb1);
			PostSerialize(subRoot, sb2);
			if (sb1.ToString().Contains(sb2.ToString()))
			{
				return true;
			}
			return false;
		}
		public void PostSerialize(TreeNode root, StringBuilder sb)
		{
			if (root == null)
			{
				sb.Append("#!");
				return;
			}
			PostSerialize(root.left, sb);
			PostSerialize(root.right, sb);
			sb.Append(root.val).Append('!');
		}

		/***************************************leetcode***************************************/
		// Title : 面试题 01.09. 字符串轮转
		// URL :   https://leetcode-cn.com/problems/string-rotation-lcci/
		// Brief : 首位拼接s1，可以发现所有情况都可以找到
		/***************************************leetcode***************************************/
		public bool IsFlipedString(string s1, string s2)
		{
			if (s1.Length != s2.Length)
			{
				return false;
			}
			return (s1 + s1).Contains(s2);
		}

		/***************************************leetcode***************************************/
		// Title : 520. 检测大写字母
		// URL :   https://leetcode-cn.com/problems/detect-capital/
		// Brief : 简单的遍历，关键在利用语言的一些技巧
		/***************************************leetcode***************************************/
		public bool DetectCapitalUse(string word)
		{
			//如果第一个是小写，但第二个是大写必是false
			if (word.Length > 1 && char.IsLower(word[0]) && char.IsUpper(word[1]))
			{
				return false;
			}
			//如果第一个是大写，那么从第二个开始每一个都要与第一个的状态保持一致
			for (int i = 2; i < word.Length; ++i)
			{
				//利用了自带 ^ 的重写，可用于比较bool值
				if (char.IsLower(word[i]) ^ char.IsLower(word[0]))
				{
					return false;
				}
			}
			return true;
		}

		/***************************************leetcode***************************************/
		// Title : Z 字形变换（明明是N型）
		// URL :   https://leetcode-cn.com/problems/zigzag-conversion/
		// Brief : 
		/***************************************leetcode***************************************/
		public string Convert(string s, int numRows)
		{
			if (numRows < 2)
			{
				return s;
			}
			List<StringBuilder> list = new List<StringBuilder>();
			//创建numsRows个行来存储
			for (int i = 0; i < numRows; ++i)
			{
				list.Add(new StringBuilder());
			}

			int flag = -1;//转折标记
			int index = 0;//index的区间为[0, numRows - 1]

			//遍历每一个字符
			for (int i = 0; i < s.Length; ++i)
			{
				list[index].Append(s[i]);
				if (index == 0 || index == numRows - 1)//到达转折点
				{
					flag = -flag;//直接反转
				}
				index += flag;//一行行插入
			}
			StringBuilder sb = new StringBuilder();
			//拼接所有行
			for (int i = 0; i < list.Count; ++i)
			{
				sb.Append(list[i]);
			}
			return sb.ToString();
		}

	}
}
