using DataStructure.Util;

namespace DataStructure.哈希表 {
	//哈希表用MapNode
	class HashNode<K, V> {
		public K key;
		public V value;
		//添加一个hashCode属性
		public int HashCode { get; set; }

		public RBNodeColor color = RBNodeColor.RED;

		public HashNode<K, V> left;
		public HashNode<K, V> right;
		public HashNode<K, V> parent;
		public HashNode(K key, V value, HashNode<K, V> parent) {
			this.key = key;
			this.value = value;
			this.parent = parent;
			HashCode = key == null ? 0 : key.GetHashCode();//创建时即计算HashCode
		}

		public bool IsRoot => (left != null || right != null) && (parent == null);
		public bool IsLeaf => (left == null) && (right == null);
		public bool HasOnlyOneChild => ((left != null) && (right == null)) || ((left == null) && (right != null));
		public bool HasTwoChildren => (left != null) && (right != null);
		public bool IsLeftChild => (parent != null) && (this == parent.left);
		public bool IsRightChild => (parent != null) && (this == parent.right);

		public HashNode<K, V> Sibling {
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

		public HashNode<K, V> Uncle => parent.Sibling;

		public HashNode<K, V> Grand => parent.parent;

		public HashNode<K, V> Clone() {
			return MemberwiseClone() as HashNode<K, V>;
		}
	}
}
