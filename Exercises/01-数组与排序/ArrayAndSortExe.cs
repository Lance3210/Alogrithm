using System;
using System.Collections.Generic;

namespace Exercises.数组与排序
{
	class ArrayAndSortExe
	{
		/***************************************leetcode***************************************/
		// Title : 977. 有序数组的平方
		// URL :   https://leetcode-cn.com/problems/squares-of-a-sorted-array/
		// Brief : 多指针，两头比较出最大的插入新数组末尾
		/***************************************leetcode***************************************/
		public int[] SortedSquares(int[] nums)
		{
			int left = 0;
			int right = nums.Length - 1;
			int current = right;
			int[] array = new int[nums.Length];
			for (int i = 0; i < nums.Length; ++i)
			{
				nums[i] = nums[i] * nums[i];//全部都平方
			}
			while (current >= 0)
			{
				if (nums[left] > nums[right])
				{
					array[current] = nums[left];
					++left;
				}
				else
				{
					array[current] = nums[right];
					--right;
				}
				--current;
			}
			return array;
		}

		/***************************************leetcode***************************************/
		// Title : 面试题 16.16. 部分排序
		// URL :   https://leetcode-cn.com/problems/sub-sort-lcci/
		// Brief : 多指针，从左到右和从右到左分别找出最后出现的逆序对，再取最左和最右的下标即可
		//比如[1,4,5,2,6,3,7]，从左到右最后出现的逆序对就是 6[4] 3[5]，从右到左就是 4[1] 2[3]
		//然后取这两个逆序对的最左和最右，4 和 3，下标为[1] 和 [5]，即为所求
		/***************************************leetcode***************************************/
		public int[] SubSort(int[] array)
		{
			if (array == null || array.Length < 2)
			{
				return new int[2] { -1, -1 };
			}
			int m = -1, n = -1;
			int max = array[0];//比max还小的，说明就是一个逆序对（注意：该题目默认升序排列）
			for (int i = 1; i < array.Length; ++i)
			{
				if (array[i] >= max)
				{
					max = array[i];
				}
				else
				{
					n = i;//在发现==时不能记录，比如[5 1 2 5]记录了5但其实是应该是2
				}
			}

			int min = array[array.Length - 1];//同理上面的max，比min还大，说明就是一个逆序对
			for (int i = array.Length - 2; i >= 0; --i)
			{
				if (array[i] <= min)
				{
					min = array[i];
				}
				else
				{
					m = i;
				}
			}
			return new int[2] { m, n };
		}

		/***************************************leetcode***************************************/
		// Title : 88. 合并两个有序数组
		// URL :   https://leetcode-cn.com/problems/merge-sorted-array/
		// Brief : 三指针，类似归并排序从尾部开始遍历替换
		/***************************************leetcode***************************************/
		public void Merge(int[] nums1, int m, int[] nums2, int n)
		{
			int x = m - 1;
			int y = n - 1;
			int current = m + n - 1;
			//只要nums2遍历完就无须遍历了，类似归并排序
			while (y >= 0)
			{
				//nums1先遍历完，之后全部填入nums2剩下的即可
				if (x < 0 || nums1[x] <= nums2[y])
				{
					nums1[current--] = nums2[y--];
				}
				else
				{
					nums1[current--] = nums1[x--];
				}
			}
		}

		/***************************************leetcode***************************************/
		// Title : 75. 颜色分类
		// URL :   https://leetcode-cn.com/problems/sort-colors/
		// Brief : 三指针，将遍历到的0与left交换放到头，2与right交换放到尾（要注意交换时相同数的处理）
		/***************************************leetcode***************************************/
		public void SortColors(int[] nums)
		{
			int left = 0, right = nums.Length - 1;
			//小于right是因为right是动态的，right后面的都已经是2就无须遍历了
			for (int i = 0; i <= right; ++i)
			{
				//之所以用while是避免2与2的调换但left移动了的问题
				while (i <= right && nums[i] == 2)
				{
					nums[i] = nums[right];//将right位置的数保存下来
					nums[right--] = 2;//尾部加一个2
				}
				if (nums[i] == 0)
				{
					nums[i] = nums[left];//将left位置的数保存下来
					nums[left++] = 0;//头部加一个0
				}
			}
		}

		/***************************************leetcode***************************************/
		// Title : 26. 删除有序数组中的重复项（简单）
		// URL :   https://leetcode-cn.com/problems/remove-duplicates-from-sorted-array/
		// Brief : 
		/***************************************leetcode***************************************/
		//双指针
		public int RemoveDuplicates(int[] nums)
		{
			if (nums == null || nums.Length == 0) return 0;
			int left = 0;
			int right = 1;
			while (right < nums.Length)
			{
				if (nums[left] != nums[right])//一旦发现两个不同，则左指针向前一步，然后把右指针值给左指针
				{
					nums[++left] = nums[right];//这样从第一个开始到左指针就不会有重复的了
				}
				++right;
			}
			return ++left;//返回的就是修改后原数组的“length”，但实际上length后面不一定都给改了
		}

		/***************************************leetcode***************************************/
		// Title : 189. 旋转数组
		// URL :   https://leetcode-cn.com/problems/rotate-array/
		// Brief : 
		/***************************************leetcode***************************************/
		public void Rotate(int[] nums, int k)
		{
			if (nums == null || nums.Length <= 1) return;
			k %= nums.Length;//有可能k会超过数组长
			RotateBase(nums, 0, nums.Length - 1);
			RotateBase(nums, 0, k - 1);
			RotateBase(nums, k, nums.Length - 1);
		}
		public void RotateBase(int[] nums, int start, int end)
		{
			int temp;
			while (start < end)
			{
				temp = nums[start];
				nums[start++] = nums[end];
				nums[end--] = temp;
			}
		}

		/***************************************leetcode***************************************/
		// Title : 350. 两个数组的交集 II
		// URL :   https://leetcode-cn.com/problems/intersection-of-two-arrays-ii/
		// Brief : 排序加双指针
		/***************************************leetcode***************************************/
		public int[] Intersect(int[] nums1, int[] nums2)
		{
			if (nums2 == null || nums2 == null) return default;
			List<int> list = new List<int>();//用列表装载后转成数组
			Array.Sort(nums1);
			Array.Sort(nums2);
			int left = 0;
			int right = 0;
			while (left < nums1.Length && right < nums2.Length)
			{
				if (nums1[left] < nums2[right])
				{
					++left;
				}
				else if (nums1[left] > nums2[right])
				{
					++right;
				}
				else
				{
					list.Add(nums1[left++]);//出现相同则加入
					++right;
				}
			}
			return list.ToArray();
		}

		/***************************************leetcode***************************************/
		// Title : 88. 合并两个有序数组
		// URL :   https://leetcode-cn.com/problems/merge-sorted-array/
		// Brief : 
		/***************************************leetcode***************************************/
		//双指针
		public void MergeTwo(int[] nums1, int m, int[] nums2, int n)
		{
			int left = m - 1;
			int right = n - 1;
			int tail = m + n - 1;
			while (right >= 0)
			{
				if (left < 0 || nums1[left] <= nums2[right])
				{
					nums1[tail--] = nums2[right--];
				}
				else
				{
					nums1[tail--] = nums1[left--];
				}
			}
		}
	}
}