using System;

namespace DataStructure.图.邻接表
{
	//定义边
	public class Edge<V, E> : IComparable<E>
	{
		public E weight;//权值
		public Vertex<V, E> from;//边的起点
		public Vertex<V, E> to;//边的终点
		public Edge(Vertex<V, E> from, Vertex<V, E> to)
		{
			this.from = from;
			this.to = to;
		}
		public Edge(Vertex<V, E> from, Vertex<V, E> to, E weight)
		{
			this.from = from;
			this.to = to;
			this.weight = weight;
		}

		//边比较（比权值）
		public int CompareTo(E other)
		{
			return ((IComparable)weight).CompareTo((other as Edge<V, E>).weight);
		}

		//用于判断Contains
		public override bool Equals(object obj)
		{
			Edge<V, E> edge = obj as Edge<V, E>;
			return edge.from.Equals(from) && edge.to.Equals(to);
		}
		public override int GetHashCode()
		{
			int fromHashCode = from.GetHashCode();
			return (fromHashCode << 5) - fromHashCode + to.GetHashCode();
		}

		//用于传递边信息
		public EdgeInfo<V, E> ConvertToInfo()
		{
			return new EdgeInfo<V, E>(from.value, to.value, weight);
		}
	}
}
