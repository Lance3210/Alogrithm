namespace DataStructure.树.二叉树
{
	//节点类
	public class Node<T>
	{
		public T element;
		public Node<T> left;
		public Node<T> right;
		public Node<T> parent;
		public Node(T element, Node<T> parent)
		{
			this.element = element;
			this.parent = parent;
		}
		//判断该节点是否为根节点
		public bool IsRoot => (left != null || right != null) && (parent == null);
		//判断该节点是否为叶子节点
		public bool IsLeaf => (left == null) && (right == null);
		//判断该节点的度是否为1
		public bool HasOnlyOneChild => ((left != null) && (right == null)) || ((left == null) && (right != null));
		//判断该节点的度是否为2
		public bool HasTwoChildren => (left != null) && (right != null);
		//判断该节点是否是父节点的左节点
		public bool IsLeftChild => (parent != null) && (this == parent.left);
		//判断该节点是否是父节点的右节点
		public bool IsRightChild => (parent != null) && (this == parent.right);

		//获取兄弟节点
		public Node<T> Sibling {
			get {
				if (IsLeftChild)
				{
					return parent.right;
				}
				if (IsRightChild)
				{
					return parent.left;
				}
				return null;
			}
		}

		//获取叔父节点
		public Node<T> Uncle => parent.Sibling;

		//获取祖父节点
		public Node<T> Grand => parent.parent;

		//浅拷贝
		public Node<T> Clone()
		{
			return MemberwiseClone() as Node<T>;
		}
	}
}
