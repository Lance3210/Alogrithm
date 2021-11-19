using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercises._04_字符串 {
	class StringExe {
		/***************************************leetcode***************************************/
		// Title : 520. 检测大写字母
		// URL :   https://leetcode-cn.com/problems/detect-capital/
		// Brief : 简单的遍历，关键在利用语言的一些技巧
		/***************************************leetcode***************************************/
		public bool DetectCapitalUse(string word) {
			//如果第一个是小写，但第二个是大写必是false
			if (word.Length > 1 && char.IsLower(word[0]) && char.IsUpper(word[1])) {
				return false;
			}
			//如果第一个是大写，那么从第二个开始每一个都要与第一个的状态保持一致
			for (int i = 2; i < word.Length; ++i) {
				//利用了自带 ^ 的重写，可用于比较bool值
				if (char.IsLower(word[i]) ^ char.IsLower(word[0])) {
					return false;
				}
			}
			return true;
		}

	}
}
