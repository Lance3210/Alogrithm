using DataStructure.Util;
using DataStructure.集合与映射;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DataStructure.哈希表 {
	//哈希表
	class HashMap<K, V> : Map<K, V> {
		//底层数组用于存放每一个红黑树根节点
		HashNode<K, V>[] table;

		//默认16个，2^n
		private const int CAPACITY = 1 << 4;
		//装填因子
		private const float LOAD_FACTOR = 0.75f;

		//默认构造
		public HashMap() {
			table = new HashNode<K, V>[CAPACITY];
		}

		#region 辅助函数
		//节点染色
		private HashNode<K, V> Dye(HashNode<K, V> node, RBNodeColor color) {
			if (node == null) {
				return node;
			}
			node.color = color;
			return node;
		}
		//染红
		private HashNode<K, V> DyeRed(HashNode<K, V> node) {
			return Dye(node, RBNodeColor.RED);
		}
		//染黑
		private HashNode<K, V> DyeBlack(HashNode<K, V> node) {
			return Dye(node, RBNodeColor.BLACK);
		}
		//颜色判断
		private RBNodeColor GetColor(HashNode<K, V> node) {
			return node == null ? RBNodeColor.BLACK : node.color;
		}
		//是否为黑色
		private bool IsBlack(HashNode<K, V> node) {
			return GetColor(node) == RBNodeColor.BLACK;
		}
		//是否为红色
		private bool IsRed(HashNode<K, V> node) {
			return GetColor(node) == RBNodeColor.RED;
		}
		//左旋转
		private void LeftRotate(HashNode<K, V> grand) {
			HashNode<K, V> p = grand.right;
			HashNode<K, V> pLeft = p.left;
			grand.right = pLeft;
			p.left = grand;

			AfterRotate(grand, p, pLeft);
		}
		//右旋转
		private void RightRotate(HashNode<K, V> grand) {
			HashNode<K, V> p = grand.left;
			HashNode<K, V> pRight = p.right;
			grand.left = pRight;
			p.right = grand;

			AfterRotate(grand, p, pRight);
		}
		//旋转后处理
		private void AfterRotate(HashNode<K, V> grand, HashNode<K, V> p, HashNode<K, V> child) {
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
				table[Index(grand)] = p;//直接获取根节点
			}
			//更新pLeft
			if (child != null) {
				child.parent = grand;
			}
			//更新grand
			grand.parent = p;
		}
		//获得当前节点的前驱结点
		public static HashNode<K, V> Predecessor(HashNode<K, V> node) {
			if (node == null) {
				return null;
			}
			//左子树不为空，则前驱节点一定在左子树
			if (node.left != null) {
				HashNode<K, V> p = node.left;
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

		//哈希表清空
		public override void Clear() {
			if (size == 0) {
				return;
			}
			size = 0;
			for (int i = 0; i < table.Length; i++) {
				table[i] = null;
			}
		}

		//根据Key计算索引
		private int Index(K key) {
			//Key为空默认索引0
			if (key == null) {
				return 0;
			}
			//取模
			return key.GetHashCode() & (table.Length - 1);
		}

		//根据节点计算索引
		private int Index(HashNode<K, V> node) {
			return node.HashCode & (table.Length - 1);
		}

		//利用key的地址进行比较
		private static int KeyCompareBySites(K key1, K key2) {
			GCHandle handle = GCHandle.Alloc(key1);
			IntPtr site1 = GCHandle.ToIntPtr(handle);
			handle = GCHandle.Alloc(key2);
			IntPtr site2 = GCHandle.ToIntPtr(handle);
			long final = site1.ToInt64() - site2.ToInt64();
			if (final > 0) {
				return 1;
			}
			else if (final < 0) {
				return -1;
			}
			else {
				return 0;
			}
		}

		//扩容
		private void ReSize() {
			//检查装填因子
			if ((float)size / table.Length <= LOAD_FACTOR) {
				return;
			}
			//否则变为原来的两倍
			HashNode<K, V>[] oldTable = table;
			table = new HashNode<K, V>[oldTable.Length << 1];

			//遍历所有节点
			Queue<HashNode<K, V>> queue = new Queue<HashNode<K, V>>();
			HashNode<K, V> node;
			for (int i = 0; i < oldTable.Length; i++) {
				if (oldTable[i] == null) {
					continue;
				}
				queue.Enqueue(oldTable[i]);
				while (queue.Count != 0) {
					node = queue.Dequeue();
					if (node.left != null) {
						queue.Enqueue(node.left);
					}
					if (node.right != null) {
						queue.Enqueue(node.right);
					}
					//移动原来节点
					MoveNode(node);
				}
			}
		}

		//节点移动
		private void MoveNode(HashNode<K, V> newNode) {
			//重置
			newNode.parent = null;
			newNode.left = null;
			newNode.right = null;
			newNode.color = RBNodeColor.RED;
			//重新计算插入位置
			int index = Index(newNode);
			HashNode<K, V> root = table[index];
			//节点为null，直接插入
			if (root == null) {
				root = newNode;
				table[index] = root;
				AfterPut(root);
				return;
			}

			//哈希冲突
			HashNode<K, V> node = root;
			HashNode<K, V> parentNode;
			int result;
			K key1 = newNode.key;//方便理解
			K key2;
			int hashCode1 = key1 == null ? 0 : key1.GetHashCode();
			int hashCode2;
			do {
				parentNode = node;
				key2 = node.key;
				hashCode2 = node.HashCode;
				//不用减法直接比较，这样不会溢出
				if (hashCode1 > hashCode2) {
					result = 1;
				}
				else if (hashCode1 < hashCode2) {
					result = -1;
				}//哈希值相等，且key1和key2不为null，具备可比较性（结果不为0，比较相等不意味着要覆盖）
				else if (key1 != null && key2 != null
					&& key1.GetType() == key2.GetType()
					&& key1 is IComparable comparable
					&& (result = comparable.CompareTo(key2)) != 0) {
				}//哈希值相等，不具备可比较性
				else {
					result = KeyCompareBySites(key1, key2);//不需要扫描，直接决定左右	
				}

				//根据上面的结果遍历左右子树
				if (result > 0) {
					node = node.right;
				}
				else if (result < 0) {
					node = node.left;
				}
			} while (node != null);

			if (result > 0) {
				parentNode.right = newNode;
			}
			else {
				parentNode.left = newNode;
			}
			newNode.parent = parentNode;
			AfterPut(newNode);//返回新插入节点做后面的操作
		}

		//和二叉树一样的节点创建
		protected virtual HashNode<K, V> CreateNode(K key, V value, HashNode<K, V> parent) {
			return new HashNode<K, V>(key, value, parent);
		}

		//添加
		public override V Put(K key, V value) {
			//扩容判断
			ReSize();

			//计算Key索引
			int index = Index(key);
			//取出index位置的节点的Root
			HashNode<K, V> root = table[index];

			//如果该位置为空则插入节点作为根节点
			if (root == null) {
				root = CreateNode(key, value, null);
				table[index] = root;
				++size;
				//修复红黑树
				AfterPut(root);
				return root.value;
			}

			//哈希冲突
			HashNode<K, V> parentNode;
			HashNode<K, V> final;//记录遍历结果
			int result;
			K key2;
			int hashCode1 = key == null ? 0 : key.GetHashCode();
			int hashCode2;
			bool isSearched = false;//是否搜索过
			do {
				parentNode = root;
				//不用KeyCompare
				key2 = root.key;
				hashCode2 = root.HashCode;
				//不用减法直接比较，这样不会溢出
				if (hashCode1 > hashCode2) {
					result = 1;
				}
				else if (hashCode1 < hashCode2) {
					result = -1;
				}//哈希值相等，符合Equals
				else if (Equals(key, key2)) {
					result = 0;
				}//key1和key2不为null，哈希值相等，不Equals，且具备可比较性（结果不为0，比较相等不意味着要覆盖）
				else if (key != null && key2 != null
					&& key.GetType() == key2.GetType()
					&& key is IComparable comparable
					&& (result = comparable.CompareTo(key2)) != 0) {

				}//哈希值相等，不Equals，不具备可比较性，直接比地址添加可能会造成存在2个同样的元素（原本一边已经有一个）
				else if (isSearched)//需要先进行扫描，前提是没被扫描过
				{
					result = KeyCompareBySites(key, key2);//由于已经扫描过这一边，直接决定左右
				}
				else//isSearched == false
				{
					if ((root.right != null && (final = GetNode(root.right, key)) != null)
						|| (root.left != null && (final = GetNode(root.left, key)) != null)) {
						//已经存在这个Key，要覆盖这个final
						result = 0;
						root = final;
					}
					else//不存在该Key，只能通过地址比较了
					{
						isSearched = true;//标记已经扫描
						result = KeyCompareBySites(key, key2);
					}
				}

				//根据上面的结果遍历左右子树
				if (result > 0) {
					root = root.right;
				}
				else if (result < 0) {
					root = root.left;
				}
				else//覆盖
				{
					root.key = key;
					V oldV = root.value;
					root.value = value;
					root.HashCode = hashCode1;//可以不覆盖，因为来到这里哈希值就是相等的
					return oldV;
				}
			} while (root != null);

			//插入
			HashNode<K, V> newNode = CreateNode(key, value, parentNode);
			if (result > 0) {
				parentNode.right = newNode;
			}
			else {
				parentNode.left = newNode;
			}
			//++
			++size;
			AfterPut(newNode);//新插入节点做后面的操作
			return newNode.value;
		}

		#region 添加后处理
		private void AfterPut(HashNode<K, V> node) {
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
				HashNode<K, V> grand = node.Grand;//必须先记录原先的Grand
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

		//删除
		public override V Remove(K key) {
			return Remove(table[Index(key)]);
		}
		//移除找到的node
		protected virtual V Remove(HashNode<K, V> node) {
			if (node == null) {
				return default;
			}
			//本来认为要删除的节点
			HashNode<K, V> originNode = node;
			--size;//记得规模减少（这里先减少，是因为已经Find到了）
			V oldV = node.value;//返回被删除的节点Value

			if (node.HasTwoChildren)//先删除度为2的节点
			{
				HashNode<K, V> pre = Predecessor(node);
				node.key = pre.key;//拿前驱节点的值覆盖删除节点
				node.value = pre.value;
				node.HashCode = pre.HashCode;//记得hashCode也要覆盖
				node = pre;//将删除的对象转移成前驱节点，交给后面来完成
			}

			//能来到下面就与一定是度为0或1的节点
			//度为1的节点
			if (node.HasOnlyOneChild) {
				HashNode<K, V> replacement = node.left ?? node.right;//找到作为代替的节点是左还是右
				replacement.parent = node.parent;//连接父节点（这里会影响下面的AfterRemove）
				if (node.IsRoot)//要注意这里还要判断这个节点是不是根节点，不然后面点parent会报null
				{
					table[Index(node)] = replacement;
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
				table[Index(node)] = null;
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
			//用于子类进一步修复
			FixedRemove(originNode, node);
			return oldV;
		}

		#region 删除后调整
		private void AfterRemove(HashNode<K, V> node) {
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
			HashNode<K, V> sibling = flag ? node.parent.right : node.parent.left;

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

		//用于进一步修复，一个是本来要删除的节点，一个是实际删除的节点
		protected virtual void FixedRemove(HashNode<K, V> originNode, HashNode<K, V> node) {

		}

		#endregion

		//获取节点
		private HashNode<K, V> GetNode(K key) {
			HashNode<K, V> root = table[Index(key)];
			return root == null ? null : GetNode(root, key);
		}
		//递归获取节点
		private HashNode<K, V> GetNode(HashNode<K, V> node, K key1) {
			int hashCode1 = key1 == null ? 0 : key1.GetHashCode();
			HashNode<K, V> final;//用于记录遍历结果
			int flag;//用于标记比较结果
			while (node != null) {
				//不用KeyCompare
				K key2 = node.key;
				int hashCode2 = node.HashCode;
				//不用减法直接比较
				if (hashCode1 > hashCode2) {
					node = node.right;
				}
				else if (hashCode1 < hashCode2) {
					node = node.left;
				}//哈希值相等，符合Equals
				else if (Equals(key1, key2)) {
					return node;
				}//哈希值相等，不符合Equals，且key1和key2不为null，具备可比较性
				else if (key1 != null && key2 != null
					&& key1.GetType() == key2.GetType()
					&& key1 is IComparable comparable) {
					flag = comparable.CompareTo(key2);
					if (flag > 0) {
						node = node.right;
					}
					else if (flag < 0) {
						node = node.left;
					}
					else {
						return node;
					}
				}//哈希值相等，不Equals，不具备可比较性，则递归扫描左右子树
				else if (node.right != null && (final = GetNode(node.right, key1)) != null)//右子树不为空，且结果不为空则继续递归
				{
					return final;
				}
				else//递归左子树
				{
					node = node.left;
				}
			}
			return null;
		}

		//根据Key获取Value
		public override V GetValue(K key) {
			HashNode<K, V> node = GetNode(key);
			return node != null ? node.value : default;
		}

		//是否包含Key
		public override bool ContainsKey(K key) {
			return GetNode(key) != null;
		}

		//是否包含Value
		public override bool ContainsValue(V value) {
			bool flag = false;
			Traversal((k, v) => {
				if (Equals(value, v)) {
					flag = true;
					return true;
				}
				return false;
			});
			return flag;
		}

		#region 遍历
		public override void Traversal(Func<K, V, bool> func) {
			base.Traversal(func);
			Visitor_KeyValuePair<K, V> visitor = new Visitor_KeyValuePair<K, V>() { mapNodeLogic = func };
			for (int i = 0; i < table.Length; i++) {
				if (table[i] == null) {
					continue;
				}
				else {
					InorderTraversal(visitor, table[i]);//每一个节点都中序遍历
				}
			}
		}

		public void InorderTraversal(Visitor_KeyValuePair<K, V> visitor, HashNode<K, V> root) {
			Stack<HashNode<K, V>> stack = new Stack<HashNode<K, V>>();//用栈来回访上一层节点
			HashNode<K, V> node = root;
			while (node != null || stack.Count != 0)//遍历完所有节点的条件
			{
				if (node != null) {
					stack.Push(node);//每遍历一次就入栈
					node = node.left;//（左 中 右）故先左边进入
				}
				else {
					node = stack.Pop();
					//中序的逻辑处理与要等左边全部遍历完时
					if (visitor.mapNodeLogic(node.key, node.value)) {
						return;
					}
					node = node.right;//返回上一层节点，取其右节点
				}
			}
		}
		#endregion
	}
}