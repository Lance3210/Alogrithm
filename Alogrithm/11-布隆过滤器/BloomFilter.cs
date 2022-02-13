using System;

namespace DataStructure.布隆过滤器
{
	class BloomFilter<T>
	{
		private long[] bits;//二进制向量容器
		private int bitSize;//二进制向量长度
		private int hashSize;//哈希函数个数

		/// <summary>
		/// 需要传入数据规模与误判率
		/// </summary>
		/// <param name="n">数据规模</param>
		/// <param name="p">误判率</param>
		public BloomFilter(int n, double p)
		{
			if (n <= 0 || p <= 0 || p >= 1)
			{
				throw new ArgumentException("n or p is wrong");
			}
			double ln2 = Math.Log(2);
			bitSize = (int)(-(n * Math.Log(p)) / (ln2 * ln2));//按公式初始化
			hashSize = (int)(bitSize * ln2 / n);
			bits = new long[(bitSize + 63) / 64];//可以实现向上取整
		}

		private void NullCheck(T value)
		{
			if (value == null)
			{
				throw new ArgumentException("value must not be null");
			}
		}

		//如果该位置已经为1则返回true，不为1返回false
		private bool Get(int index)
		{
			return (bits[index / 64] & (1 << (index % 64))) != 0;//按位与
		}

		//设置对应位置二进制为1
		//如果该位置已经为1则返回true，不为1返回false
		private bool Set(int index)
		{
			long bitValue = 1 << (index % 64);
			bits[index / 64] |= bitValue;//按位或
			return ((bits[index / 64]) & bitValue) != 0;
		}

		//放入元素到二进制向量
		//如果已经放入过则返回true，未被放入过返回false
		public bool Put(T value)
		{
			NullCheck(value);
			bool result = false;
			int hash1 = value.GetHashCode();
			int hash2 = hash1 >> 16;//利用value生成两个哈希值，这里是简化版
			for (int i = 1; i <= hashSize; i++)
			{
				int combinedHash = hash1 + (i * hash2);
				if (combinedHash < 0)
				{
					combinedHash = ~combinedHash;//总之就是要尽可能生成不同的整数值
				}
				result = Set(combinedHash % bitSize);//生成索引后设置其在二进制中的位置为1
			}
			return result;
		}

		/// <summary>
		/// 判断元素是否存在
		/// </summary>
		/// <param name="value">元素</param>
		/// <returns>False为一定不存在，True为有可能不存在</returns>
		public bool IsContains(T value)
		{
			NullCheck(value);
			int hash1 = value.GetHashCode();
			int hash2 = hash1 >> 16;
			for (int i = 1; i <= hashSize; i++)
			{
				int combinedHash = hash1 + (i * hash2);
				if (combinedHash < 0)
				{
					combinedHash = ~combinedHash;
				}
				if (!Get(combinedHash % bitSize))
				{
					return false;//对应位置二进制为0则必不存在
				}
			}
			return true;
		}
	}
}
