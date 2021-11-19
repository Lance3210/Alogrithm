using System;

namespace DataStructure.树.字典树 {
	//Trie，前缀树，字典查找树
	class Trie<V> {
		private int size;
		//根节点一般不存放key
		private TrieNode<V> root;

		public int Size => size;
		public bool IsEmpty => size == 0;

		public void Clear() {
			size = 0;
			root = null;
		}

		private static void KeyCheck(string str) {
			if (str == null || str.Length == 0) {
				throw new NullReferenceException("Key is null");
			}
		}

		//根据字符串（key）获取对应的节点
		private TrieNode<V> Find(string str) {
			KeyCheck(str);
			TrieNode<V> node = root;
			for (int i = 0; i < str.Length; i++) {
				if (node == null || node.children == null || node.children.IsEmpty)//如果获取的是空或者无子节点直接退出
				{
					return null;
				}
				node = node.children.GetValue(str[i]);//取出每一个字符，获取对应子节点
			}
			return node;//返回的不一定是单词，有可能只是某个前缀
		}

		//根据字符串（一般要求是一个word）获取对应的值
		public V Get(string word) {
			TrieNode<V> node = Find(word);
			return node != null && node.word ? node.value : default;
		}

		//是否包含该单词
		public bool Contains(string word) {
			TrieNode<V> node = Find(word);
			return node != null && node.word;
		}

		//添加键值对
		public V Add(string word, V value) {
			KeyCheck(word);
			//创建根节点
			if (root == null) {
				root = new(null);
			}
			TrieNode<V> node = root;

			for (int i = 0; i < word.Length; i++) {
				char ch = word[i];//取出每一个字符
				bool isEmptyChildren = node.children == null;//查看该节点的子节点是否为null
				TrieNode<V> childNode = isEmptyChildren ? null : node.children.GetValue(ch);//根据父节点获取子节点
				if (childNode == null)//无论是找不到还是children为null都要创建新节点
				{
					childNode = new(node);//创建新节点
					childNode.ch = ch;//标记对应字符
					node.children = isEmptyChildren ? new 哈希表.HashMap<char, TrieNode<V>>() : node.children;//子节点为null才创建
					node.children.Put(ch, childNode);//插入该新节点
				}
				node = childNode;//下移指针
			}
			//找到了即覆盖
			if (node.word) {
				V oldValue = node.value;
				node.value = value;
				return oldValue;//返回旧value
			}
			//没找到就新增单词			
			node.word = true;
			node.value = value;
			++size;
			return default;
		}

		//移除
		public V Remove(string word) {
			//找到最后一个节点
			TrieNode<V> node = Find(word);
			//如果不是单词结尾则无须处理
			if (node == null || !node.word) {
				return default;
			}
			--size;
			V oldValue = node.value;
			//判断删除节点后面是否还有子节点
			if (node.children != null && !node.children.IsEmpty) {
				//直接标记为非word即可
				node.word = false;
				oldValue = node.value;
				node.value = default;
				return oldValue;
			}
			//无子节点，一个个往回删
			TrieNode<V> parentNode = null;
			while ((parentNode = node.parent) != null) {
				parentNode.children.Remove(node.ch);
				if (parentNode.word || !parentNode.children.IsEmpty)//如果该父节点还有其他子节点或父节点就是word就不用删了
				{
					break;
				}
				node = parentNode;//移动指针
			}
			return oldValue;
		}

		//查找是否有指定前缀
		public bool StartsWith(string prefix) {
			return Find(prefix) != null;//只要不为空就是存在该前缀
		}
	}
}
