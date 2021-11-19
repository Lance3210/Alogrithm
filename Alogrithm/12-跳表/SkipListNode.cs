namespace DataStructure.跳表 {
	class SkipListNode<K, V> {
		public K key;
		public V value;
		public SkipListNode<K, V>[] nexts;

		public SkipListNode(SkipListNode<K, V>[] nexts) {
			this.nexts = nexts;
		}
		public SkipListNode(K key, V value, int level) {
			this.key = key;
			this.value = value;
			nexts = new SkipListNode<K, V>[level];
		}

		public override string ToString() {
			return "Key: " + key + "\tValue: " + value;
		}
	}
}