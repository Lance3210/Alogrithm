namespace Exercises.位运算
{
	class BitOperationExe
	{
		/***************************************leetcode***************************************/
		// Title : 136. 只出现一次的数字
		// URL :   https://leetcode-cn.com/problems/single-number/
		// Brief :
		// 异或运算
		// 交换律：a ^ b ^ c <=> a ^ c ^ b
		// 任何数与0异或为本身 0 ^ n => n
		// 相同的数异或为0: n ^ n => 0
		// var a = [2, 3, 2, 4, 4]
		// 2 ^ 3 ^ 2 ^ 4 ^ 4等价于 2 ^ 2 ^ 4 ^ 4 ^ 3 => 0 ^ 0 ^3 => 3
		/***************************************leetcode***************************************/
		public int SingleNumber(int[] nums)
		{
			if (nums == null) return default;
			int result = 0;
			for (int i = 0; i < nums.Length; i++)
			{
				result ^= nums[i];
			}
			return result;
		}

		/***************************************leetcode***************************************/
		// Title : 476. 数字的补数
		// URL :   https://leetcode-cn.com/problems/number-complement/
		// Brief : 
		/***************************************leetcode***************************************/
		public int FindComplement(int num)
		{
			int bit = 1;
			while (bit < num)
			{
				bit |= bit << 1;//num有多少位就创建一个有多少位全是1的数
			}
			return bit ^ num;//最后另这个数与num异或，则能达到取反位效果
		}

		/***************************************leetcode***************************************/
		// Title : 7. 整数反转
		// URL :   https://leetcode-cn.com/problems/reverse-integer/
		// Brief : 如果反转后整数超过 32 位的有符号整数的范围 [−231,  231 − 1] ，就返回 0。
		// 假设环境不允许存储 64 位整数（有符号或无符号）。
		/***************************************leetcode***************************************/
		public int Reverse(int x)
		{
			int result = 0;
			while (x != 0)
			{
				//就是每次将最高位放到个位时都要判断是否溢出
				//而且要在最大位前一个就开始判断了
				if (result > int.MaxValue / 10 || result < int.MinValue / 10)
				{
					return 0;
				}
				int digit = x % 10;
				x /= 10;
				result = result * 10 + digit;//这里是从0位一个个向前补的
			}
			return result;
		}

		/***************************************leetcode***************************************/
		// Title : 9. 回文数
		// URL :   https://leetcode-cn.com/problems/palindrome-number/
		// Brief : 
		/***************************************leetcode***************************************/
		public bool IsPalindrome(int x)
		{
			if (x < 0 || (x != 0 && x % 10 == 0))
			{
				return false;
			}

			int halfReverse = 0;
			//只要后半段反转的结果大于或等于前半段则刚好到中间了
			while (halfReverse < x)
			{
				halfReverse = halfReverse * 10 + x % 10;
				x /= 10;
			}
			//奇数则另外/10
			return x == halfReverse || x == halfReverse / 10;
		}
	}
}
