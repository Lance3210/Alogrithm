using DataStructure.哈希表;
using DataStructure.集合与映射;
using DataStructure.集合与映射.哈希集合;
using DataStructure.集合与映射.红黑树映射;
using System;
namespace DataStructure.测试 {
	class Program3 {
		static void Main3(string[] args) {
			//Map<int, int> map = new LinkedHashMap<int, int>();
			//MapTest(map);

			TreeSetTest(new MyLinkedHashSet<int>());
		}

		#region MapTest
		private static void MapTest(Map<int, int> map) {
			DateTime time = DateTime.Now;
			for (int i = 0; i < 20; i++) {
				map.Put(i, i);
			}
			Console.WriteLine("------------------------------");

			map.Traversal((k, v) => {
				Console.WriteLine($"{v}");
				return false;
			});
			Console.WriteLine("------------------------------");

			for (int i = 0; i < 10; i++) {
				map.Remove(i);
			}
			Console.WriteLine("------------------------------");

			Console.WriteLine($"{map.GetType().Name}:  " + (DateTime.Now - time));
		}
		#endregion

		#region HashMapTest
		private static void HashMapTest2() {
			HashMap<string, int> map = new();
			Random random = new Random();
			for (int i = 0; i < 1000; i++) {
				//Person person = new Person(i, i, "test");
				if (i == 999) {
					Console.WriteLine();
				}
				map.Put(string.Format("{0}", i), i);
			}
		}

		private static void HashMapTest1() {
			Person person = new Person(10, 10.1f, "sb");
			Person person2 = new Person(10, 10.1f, "sb");
			Person person3 = new Person(12, 10.1f, "ww");
			string person4 = "ssljhfd";
			Console.WriteLine("p1 HashCode : " + person.GetHashCode());
			Console.WriteLine("p2 HashCode : " + person2.GetHashCode());
			Console.WriteLine("p1 Equals p2 : " + person.Equals(person2));
			Console.WriteLine("p3 HashCode : " + person3.GetHashCode());
			Console.WriteLine("p4 HashCode : " + person4.GetHashCode());
			Console.WriteLine();

			Map<object, int> map = new HashMap<object, int>();
			map.Put(person, 666);
			map.Put(person2, 777);
			map.Put(person3, 888);
			map.Put(person4, 999);
			map.Traversal((k, v) => {
				Console.WriteLine($"key : {k}  value : {v}");
				return false;
			});
			Console.WriteLine();

			Console.WriteLine("Contains Value : " + map.ContainsValue(666));
			//Console.WriteLine("Remove : " + map.Remove(person));
			Console.WriteLine($"Contains Key {person.name} : " + map.ContainsKey(person));
			Console.WriteLine($"Get Value : " + map.GetValue("ssljhfd"));
		}
		#endregion

		#region TreeMapTest
		private static void TreeMapTest() {
			int testKey = 1;
			int testKey2 = 2;
			string testValue = "sb";
			string testValue2 = "qq";
			Map<int, string> map = new TreeMap<int, string>();
			map.Put(1, "sb");
			map.Put(1, "rr");
			map.Put(2, "qq");
			map.Put(3, "ww");
			map.Put(4, "ee");
			map.Traversal((k, v) => {
				Console.Write($"{k}_{v}  ");
				return false;
			});
			Console.WriteLine();
			map.Remove(2);
			map.Traversal((k, v) => {
				Console.Write($"{k}_{v}  ");
				return false;
			});
			Console.WriteLine();
			Console.WriteLine($"Is contains key {testKey}: {map.ContainsKey(testKey)}");
			Console.WriteLine($"Is contains key {testKey2}: {map.ContainsKey(testKey2)}");
			Console.WriteLine($"Is contains value {testValue}: {map.ContainsValue(testValue)}");
			Console.WriteLine($"Is contains value {testValue2}: {map.ContainsValue(testValue2)}");
		}

		#endregion

		#region TreeSetTest
		private static void TreeSetTest(Set<int> set) {
			set.Add(1);
			set.Add(2);
			set.Add(5);
			set.Add(3);
			set.Add(0);
			set.Add(-1);
			set.Add(5);
			set.Traversal((e) => {
				Console.Write($"{e}  ");
				return false;
			});
		}
		#endregion
	}
}