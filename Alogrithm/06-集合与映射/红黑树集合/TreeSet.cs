using DataStructure.Util;
using DataStructure.集合与映射.红黑树映射;
using System;
using System.Collections.Generic;

namespace DataStructure.集合与映射.红黑树集合 {
	//利用TreeMap（红黑树实现）实现的Set
	class TreeSet<T> : Set<T> {
		TreeMap<T, object> map = new TreeMap<T, object>();
		protected override int GetSize() {
			return map.Size();
		}

		public override void Clear() {
			map.Clear();
		}

		public override void Add(T element) {
			map.Put(element, null);
		}

		public override void Remove(T element) {
			map.Remove(element);
		}

		public override bool Contains(T element) {
			return map.ContainsKey(element);
		}

		public override void Traversal(Func<T, bool> func) {
			if (func == null) {
				return;
			}
			Visitor<T> visitor = new Visitor<T>() { logic = func };
			//默认执行中序遍历（即按大小）
			InorderTraversal(visitor);
		}

		//中序遍历
		private void InorderTraversal(Visitor<T> visitor) {
			if (visitor == null) return;
			Stack<MapNode<T, object>> stack = new Stack<MapNode<T, object>>();//用栈来回访上一层节点
			MapNode<T, object> node = map.Root;
			while (node != null || stack.Count != 0)//遍历完所有节点的条件
			{
				if (node != null) {
					stack.Push(node);//每遍历一次就入栈
					node = node.left;//（左 中 右）故先左边进入
				}
				else {
					node = stack.Pop();
					//中序的逻辑处理与要等左边全部遍历完时
					visitor.logic(node.key);
					node = node.right;//返回上一层节点，取其右节点
				}
			}
		}
	}
}
