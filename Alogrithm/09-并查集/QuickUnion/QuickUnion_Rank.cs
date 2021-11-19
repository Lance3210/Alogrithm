namespace DataStructure.并查集.QuickUnion {
	//基于rank优化的QuickUnion
	//将树高较矮的集合合并到树高度较高的集合
	class QuickUnion_Rank : UnionFind {
		//用于记录每个集合树高
		int[] ranks;
		public QuickUnion_Rank(int capacity) : base(capacity) {
			//初始化每个集合的rank
			ranks = new int[capacity];
			for (int i = 0; i < capacity; i++) {
				ranks[i] = 1;
			}
		}

		//树高不一定为2，要向上找到根节点为止，根节点必是指向自己的
		public override int Find(int value) {
			RangeCheck(value);
			while (value != parents[value]) {
				value = parents[value];
			}
			return value;
		}

		//基于rank优化的QuickUnion，将树高较矮的集合合并到树高度较高的集合
		public override void Union(int value1, int value2) {
			int root1 = Find(value1);
			int root2 = Find(value2);
			if (root1 == root2) {
				return;
			}
			if (ranks[root1] < ranks[root2]) {
				parents[root1] = root2;
			}
			else if (ranks[root1] > ranks[root2]) {
				parents[root2] = root1;
			}
			else//只有相等时才需要更新高度，任意嫁接后更新嫁接后的根节点高度即可
			{
				parents[root1] = root2;
				ranks[root2] += 1;
			}
		}
	}
}
