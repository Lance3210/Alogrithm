using System;
using System.Collections.Generic;

namespace DataStructure.图 {
	//图的抽象数据结构
	public abstract class Graph<V, E> {
		public abstract int VerticesSize { get; }
		public bool IsEmpty => VerticesSize == 0;
		public abstract int EdgesSize { get; }
		public abstract void AddVertex(V value);
		public abstract void AddEdge(V from, V to);
		public abstract void AddEdge(V from, V to, E weight);
		public abstract void RemoveVertex(V value);
		public abstract void RemoveEdge(V from, V to);
		public abstract void BreadthFirstSearch(V origin, Func<V, bool> func);
		public abstract void DepthFirstSearch(V origin, Func<V, bool> func);
		public abstract void ToStringTraversal();
		public abstract List<V> TopologicalSort();//返回拓扑排序结果
		public abstract Dictionary<V, PathInfo<V, E>> ShortestPath_Dijkstra(V origin, Func<E, E, E> add);//最短路径 Dijkstra
		public abstract Dictionary<V, PathInfo<V, E>> ShortestPath_BellmanFord(V origin, Func<E, E, E> add);//最短路径 BellmanFord
		public abstract Dictionary<V, Dictionary<V, PathInfo<V, E>>> ShortestPath_Floyd(Func<E, E, E> add);//最短路径 BellmanFord
	}
	//抽象边信息
	public class EdgeInfo<V, E> {
		public V From { get; }
		public V To { get; }
		public E Weight { get; }
		public EdgeInfo(V from, V to, E weight) {
			From = from;
			To = to;
			Weight = weight;
		}
	}
	//最短路径信息
	public class PathInfo<V, E> {
		public E Weight { get; set; }
		public LinkedList<EdgeInfo<V, E>> EdgeInfos { get; }
		public PathInfo() {
			Weight = default;
			EdgeInfos = new LinkedList<EdgeInfo<V, E>>();
		}
		public PathInfo(E weight) {
			Weight = weight;
			EdgeInfos = new LinkedList<EdgeInfo<V, E>>();
		}
		public void AddPath(EdgeInfo<V, E> edgeInfo) {
			EdgeInfos.AddLast(edgeInfo);
		}
		public void AddPaths(LinkedList<EdgeInfo<V, E>> edgeInfos) {
			foreach (var item in edgeInfos) {
				EdgeInfos.AddLast(item);
			}
		}
	}
}
