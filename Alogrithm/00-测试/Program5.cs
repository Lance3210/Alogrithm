using DataStructure.并查集;
using DataStructure.并查集.Generic;
using DataStructure.并查集.QuickFind;
using DataStructure.并查集.QuickUnion;
using System;
using System.Collections.Generic;
using System.Linq;
namespace DataStructure.测试
{
	class Program5
	{
		const int count = 10000;
		static void Main0(string[] args)
		{
			//UnionFindTest();
			//UnionFindTest2();

		}

		private static void UnionFindTest2()
		{
			Person p1 = new Person(1, 414, "sb1");
			Person p2 = new Person(2, 123, "sb2");
			Person p3 = new Person(3, 24, "sb3");
			Person p4 = new Person(4, 515, "sb4");
			Person p5 = new Person(5, 515, "sb5");

			UnionFind<Person> unionFind = new UnionFind<Person>(
				p1, p2, p3, p4
			);
			unionFind.CreatNode(p5);
			Console.WriteLine(unionFind.IsSame(p1, p5));
			unionFind.Union(p1, p5);
			Console.WriteLine(unionFind.IsSame(p1, p5));
			unionFind.Union(p4, p3);
			Console.WriteLine(unionFind.IsSame(p3, p4));
			unionFind.Union(p3, p5);
			Console.WriteLine(unionFind.IsSame(p3, p5));
			unionFind.Union(p5, p2);
			Console.WriteLine(unionFind.Find(p2).age);
		}

		private static void UnionFindTest()
		{
			Test(new UnionFind[]{
				new QuickFind(count),
				new QucikUnion_Base(count),
				new QuickUnion_Size(count),
				new QuickUnion_Rank(count),
				new QuickUnion_Rank_PC(count),
				new QuickUnion_Rank_PS(count),
				new QuickUnion_Rank_PH(count)
			});
		}

		//并查集测试
		static void Test(UnionFind[] unionFind)
		{
			TimeTest timeTest = new TimeTest();
			Random random = new Random();
			//依次进行Union
			Console.WriteLine("Union Test");
			for (int i = 0; i < unionFind.Length; i++)
			{
				DateTime unionTime = DateTime.Now;
				for (int j = 0; j < count; j++)
				{
					unionFind[i].Union(random.Next(0, count), random.Next(0, count));
				}
				TimeSpan endUnionTime = DateTime.Now - unionTime;
				timeTest.Add(unionFind[i].GetType(), endUnionTime);
			}
			timeTest.PrintByTime();

			Console.WriteLine();
			Console.WriteLine("Find Test");
			for (int i = 0; i < unionFind.Length; i++)
			{
				DateTime unionTime = DateTime.Now;
				for (int j = 0; j < count; j++)
				{
					unionFind[i].Find(random.Next(0, count));
				}
				TimeSpan endUnionTime = DateTime.Now - unionTime;
				timeTest.Add(unionFind[i].GetType(), endUnionTime);
			}
			timeTest.PrintByTime();
		}

	}

	//时间测试类
	class TimeTest
	{
		Dictionary<string, TimeSpan> dic = new();
		public void Add(Type type, TimeSpan time)
		{
			if (!dic.ContainsKey(type.Name))
			{
				dic.Add(type.Name, time);
			}
			else
			{
				dic[type.Name] = time;
			}
		}

		public void PrintByTime()
		{
			var dicSort = from obj in dic orderby obj.Value select obj;
			foreach (var item in dicSort)
			{
				Console.WriteLine(item.Key + "\t\t" + item.Value.TotalMilliseconds + " ms");
				Console.WriteLine("-----------------------------------------------------------------------------------------");
			}
		}
	}
}
