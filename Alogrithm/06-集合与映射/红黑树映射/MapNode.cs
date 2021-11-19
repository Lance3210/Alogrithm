using DataStructure.Util;

namespace DataStructure.集合与映射.红黑树映射 {
	//直接创建一种基于红黑树的映射节点
	class MapNode<K, V> {
		public K key;
		public V value;

		public RBNodeColor color = RBNodeColor.RED;

		public MapNode<K, V> left;
		public MapNode<K, V> right;
		public MapNode<K, V> parent;
		public MapNode(K key, V value, MapNode<K, V> parent) {
			this.key = key;
			this.value = value;
			this.parent = parent;
		}

		public bool IsRoot => (left != null || right != null) && (parent == null);
		public bool IsLeaf => (left == null) && (right == null);
		public bool HasOnlyOneChild => ((left != null) && (right == null)) || ((left == null) && (right != null));
		public bool HasTwoChildren => (left != null) && (right != null);
		public bool IsLeftChild => (parent != null) && (this == parent.left);
		public bool IsRightChild => (parent != null) && (this == parent.right);

		public MapNode<K, V> Sibling {
			get {
				if (IsLeftChild) {
					return parent.right;
				}
				if (IsRightChild) {
					return parent.left;
				}
				return null;
			}
		}

		public MapNode<K, V> Uncle => parent.Sibling;

		public MapNode<K, V> Grand => parent.parent;

		public MapNode<K, V> Clone() {
			return MemberwiseClone() as MapNode<K, V>;
		}
	}
}
