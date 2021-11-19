using DataStructure.树.二叉树;

namespace DataStructure.树.二叉搜索树.平衡二叉搜索树 {
	//抽象出统一旋转行为的抽象类
	abstract class BalancedBinarySearchTree<T> : BinarySearchTree<T> {
		//左旋转
		protected void LeftRotate(Node<T> grand) {
			Node<T> p = grand.right;
			Node<T> pLeft = p.left;
			grand.right = pLeft;
			p.left = grand;

			AfterRotate(grand, p, pLeft);
		}

		//右旋转
		protected void RightRotate(Node<T> grand) {
			Node<T> p = grand.left;
			Node<T> pRight = p.right;
			grand.left = pRight;
			p.right = grand;

			AfterRotate(grand, p, pRight);
		}

		//旋转后处理（父子关系调整；若是AVL树，则还需要处理高度）
		protected virtual void AfterRotate(Node<T> grand, Node<T> p, Node<T> child) {
			//修正p,grand,pLeft的父节点
			//更新p
			p.parent = grand.parent;
			if (grand.IsLeftChild)//判断grand在其父节点的位置
			{
				grand.parent.left = p;
			}
			else if (grand.IsRightChild) {
				grand.parent.right = p;
			}
			else//grand是根节点
			{
				root = p;
			}
			//更新child
			if (child != null) {
				child.parent = grand;
			}
			//更新grand
			grand.parent = p;
		}

		//统一形旋转
		//因为旋转完成后的所有子树模式依然遵从从小到大，可以利用这个规律统一旋转
		//将旋转后的树分为根节点和左右子树，从左到右a到g
		//r该树原根节点，d该树的目标根，abc为左子树的左根右，efg为右子树的左根右
		protected virtual void Rotate(Node<T> r,
			Node<T> a, Node<T> b, Node<T> c,
			Node<T> d,
			Node<T> e, Node<T> f, Node<T> g)//实际上在AVL树中，a和g不需要处理
		{
			//让d成为该树根节点
			d.parent = r.parent;
			if (r.IsLeftChild) {
				r.parent.left = d;
			}
			else if (r.IsRightChild) {
				r.parent.right = d;
			}
			else {
				root = d;
			}

			//处理abc
			b.left = a;
			if (a != null) {
				a.parent = b;
			}
			b.right = c;
			if (c != null) {
				c.parent = b;
			}

			//处理efg
			f.left = e;
			if (e != null) {
				e.parent = f;
			}
			b.right = c;
			if (g != null) {
				g.parent = f;
			}

			//处理bdf
			d.left = b;
			b.parent = d;
			d.right = f;
			f.parent = d;
		}
	}
}
