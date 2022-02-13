namespace Exercises.Util
{
	public class TreeNode<T>
	{
		public T val;
		public TreeNode<T> left;
		public TreeNode<T> right;
		public TreeNode<T> parent;
		public TreeNode(T val)
		{
			this.val = val;
		}
		public TreeNode(T val, TreeNode<T> parent)
		{
			this.val = val;
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
		public TreeNode<T> Sibling {
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
		public TreeNode<T> Uncle => parent.Sibling;

		//获取祖父节点
		public TreeNode<T> Grand => parent.parent;

		//浅拷贝
		public TreeNode<T> Clone()
		{
			return MemberwiseClone() as TreeNode<T>;
		}
	}

	/// <summary>
	/// int类型的TreeNode
	/// </summary>
	public class TreeNode
	{
		public int val;
		public TreeNode left;
		public TreeNode right;
		public TreeNode parent;
		public TreeNode(int val)
		{
			this.val = val;
		}
		public TreeNode(int val, TreeNode parent)
		{
			this.val = val;
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
		public TreeNode Sibling {
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
		public TreeNode Uncle => parent.Sibling;

		//获取祖父节点
		public TreeNode Grand => parent.parent;

		//浅拷贝
		public TreeNode Clone()
		{
			return MemberwiseClone() as TreeNode;
		}
	}

}
