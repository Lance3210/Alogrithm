using DataStructure.Util;
using System;
using System.Collections.Generic;

namespace DataStructure.集合与映射.红黑树映射 {
	//红黑树实现的映射
	class TreeMap<K, V> : Map<K, V> {
		//根节点
		MapNode<K, V> root;
		public MapNode<K, V> Root => root;

		#region 各种构造函数
		//默认构造
		public TreeMap() {

		}

		//Key的比较方法
		private Func<K, K, int> keyComparetor;
		//Value的比较方法
		private Func<V, V, int> valueComparetor;

		//需要传入Key比较方法的构造函数
		public TreeMap(Func<K, K, int> keyComparetor) {
			this.keyComparetor = keyComparetor;
		}

		//需要传入Value比较方法的构造函数
		public TreeMap(Func<V, V, int> valueComparetor) {
			this.valueComparetor = valueComparetor;
		}

		//需要传入比较方法的构造函数
		public TreeMap(Func<K, K, int> keyComparetor, Func<V, V, int> valueComparetor) {
			this.keyComparetor = keyComparetor;
			this.valueComparetor = valueComparetor;
		}

		//元素Key比较
		private int KeyCompare(K key1, K key2) {
			if (keyComparetor != null) {
				return keyComparetor(key1, key2);
			}
			return ((IComparable<K>)key1).CompareTo(key2);
		}

		//元素Value比较
		private int ValueCompare(V value1, V value2) {
			if (keyComparetor != null) {
				return valueComparetor(value1, value2);
			}
			return ((IComparable<V>)value1).CompareTo(value2);
		}

		#endregion

		#region 辅助函数
		//Key检测
		private void NullCheck(K key) {
			if (key == null) {
				throw new ArgumentException("Key must not be null");
			}
		}
		//节点染色
		private MapNode<K, V> Dye(MapNode<K, V> node, RBNodeColor color) {
			if (node == null) {
				return node;
			}
			node.color = color;
			return node;
		}
		//染红
		private MapNode<K, V> DyeRed(MapNode<K, V> node) {
			return Dye(node, RBNodeColor.RED);
		}
		//染黑
		private MapNode<K, V> DyeBlack(MapNode<K, V> node) {
			return Dye(node, RBNodeColor.BLACK);
		}
		//颜色判断
		private RBNodeColor GetColor(MapNode<K, V> node) {
			return node == null ? RBNodeColor.BLACK : node.color;
		}
		//是否为黑色
		private bool IsBlack(MapNode<K, V> node) {
			return GetColor(node) == RBNodeColor.BLACK;
		}
		//是否为红色
		private bool IsRed(MapNode<K, V> node) {
			return GetColor(node) == RBNodeColor.RED;
		}
		//左旋转
		private void LeftRotate(MapNode<K, V> grand) {
			MapNode<K, V> p = grand.right;
			MapNode<K, V> pLeft = p.left;
			grand.right = pLeft;
			p.left = grand;

			AfterRotate(grand, p, pLeft);
		}
		//右旋转
		private void RightRotate(MapNode<K, V> grand) {
			MapNode<K, V> p = grand.left;
			MapNode<K, V> pRight = p.right;
			grand.left = pRight;
			p.right = grand;

			AfterRotate(grand, p, pRight);
		}
		//旋转后处理
		private void AfterRotate(MapNode<K, V> grand, MapNode<K, V> p, MapNode<K, V> child) {
			//修正p,grand,pLeft的父节点
			//更新p
			p.parent = grand.parent;
			if (grand.IsLeftChild)//判断grand在其父节点的位置
			{
				grand.parent.left = p;
			}
			else if (grand.IsRightChild) {
				grand.parent.right = p;
			}
			else//grand是根节点
			{
				root = p;
			}
			//更新pLeft
			if (child != null) {
				child.parent = grand;
			}
			//更新grand
			grand.parent = p;
		}
		//获得当前节点的前驱结点
		public static MapNode<K, V> Predecessor(MapNode<K, V> node) {
			if (node == null) {
				return null;
			}
			//左子树不为空，则前驱节点一定在左子树
			if (node.left != null) {
				MapNode<K, V> p = node.left;
				while (p.right != null) {
					p = p.right;
				}
				return p;
			}
			//左子树为空，则从父节点向上开始找
			while (node.parent.left == node && node.parent != null) {
				node = node.parent;
			}
			return node.parent;//表示该祖辈节点就是你的前驱
		}
		#endregion

		//清空该映射
		public override void Clear() {
			root = null;
			size = 0;
		}

		//键值对添加
		public override V Put(K key, V value) {
			//利用BST的Add作为添加方法，返回原覆盖Value
			NullCheck(key);
			//根节点
			if (root == null) {
				root = new MapNode<K, V>(key, value, null);
				++size;
				AfterPut(root);//添加根节点也要处理（在红黑树中）
				return default;
			}
			//非根节点
			MapNode<K, V> node = root;
			MapNode<K, V> parentNode = root;
			int result;
			do {
				result = KeyCompare(key, node.key);
				parentNode = node;
				if (result > 0) {
					node = node.right;
				}
				else if (result < 0) {
					node = node.left;
				}
				else {
					node.key = key;//建议覆盖，因为是添加一般都是覆盖，不然添加干什么
					V oldV = node.value;
					node.value = value;
					return oldV;
				}
			} while (node != null);
			//插入
			MapNode<K, V> newNode = new MapNode<K, V>(key, value, parentNode);
			if (result > 0) {
				parentNode.right = newNode;
			}
			else {
				parentNode.left = newNode;
			}
			//++
			++size;
			AfterPut(newNode);//返回新插入节点做后面的操作
			return default;
		}

		#region 添加后处理
		private void AfterPut(MapNode<K, V> node) {
			//parent = null，root染黑
			if (node.parent == null) {
				DyeBlack(node);
				return;
			}
			//parent黑色，无须处理
			if (IsBlack(node.parent)) {
				return;
			}
			//uncle为红色
			if (IsRed(node.Uncle)) {
				//parent uncle 染黑
				DyeBlack(node.parent);
				DyeBlack(node.Uncle);
				//grand染红向上合并当做新添加节点
				AfterPut(DyeRed(node.Grand));
				return;
			}
			//uncle为黑色
			if (IsBlack(node.Uncle)) {
				MapNode<K, V> grand = node.Grand;//必须先记录原先的Grand
				if (node.parent.IsLeftChild) {
					DyeRed(node.Grand);
					if (node.IsLeftChild)//LL
					{
						DyeBlack(node.parent);
					}
					else//LR
					{
						DyeBlack(node);
						LeftRotate(node.parent);
					}
					RightRotate(grand);
				}
				else {
					DyeRed(node.Grand);
					if (node.IsLeftChild)//RL
					{
						DyeBlack(node);
						RightRotate(node.parent);
					}
					else//RR
					{
						DyeBlack(node.parent);
					}
					LeftRotate(grand);
				}
			}
		}
		#endregion

		//获取根据key键值对
		public MapNode<K, V> GetKeyValuePair(K key) {
			MapNode<K, V> node = root;
			int result;
			while (node != null) {
				result = KeyCompare(key, node.key);
				if (result == 0) {
					return node;
				}
				else if (result < 0) {
					node = node.left;
				}
				else {
					node = node.right;
				}
			}
			return null;
		}

		//根据Key获取Value
		public override V GetValue(K key) {
			MapNode<K, V> node = GetKeyValuePair(key);
			return node != null ? node.value : default;
		}

		//根据Key移除Key和Value
		public override V Remove(K key) {
			MapNode<K, V> node = GetKeyValuePair(key);
			return Remove(node);
		}
		//移除找到的node
		private V Remove(MapNode<K, V> node) {
			if (node == null) {
				return default;
			}
			--size;//记得规模减少（这里先减少，是因为已经Find到了）
			V oldV = node.value;//返回被删除的节点Value

			if (node.HasTwoChildren)//先删除度为2的节点
			{
				MapNode<K, V> pre = Predecessor(node);
				node.key = pre.key;//拿前驱节点的值覆盖删除节点
				node.value = pre.value;
				node = pre;//将删除的对象转移成前驱节点，交给后面来完成
			}

			//能来到下面就与一定是度为0或1的节点
			//度为1的节点
			if (node.HasOnlyOneChild) {
				MapNode<K, V> replacement = node.left ?? node.right;//找到作为代替的节点是左还是右
				replacement.parent = node.parent;//连接父节点（这里会影响下面的AfterRemove）
				if (node.IsRoot)//要注意这里还要判断这个节点是不是根节点，不然后面点parent会报null
				{
					root = replacement;
				}
				else if (node.parent.left == node)//与下面叶子的逻辑类似
				{
					node.parent.left = replacement;
				}
				else {
					node.parent.right = replacement;
				}
				//因为度为2的节点是覆盖且需要等真正删除后这里才好处理（如AVL中的恢复平衡）
				//传入代替节点方便红黑树处理，而传入AVL树中则无影响（AVL树只拿parent处理高度，而node和replacement的parent一致）
				AfterRemove(replacement);
			}
			else if (node.parent == null)//说明删除的是根节点
			{
				root = null;
				AfterRemove(node);
			}
			else//删除的是叶子
			{
				if (node.parent.left == node) {
					node.parent.left = null;
				}
				else {
					node.parent.right = null;
				}
				AfterRemove(node);
			}
			return oldV;
		}

		#region 删除后调整
		private void AfterRemove(MapNode<K, V> node) {
			//删除节点为红色或用于取代删除节点的节点（replacement）为红色（即合并了最终删除的是RED的情况）
			if (IsRed(node)) {
				DyeBlack(node);//虽然其中删除RED节点的情况不需要染黑，但也无妨，可以省下传入replacement参数（与AVL树统一函数格式）
				return;
			}
			//删除的是根节点
			if (node.parent == null) {
				return;
			}

			//删除节点为黑色叶子节点，产生下溢
			//注意：如果是删除叶子节点，那AfterRemove的node已经是删除的node，但node的parent的left或right已经不指向node了
			//故可以对sibling的位置进行反推判断，node的parent的left为null即删除节点的sibling为右，另一边亦然
			//但是，如果是向下合并产生的递归，传入的node的parent的左右子树不一定为null，故要进行多一步判断
			bool flag = node.parent.left == null || node.IsLeftChild;
			MapNode<K, V> sibling = flag ? node.parent.right : node.parent.left;

			//被删除节点在左边，sibling在右边	
			if (flag) {
				//删除节点的sibling为Red
				if (IsRed(sibling)) {
					DyeBlack(sibling);
					DyeRed(node.parent);
					LeftRotate(node.parent);
					sibling = node.parent.right;//更新兄弟
				}
				//删除节点的sibling为Black
				if (IsBlack(sibling.left) && IsBlack(sibling.right))//sibling无Red子节点，产生下溢
				{
					//需要提前判断parent的颜色
					bool parentIsBlack = IsBlack(node.parent);
					//parent向下合并
					DyeBlack(node.parent);
					DyeRed(sibling);
					if (parentIsBlack)//parent为黑合并必下溢，则递归
					{
						AfterRemove(node.parent);
					}
				}
				else//sibling至少有一个Red子节点，向sibling“借一个”
				{
					if (IsBlack(sibling.right))//sibling需要进行右旋先的情况
					{
						RightRotate(sibling);
						sibling = node.parent.right;//更新兄弟
					}
					//将sibling染成parent原来的颜色,sibling将来的两个子节点染黑
					Dye(sibling, GetColor(node.parent));
					DyeBlack(sibling.right);
					DyeBlack(node.parent);
					//然后parent再左旋转
					LeftRotate(node.parent);
				}
			}
			else//被删除节点在右边，sibling在左边，与上面对称
			{
				//删除节点的sibling为Red
				if (IsRed(sibling)) {
					DyeBlack(sibling);
					DyeRed(node.parent);
					RightRotate(node.parent);
					sibling = node.parent.left;//更新兄弟
				}
				//删除节点的sibling为Black
				if (IsBlack(sibling.left) && IsBlack(sibling.right))//sibling无Red子节点，
				{
					//需要提前判断parent的颜色
					bool parentIsBlack = IsBlack(node.parent);
					//parent向下合并
					DyeBlack(node.parent);
					DyeRed(sibling);
					if (parentIsBlack)//parent为黑合并必下溢，则递归
					{
						AfterRemove(node.parent);
					}
				}
				else//sibling至少有一个Red子节点，向sibling“借一个”
				{
					if (IsBlack(sibling.left))//sibling需要进行左旋先的情况
					{
						LeftRotate(sibling);
						sibling = node.parent.left;//更新兄弟
					}
					//将sibling染成parent原来的颜色,sibling将来的两个子节点染黑
					Dye(sibling, GetColor(node.parent));
					DyeBlack(sibling.left);
					DyeBlack(node.parent);
					//然后parent再右旋转
					RightRotate(node.parent);
				}
			}
		}
		#endregion

		//是否包含Key（key找得到Value就相当于包含该Key）
		public override bool ContainsKey(K key) {
			return GetKeyValuePair(key) != null;
		}

		//是否包含Value
		public override bool ContainsValue(V value) {
			//要遍历所有键值对
			if (root == null) {
				return false;
			}
			Queue<MapNode<K, V>> queue = new Queue<MapNode<K, V>>();
			queue.Enqueue(root);
			MapNode<K, V> node;
			while (queue.Count != 0) {
				node = queue.Dequeue();
				if (ValueCompare(node.value, value) == 0) {
					return true;
				}
				if (node.left != null) {
					queue.Enqueue(node.left);
				}
				if (node.right != null) {
					queue.Enqueue(node.right);
				}
			}
			return false;
		}

		//遍历映射
		public override void Traversal(Func<K, V, bool> func) {
			base.Traversal(func);
			//一般利用中序遍历比较合理（按顺序）
			Visitor_KeyValuePair<K, V> visitor = new Visitor_KeyValuePair<K, V>() { mapNodeLogic = func };
			InorderTraversal(visitor);
		}

		//中序遍历
		private void InorderTraversal(Visitor_KeyValuePair<K, V> visitor) {
			if (visitor == null) return;
			Stack<MapNode<K, V>> stack = new Stack<MapNode<K, V>>();//用栈来回访上一层节点
			MapNode<K, V> node = root;
			while (node != null || stack.Count != 0)//遍历完所有节点的条件
			{
				if (node != null) {
					stack.Push(node);//每遍历一次就入栈
					node = node.left;//（左 中 右）故先左边进入
				}
				else {
					node = stack.Pop();
					//中序的逻辑处理与要等左边全部遍历完时
					visitor.mapNodeLogic(node.key, node.value);
					node = node.right;//返回上一层节点，取其右节点
				}
			}
		}
	}
}
