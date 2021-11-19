using System;

namespace DataStructure.Util {
	//用于某种数据结构遍历时传入逻辑
	public class Visitor<T> {
		public bool stop;
		public Func<T, bool> logic;//处理元素的逻辑
	}

	//用于二叉树节点遍历
	public class Visitor_BinaryTree<T> : Visitor<T> {
		public Func<树.二叉树.Node<T>, bool> treeNodeLogic;
	}

	//用于键值对遍历
	public class Visitor_KeyValuePair<K, V> : Visitor<K> {
		public Func<K, V, bool> mapNodeLogic;
	}
}
