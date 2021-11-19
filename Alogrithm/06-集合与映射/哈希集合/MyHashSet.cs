using DataStructure.哈希表;
using System;

namespace DataStructure.集合与映射.哈希集合 {
	class MyHashSet<T> : Set<T> {
		//利用HashMap实现的Set
		HashMap<T, object> map = new HashMap<T, object>();
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

		//遍历只需要key
		public override void Traversal(Func<T, bool> func) {
			map.Traversal((k, v) => {
				return func(k);
			});
		}
	}
}
