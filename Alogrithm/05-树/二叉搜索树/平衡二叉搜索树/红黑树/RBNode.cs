using DataStructure.Util;
using DataStructure.树.二叉树;

namespace DataStructure.树.二叉搜索树.平衡二叉搜索树.红黑树 {
	//红黑树节点
	class RBNode<T> : Node<T> {
		public RBNodeColor color = RBNodeColor.RED;//用bool代表颜色，直接默认为红色方便添加
		public RBNode(T element, Node<T> parent) : base(element, parent) {

		}
	}
}
