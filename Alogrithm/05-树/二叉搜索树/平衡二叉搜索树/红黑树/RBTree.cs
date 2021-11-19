using DataStructure.Util;
using DataStructure.树.二叉树;
using System;

namespace DataStructure.树.二叉搜索树.平衡二叉搜索树.红黑树 {
	//红黑树
	class RBTree<T> : BalancedBinarySearchTree<T> {
		//默认构造
		public RBTree() {

		}

		//需要传入比较方法的构造函数
		public RBTree(Func<T, T, int> comparer) : base() {

		}

		#region 辅助函数
		//节点染色
		private Node<T> Dye(Node<T> node, RBNodeColor color) {
			if (node == null) {
				return node;
			}
			((RBNode<T>)node).color = color;
			return node;
		}
		//染红
		private Node<T> DyeRed(Node<T> node) {
			return Dye(node, RBNodeColor.RED);
		}
		//染黑
		private Node<T> DyeBlack(Node<T> node) {
			return Dye(node, RBNodeColor.BLACK);
		}
		//颜色判断
		private RBNodeColor GetColor(Node<T> node) {
			return node == null ? RBNodeColor.BLACK : ((RBNode<T>)node).color;
		}
		//是否为黑色
		private bool IsBlack(Node<T> node) {
			return GetColor(node) == RBNodeColor.BLACK;
		}
		//是否为红色
		private bool IsRed(Node<T> node) {
			return GetColor(node) == RBNodeColor.RED;
		}
		#endregion

		//重写节点创建
		protected override Node<T> CreatNode(T element, Node<T> parent) {
			return new RBNode<T>(element, parent);
		}

		//添加
		//红黑树可以合并整理成一颗4阶B树（黑节点与其红子节点组合），因此可以用4阶B树的性质去理解它

		//在B树中，真正添加的节点都在叶子节点
		//故若添加节点必然在子节点位置，且可以分为12种不同情况
		//但可归类为：添加节点的parent是黑色和非黑色两种

		//添加节点为root则染黑；
		//如果添加节点parent为黑色，则无须任何操作（有4种情况，直接添加符合4阶B树）
		//若添加节点parent为红色，则有以下情况（LL指的是添加节点在grand的位置）
		//一、uncle为红色
		//添加节点的parent和uncle染黑，取出grand染红后向上合并，grand可以看做添加到上一层的新节点（递归），直到上溢到root（染黑）
		//二、uncle为黑色（或null）
		//LL/RR，grand为黑色，则对grand右旋/左旋，parent染黑，grand染红。parent为本层根节点
		//LR/RL，grand为黑色，则对parent左旋/右旋，grand右旋/左旋，node染黑，grand染红。添加节点为本层根节点
		protected override void AfterAdd(Node<T> node) {
			//parent = null，root染黑
			if (node.parent == null) {
				DyeBlack(node);
				return;
			}
			//parent黑色，无须处理
			if (IsBlack(node.parent)) {
				return;
			}
			//uncle为红色
			if (IsRed(node.Uncle)) {
				//parent uncle 染黑
				DyeBlack(node.parent);
				DyeBlack(node.Uncle);
				//grand染红向上合并当做新添加节点
				AfterAdd(DyeRed(node.Grand));
				return;
			}
			//uncle为黑色
			if (IsBlack(node.Uncle)) {
				Node<T> grand = node.Grand;
				if (node.parent.IsLeftChild) {
					DyeRed(node.Grand);
					if (node.IsLeftChild)//LL
					{
						DyeBlack(node.parent);
					}
					else//LR
					{
						DyeBlack(node);
						LeftRotate(node.parent);
					}
					RightRotate(grand);
				}
				else {
					DyeRed(node.Grand);
					if (node.IsLeftChild)//RL
					{
						DyeBlack(node);
						RightRotate(node.parent);
					}
					else//RR
					{
						DyeBlack(node.parent);
					}
					LeftRotate(grand);
				}
			}
		}

		//删除
		//在B树中，真正删除的节点都在叶子节点

		//若删除的是RED节点；无须任何操作（无论度是0，1还是2，因为最终都会删除叶子节点，BST会找到前驱）
		//若删除的是拥有一个RED子节点的BLACK节点；将用于替代的子节点（BST传入的replacement）染黑即可

		//若删除的是拥有两个RED子节点的BLACK节点；无须任何操作（本质就是处理BST传入的replacement）
		//若删除的是BLACK叶子节点，会产生下溢（或者可以看成红黑树中的一条性质“任何到null节点的所有路径上BLACK数量一致”会被破坏）
		//一、如果sibling为BLACK且至少有一个RED节点，这就需要向sibling“借一个”
		//即对sibling的parent进行旋转（到删除位置），sibling变成其parent原本的颜色，sibling将来的两个子节点染黑
		//二、如果sibling为BLACK但无子节点，则其parent向下与sibling合并
		//对sibling染红，parent染黑；但一发现parent为BLACK时，parent向下合并必下溢，则可把parent当做新删除节点进行递归
		//三、如果sibling为RED，则sibling染黑，parent染红进行旋转
		//原sibling的原子节点就会变成删除节点的新sibling，其必然为黑，则可套用上面操作

		protected override void AfterRemove(Node<T> node)//传入的node有两种意思，一种就是叶子节点，一种是用于替代的叶子节点
		{
			//删除节点为红色或用于取代删除节点的节点（replacement）为红色（即合并了最终删除的是RED的情况）
			if (IsRed(node)) {
				DyeBlack(node);//虽然其中删除RED叶子节点的情况不需要染黑，但也无妨，可以省下传入replacement参数（与AVL树统一函数格式）
				return;
			}
			//删除的是根节点
			if (node.parent == null) {
				return;
			}

			//删除节点为黑色叶子节点，产生下溢
			//注意：如果是删除叶子节点，那AfterRemove的node已经是删除的node，但node的parent的left或right已经不指向node了
			//故可以对sibling的位置进行反推判断，node的parent的left为null即删除节点的sibling为右，另一边亦然
			//但是，如果是向下合并产生的递归，传入的node的parent的左右子树不一定为null，故要进行多一步判断
			bool flag = node.parent.left == null || node.IsLeftChild;
			Node<T> sibling = flag ? node.parent.right : node.parent.left;

			//被删除节点在左边，sibling在右边	
			if (flag) {
				//删除节点的sibling为Red
				if (IsRed(sibling)) {
					DyeBlack(sibling);
					DyeRed(node.parent);
					LeftRotate(node.parent);
					sibling = node.parent.right;//更新兄弟
				}
				//删除节点的sibling为Black
				if (IsBlack(sibling.left) && IsBlack(sibling.right))//sibling无Red子节点，产生下溢
				{
					//需要提前判断parent的颜色
					bool parentIsBlack = IsBlack(node.parent);
					//parent向下合并
					DyeBlack(node.parent);
					DyeRed(sibling);
					if (parentIsBlack)//parent为黑合并必下溢，则递归
					{
						AfterRemove(node.parent);
					}
				}
				else//sibling至少有一个Red子节点，向sibling“借一个”
				{
					if (IsBlack(sibling.right))//sibling需要进行右旋先的情况
					{
						RightRotate(sibling);
						sibling = node.parent.right;//更新兄弟
					}
					//将sibling染成parent原来的颜色,sibling将来的两个子节点染黑
					Dye(sibling, GetColor(node.parent));
					DyeBlack(sibling.right);
					DyeBlack(node.parent);
					//然后parent再左旋转
					LeftRotate(node.parent);
				}
			}
			else//被删除节点在右边，sibling在左边，与上面对称
			{
				//删除节点的sibling为Red
				if (IsRed(sibling)) {
					DyeBlack(sibling);
					DyeRed(node.parent);
					RightRotate(node.parent);
					sibling = node.parent.left;//更新兄弟
				}
				//删除节点的sibling为Black
				if (IsBlack(sibling.left) && IsBlack(sibling.right))//sibling无Red子节点，
				{
					//需要提前判断parent的颜色
					bool parentIsBlack = IsBlack(node.parent);
					//parent向下合并
					DyeBlack(node.parent);
					DyeRed(sibling);
					if (parentIsBlack)//parent为黑合并必下溢，则递归
					{
						AfterRemove(node.parent);
					}
				}
				else//sibling至少有一个Red子节点，向sibling“借一个”
				{
					if (IsBlack(sibling.left))//sibling需要进行左旋先的情况
					{
						LeftRotate(sibling);
						sibling = node.parent.left;//更新兄弟
					}
					//将sibling染成parent原来的颜色,sibling将来的两个子节点染黑
					Dye(sibling, GetColor(node.parent));
					DyeBlack(sibling.left);
					DyeBlack(node.parent);
					//然后parent再右旋转
					RightRotate(node.parent);
				}
			}
		}
	}
}
