namespace DataStructure.并查集.QuickUnion {
	//快合并型并查集
	//在基础模式下，每个集合的树高不定，固定方向合并有时候会造成退化为链表
	//两种优化方案：基于size（树元素个数），基于rank（树高矮）
	//Find和Union O(α(n))，α(n) < 5（前提是再进行路径分裂、减半等优化）
	class QucikUnion_Base : UnionFind {
		public QucikUnion_Base(int capacity) : base(capacity) {

		}

		//树高不一定为2，要向上找到根节点为止，根节点必是指向自己的
		public override int Find(int value) {
			RangeCheck(value);
			while (value != parents[value]) {
				value = parents[value];
			}
			return value;
		}

		//找到value1和value2所在的集合的根节点，将value1的根节点嫁接到value2根节点
		//存在问题：可能存在退化为链表的情况
		public override void Union(int value1, int value2) {
			int root1 = Find(value1);
			int root2 = Find(value2);
			if (root1 == root2) {
				return;
			}
			parents[root1] = root2;
		}
	}
}
