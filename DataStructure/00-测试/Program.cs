using DataStructure.布隆过滤器;
using DataStructure.跳表;
using System;
namespace DataStructure.测试
{
	class Program
	{
		static void Main(string[] args)
		{
			SkipListTest();
		}

		//跳表
		private static void SkipListTest()
		{
			int count = 30;
			SkipList<int, string> skipList = new SkipList<int, string>();
			for (int i = 0; i < count; i++)
			{
				skipList.Put(i, i.ToString());
			}
			Console.WriteLine(skipList.ToString());
			Console.WriteLine($"find: {29} result: {skipList.IsContains(29)}");
			Console.WriteLine($"find: {40} result: {skipList.IsContains(40)}");
			Console.WriteLine($"remove: {29} result: {skipList.Remove(29)}");
			Console.WriteLine($"remove: {40} result: {skipList.Remove(40)}");
			Console.WriteLine(skipList.ToString());
		}

		//爬虫应用
		private static void BloomFilterTest2()
		{
			int count = 1000000;
			string[] urls = new string[count];//假设有大量url
			BloomFilter<string> bloomFilter = new(count, 0.01);
			//对这些urls进行爬虫
			foreach (var url in urls)
			{
				//如果Put返回true表示已经爬过，返回false则有可能没有爬过
				//实现了无论有没有爬过都将其Put进去，就无须再IsContains了
				if (bloomFilter.Put(url))
				{
					continue;
				}
				//返回true则爬这个url......
			}
		}

		private static void BloomFilterTest1()
		{
			int count = 1000000;
			BloomFilter<int> bloomFilter = new(count, 0.01);
			for (int i = 0; i < count; i++)
			{
				bloomFilter.Put(i);
			}
			int miss = 0;
			for (int i = count + 1; i < 2 * count; i++)
			{
				if (bloomFilter.IsContains(i))
				{
					++miss;
				}
			}
			Console.WriteLine(miss);
		}
	}
}