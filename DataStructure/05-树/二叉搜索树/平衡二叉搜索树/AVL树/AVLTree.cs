using DataStructure.树.二叉树;
using System;

namespace DataStructure.树.二叉搜索树.平衡二叉搜索树.AVL树
{
	//AVL树
	//关键：保存任意节点左右子树的高度差绝对值在1以内
	//添加：可能会导致所有祖先节点失衡，但只要让高度最低的那个恢复平衡即可，恢复仅需要O(1)调整
	//删除：可能会导致父节点或祖先节点失衡（但只会有一个失衡），该节点恢复平衡后可能会导致更高祖先节点失衡，恢复需要O(logn)调整
	//平均时间复杂度：添加、搜索、删除 O(logn)
	class AVLTree<T> : BalancedBinarySearchTree<T>
	{
		//默认构造
		public AVLTree()
		{

		}

		//需要传入比较方法的构造函数
		public AVLTree(Func<T, T, int> comparetor) : base()
		{

		}

		//重写节点创建
		protected override Node<T> CreatNode(T element, Node<T> parent)
		{
			return new AVLNode<T>(element, parent);
		}

		//判断是否平衡
		private bool IsBalanced(Node<T> node)
		{
			return Math.Abs(((AVLNode<T>)node).BalanceFactor) <= 1;//绝对值
		}

		//在父类添加逻辑后面进行旋转
		//添加会导致失衡的就只有祖父节点以上

		//添加后处理
		protected override void AfterAdd(Node<T> node)
		{
			while ((node = node.parent) != null)//每次都向上走
			{
				if (IsBalanced(node))
				{
					//发现是平衡，则更新每一个父节点的高度
					((AVLNode<T>)node).UpdateHeight();
				}
				else
				{
					//恢复平衡，这里进来的node一定是第一个不平衡的祖父节点
					Rebalance2((AVLNode<T>)node);
					break;//第一个平衡即整棵树平衡
				}
			}
		}

		//旋转后高度修正
		protected sealed override void AfterRotate(Node<T> grand, Node<T> p, Node<T> child)
		{
			base.AfterRotate(grand, p, child);
			//修正高度（先矮后高）			
			((AVLNode<T>)grand).UpdateHeight();
			((AVLNode<T>)p).UpdateHeight();
		}

		//恢复平衡1（分四种情况）
		private void Rebalance1(AVLNode<T> grand)
		{
			AVLNode<T> parentNode = grand.TallestChild();//肯定是最高的那个插入后失衡
			AVLNode<T> node = parentNode.TallestChild();//取出最高的子节点

			//旋转
			//四种情况
			//单旋：LL 右旋 RR 左旋
			//双旋：LR RL 

			if (parentNode.IsLeftChild)//L
			{
				if (node.IsLeftChild)//LL
				{
					RightRotate(grand);
				}
				else//LR
				{
					LeftRotate(node);
					RightRotate(grand);
				}
			}
			else//R
			{
				if (node.IsLeftChild)//RL
				{
					RightRotate(node);
					LeftRotate(grand);
				}
				else//RR
				{
					LeftRotate(grand);
				}
			}
		}

		//恢复平衡（统一旋转）
		private void Rebalance2(AVLNode<T> grand)
		{
			AVLNode<T> parentNode = grand.TallestChild();//肯定是最高的那个插入后失衡
			AVLNode<T> node = parentNode.TallestChild();//取出最高的子节点

			if (parentNode.IsLeftChild)//L
			{
				if (node.IsLeftChild)//LL
				{
					Rotate(grand, node.left, node, node.right, parentNode, parentNode.right, grand, grand.right);
				}
				else//LR
				{
					Rotate(grand, parentNode.left, parentNode, node.left, node, node.right, grand, grand.right);
				}
			}
			else//R
			{
				if (node.IsLeftChild)//RL
				{
					Rotate(grand, grand.left, grand, node.left, node, node.right, parentNode, parentNode.right);
				}
				else//RR
				{
					Rotate(grand, grand.left, grand, parentNode.left, parentNode, node.left, node, node.right);
				}
			}
		}

		//旋转后高度修正
		protected sealed override void Rotate(Node<T> r,
			Node<T> a, Node<T> b, Node<T> c,
			Node<T> d,
			Node<T> e, Node<T> f, Node<T> g)
		{
			base.Rotate(r, a, b, c, d, e, f, g);
			//高度更新
			((AVLNode<T>)b).UpdateHeight();
			((AVLNode<T>)f).UpdateHeight();
			((AVLNode<T>)d).UpdateHeight();
		}

		//删除后处理
		//其实与添加后处理一样，只是需要对所有祖父节点进行高度更新
		protected override void AfterRemove(Node<T> node)//传入的是代替节点，但其父节点也和删除节点一致（BST中已处理）
		{
			while ((node = node.parent) != null)
			{
				if (IsBalanced(node))
				{
					((AVLNode<T>)node).UpdateHeight();
				}
				else
				{
					Rebalance2((AVLNode<T>)node);//不用break，因为不清楚祖父节点是否失衡
				}
			}
		}
	}
}
