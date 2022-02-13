using DataStructure.Util;
using System;
using System.Collections.Generic;

namespace DataStructure.树.二叉树
{
	class BinaryTree<T>
	{
		//二叉树大小
		protected int size;
		public int Size => size;
		//二叉树的根
		protected Node<T> root;
		public Node<T> Root => root;
		//二叉树元素的比较方法
		protected Func<T, T, int> comparer;
		//空检测
		public bool IsEmpty => size == 0;
		//清空二叉树
		public void Clear() => size = 0;
		//计算树的高度
		public int TreeHeight => RootHeight();
		//用递归获取某个节点高度
		public int Height(Node<T> node)
		{
			if (node == null) return 0;//注意我们默认规定叶子节点高度为1
			return 1 + (int)MathF.Max(Height(node.left), Height(node.right));//
		}
		//用层序遍历获取根高度
		private int RootHeight()
		{
			if (root == null) return 0;
			Queue<Node<T>> queue = new Queue<Node<T>>();
			queue.Enqueue(root);//先进入一个根
			int height = 0;
			int levelSize = 1;  //记录每一层数量，默认root开始是1
			Node<T> node;
			while (queue.Count != 0)
			{
				node = queue.Dequeue(); //每次出队一个
				--levelSize;//每出队一个表示遍历完该层一个

				if (node.left != null)
				{
					queue.Enqueue(node.left);
				}
				if (node.right != null)
				{
					queue.Enqueue(node.right);
				}

				if (levelSize == 0)//表示该层遍历完
				{
					levelSize = queue.Count;//下一层数量就是队列中待出队数量
					++height;//遍历完一层就++
				}
			}
			return height;
		}

		//获得当前节点的前驱结点
		public static Node<T> Predecessor(Node<T> node)
		{
			if (node == null)
			{
				return null;
			}
			//左子树不为空，则前驱节点一定在左子树
			if (node.left != null)
			{
				Node<T> p = node.left;
				while (p.right != null)
				{
					p = p.right;
				}
				return p;
			}
			//左子树为空，则从父节点向上开始找
			while (node.parent.left == node && node.parent != null)
			{
				node = node.parent;
			}
			return node.parent;//表示该祖辈节点就是你的前驱
		}

		//获得当前节点的后驱节点 
		public static Node<T> Successor(Node<T> node)
		{
			if (node == null)
			{
				return null;
			}
			//右子树不为空，则后驱节点一定在右子树
			if (node.right != null)
			{
				Node<T> p = node.right;
				while (p.left != null)
				{
					p = p.left;
				}
				return p;
			}
			//右子树为空，则从父节点向上开始找
			while (node.parent.right == node && node.parent != null)
			{
				node = node.parent;
			}
			return node.parent;//表示该祖辈节点就是你的后驱
		}

		//判断是否是完全二叉树
		//1. 左右不为空，按层序遍历入队
		//2. 左空，右不空，false
		//3. 左不空，右空，表示从此处后面待入队节点全是叶子
		//4. 左右皆空，表示从此处后面待入队节点全是叶子
		public bool IsComplete()
		{
			if (root == null)
			{
				return false;
			}
			Queue<Node<T>> queue = new Queue<Node<T>>();
			queue.Enqueue(root);

			bool leaf = false;//用于标记该节点是否为叶子
			Node<T> node;
			while (queue.Count != 0)
			{
				node = queue.Dequeue();
				//下一轮开始检测叶子，如果发现被标明为叶子的节点事实上并非节点则不是完全二叉树
				if (leaf && !node.IsLeaf)//node.leaf == null && node.right == null
				{
					return false;
				}

				//层序遍历左节点
				if (node.left != null)
				{
					queue.Enqueue(node.left);
				}
				else if (node.right != null)//左边为空右边不为空的节点必不是完全二叉树
				{
					return false;
				}

				//层序遍历右节点
				if (node.right != null)
				{
					queue.Enqueue(node.right);
				}
				else//左边不为空右边为空或者左右都为空，则表示从这个节点开始，后面所有节点为叶子节点
				{
					leaf = true;
				}
			}
			return true;//队列为空了都没false就是完全二叉树
		}

		//参数检测
		protected void NullCheck(T element)
		{
			if (element == null)
			{
				throw new ArgumentException("非法参数");
			}
		}

		//节点比较
		protected int ElementCompare(T element1, T element2)
		{
			if (comparer != null)
			{
				return comparer(element1, element2);//让接口实现交给传入类型，不限制死必须传比较方法
			}
			return ((IComparable<T>)element1).CompareTo(element2);//强制要求该元素实现自身比较器，但不使用泛型约束
		}

		//是否包含元素
		public virtual bool Contains(T element)
		{
			int flag = -1;
			LevelOrderTraversal(root, (e) => {
				flag = ElementCompare(e.element, element);
				return flag == 0;
			});
			return flag == 0;
		}

		//寻找某个元素
		public virtual Node<T> Find(T element)
		{
			Node<T> node = null;
			LevelOrderTraversal(root, (e) => {
				node = e;
				return ElementCompare(e.element, element) == 0;
			});
			return node;
		}

		#region 遍历
		//遍历
		//看的是根节点遍历的位置

		//层序遍历
		//从上到下，从左到右
		//迭代，利用队列，一个节点出队，然后将左右节点入队
		public void LevelOrderTraversal(Func<Node<T>, bool> fun)//限定了传入的是处理节点的逻辑
		{
			if (fun == null) return;//没必要在下面判断
			LevelOrderTraversal(root, fun);
		}
		private void LevelOrderTraversal(Node<T> root, Func<Node<T>, bool> fun)
		{
			if (root == null) return;
			Queue<Node<T>> queue = new Queue<Node<T>>();
			queue.Enqueue(root);//先进入一个根
			Node<T> node;
			while (queue.Count != 0)
			{
				node = queue.Dequeue(); //每次出队一个
				if (fun(node)) return; //可以获得element并在外部自定义逻辑，这里传入true就表示终止
				if (node.left != null)  //让其左右子树进入
				{
					queue.Enqueue(node.left);
				}
				if (node.right != null)
				{
					queue.Enqueue(node.right);
				}
			}
		}

		//前序遍历
		//（中 左 右）或（中 右 左）
		//遍历加强：可以传入一个类型，这个类型里面实现对遍历到的节点内容做逻辑处理，同时可以返回true表示终止遍历
		//递归
		public void PreorderTraversal(Func<Node<T>, bool> func)
		{
			Visitor_BinaryTree<T> visitor = new Visitor_BinaryTree<T>() { treeNodeLogic = func };
			if (visitor == null) return;
			PreorderTraversal(visitor);
		}
		public void PreorderTraversal(Func<T, bool> func)
		{
			Visitor_BinaryTree<T> visitor = new Visitor_BinaryTree<T>() { logic = func };
			if (visitor == null) return;
			PreorderTraversal(visitor);
		}
		private void PreorderTraversal(Node<T> root, Visitor_BinaryTree<T> visitor)
		{
			if (root == null || visitor.stop) return;//这个stop同时解决了递归和逻辑的终止

			//看你传入的是那种逻辑，分为处理元素或是处理节点
			if (visitor.logic != null)
			{
				visitor.stop = visitor.logic(root.element);
			}
			else if (visitor.treeNodeLogic != null)
			{
				visitor.stop = visitor.treeNodeLogic(root);
			}

			PreorderTraversal(root.left, visitor);
			PreorderTraversal(root.right, visitor);
		}
		//迭代
		public void PreorderTraversal2(Visitor_BinaryTree<T> visitor)//重载
		{
			if (visitor == null || root == null)
			{
				return;
			}
			Stack<Node<T>> stack = new();//用栈来回访上一层节点并以此来找该节点的右节点
			Node<T> node = root;
			while (node != null || stack.Count != 0)//遍历完所有节点的条件
			{
				if (node != null)
				{
					if (VisitorChoose(node, visitor))
					{
						return;
					}
					stack.Push(node);//每遍历一次就入栈
					node = node.left;//（中 左 右）故然后左边进入
				}
				else
				{
					node = stack.Pop();
					node = node.right;//返回上一层节点，取其右节点
				}
			}
		}
		//迭代2 类似层序遍历
		public void PreorderTraversal(Visitor_BinaryTree<T> visitor)
		{
			if (visitor == null || root == null)
			{
				return;
			}
			Stack<Node<T>> stack = new Stack<Node<T>>();
			stack.Push(root);
			Node<T> node;
			while (stack.Count != 0)
			{
				node = stack.Pop();
				if (VisitorChoose(node, visitor))
				{
					return;
				}
				if (node.right != null)//必须先进右再左，顺序不能错
				{
					stack.Push(node.right);
				}
				if (node.left != null)
				{
					stack.Push(node.left);
				}
			}
		}

		//中序遍历
		//（左 中 右）或（右 中 左）
		//中序遍历（二叉搜索树）得到的是升序，或者是降序
		//递归
		public void InorderTraversal(Func<Node<T>, bool> func)
		{
			Visitor_BinaryTree<T> visitor = new Visitor_BinaryTree<T>() { treeNodeLogic = func };
			if (visitor == null) return;
			InorderTraversal(visitor);
		}
		public void InorderTraversal(Func<T, bool> func)
		{
			Visitor_BinaryTree<T> visitor = new Visitor_BinaryTree<T>() { logic = func };
			if (visitor == null) return;
			InorderTraversal(visitor);
		}
		private void InorderTraversal(Node<T> root, Visitor_BinaryTree<T> visitor)
		{
			if (root == null || visitor.stop) return;//这个stop是用于解决递归终止，没有stop判断则仅逻辑终止而已，递归还是会继续
			InorderTraversal(root.left, visitor);
			if (visitor.stop) return;//这个stop是终止我们传入的逻辑

			//必须用一个外部变量记录是否stop并在执行前判断，后面是执行我们的逻辑
			if (visitor.logic != null)
			{
				visitor.stop = visitor.logic(root.element);
			}
			else if (visitor.treeNodeLogic != null)
			{
				visitor.stop = visitor.treeNodeLogic(root);
			}

			InorderTraversal(root.right, visitor);
		}
		//迭代
		//中序遍历在决定是否可以输出当前节点的值的时候，需要考虑其左子树是否都已经遍历完成
		public void InorderTraversal(Visitor_BinaryTree<T> visitor)
		{
			if (visitor == null || root == null)
			{
				return;
			}
			Stack<Node<T>> stack = new Stack<Node<T>>();//用栈来回访上一层节点
			Node<T> node = root;
			while (node != null || stack.Count != 0)//遍历完所有节点的条件
			{
				if (node != null)
				{
					stack.Push(node);//每遍历一次就入栈
					node = node.left;//（左 中 右）故先左边进入
				}
				else
				{
					node = stack.Pop();
					//中序的逻辑处理与要等左边全部遍历完时
					if (VisitorChoose(node, visitor))
					{
						return;
					}
					node = node.right;//返回上一层节点，取其右节点
				}
			}
		}

		//后序遍历
		//（左 右 中）或（右 左 中）
		//递归
		public void PostorderTraversal(Func<Node<T>, bool> func)
		{
			Visitor_BinaryTree<T> visitor = new Visitor_BinaryTree<T>() { treeNodeLogic = func };
			if (visitor == null) return;
			PostorderTraversal(visitor);
		}
		public void PostorderTraversal(Func<T, bool> func)
		{
			Visitor_BinaryTree<T> visitor = new Visitor_BinaryTree<T>() { logic = func };
			if (visitor == null) return;
			PostorderTraversal(visitor);
		}
		private void PostorderTraversal(Node<T> root, Visitor_BinaryTree<T> visitor)
		{
			if (root == null || visitor.stop) return;//这个stop是用于解决递归终止
			PostorderTraversal(root.left, visitor);
			PostorderTraversal(root.right, visitor);
			if (visitor.stop) return;//这个stop是终止我们传入的逻辑
			if (visitor.logic != null)
			{
				visitor.stop = visitor.logic(root.element);
			}
			else if (visitor.treeNodeLogic != null)
			{
				visitor.stop = visitor.treeNodeLogic(root);
			}
		}
		//迭代1
		//后序遍历在决定是否可以输出当前节点的值的时候，需要考虑其左右子树是否都已经遍历完成
		public void PostorderTraversal2(Visitor_BinaryTree<T> visitor)
		{
			if (visitor == null || root == null)
			{
				return;
			}
			Stack<Node<T>> stack = new Stack<Node<T>>();//用栈来回访上一层节点
			Node<T> node = root;
			Node<T> lastNode = root;//用于记录上一个节点情况
			while (node != null || stack.Count != 0)//遍历完所有节点的条件
			{
				//这里用while是要找到最左边
				while (node != null)
				{
					stack.Push(node);//每遍历一次就入栈
					node = node.left;//（左 右 中）故先左边进入
				}

				//查看当前栈顶元素，该元素需要进一步判断
				node = stack.Peek();

				//若右子节点是空（即叶子节点），或者右子节点是上一轮处理的节点（前节点是lastNode的父节点）
				if (node.right == null || node.right == lastNode)
				{
					//后序遍历逻辑要在处理完左边右边全部后
					if (VisitorChoose(node, visitor))
					{
						return;
					}
					stack.Pop();
					lastNode = node;//直接标记该节点
					node = null;//表示已经无，则之后就会再进入这里，达到后序遍历效果
				}
				else
				{
					node = node.right;//一路向右
				}
			}
		}
		//迭代2
		public void PostorderTraversal(Visitor_BinaryTree<T> visitor)
		{
			if (visitor == null || root == null)
			{
				return;
			}
			Stack<Node<T>> stack = new Stack<Node<T>>();
			stack.Push(root);//先将root入队
			Node<T> lastNode = null;//用于记录上一个遍历的节点
			while (stack.Count != 0)//遍历完所有节点的条件
			{
				Node<T> top = stack.Peek();//取出最后入栈的元素

				//两种情况需要弹出：
				//一是该节点是叶子，后续无可遍历元素，即弹出top，并标记该元素为下一轮遍历时的lastNode
				//二是提前标记上一轮的lastNode是目前遍历节点top的子节点，表示top后续的节点已经遍历完毕，弹出top，同上
				if (top.IsLeaf || (lastNode != null) && lastNode.parent == top)
				{
					lastNode = stack.Pop();
					if (VisitorChoose(lastNode, visitor))
					{
						return;
					}
				}
				//入栈的顺序：先根再右后左，这样弹出时就是相反的顺序即后序遍历
				else
				{
					if (top.right != null)
					{
						stack.Push(top.right);
					}
					if (top.left != null)
					{
						stack.Push(top.left);
					}
				}
			}
		}

		//遍历执行的方法
		private bool VisitorChoose(Node<T> node, Visitor_BinaryTree<T> visitor)
		{
			if (visitor.logic != null)
			{
				if (visitor.logic(node.element))
				{
					return true;
				}
			}
			else if (visitor.treeNodeLogic != null)
			{
				if (visitor.treeNodeLogic(node))
				{
					return true;
				}
			}
			return false;
		}
		#endregion
	}
}
