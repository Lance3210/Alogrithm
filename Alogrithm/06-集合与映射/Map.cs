using System;

namespace DataStructure.集合与映射 {
	//映射基类
	public abstract class Map<K, V> {
		protected int size;
		public int Size() => size;
		public bool IsEmpty => size == 0;
		public abstract void Clear();
		public abstract V Put(K key, V value);
		public abstract V GetValue(K key);
		public abstract V Remove(K key);
		public abstract bool ContainsKey(K key);
		public abstract bool ContainsValue(V value);
		public virtual void Traversal(Func<K, V, bool> func) {
			if (size == 0 || func == null) {
				return;
			}
		}
	}
}
