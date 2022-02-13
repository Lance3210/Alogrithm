namespace DataStructure.并查集.QuickUnion
{
	//基于size优化的QuickUnion
	//将元素数量少的集合合并到元素数量大的集合
	class QuickUnion_Size : UnionFind
	{
		//用于记录每个集合的元素个数
		int[] sizes;
		public QuickUnion_Size(int capacity) : base(capacity)
		{
			//基于size的优化，初始化每个集合的size
			sizes = new int[capacity];
			for (int i = 0; i < capacity; i++)
			{
				sizes[i] = 1;
			}
		}

		//树高不一定为2，要向上找到根节点为止，根节点必是指向自己的
		public override int Find(int value)
		{
			RangeCheck(value);
			while (value != parents[value])
			{
				value = parents[value];
			}
			return value;
		}

		//基于size的优化，将元素数量少的集合合并到元素数量大的集合
		public override void Union(int value1, int value2)
		{
			int root1 = Find(value1);
			int root2 = Find(value2);
			if (root1 == root2)
			{
				return;
			}
			if (sizes[root1] < sizes[root2])
			{
				parents[root1] = root2;
				sizes[root2] += sizes[root1];
			}
			else
			{
				parents[root2] = root1;
				sizes[root1] += sizes[root2];
			}
		}
	}
}
