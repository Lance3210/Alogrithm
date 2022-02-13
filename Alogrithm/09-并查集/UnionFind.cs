using System;

namespace DataStructure.并查集
{
	abstract class UnionFind
	{
		//用一个数组来实现并查集，其中索引即节点，值即对应父节点
		protected int[] parents;
		//初始化每一个元素，每个元素都是指向自己的独立集合
		public UnionFind(int capacity)
		{
			if (capacity < 0)
			{
				throw new ArgumentException("Capacity must be >= 1");
			}
			parents = new int[capacity];
			for (int i = 0; i < parents.Length; i++)
			{
				parents[i] = i;
			}
		}
		//查找该元素所在的集合，返回“根节点”
		public abstract int Find(int value);
		//合并两个元素所在的集合
		public abstract void Union(int value1, int value2);
		//检查两个元素是否在同一个集合
		public bool IsSame(int value1, int value2)
		{
			return Find(value1) == Find(value2);
		}
		protected void RangeCheck(int value)
		{
			if (value < 0 || value >= parents.Length)
			{
				throw new ArgumentException("Value is not be allowed");
			}
		}
		//打印
		public void Print()
		{
			Console.Write("index：  ");
			for (int i = 0; i < parents.Length; i++)
			{
				Console.Write($"{i} ");
			}
			Console.WriteLine();
			Console.Write("parent： ");
			for (int i = 0; i < parents.Length; i++)
			{
				Console.Write($"{parents[i]} ");
			}
			Console.WriteLine();
			Console.WriteLine("-----------------------------------------------------------------------------------------");
		}
	}
}
