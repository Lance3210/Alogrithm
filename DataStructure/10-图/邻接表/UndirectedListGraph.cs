using DataStructure.堆.二叉堆;
using DataStructure.并查集.Generic;
using System;
using System.Collections.Generic;

namespace DataStructure.图.邻接表
{
	//无向图
	class UndirectedListGraph<V, E> : ListGraph<V, E>
	{
		public UndirectedListGraph()
		{
		}
		public UndirectedListGraph(Func<E, E, int> weightComparer) : base(weightComparer)
		{
		}

		public override void AddEdge(V from, V to)
		{
			base.AddEdge(from, to);
			base.AddEdge(to, from);
		}
		public override void AddEdge(V from, V to, E weight)
		{
			base.AddEdge(from, to, weight);
			base.AddEdge(to, from, weight);
		}
		public override void RemoveEdge(V from, V to)
		{
			base.RemoveEdge(from, to);
			base.RemoveEdge(to, from);
		}

		//最小生成树（Prim算法）
		public HashSet<EdgeInfo<V, E>> Prim()
		{
			if (IsEmpty)
			{
				return null;
			}
			HashSet<EdgeInfo<V, E>> mst = new();//用set返回最小生成树组成的边
			Vertex<V, E> vertex = null;
			foreach (var item in vertices)
			{
				vertex = item.Value;//先随机取出一个顶点作为起始点
				if (vertex == null)
				{
					return null;
				}
				break;
			}
			//使用一个最小堆，每轮可以选出权值最小的边
			BinaryHeap<Edge<V, E>> heap = new((e1, e2) => {
				return -EdgesCompare(e1, e2);
			}, vertex.outEdges);//初始化时自动建堆，因为遍历所有元素一个个插入堆效率不高

			HashSet<Vertex<V, E>> addedVerteices = new();//使用一个set来记录已遍历顶点
			addedVerteices.Add(vertex);
			//不断从堆中取出最小的边，已经遍历过的节点数量等于全部节点数量即可停止
			//或者是只要最小生成树中的边数刚好等于顶点数-1即可停止
			while (!heap.IsEmpty && addedVerteices.Count < vertices.Count)
			{
				Edge<V, E> edge = heap.Remove();//拿到最小边
				if (addedVerteices.Contains(edge.to))
				{
					continue;//遍历过的顶点无须再取出其出边
				}
				mst.Add(edge.ConvertToInfo());
				addedVerteices.Add(edge.to);//记录已遍历顶点
				heap.AddAll(edge.to.outEdges);//将新边的to的outEdges全部添加进堆（但其中有重复边）
			}
			return mst;
		}

		//最小生成树（Kruskal算法）
		public HashSet<EdgeInfo<V, E>> Kruskal()
		{
			if (IsEmpty)
			{
				return null;
			}
			HashSet<EdgeInfo<V, E>> mst = new();//用set返回最小生成树组成的边	
			UnionFind<Vertex<V, E>> unionFind = new();//使用一个并查集来检测是否会构成环
			foreach (var item in vertices)
			{
				unionFind.CreatNode(item.Value);//初始化并查集
			}
			//使用一个最小堆，每轮可以选出权值最小的边
			BinaryHeap<Edge<V, E>> heap = new((e1, e2) => {
				return -EdgesCompare(e1, e2);
			}, edges);//将所有边建堆

			//不断从堆中取出最小的边，只要最小生成树中的边数刚好等于顶点数-1即可停止
			int edgeSize = vertices.Count - 1;
			while (!heap.IsEmpty && mst.Count < edgeSize)
			{
				Edge<V, E> edge = heap.Remove();
				if (unionFind.IsSame(edge.from, edge.to))
				{
					continue;//如果该边的两个顶点都已经在一个集合中了，添加该边就会变成环
				}
				mst.Add(edge.ConvertToInfo());//不构成环则说明是组成最小生成树的边
				unionFind.Union(edge.from, edge.to);//同时合并成一个集合
			}
			return mst;
		}

		public override void ToStringTraversal()
		{
			HashSet<Edge<V, E>> edges = new();
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine($"All Vertices: {VerticesSize}");
			Console.WriteLine($"All Edges: {EdgesSize >> 1}");
			Console.ForegroundColor = ConsoleColor.White;

			foreach (var item in vertices)
			{
				item.Value.Traversal();
			}
			Console.WriteLine();

			foreach (var item in edges)
			{
				if (edges.Contains(item))
				{
					continue;
				}
				Console.WriteLine($"{item.from.value} --> {item.to.value}   weight: {item.weight}");
				edges.Add(item);
			}
			Console.WriteLine("---------------------------------------------------------------------");
		}
	}
}
