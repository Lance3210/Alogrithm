using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercises.数组与排序 {
	class ArrayAndSort {
		/***************************************leetcode***************************************/
		// Title : 88. 合并两个有序数组
		// URL :   https://leetcode-cn.com/problems/merge-sorted-array/
		// Brief : 三指针，类似归并排序从尾部开始遍历替换
		/***************************************leetcode***************************************/
		public void Merge(int[] nums1, int m, int[] nums2, int n) {
			int x = m - 1;
			int y = n - 1;
			int current = m + n - 1;
			//只要nums2遍历完就无须遍历了，类似归并排序
			while (y >= 0) {
				//nums1先遍历完，之后全部填入nums2剩下的即可
				if (x < 0 || nums1[x] <= nums2[y]) {
					nums1[current--] = nums2[y--];
				}
				else {
					nums1[current--] = nums1[x--];
				}
			}
		}

		/***************************************leetcode***************************************/
		// Title : 75. 颜色分类
		// URL :   https://leetcode-cn.com/problems/sort-colors/
		// Brief : 三指针，将遍历到的0与left交换放到头，2与right交换放到尾（要注意交换时相同数的处理）
		/***************************************leetcode***************************************/
		public void SortColors(int[] nums) {
			int left = 0, right = nums.Length - 1;
			//小于right是因为right是动态的，right后面的都已经是2就无须遍历了
			for (int i = 0; i <= right; ++i) {
				//之所以用while是避免2与2的调换但left移动了的问题
				while (i <= right && nums[i] == 2) {
					nums[i] = nums[right];//将right位置的数保存下来
					nums[right--] = 2;//尾部加一个2
				}
				if (nums[i] == 0) {
					nums[i] = nums[left];//将left位置的数保存下来
					nums[left++] = 0;//头部加一个0
				}
			}
		}

	}
}