namespace DataStructure.哈希表.有序哈希表 {
	//带指针的hashNode
	class LinkedHashNode<K, V> : HashNode<K, V> {
		public LinkedHashNode<K, V> pre;
		public LinkedHashNode<K, V> next;

		public LinkedHashNode(K key, V value, HashNode<K, V> parent) : base(key, value, parent) {

		}
	}
}
