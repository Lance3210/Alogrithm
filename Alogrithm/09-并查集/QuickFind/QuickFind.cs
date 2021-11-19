namespace DataStructure.并查集.QuickFind {
	//快查找型并查集
	//在这种模式下，每个集合的树高为二，每个元素指向的父节点一致因此查找（根节点）效率高
	//Find O(1) ，Union O(n)
	class QuickFind : UnionFind {
		public QuickFind(int capacity) : base(capacity) {

		}

		//因为树高为2，索引对应的parent就是根节点
		public override int Find(int value) {
			RangeCheck(value);
			return parents[value];
		}

		//将value1所在集合所有元素嫁接到value2所在集合，即value1集合所有元素的parent指向value2
		public override void Union(int value1, int value2) {
			int parent1 = Find(value1);
			int parent2 = Find(value2);
			if (parent1 == parent2) {
				return;
			}
			for (int i = 0; i < parents.Length; i++) {
				if (parents[i] == parent1)//与value1同一个父节点的节点都要改成value2的父节点
				{
					parents[i] = parent2;
				}
			}
		}
	}
}
