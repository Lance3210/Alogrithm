using System.Collections.Generic;
using System.Text;

namespace Exercises.DFS
{
	class DFSExe
	{
		/***************************************leetcode***************************************/
		// Title : Z 字形变换（明明是N型）
		// URL :   https://leetcode-cn.com/problems/zigzag-conversion/
		// Brief : 
		/***************************************leetcode***************************************/
		public string Convert(string s, int numRows)
		{
			if (numRows < 2)
			{
				return s;
			}
			List<StringBuilder> list = new List<StringBuilder>();
			//创建numsRows个行来存储
			for (int i = 0; i < numRows; ++i)
			{
				list.Add(new StringBuilder());
			}

			int flag = -1;//转折标记
			int index = 0;//index的区间为[0, numRows - 1]

			//遍历每一个字符
			for (int i = 0; i < s.Length; ++i)
			{
				list[index].Append(s[i]);
				if (index == 0 || index == numRows - 1)//到达转折点
				{
					flag = -flag;//直接反转
				}
				index += flag;//一行行插入
			}
			StringBuilder sb = new StringBuilder();
			//拼接所有行
			for (int i = 0; i < list.Count; ++i)
			{
				sb.Append(list[i]);
			}
			return sb.ToString();
		}

	}
}
