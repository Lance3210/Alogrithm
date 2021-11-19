using System.Collections.Generic;

namespace DataStructure.并查集.Generic {
	//自定义数据类型并查集
	//将原来数组中的每一项封装为一个节点
	//其中value即索引，parent即指向的父节点

	//通用节点，链表形式
	class UnionFindNode<T> {
		public int rank = 1;//高度记录默认为1
		public T value;//即原来的索引
		public UnionFindNode<T> parent;//记录父母指针
		public UnionFindNode(T value) {
			this.value = value;
			parent = this;//每一个节点创建时都应该指向其自身
		}
	}
	//通用并查集，实现自定义数据的并查集
	class UnionFind<T> {
		//用字典来代替原来的数组，索引value即键，父节点parent即值
		private Dictionary<T, UnionFindNode<T>> dic = new();

		//传入的新节点
		public void CreatNode(T value) {
			if (dic.ContainsKey(value)) {
				return;
			}
			dic.Add(value, new UnionFindNode<T>(value));//当并查集中无该元素才新增节点
		}

		//可以在创建时传入多个元素
		public UnionFind() {

		}
		public UnionFind(params T[] values) {
			for (int i = 0; i < values.Length; i++) {
				CreatNode(values[i]);
			}
		}

		//返回该元素对应的根节点的value
		public T Find(T value) {
			UnionFindNode<T> node = FindNode(value);//找到根节点
			return node == null ? default : node.value;//查找时可能传入的value不在并查集中，要做空判断
		}
		//返回该元素对应的根节点
		private UnionFindNode<T> FindNode(T value) {
			UnionFindNode<T> node = dic[value];
			if (node == null) {
				return null;
			}
			//使用路径减半
			//由于是自定义类型，不一定能直接==
			while (!node.value.Equals(node.parent.value)) {
				node.parent = node.parent.parent;//指向祖父
				node = node.parent.parent;//隔一个再操作
			}
			return node;
		}

		//基于rank的QuickUnion，合并两个元素所在集合
		public void Union(T value1, T value2) {
			UnionFindNode<T> root1 = FindNode(value1);
			UnionFindNode<T> root2 = FindNode(value2);
			if (root1 == null || root2 == null) {
				return;//查找后可能检测到传入的value不在并查集中，要做空判断
			}
			if (root1.Equals(root2)) {
				return;
			}
			if (root1.rank < root2.rank) {
				root1.parent = root2;
			}
			else if (root1.rank > root2.rank) {
				root2.parent = root1;
			}
			else//只有相等时才需要更新高度，任意嫁接后更新嫁接后的根节点高度即可
			{
				root1.parent = root2;
				root2.rank += 1;
			}
		}

		//判断两个元素是否在同一集合
		public bool IsSame(T value1, T value2) {
			return Find(value1).Equals(Find(value2));
		}
	}
}
