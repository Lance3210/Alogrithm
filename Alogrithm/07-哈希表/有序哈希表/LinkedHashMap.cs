using System;

namespace DataStructure.哈希表.有序哈希表
{
	class LinkedHashMap<K, V> : HashMap<K, V>
	{
		private LinkedHashNode<K, V> first;
		private LinkedHashNode<K, V> last;
		public LinkedHashNode<K, V> First => first;
		public LinkedHashNode<K, V> Last => last;

		//创建节点时即维护其顺序
		protected override HashNode<K, V> CreateNode(K key, V value, HashNode<K, V> parent)
		{
			LinkedHashNode<K, V> node = new LinkedHashNode<K, V>(key, value, parent);
			//串线
			if (first == null)
			{
				first = last = node;
			}
			else
			{
				last.next = node;
				node.pre = last;
				last = node;
			}
			return node;
		}

		//删除调整
		protected override void FixedRemove(HashNode<K, V> originNode, HashNode<K, V> node)
		{
			LinkedHashNode<K, V> node1 = (LinkedHashNode<K, V>)originNode;
			LinkedHashNode<K, V> node2 = (LinkedHashNode<K, V>)node;
			//由于存在度为2的节点删除，其修复顺序可能会存在bug
			//判断这两个node是否是同一个，不是则删除的是度为2的节点
			if (originNode != node)
			{
				//交换这两个node在链表中的位置
				//交换pre
				LinkedHashNode<K, V> temp = node1.pre;
				node1.pre = node2.pre;
				node2.pre = temp;
				if (node1.pre == null)
				{
					first = node1;
				}
				else
				{
					node1.pre.next = node1;
				}
				if (node2.pre == null)
				{
					first = node2;
				}
				else
				{
					node2.pre.next = node2;
				}

				//交换next
				temp = node1.next;
				node1.next = node2.next;
				node2.next = temp;
				if (node1.next == null)
				{
					last = node1;
				}
				else
				{
					node1.next.pre = node1;
				}
				if (node2.next == null)
				{
					last = node2;
				}
				else
				{
					node2.next.pre = node2;
				}
			}
			//修复的是node2，度为2的节点已经交换，故下面的修复可以使链表顺序不变
			LinkedHashNode<K, V> preNode = node2.pre;
			LinkedHashNode<K, V> nextNode = node2.next;
			if (preNode == null)
			{
				first = nextNode;
			}
			else
			{
				preNode.next = nextNode;
			}

			if (nextNode == null)
			{
				last = preNode;
			}
			else
			{
				nextNode.pre = preNode;
			}
		}

		//要清空头尾
		public override void Clear()
		{
			base.Clear();
			first = null;
			last = null;
		}

		public override bool ContainsValue(V value)
		{
			LinkedHashNode<K, V> node = first;
			while (node != null)
			{
				if (Equals(value, node.value))
				{
					return true;
				}
				node = node.next;
			}
			return false;
		}

		//遍历每一个节点
		public override void Traversal(Func<K, V, bool> func)
		{
			LinkedHashNode<K, V> node = first;
			while (node != null)
			{
				func(node.key, node.value);
				node = node.next;
			}
		}
	}
}
