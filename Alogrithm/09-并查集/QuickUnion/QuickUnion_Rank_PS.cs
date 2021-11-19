﻿namespace DataStructure.并查集.QuickUnion {
	//基于rank优化的QuickUnion并进行PathSplitting
	//路径分裂，将节点到根节点上的所有节点指向其祖父节点，开销降低，同时也能降低树高
	class QuickUnion_Rank_PS : UnionFind {
		//用于记录每个集合树高
		int[] ranks;
		public QuickUnion_Rank_PS(int capacity) : base(capacity) {
			//初始化每个集合的rank
			ranks = new int[capacity];
			for (int i = 0; i < capacity; i++) {
				ranks[i] = 1;
			}
		}

		//路径分裂，将节点到根节点上的所有节点指向其祖父节点
		public override int Find(int value) {
			RangeCheck(value);
			int parent;//指针记录父节点
			while (value != parents[value]) {
				parent = parents[value];
				parents[value] = parents[parents[value]];//指向祖父
				value = parent;//指针指向原来的父节点
			}
			return parents[value];
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
