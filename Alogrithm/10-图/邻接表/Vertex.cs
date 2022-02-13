using System;
using System.Collections.Generic;

namespace DataStructure.图.邻接表
{
	//定义顶点
	public class Vertex<V, E>
	{
		public V value;//顶点
		public HashSet<Edge<V, E>> outEdges = new();//出边，以该顶点为起点的边的集合
		public HashSet<Edge<V, E>> inEdges = new();//入边，以该顶点为终点的边的集合
		public Vertex(V value)
		{
			this.value = value;
		}
		//实现比较，用于判断Contains
		public override bool Equals(object obj)
		{
			return ((Vertex<V, E>)obj).value.Equals(value);
		}
		public override int GetHashCode()
		{
			return value == null ? 0 : value.GetHashCode();//允许为空
		}
		public void Traversal()
		{
			Console.WriteLine(value.ToString());
			Console.WriteLine("Out Edges:");
			foreach (var item in outEdges)
			{
				Console.WriteLine($"{item.from.value} --> {item.to.value}   weight: {item.weight}");
			}
			Console.WriteLine("In  Edges:");
			foreach (var item in inEdges)
			{
				Console.WriteLine($"{item.from.value} --> {item.to.value}   weight: {item.weight}");
			}
			Console.WriteLine();
			Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
		}
	}
}
