using DataStructure.哈希表;

namespace DataStructure.树.字典树
{
	//字典树的节点
	class TrieNode<V>
	{
		public HashMap<char, TrieNode<V>> children;//用哈希表代表子节点
		public TrieNode<V> parent;
		public char ch;//对应的字符
		public V value;
		public bool word;
		public TrieNode(TrieNode<V> parent)
		{
			this.parent = parent;
		}
	}
}
