namespace Algorithm.串匹配 {
	static class Sequence {
		//n为text长度，m为模式串长度，返回字符串第一个匹配的起始位置IndexOf，没找到则返回-1

		//BF蛮力
		//最好时间复杂度：O(m)，一般m远小于n，仅需1轮即可匹配
		//最坏时间复杂度：O(m * (n - m + 1))，最多比较n - m + 1轮，且每轮都要逼到m - 1后才失败

		//BF，ti指针回溯，比较pi和ti
		public static int BF(string text, string pattern) {
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(pattern)) {
				return -1;
			}
			int ti = 0, pi = 0;
			//正在匹配的串的首索引（ti - pi）无须超过两个串长度相减的位置，因为没必要再比超出的部分
			//其实这里不减pi也好像没有问题......
			while (ti - pi <= text.Length - pattern.Length && pi < pattern.Length) {
				if (text[ti] == pattern[pi]) {
					++ti;
					++pi;
				}
				else {
					ti = ti - pi + 1;//一旦不符合，text的指针回溯到最开始并且向前进一个
					pi = 0;
				}
			}
			return (pi == pattern.Length) ? (ti - pi) : -1;//pattern已经完全遍历完就相当于匹配成功
		}

		//BF优化，ti不变，比较pi和ti + pi
		public static int BF_Better(string text, string pattern) {
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(pattern)) {
				return -1;
			}
			for (int ti = 0; ti <= text.Length - pattern.Length; ++ti) {
				int pi = 0;
				for (; pi < pattern.Length; ++pi) {
					if (text[ti + pi] != pattern[pi]) {
						break;
					}
				}
				if (pi == pattern.Length) {
					return ti;
				}
			}
			return -1;
		}

		//KMP
		//预先根据模式串的内容生成一张next表，pattern能够根据该表在不符合时右移一大段距离从而跳过不必要的字符
		//最好时间复杂度：O(m)
		//最坏时间复杂度：O(n + m)

		//直接在BF的基础上改进
		public static int KMP(string text, string pattern) {
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(pattern)) {
				return -1;
			}
			int ti = 0, pi = 0;
			int[] next = Next(pattern);//根据函数计算出next数组
			while (ti - pi <= text.Length - pattern.Length && pi < pattern.Length) {
				//当pattern的0号位置不匹配时（pi = -1），要++ti以比较text的下一个元素，同时++pi也会恢复为0
				if (pi < 0 || text[ti] == pattern[pi]) {
					++ti;
					++pi;
				}
				else {
					pi = next[pi];//只需要根据next[pi]右移即可
				}
			}
			return (pi == pattern.Length) ? (ti - pi) : -1;
		}

		//Next方法，根据pattern计算next[]数组
		//next[pi] 是pi左边所有字符中，真前缀与真后缀的最大公共子串长度
		private static int[] Next(string pattern) {
			int[] next = new int[pattern.Length];
			//假设有两个位置i与n，而n = next[i]
			int i = 0, n = -1;
			next[0] = -1;//初始化首元素为-1
			while (i < pattern.Length - 1) {
				//n < 0就是相当于没有公共子串（next[0]是-1），那么next[i]的位置自然就是0
				//如果pattern[i] == pattern[n]，说明前面的子串都是相等的，next[i + 1]位置自然要在n后面一个
				if (n < 0 || pattern[i] == pattern[n]) {
					next[++i] = ++n;//以上两种情况可以合二为一
				}
				else {
					n = next[n];//不相等，就要继续向左边找
				}
			}
			return next;
		}

		//Next方法的优化
		//如果是重复度很高的模式串，生成的next[]会显得很笨拙，完全是相同元素的模式串在KMP时甚至会退化为BF
		private static int[] Next_Better(string pattern) {
			int[] next = new int[pattern.Length];
			int i = 0, n = -1;
			next[0] = -1;
			while (i < pattern.Length - 1) {
				if (n < 0 || pattern[i] == pattern[n]) {
					++i;
					++n;
					//加一个判断，看+1后的值是否还相等，如果还是相等的直接采用next[n]即可
					//这样处理完后，在KMP中遇到连续相同元素时next返回的值会是最前面的
					if (pattern[i] == pattern[n]) {
						next[i] = next[n];
					}
					else {
						next[i] = n;
					}
				}
				else {
					n = next[n];
				}
			}
			return next;
		}
		//使用改进过的Next方法
		public static int KMP_Better(string text, string pattern) {
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(pattern)) {
				return -1;
			}
			int ti = 0, pi = 0;
			int[] next = Next_Better(pattern);//根据函数计算出next数组
			while (ti - pi <= text.Length - pattern.Length && pi < pattern.Length) {
				//当pattern的0号位置不匹配时（pi = -1），要++ti以比较text的下一个元素，同时++pi也会恢复为0
				if (pi < 0 || text[ti] == pattern[pi]) {
					++ti;
					++pi;
				}
				else {
					pi = next[pi];//只需要根据next[pi]右移即可
				}
			}
			return (pi == pattern.Length) ? (ti - pi) : -1;
		}

		//Boyer-Moore
		//从模式串的尾部开始匹配
		//最好时间复杂度：O(n/m)
		//最坏时间复杂度：O(n + m)，其中O(m)是构造BC、GS表

		//Rabin-Karp
		//基于hash的字符串匹配，将pattern的hash值与text中每一个子串的hash值进行比较
		//某一串的hash值可以根据上一串的hash值在某种O(1)时间内的方法计算出来

		//Sunday
		//与BM类似，但从模式串的头部开始匹配
		//关注的是text中正在参与匹配的子串的下一个字符A
		//如果A没有在pattern中出现，则直接跳过，移动位数 = pattern.Length + 1
		//否则让pattern中最靠的右A与text中的A对齐
	}
}