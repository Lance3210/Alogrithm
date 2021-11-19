namespace Algorithm.排序 {
	//选择排序
	//每一轮找到最大一个元素插入到该轮end的最后面

	//总结
	//最坏时间复杂度：O(n^2)，即使在完全有序也会遍历完毕
	//最好时间复杂度：O(n^2)
	//空间复杂度：O(1)
	//稳定性：不稳定（对数组），因为每一轮都要选出最大的交换，那交换时靠后面元素会被提前
	//原地算法；
	class SelectionSort<T> : SortBase<T> {
		public override void Sorting(T[] array) {
			base.Sorting(array);
			int maxIndex;
			for (int end = array.Length - 1; end > 0; end--) {
				maxIndex = 0;
				for (int begin = 1; begin <= end; begin++) {
					if (CompareByIndex(begin, maxIndex) > 0)//>=可以提高选择排序的稳定性，不用>=则提高效率
					{
						maxIndex = begin;
					}
				}
				Swap(end, maxIndex);//每轮循环后再交换
			}
		}
	}
}
