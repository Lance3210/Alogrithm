namespace Algorithm.排序 {
	//堆排序
	//可以认为是对选择排序的一种优化，每轮找到一个最大（即堆顶），然后交换到数组末尾
	//前提是要先对数组原地建堆，然后再每一轮交换后还要SiftDown以保证下轮堆顶是剩下中的最大元素

	//总结
	//最坏时间复杂度：O(nlogn)，n轮SiftDown
	//最好时间复杂度：O(nlogn)
	//空间复杂度：O(1)
	//稳定性：不稳定，一定会将相同的元素中的前一个元素先移动到后面
	//原地算法，原地建堆
	class HeapSort<T> : SortBase<T> {
		//表示堆目前容量
		private int heapSize;
		public override void Sorting(T[] array) {
			base.Sorting(array);
			//堆开始的大小即数组大小
			heapSize = array.Length;
			//原地建堆，自下而上的下滤
			for (int i = (heapSize >> 1) - 1; i >= 0; i--) {
				SiftDown(i);
			}

			while (heapSize > 1) {
				//每一轮都要交换堆顶和堆尾（堆尾是heapSize - 1，又因为要减少堆容量，故--heapSize）
				//因为是最大堆，所以堆顶最大，每轮移动到最后面就会成为升序
				Swap(0, --heapSize);
				//将堆顶元素下滤（恢复堆的性质）
				SiftDown(0);
			}
		}

		//下滤，传入一个索引让该元素下移到合适位置
		private void SiftDown(int index) {
			T element = elements[index];
			int half = heapSize >> 1;
			//非叶子节点才能进来（小于第一个叶子节点的索引，即非叶子节点的数量，完全二叉树的性质）
			while (index < half) {
				//且非叶子节点只有两种情况，只有左子节点，或左右都有子节点
				//默认节点为左子节点
				int childIndex = (index << 1) + 1;
				T betterChild = elements[childIndex];
				//右子节点
				int rightIndex = childIndex + 1;

				//右子节点肯定不超过数组的Length，同时选出左右子节点中最大的（没必要=0）				
				if (rightIndex < heapSize && CompareByElement(elements[rightIndex], betterChild) > 0) {
					betterChild = elements[childIndex = rightIndex];//取出右子节点，同时将子节点标记为右子节点
				}
				//发现需要下滤的节点比自己最大的子节点还要大（或等于）就无须遍历
				if (CompareByElement(element, betterChild) >= 0) {
					break;
				}
				//下滤该节点，将子节点提上去
				elements[index] = betterChild;
				//移动指针
				index = childIndex;
			}
			elements[index] = element;
		}
	}
}
