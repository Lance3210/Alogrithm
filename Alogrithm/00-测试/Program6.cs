using DataStructure.图;
using DataStructure.图.邻接表;
using System;
using System.Collections.Generic;

namespace DataStructure.测试 {
	class Program6 {
		const int count = 10;
		static void Main0(string[] args) {

			SPTest2();
		}

		private static void SPTest2() {
			ListGraph<string, int> graph = new((w1, w2) => {
				return w1.CompareTo(w2);
			});
			GenerateGraph_ShortestPaths(graph);
			Dictionary<string, Dictionary<string, PathInfo<string, int>>> dic = graph.ShortestPath_Floyd((e1, e2) => {
				return e1 + e2;
			});
			foreach (var paths in dic) {
				Console.WriteLine($"起点：{paths.Key}");
				foreach (var info in paths.Value) {
					Console.Write($"终点：{info.Key} ");
					Console.Write($"  路径长度： {info.Value.Weight}   ");
					Console.Write(paths.Key);
					foreach (var edge in info.Value.EdgeInfos) {
						Console.Write($" -> {edge.To}");
					}
					Console.WriteLine();
				}
				Console.WriteLine("----------------------------------------------");
			}
		}

		private static void SPTest() {
			UndirectedListGraph<string, int> graph = new((w1, w2) => {
				return w1.CompareTo(w2);
			});
			GenerateGraph_ShortestPaths(graph);
			Dictionary<string, PathInfo<string, int>> dic = graph.ShortestPath_Dijkstra("A", (e1, e2) => {
				return e1 + e2;
			});
			foreach (var item in dic) {
				Console.Write($"终点：{item.Key } ");
				Console.Write($"路径：");
				foreach (var edgeInfo in item.Value.EdgeInfos) {
					Console.Write($"{edgeInfo.From} -> ");
				}
				Console.Write($"{item.Key} ");
				Console.WriteLine($"权值：{item.Value.Weight}");
			}
		}

		private static void UndirectedGraph() {
			UndirectedListGraph<string, int> graph = new((w1, w2) => {
				return w1.CompareTo(w2);
			});
			GenerateGraph(graph);
			//最小生成树
			HashSet<EdgeInfo<string, int>> set = graph.Prim();
			foreach (var item in set) {
				Console.WriteLine($"from:{item.From}  to:{item.To}  weight:{item.Weight}");
			}
			Console.WriteLine("-------------------------------------------------------------");
			set = graph.Kruskal();
			foreach (var item in set) {
				Console.WriteLine($"from:{item.From}  to:{item.To}  weight:{item.Weight}");
			}
		}

		private static void ListGraphTest() {
			ListGraph<string, int> graph = new((w1, w2) => {
				return w1.CompareTo(w2);
			});
			GenerateGraph(graph);

			//广度优先遍历
			graph.BreadthFirstSearch("V0", (v) => {
				Console.Write(v + " ");
				return false;
			});
			Console.WriteLine();

			//深度优先遍历
			graph.DepthFirstSearch("V0", (v) => {
				Console.Write(v + " ");
				return false;
			});
			Console.WriteLine();
			graph.DepthFirstSearch0("V0", (v) => {
				Console.Write(v + " ");
				return false;
			});
			Console.WriteLine();

			//拓扑排序
			foreach (var item in graph.TopologicalSort()) {
				Console.Write(item + " ");
			}
			Console.WriteLine();
		}

		private static void GenerateGraph(Graph<string, int> graph) {
			graph.AddEdge("A", "B", 17);
			graph.AddEdge("A", "F", 1);
			graph.AddEdge("A", "E", 16);
			graph.AddEdge("B", "C", 6);
			graph.AddEdge("B", "F", 11);
			graph.AddEdge("B", "D", 5);
			graph.AddEdge("C", "D", 10);
			graph.AddEdge("D", "F", 14);
			graph.AddEdge("D", "E", 4);
			graph.AddEdge("E", "F", 33);
			graph.ToStringTraversal();
		}

		private static void GenerateGraph_ShortestPaths(Graph<string, int> graph) {
			graph.AddEdge("A", "B", 10);
			graph.AddEdge("A", "C", 100);
			graph.AddEdge("B", "C", 20);
			graph.AddEdge("B", "D", 200);
			graph.AddEdge("C", "D", 30);
			//graph.AddEdge("A", "B", 10);
			//graph.AddEdge("A", "D", 30);
			//graph.AddEdge("A", "E", 100);
			//graph.AddEdge("B", "C", 50);
			//graph.AddEdge("C", "E", 10);
			//graph.AddEdge("D", "C", 20);
			//graph.AddEdge("D", "E", 60);
			graph.ToStringTraversal();
		}
	}
}
