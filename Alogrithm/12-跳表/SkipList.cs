using System;
using System.Text;

namespace DataStructure.跳表 {
	class SkipList<K, V> {
		private int size;
		public int Size => size;
		public bool IsEmpty => size == 0;
		private Func<K, K, int> comparer;
		private int currentMaxLevel = 0;//当前有效层数
		private static int MAX_LEVEL = 32;//最大层数
		private static double p = 0.25f;//用于生成随机层数
		private SkipListNode<K, V> first;//不存放任何东西的头结点

		//初始化，first即为最大层
		public SkipList() {
			first = new SkipListNode<K, V>(new SkipListNode<K, V>[MAX_LEVEL]); ;
		}
		public SkipList(Func<K, K, int> comparer) : base() {
			this.comparer = comparer;
		}

		//Key比较器
		private int KeyComparer(K key1, K key2) {
			return comparer != null ? comparer(key1, key2) : ((IComparable)key1).CompareTo(key2);
		}

		//Null检测
		private void NullCheck(K key) {
			if (key == null) {
				throw new ArgumentException("Key must not be null");
			}
		}

		//随机生成层数
		private int RandomLevel() {
			int level = 1;
			Random random = new Random();
			while (random.NextDouble() < p && level < MAX_LEVEL) {
				++level;//类似抛硬币......
			}
			return level;
		}

		//搜索
		public V Get(K key) {
			NullCheck(key);
			SkipListNode<K, V> node = first;
			//从最上层开始找，每一层都一路往右，直到不满足则到下一层
			int result = 0;//比较结果
			for (int i = currentMaxLevel - 1; i >= 0; --i) {
				while (node.nexts[i] != null && (result = KeyComparer(key, node.nexts[i].key)) > 0) {
					node = node.nexts[i];//key比当前节点还大就继续往右找
				}
				if (result == 0) {
					return node.value;//每一次回退时做一次比较
				}
			}
			return default;
		}

		//查询
		public bool IsContains(K key) {
			return Get(key) != null;
		}

		//添加
		public V Put(K key, V value) {
			NullCheck(key);
			SkipListNode<K, V> node = first;
			SkipListNode<K, V>[] prevs = new SkipListNode<K, V>[currentMaxLevel];//用于保存遍历到的前驱节点		
			int result = 0;//先找到元素的位置，顺便查找是否有相同的
			for (int i = currentMaxLevel - 1; i >= 0; --i) {
				while (node.nexts[i] != null && (result = KeyComparer(key, node.nexts[i].key)) > 0) {
					node = node.nexts[i];
				}
				if (result == 0) {
					V oldV = node.nexts[i].value;
					node.nexts[i].value = value;
					return oldV;//返回旧值
				}
				prevs[i] = node;//记录前驱节点（是可以重复的）
			}
			//添加新节点
			int newLevel = RandomLevel();//随机生成层数
			SkipListNode<K, V> newNode = new(key, value, newLevel);
			for (int i = 0; i < newLevel; ++i) {
				if (i >= currentMaxLevel) {
					first.nexts[i] = newNode;//考虑到newLevel比currentMaxLevel还高
				}
				else {
					newNode.nexts[i] = prevs[i].nexts[i];//改变前驱指向
					prevs[i].nexts[i] = newNode;
				}
			}
			++size;//数据规模增加
			currentMaxLevel = Math.Max(currentMaxLevel, newLevel);//更新当前最大层
			return default;//无旧值
		}

		//删除
		public V Remove(K key) {
			NullCheck(key);
			SkipListNode<K, V> node = first;
			SkipListNode<K, V>[] prevs = new SkipListNode<K, V>[currentMaxLevel];//用于保存遍历到的前驱节点		
			int result = 0;//先找到元素的位置
			for (int i = currentMaxLevel - 1; i >= 0; --i) {
				while (node.nexts[i] != null && (result = KeyComparer(key, node.nexts[i].key)) > 0) {
					node = node.nexts[i];
				}
				prevs[i] = node;//记录前驱节点（是可以重复的）
			}
			//没找到
			if (result != 0) {
				return default;//无该值
			}
			//若有找到则删除节点
			SkipListNode<K, V> removedNode = node.nexts[0];//要移除的元素（至少有1层）
			for (int i = 0; i < removedNode.nexts.Length; ++i) {
				prevs[i].nexts[i] = removedNode.nexts[i];//改变前驱指向
			}
			--size;//数据规模减少
			int newLevel = currentMaxLevel;
			while (--newLevel >= 0 && first.nexts[newLevel] == null) {
				currentMaxLevel = newLevel;//更新当前最大层
			}
			return removedNode.value;
		}

		//按层打印
		public override string ToString() {
			StringBuilder sb = new();
			sb.Append("----------------------------------------\n");
			sb.Append("一共" + currentMaxLevel + "层\n");
			for (int i = currentMaxLevel - 1; i >= 0; --i) {
				SkipListNode<K, V> node = first;
				while (node.nexts[i] != null) {
					sb.Append(node.nexts[i].key + " ");
					node = node.nexts[i];
				}
				sb.Append('\n');
			}
			sb.Append("----------------------------------------\n");
			return sb.ToString();
		}
	}
}
