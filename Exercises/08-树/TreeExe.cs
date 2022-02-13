using Exercises.Util.LeetCode;
using System.Collections.Generic;

namespace Exercises.树
{
	class TreeExe
	{
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
		// Title : 1609. 奇偶树（中等）
		// URL :   https://leetcode-cn.com/problems/even-odd-tree/
		// Brief : 层序遍历，添加一层内循环即可实现按层处理
		/***************************************leetcode***************************************/
		public bool IsEvenOddTree(TreeNode root)
		{
			Queue<TreeNode> queue = new Queue<TreeNode>();
			queue.Enqueue(root);
			TreeNode node = new TreeNode();
			int lastValue;
			int OddOrEven;
			int level = 0;
			int size;
			while (queue.Count > 0)
			{
				size = queue.Count; // 注意需要记录本层的Count
				lastValue = (level & 1) == 0 ? int.MinValue : int.MaxValue;
				for (int i = 0; i < size; i++)
				{
					node = queue.Dequeue();
					OddOrEven = (level & 1);
					// 奇层偶数，偶层奇数，固两者不可以同为奇偶
					if (OddOrEven == (node.val & 1))
					{
						return false;
					}
					if ((OddOrEven == 0 && lastValue >= node.val) || (OddOrEven == 1 && lastValue <= node.val))
					{
						return false;
					}
					lastValue = node.val;
					if (node.left != null)
					{
						queue.Enqueue(node.left);
					}
					if (node.right != null)
					{
						queue.Enqueue(node.right);
					}
				}
				++level;
			}
			return true;
		}

		/***************************************leetcode***************************************/
		// Title : 226. 翻转二叉树
		// URL :   https://leetcode-cn.com/problems/invert-binary-tree/
		// Brief : 因为必须遍历到所有节点，故每一种遍历方法都可以
		/***************************************leetcode***************************************/
		//递归
		//前序
		//后序也差不多
		public TreeNode InvertTree(TreeNode root)
		{
			if (root == null) return root;

			TreeNode temp = root.left;
			root.left = root.right;
			root.right = temp;

			InvertTree(root.left);
			InvertTree(root.right);
			return root;
		}
		//中序就不同了
		public TreeNode InvertTree2(TreeNode root)
		{
			if (root == null) return root;
			InvertTree2(root.left);

			TreeNode temp = root.left;
			root.left = root.right;
			root.right = temp;

			InvertTree2(root.left);//注意：因为上面以及将左右调换，故应遍历原本的左子树
			return root;
		}
		//迭代
		//层序遍历
		public TreeNode InvertTree3(TreeNode root)
		{
			if (root == null) return root;
			Queue<TreeNode> queue = new Queue<TreeNode>();
			queue.Enqueue(root);
			while (queue.Count != 0)
			{
				TreeNode node = queue.Dequeue();
				TreeNode temp = node.left;
				node.left = node.right;
				node.right = temp;

				if (node.left != null)
				{
					queue.Enqueue(node.left);
				}
				if (node.right != null)
				{
					queue.Enqueue(node.right);
				}
			}
			return root;
		}
	}
}
