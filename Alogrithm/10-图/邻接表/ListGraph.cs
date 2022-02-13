using System;
using System.Collections.Generic;

namespace DataStructure.图.邻接表
{
	//邻接表实现有向图
	//也可以表示无向图，双向添加即可
	public class ListGraph<V, E> : Graph<V, E>
	{
		protected Dictionary<V, Vertex<V, E>> vertices = new();//使用一个Dictionary来映射V和Vertex	
		protected HashSet<Edge<V, E>> edges = new();//使用一个Set来存储Edge
		public override int VerticesSize => vertices.Count;
		public override int EdgesSize => edges.Count;

		protected Func<E, E, int> weightComparer;//权值比较
		protected Func<E, E, E> weightAdd;//权值相加

		public ListGraph()
		{
		}

		public ListGraph(Func<E, E, int> weightComparer)
		{
			this.weightComparer = weightComparer;
		}

		public override void ToStringTraversal()
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine($"All Vertices: {VerticesSize}");
			Console.ForegroundColor = ConsoleColor.White;
			foreach (var item in vertices)
			{
				item.Value.Traversal();
			}
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine($"All Edges: {EdgesSize}");
			Console.ForegroundColor = ConsoleColor.White;
			foreach (var item in edges)
			{
				Console.WriteLine($"{item.from.value} --> {item.to.value}   weight: {item.weight}");
			}
			Console.WriteLine("---------------------------------------------------------------------");
		}

		//添加顶点
		public override void AddVertex(V value)
		{
			if (!vertices.ContainsKey(value))
			{
				vertices.Add(value, new Vertex<V, E>(value));
			}
		}

		//添加边
		public override void AddEdge(V from, V to)
		{
			AddEdge(from, to, default);
		}
		public override void AddEdge(V from, V to, E weight)
		{
			//判断from和to是否已经存在，不存在就添加
			Vertex<V, E> fromVertex;
			if (vertices.ContainsKey(from))
			{
				fromVertex = vertices[from];
			}
			else
			{
				fromVertex = new Vertex<V, E>(from);
				vertices.Add(from, fromVertex);
			}
			Vertex<V, E> toVertex;
			if (vertices.ContainsKey(to))
			{
				toVertex = vertices[to];
			}
			else
			{
				toVertex = new Vertex<V, E>(to);
				vertices.Add(to, toVertex);
			}

			//创建新边
			Edge<V, E> edge = new(fromVertex, toVertex, weight);
			//若存在与新边Equals的各个边，将它们删除
			//之所以这样做，是因为如果按照“先找到再修改”，Set是需要一个个迭代才能找到的，过于麻烦不如直接删除再添加，效率也不差
			if (fromVertex.outEdges.Remove(edge))//Remove返回bool，有存在删除才true，也就才执行删除
			{
				toVertex.inEdges.Remove(edge);
				edges.Remove(edge);
			}
			//上面清除完Equals的边后再加入新边
			fromVertex.outEdges.Add(edge);
			toVertex.inEdges.Add(edge);
			edges.Add(edge);
		}

		//移除边
		public override void RemoveEdge(V from, V to)
		{
			//判断from和to是否已经存在
			if (!vertices.ContainsKey(from) || !vertices.ContainsKey(to))
			{
				return;
			}
			Vertex<V, E> fromVertex = vertices[from];
			Vertex<V, E> toVertex = vertices[to];
			//创建一条与要删除边Equals的边
			Edge<V, E> edge = new(fromVertex, toVertex);
			//删除与其Equals的各个边
			if (fromVertex.outEdges.Remove(edge))//Remove返回bool，有存在删除才true
			{
				toVertex.inEdges.Remove(edge);
				edges.Remove(edge);
			}
		}

		//移除顶点
		public override void RemoveVertex(V value)
		{
			//判断该顶点是否已经存在
			if (!vertices.ContainsKey(value))
			{
				return;
			}
			Vertex<V, E> vertex = vertices[value];

			//迭代删除顶点中的outEdges和inEdges对应的边，以及总边集合中的对应边
			foreach (var item in vertex.outEdges)
			{
				item.to.inEdges.Remove(item);//获取该边的终点，并删除其入边集合中的这个边
				edges.Remove(item);//删除总边集合中的这个边
			}
			foreach (var item in vertex.inEdges)
			{
				item.from.outEdges.Remove(item);//获取该边的起点，并删除其出边集合中的这个边
				edges.Remove(item);//删除总边集合中的这个边
			}
			//清空顶点的出入边集合，最后从顶点集合中移除这个顶点
			vertex.outEdges.Clear();
			vertex.inEdges.Clear();
			vertices.Remove(value);
		}

		//广度优先搜索
		public override void BreadthFirstSearch(V origin, Func<V, bool> func)
		{
			if (!vertices.ContainsKey(origin) || func == null)
			{
				throw new ArgumentException("origin cannot be found or func is null");
			}
			Queue<Vertex<V, E>> queue = new();//使用队列实现
			Vertex<V, E> vertex = vertices[origin];
			queue.Enqueue(vertex);
			HashSet<Vertex<V, E>> visitedVertices = new();//使用Set记录已经访问过的顶点
			while (queue.Count != 0)
			{
				vertex = queue.Dequeue();
				if (func(vertex.value))
				{
					return;
				}
				foreach (var item in vertex.outEdges)
				{
					if (!visitedVertices.Contains(item.to))
					{
						queue.Enqueue(item.to);
						visitedVertices.Add(item.to);//该顶点能到达的顶点都应该标记以防其他顶点重复遍历
					}
				}
			}
		}

		//深度优先搜索递归实现
		public override void DepthFirstSearch(V origin, Func<V, bool> func)
		{
			if (!vertices.ContainsKey(origin) || func == null)
			{
				throw new ArgumentException("origin cannot be found or func is null");
			}
			DFS(vertices[origin], new HashSet<Vertex<V, E>>(), func);//使用Set记录已经访问过的顶点
		}
		private void DFS(Vertex<V, E> vertex, HashSet<Vertex<V, E>> visitedVertices, Func<V, bool> func)
		{
			visitedVertices.Add(vertex);
			if (func(vertex.value))
			{
				return;
			}
			foreach (var item in vertex.outEdges)
			{
				if (!visitedVertices.Contains(item.to))
				{
					DFS(item.to, visitedVertices, func);
				}
			}
		}

		//深度优先搜索非递归实现
		public void DepthFirstSearch0(V origin, Func<V, bool> func)
		{
			if (!vertices.ContainsKey(origin) || func == null)
			{
				throw new ArgumentException("origin cannot be found or func is null");
			}
			Vertex<V, E> vertex = vertices[origin];//记录当前遍历顶点
			Stack<Vertex<V, E>> stack = new();//用栈来深度遍历
			stack.Push(vertex);
			if (func(vertex.value))
			{
				return;
			}
			HashSet<Vertex<V, E>> visitedVertices = new();//使用Set记录已经访问过的顶点
			visitedVertices.Add(vertex);
			while (stack.Count != 0)
			{
				vertex = stack.Pop();
				foreach (var item in vertex.outEdges)
				{
					if (!visitedVertices.Contains(item.to))
					{
						stack.Push(vertex);//其实就是item.from，访问完还要保留
						stack.Push(item.to);
						if (func(item.to.value))
						{
							return;
						}
						visitedVertices.Add(item.to);
						break;//退出循环，不再遍历同一层的其他边，直到某一方向的最深处
					}
				}
			}
		}

		//拓扑排序（非直接移除的Kahn算法）
		public override List<V> TopologicalSort()
		{
			List<V> list = new();//排序后返回的列表
			Queue<Vertex<V, E>> queue = new();//用来装载入度为0的顶点
			Dictionary<Vertex<V, E>, int> indegrees = new();//用来记录每一个顶点的入度，达到-1模拟移除
			int indegree;
			//先遍历所有顶点，划分度为0和不为0的顶点
			foreach (var item in vertices)
			{
				indegree = item.Value.inEdges.Count;
				if (indegree == 0)//发现一个节顶点入度为0就入队
				{
					queue.Enqueue(item.Value);
				}
				else//入度不为0的节点则将其入度记录（在最开始时度为0的顶点没必要记录直接入队即可）
				{
					indegrees.Add(item.Value, indegree);
				}
			}
			//不断从队列取出度为0的顶点
			while (queue.Count != 0)
			{
				Vertex<V, E> vertex = queue.Dequeue();
				list.Add(vertex.value);//度为0的直接加入list			
				foreach (var item in vertex.outEdges)//遍历度为0的顶点的出边，找到出边终点，将它们的入度-1（模拟Remove）
				{
					if ((--indegrees[item.to]) == 0)//发现-1后为0的顶点则入队
					{
						queue.Enqueue(item.to);
					}
				}
			}
			return list;
		}

		//边权值比较器
		protected int EdgesCompare(Edge<V, E> e1, Edge<V, E> e2)
		{
			if (weightComparer != null)
			{
				return weightComparer(e1.weight, e2.weight);
			}
			else
			{
				return ((IComparable)e1).CompareTo((IComparable)e2);
			}
		}

		//最短路径
		//Dijkstra算法
		public override Dictionary<V, PathInfo<V, E>> ShortestPath_Dijkstra(V origin, Func<E, E, E> add)
		{
			if (!vertices.ContainsKey(origin))
			{
				return null;
			}
			Vertex<V, E> vertex = vertices[origin];
			weightAdd = add;
			Dictionary<V, PathInfo<V, E>> selectedPaths = new();//用于标记确定为最短路径的顶点
			Dictionary<Vertex<V, E>, PathInfo<V, E>> paths = new();//还未计算出的最短路径的顶点
			paths[vertex] = new PathInfo<V, E>(default);//初始化源点，即让源点指向自己的路径权值设置为默认值default

			while (paths.Count != 0)
			{
				KeyValuePair<Vertex<V, E>, PathInfo<V, E>> minEntry = GetMinEntry(paths);//选出目标顶点中weight最小的
				selectedPaths.Add(minEntry.Key.value, minEntry.Value);//加入找到的顶点与最短路径值
				paths.Remove(minEntry.Key);//代表离开桌面

				//遍历该顶点的出边，对这些边进行松弛操作
				foreach (var item in minEntry.Key.outEdges)
				{
					if (item.to == vertex || selectedPaths.ContainsKey(item.to.value))
					{
						continue;//考虑到无向图情况，要略过源点；或是略过已经完成松弛操作的顶点避免重复松弛
					}
					Relax(paths, minEntry.Value, item);//对每一条边进行松弛操作
				}
			}
			selectedPaths.Remove(origin);//因为还存在源点指向源点的路径信息
			return selectedPaths;
		}

		//获得当前情况下的路径最短的点与路径长度
		protected KeyValuePair<Vertex<V, E>, PathInfo<V, E>> GetMinEntry(Dictionary<Vertex<V, E>, PathInfo<V, E>> paths)
		{
			KeyValuePair<Vertex<V, E>, PathInfo<V, E>> minEntry = default;
			Vertex<V, E> minVertex = null;
			E minWeight = default;
			foreach (var item in paths)
			{
				minVertex = item.Key;
				minWeight = item.Value.Weight;
				break;
			}
			foreach (var item in paths)
			{
				if (weightComparer(item.Value.Weight, minWeight) <= 0)
				{
					minVertex = item.Key;
					minWeight = item.Value.Weight;
					minEntry = item;//更新最短路径
				}
			}
			return minEntry;
		}

		//对该顶点的出边进行松弛操作
		//以前的最短路径：源点经过xxx到终点的路径
		//新可选择的最短路径：源点经过xxx到新顶点的路径 + 新顶点到终点的路径
		private void Relax(Dictionary<Vertex<V, E>, PathInfo<V, E>> paths, PathInfo<V, E> minPath, Edge<V, E> edge)
		{
			//新路径权值：当前顶点的出边权值与其本身路径长度相加
			E newWeight = weightAdd(minPath.Weight, edge.weight);
			if (paths.ContainsKey(edge.to) && weightComparer(paths[edge.to].Weight, newWeight) <= 0)
			{
				return;//原先已经存在到该顶点的路径，且计算出来的新路径比旧路径长则无须松弛
			}
			PathInfo<V, E> oldPath;
			if (!paths.ContainsKey(edge.to))
			{
				oldPath = new(edge.weight);//不存在到该顶点的路径时新建
				paths[edge.to] = oldPath;//并添加进paths
			}
			oldPath = paths[edge.to];//找到旧路径，更新所有信息即可
			oldPath.EdgeInfos.Clear();//需要将旧的路径信息清空
			oldPath.Weight = newWeight;//更新为最新路径大小
			oldPath.AddPaths(minPath.EdgeInfos);//添加minEntry已经确定好的最短路径信息
			oldPath.AddPath(edge.ConvertToInfo());//再在此路径基础上加上新边的信息即得到新路径信息
		}

		//Key为V的重写，用于Bellman-Ford
		private bool Relax(Dictionary<V, PathInfo<V, E>> paths, PathInfo<V, E> minPath, Edge<V, E> edge)
		{
			//新路径权值：当前顶点的出边权值与其本身路径长度相加
			E newWeight = weightAdd(minPath.Weight, edge.weight);
			if (paths.ContainsKey(edge.to.value) && weightComparer(paths[edge.to.value].Weight, newWeight) <= 0)
			{
				return false;//原先已经存在到该顶点的路径，且计算出来的新路径比旧路径长则无须松弛
			}
			PathInfo<V, E> oldPath;
			if (!paths.ContainsKey(edge.to.value))
			{
				oldPath = new(edge.weight);//不存在到该顶点的路径时新建
				paths[edge.to.value] = oldPath;//并添加进paths
			}
			oldPath = paths[edge.to.value];//找到旧路径，更新所有信息即可
			oldPath.EdgeInfos.Clear();//需要将旧的路径信息清空
			oldPath.Weight = newWeight;//更新为最新路径大小
			oldPath.AddPaths(minPath.EdgeInfos);//添加minEntry已经确定好的最短路径信息
			oldPath.AddPath(edge.ConvertToInfo());//再在此路径基础上加上新边的信息即得到新路径信息
			return true;
		}

		//Bellman-Ford算法
		public override Dictionary<V, PathInfo<V, E>> ShortestPath_BellmanFord(V origin, Func<E, E, E> add)
		{
			if (!vertices.ContainsKey(origin))
			{
				return null;
			}
			weightAdd = add;
			Dictionary<V, PathInfo<V, E>> selectedPaths = new();//用于标记松弛完成的路径
			selectedPaths[origin] = new PathInfo<V, E>(default);//初始化源点，即让源点指向自己的路径权值设置为默认值default

			//每条边至少要进行V-1次松弛
			for (int i = 0; i < vertices.Count - 1; i++)
			{
				foreach (var item in edges)
				{
					if (!selectedPaths.ContainsKey(item.from.value))
					{
						continue;//略过还未算过路径的边
					}
					Relax(selectedPaths, selectedPaths[item.from.value], item);
				}
			}
			//只要超过V-1次松弛操作后还能继续找到更短的路径，即存在负权环		
			foreach (var item in edges)
			{
				if (!selectedPaths.ContainsKey(item.from.value))
				{
					continue;//略过还未算过路径的边
				}
				if (Relax(selectedPaths, selectedPaths[item.from.value], item))
				{
					Console.WriteLine("存在负权环");
					break;
				}
			}
			selectedPaths.Remove(origin);//因为还存在源点指向源点的路径信息
			return selectedPaths;
		}

		//Floyd算法
		public override Dictionary<V, Dictionary<V, PathInfo<V, E>>> ShortestPath_Floyd(Func<E, E, E> add)
		{
			weightAdd = add;
			Dictionary<V, Dictionary<V, PathInfo<V, E>>> paths = new();
			//初始化，将每一条边的信息添加到paths
			foreach (var edge in edges)
			{
				if (!paths.ContainsKey(edge.from.value))
				{
					paths[edge.from.value] = new Dictionary<V, PathInfo<V, E>>();
				}
				paths[edge.from.value][edge.to.value] = new PathInfo<V, E>(edge.weight);
				paths[edge.from.value][edge.to.value].EdgeInfos.AddLast(edge.ConvertToInfo());
			}
			//让每一个顶点k都作为i和j进行计算
			foreach (var vertex2 in vertices)
			{
				foreach (var vertex1 in vertices)
				{
					foreach (var vertex3 in vertices)
					{
						if (vertex1.Equals(vertex2) || vertex2.Equals(vertex3) || vertex1.Equals(vertex3))
						{
							continue;//没必要计算遍历到是相同的点
						}
						PathInfo<V, E> path12 = GetPathInfo(vertex1.Key, vertex2.Key, paths);//v1 -> v2		
						if (path12 == null)
						{
							continue;//不存在path12则无须计算了
						}
						PathInfo<V, E> path23 = GetPathInfo(vertex2.Key, vertex3.Key, paths);//v2 -> v3						
						if (path23 == null)
						{
							continue;//同理上
						}
						PathInfo<V, E> path13 = GetPathInfo(vertex1.Key, vertex3.Key, paths);//v1 -> v3
						E newWeight = weightAdd(path12.Weight, path23.Weight);//新路径
																			  //如果存在path13且相加够得到新路径比原先路径长则略过本次循环
						if (path13 != null && weightComparer(newWeight, path13.Weight) >= 0)
						{
							continue;
						}
						//如果不存在path13或相加够得到新路径比原先路径短则更新最短路径
						if (path13 == null)
						{
							path13 = new();//不存在到该顶点的路径时新建
							paths[vertex1.Key][vertex3.Key] = path13;//并添加进paths
						}
						else
						{
							path13.EdgeInfos.Clear();//需要将旧的路径信息清空
						}
						path13.Weight = newWeight;//更新新路径信息，拼接path12、path23为新的path13
						foreach (var item in path12.EdgeInfos)
						{
							path13.EdgeInfos.AddLast(item);
						}
						foreach (var item in path23.EdgeInfos)
						{
							path13.EdgeInfos.AddLast(item);
						}
					}
				}
			}
			return paths;
		}

		//封装获取路径信息方法
		private static PathInfo<V, E> GetPathInfo(V from, V to, Dictionary<V, Dictionary<V, PathInfo<V, E>>> paths)
		{
			PathInfo<V, E> info = null;
			if (paths.ContainsKey(from))
			{
				if (paths[from].ContainsKey(to))
				{
					info = paths[from][to];
				}
			}
			return info;
		}
	}
}