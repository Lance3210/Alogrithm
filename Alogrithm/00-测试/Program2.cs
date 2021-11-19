using DataStructure.树.二叉搜索树;
using DataStructure.树.二叉搜索树.平衡二叉搜索树.AVL树;
using DataStructure.树.二叉搜索树.平衡二叉搜索树.红黑树;
using DataStructure.树.二叉树;
using System;

namespace DataStructure.测试 {
	class Program2 {
		static int removeElement = 4;
		static void Main2(string[] args) {
			int[] nums = new int[] { 1, 2, 3 };
			RBTree<int> rbt = new();
			BuildTree(nums, rbt);

			RETreeTest(nums, rbt);
		}

		//生成二叉搜索树
		private static void BuildTree<T>(T[] nums, BinarySearchTree<T> bst) {
			//Random random = new Random();
			for (int i = 0; i < nums.Length; i++) {
				//nums[i] = random.Next(1, count);
				Console.Write(nums[i] + " ");
			}
			for (int i = 0; i < nums.Length; i++) {
				bst.Add(nums[i]);
			}
			Console.WriteLine();
		}

		//二叉树遍历测试
		private static void TraversalTest(BinarySearchTree<int> bst) {
			//前序遍历
			Console.Write("前序遍历：");
			bst.PreorderTraversal((n) => {
				Console.Write($"{n.element}({bst.Height(n)}");
				return n.element == 3;//表示遍历到3就停止
			});
			Console.WriteLine();
			//中序遍历
			Console.Write("中序遍历：");
			bst.InorderTraversal((n) => {
				Console.Write($"{n.element}({bst.Height(n)}");
				return n.element == 3;//表示遍历到3就停止
			});
			Console.WriteLine();
			//后序遍历
			Console.Write("后序遍历：");
			bst.PostorderTraversal((n) => {
				Console.Write($"{n.element}({bst.Height(n)}");
				return n.element == 3;//表示遍历到3就停止
			});
			Console.WriteLine();
			//层序遍历
			Console.Write("层序遍历：");
			bst.LevelOrderTraversal((n) => {
				Console.Write($"{n.element}({bst.Height(n)}");
				//	return n.element == 3;//表示遍历到3就停止
				return false;
			});
			Console.WriteLine();
		}

		//红黑树遍历测试
		private static void RBTreeTraversalTest(BinarySearchTree<int> bst) {
			//前序遍历
			Console.Write("前序遍历：");
			bst.PreorderTraversal((n) => {
				Console.Write($"{n.element}({bst.Height(n)} {((RBNode<int>)n).color})  ");
				return n.element == 3;//表示遍历到3就停止
			});
			Console.WriteLine();
			//中序遍历
			Console.Write("中序遍历：");
			bst.InorderTraversal((n) => {
				Console.Write($"{n.element}({bst.Height(n)} {((RBNode<int>)n).color})  ");
				return n.element == 3;//表示遍历到3就停止
			});
			Console.WriteLine();
			//后序遍历
			Console.Write("后序遍历：");
			bst.PostorderTraversal((n) => {
				Console.Write($"{n.element}({bst.Height(n)} {((RBNode<int>)n).color})  ");
				return n.element == 3;//表示遍历到3就停止
			});
			Console.WriteLine();
			//层序遍历
			Console.Write("层序遍历：");
			bst.LevelOrderTraversal((n) => {
				Console.Write($"{n.element}({bst.Height(n)} {((RBNode<int>)n).color})  ");
				return n.element == 3;//表示遍历到3就停止
			});
			Console.WriteLine();
		}

		#region RBTree
		private static void RETreeTest(int[] nums, RBTree<int> rbt) {
			RBTreeTraversalTest(rbt);
			Console.WriteLine("-----------------------------------------------------------");
			Console.WriteLine($"Remove {removeElement}");
			rbt.Remove(removeElement);
			RBTreeTraversalTest(rbt);
		}
		#endregion

		#region AVLTree
		private static void AVLTreeTest(int[] nums, AVLTree<int> avl) {
			Console.WriteLine("树的高度：" + avl.TreeHeight);
			Console.WriteLine("根节点高度：" + avl.Height(avl.Root));
			Console.WriteLine("根节点右节点高度：" + (avl.Root.right as AVLNode<int>).Height);
			Console.WriteLine("根节点右右节点高度：" + (avl.Root.right.right as AVLNode<int>).Height);
			avl.Remove(1);
			avl.Remove(2);
			avl.Remove(3);
			TraversalTest(avl);
		}
		#endregion

		#region BSTree
		private static void BSTTest() {
			int[] nums = new int[] { 4, 2, 3, 1, 5, 6, 7, 8, 9, 10 };
			BinarySearchTree<int> bst = new BinarySearchTree<int>();
			BuildTree(nums, bst);
			Console.WriteLine();

			TraversalTest(bst);
			Console.WriteLine();

			Console.WriteLine("是否是完全二叉树：" + bst.IsComplete());
			Console.WriteLine();

			Console.WriteLine("数的高度：" + bst.TreeHeight);
			Console.WriteLine();

			PredecessorTest(bst);
			SuccessorTest(bst);
			Console.WriteLine();

			RemoveNodeTest(nums, bst);
			Console.WriteLine();
		}

		//移除节点测试
		private static void RemoveNodeTest(int[] nums, BinarySearchTree<int> bst) {
			Console.WriteLine($"是否包含：{removeElement} " + bst.Contains(removeElement));

			Console.WriteLine("移除了：" + bst.Remove(removeElement).element);
			Console.WriteLine();

			TraversalTest(bst);
			Console.WriteLine();

			Console.WriteLine($"是否包含：{removeElement} " + bst.Contains(removeElement));
		}

		//前驱结点测试
		private static void PredecessorTest(BinarySearchTree<int> bst) {
			Node<int> root = bst.Root;
			Node<int> node0 = root.right ?? null;
			Node<int> node1 = node0?.right;

			Node<int> node2 = node1 == null ? null : BinaryTree<int>.Predecessor(node1);
			Node<int> node3 = node2 == null ? null : BinaryTree<int>.Predecessor(node2);
			Node<int> node4 = node3 == null ? null : BinaryTree<int>.Predecessor(node3);

			Console.WriteLine("当前节点：" + (node1 == null ? "空" : node1.element));
			Console.WriteLine("当前节点的前驱结点：" + (node2 == null ? "空" : node2.element));
			Console.WriteLine("当前节点的前驱结点：" + (node3 == null ? "空" : node3.element));
			Console.WriteLine("当前节点的前驱结点：" + (node4 == null ? "空" : node4.element));
		}
		//后驱节点测试
		private static void SuccessorTest(BinarySearchTree<int> bst) {
			Node<int> root = bst.Root;
			Node<int> node0 = root.left ?? null;
			Node<int> node1 = node0?.left;

			Node<int> node2 = node1 == null ? null : BinaryTree<int>.Successor(node1);
			Node<int> node3 = node2 == null ? null : BinaryTree<int>.Successor(node2);
			Node<int> node4 = node3 == null ? null : BinaryTree<int>.Successor(node3);

			Console.WriteLine("当前节点：" + (node1 == null ? "空" : node1.element));
			Console.WriteLine("当前节点的后驱结点：" + (node2 == null ? "空" : node2.element));
			Console.WriteLine("当前节点的后驱结点：" + (node3 == null ? "空" : node3.element));
			Console.WriteLine("当前节点的后驱结点：" + (node4 == null ? "空" : node4.element));
		}
		#endregion
	}
}
