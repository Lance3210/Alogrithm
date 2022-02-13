using System.Collections.Generic;

namespace Exercises.哈希表
{
	class HashSetExe
	{
		/***************************************leetcode***************************************/
		// Title : 217. 存在重复元素
		// URL :   https://leetcode-cn.com/problems/contains-duplicate/
		// Brief : 哈希表
		/***************************************leetcode***************************************/
		public bool ContainsDuplicate(int[] nums)
		{
			HashSet<int> set = new HashSet<int>();
			for (int i = 0; i < nums.Length; i++)
			{
				if (!set.Add(nums[i]))
				{
					return true;
				}
			}
			return false;
		}
	}
}
