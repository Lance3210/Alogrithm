using DataStructure.树.二叉树;
using System;

namespace DataStructure.树.二叉搜索树.平衡二叉搜索树.AVL树 {
	//创建一个AVL专有的节点类型
	class AVLNode<T> : Node<T> {
		//AVL特有的节点高度记录
		public int Height = 1;//我们规定叶子高度默认1

		//获得该节点的平衡因子
		public int BalanceFactor {
			get {
				int leftHeight = left == null ? 0 : ((AVLNode<T>)left).Height;
				int rightHeight = right == null ? 0 : ((AVLNode<T>)right).Height;
				return leftHeight - rightHeight;
			}
		}

		public AVLNode(T element, Node<T> parent) : base(element, parent) {

		}

		//节点高度更新
		public void UpdateHeight() {
			int leftHeight = left == null ? 0 : ((AVLNode<T>)left).Height;
			int rightHeight = right == null ? 0 : ((AVLNode<T>)right).Height;
			Height = 1 + Math.Max(leftHeight, rightHeight);//就是在左右节点高度最大的值上加1
		}

		//取出高度最高的子节点
		public AVLNode<T> TallestChild() {
			int leftHeight = left == null ? 0 : ((AVLNode<T>)left).Height;
			int rightHeight = right == null ? 0 : ((AVLNode<T>)right).Height;
			if (leftHeight > rightHeight) return (AVLNode<T>)left;
			if (leftHeight < rightHeight) return (AVLNode<T>)right;
			return IsLeftChild ? (AVLNode<T>)left : (AVLNode<T>)right;//相等时的处理，返回同方向的子节点
		}
	}
}
