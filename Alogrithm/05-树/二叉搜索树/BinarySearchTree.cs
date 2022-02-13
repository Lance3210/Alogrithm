using DataStructure.树.二叉树;
using System;

namespace DataStructure.树.二叉搜索树
{
	//Binary Search Tree
	//二叉搜索树 / 二叉排序树 / 二叉查找树
	//任意节点的值大于其左子树的所有值，小于右子树的所有值

	class BinarySearchTree<T> : BinaryTree<T>
	{
		//默认构造
		public BinarySearchTree()
		{

		}

		//需要传入比较方法的构造函数
		public BinarySearchTree(Func<T, T, int> comparer)
		{
			this.comparer = comparer;
		}

		//添加指定类型节点（用于继承的子类型创建）
		protected virtual Node<T> CreatNode(T element, Node<T> parent)
		{
			return new Node<T>(element, parent);//默认类型
		}

		//添加节点
		public Node<T> Add(T element)
		{
			NullCheck(element);
			//根节点
			if (root == null)
			{
				root = CreatNode(element, null);
				++size;
				AfterAdd(root);//添加根节点也要处理（在红黑树中）
				return root;
			}
			//非根节点
			Node<T> node = root;//记录插入位置
			Node<T> parentNode = root;
			int result;
			do
			{
				result = ElementCompare(element, node.element);
				parentNode = node;
				if (result > 0)
				{
					node = node.right;
				}
				else if (result < 0)
				{
					node = node.left;
				}
				else
				{
					node.element = element;//建议覆盖，因为是添加一般都是覆盖，不然添加干什么
					return node;
				}
			} while (node != null);
			//插入
			Node<T> newNode = CreatNode(element, parentNode);
			if (result > 0)
			{
				parentNode.right = newNode;
			}
			else
			{
				parentNode.left = newNode;
			}
			//++
			++size;
			AfterAdd(newNode);//返回新插入节点做后面的操作
			return node;
		}

		//添加后处理（用于AVL树等）
		protected virtual void AfterAdd(Node<T> node)
		{

		}

		//移除节点（封装）
		public Node<T> Remove(T element)
		{
			Node<T> node = Find(element);
			if (node == null)
			{
				return null;
			}
			//对删除节点进行拷贝用于返回，因为度为2的节点的删除是覆盖而不是删掉，element有可能会被更改
			Node<T> deletedNode = node.Clone();
			Remove(node);
			return deletedNode;
		}
		private void Remove(Node<T> node)
		{
			if (node == null)
			{
				return;
			}
			--size;//记得规模减少（这里先减少，是因为已经Find到了）

			if (node.HasTwoChildren)//先删除度为2的节点
			{
				Node<T> pre = Predecessor(node);
				node.element = pre.element;//拿前驱节点的值覆盖删除节点
				node = pre;//将删除的对象转移成前驱节点，交给后面来完成
			}

			//能来到下面就与一定是度为0或1的节点
			//度为1的节点
			if (node.HasOnlyOneChild)
			{
				Node<T> replacement = node.left ?? node.right;//找到作为代替的节点是左还是右
				replacement.parent = node.parent;//连接父节点（这里会影响下面的AfterRemove）

				//要注意这里还要判断这个节点是不是根节点，不然后面点parent会报null
				if (node.IsRoot)
				{
					root = replacement;
				}//与下面叶子的逻辑类似
				else if (node.parent.left == node)
				{
					node.parent.left = replacement;
				}
				else
				{
					node.parent.right = replacement;
				}
				//因为度为2的节点是覆盖且需要等真正删除后这里才好处理（如AVL中的恢复平衡）
				//传入代替节点方便红黑树处理，而传入AVL树中则无影响（AVL树只拿parent处理高度，而node和replacement的parent一致）
				AfterRemove(replacement);
			}//说明删除的是根节点
			else if (node.parent == null)
			{
				root = null;
				AfterRemove(node);
			}//删除的是叶子
			else
			{
				if (node.parent.left == node)
				{
					node.parent.left = null;
				}
				else
				{
					node.parent.right = null;
				}
				AfterRemove(node);
			}
		}

		//删除后调整（用于子类）
		protected virtual void AfterRemove(Node<T> node)
		{

		}

		//重写父类的查找方法
		public override Node<T> Find(T element)
		{
			Node<T> node = root;
			int result;
			while (node != null)
			{
				result = ElementCompare(element, node.element);
				if (result == 0)
				{
					return node;
				}
				else if (result < 0)
				{
					node = node.left;
				}
				else
				{
					node = node.right;
				}
			}
			return null;
		}

		//重写父类判断是否包含方法
		public override bool Contains(T element)
		{
			return Find(element) != null;
		}
	}
}

