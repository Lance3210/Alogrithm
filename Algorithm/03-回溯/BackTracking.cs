using System;

namespace Algorithm.回溯 {
	class BackTracking {
		//N皇后问题
		private int[] queens;//索引代表行号，元素代表在该行的某一列
		private int ways;//代表有多少种摆法

		#region 基础形态
		public void Queens_Base(int n) {
			if (n < 1) {
				return;
			}
			ways = 0;
			queens = new int[n];
			Place(0);//从第一行即索引为0开始摆放
			Console.WriteLine($"{n}皇后问题共有{ways}种摆法");
		}
		//代表从第row行开始摆放
		private void Place(int row) {
			if (row == queens.Length) {
				++ways;//摆法++
				return;//代表第queens.Length行（索引-1）已经摆放了一个符合要求的皇后，无须再找了
			}
			for (int col = 0; col < queens.Length; col++) {
				//符合要求则摆放皇后，如果不符合则自动剪枝
				if (IsValid(row, col)) {
					queens[row] = col;//标记该位置
					Place(row + 1);//递归下一行，不符合就会返回上一行，实现了回溯	
				}
				//因为回溯后新的值会覆盖，故后面无须重置queens内的值
			}
		}
		//用于检查第row行第col列是否可以摆放
		private bool IsValid(int row, int col) {
			//遍历该行之前的所有行，取出这些行中已经确定位置的皇后位置拿来比较
			for (int i = 0; i < row; i++) {
				if (queens[i] == col) {
					return false;//同一列已经存在皇后
				}
				if (row - i == Math.Abs(col - queens[i])) {
					return false;//同一斜线上已经存在皇后（斜率公式）
				}
			}
			return true;
		}
		#endregion

		#region 优化
		//无须在IsValid中一个个遍历之前的行，可以用一些变量用于标记以提高效率
		private bool[] cols;//用于标记某一列是否已经存在皇后
		private bool[] leftTops;//用于标记从左上角到右下角斜线上是否已经存在皇后
		private bool[] rightTops;//用于标记从右上角到左下角斜线上是否已经存在皇后
		public void Queens_Better(int n) {
			if (n < 1) {
				return;
			}
			ways = 0;
			queens = new int[n];//用于记录皇后位置
			cols = new bool[n];//初始化标记用的变量
			leftTops = new bool[(n << 1) - 1];
			rightTops = new bool[leftTops.Length];
			Place_Better(0);//从第一行即索引为0开始摆放
			Console.WriteLine($"{n}皇后问题共有{ways}种摆法");
		}
		//代表从第row行开始摆放
		private void Place_Better(int row) {
			if (row == cols.Length) {
				++ways;//摆法++
				return;//已经摆放满了，无须再找了
			}
			int lIndex, rIndex;//用于标记斜线索引 
			for (int col = 0; col < cols.Length; col++) {
				if (cols[col]) {
					continue;//说明第col列上已经有皇后了
				}
				lIndex = row - col + cols.Length - 1;//算出左上角到右下角斜线数组中的索引
				if (leftTops[lIndex]) {
					continue;//说明斜线上已经有皇后了
				}
				rIndex = row + col;//算出右上角到左下角斜线数组中的索引
				if (rightTops[rIndex]) {
					continue;//说明斜线上已经有皇后了
				}

				//标记该列和两组斜线上有皇后
				queens[row] = col;
				cols[col] = leftTops[lIndex] = rightTops[rIndex] = true;
				Place_Better(row + 1);//递归下一行，不符合就会返回上一行，实现了回溯

				//如果执行到这里说明之前做的并不符合（continue了），也就是回溯了，要重置标记
				//因为回溯后新的值会覆盖，故后面无须重置queens内的值
				cols[col] = leftTops[lIndex] = rightTops[rIndex] = false;
			}
		}

		#endregion

		#region 八皇后的特殊解法
		//如果是八皇后，则可以使用位运算压缩空间（一定修改后可以适合其他数量的皇后问题）
		//将列数压缩为一个字节8位，从右向左，一位代表一列
		//同理将斜线压缩为一个short16位，一位代表一条
		private byte byteCol;
		private short leftTop, rightTop;
		public void Queens_Eight() {
			queens = new int[8];//用于记录皇后位置
			Place_EightQueens(0);//从第一行即索引为0开始摆放
			Console.WriteLine($"{8}皇后问题共有{ways}种摆法");
		}
		private void Place_EightQueens(int row) {
			if (row == 8) {
				++ways;//摆法++
				return;//已经摆放满了，无须再找了
			}
			int colIndex, lIndex, rindex;//用于标记列数与斜线索引
			for (int col = 0; col < 8; col++) {
				//令当前位为1（左移到对应位），其他为0，将该数与byteCol位相与，若为0则表示该列无皇后
				colIndex = 1 << col;
				if ((byteCol & colIndex) != 0) {
					continue;//若不为0，说明该位已有皇后
				}
				lIndex = 1 << (row - col + 7);//算出左上角到右下角斜线数组中的索引
				if ((leftTop & lIndex) != 0) {
					continue;//同理上
				}
				rindex = 1 << (row + col);//算出右上角到左下角斜线数组中的索引
				if ((rightTop & rindex) != 0) {
					continue;
				}

				//标记该列和两组斜线上有皇后
				queens[row] = col;
				byteCol |= (byte)colIndex;//|=或等，将对应位置为1
				leftTop |= (short)lIndex;
				rightTop |= (short)rindex;
				Place_EightQueens(row + 1);//递归下一行，不符合就会返回上一行，实现了回溯

				//如果执行到这里说明之前做的并不符合（continue了），也就是回溯了，要重置标记
				//byteCol &= (byte)~(1 << col);//先取反，然后&=与等，将对应位置为0（也可以直接异或）
				//leftTop &= (short)~(1 << lIndex);
				//rightTop &= (short)~(1 << rindex);
				byteCol ^= (byte)colIndex;
				leftTop ^= (short)lIndex;
				rightTop ^= (short)rindex;
			}
		}
		#endregion
		public void Show() {
			for (int row = 0; row < queens.Length; row++) {
				for (int col = 0; col < queens.Length; col++) {
					if (queens[row] == col) {
						Console.Write("1 ");
					}
					else {
						Console.Write("0 ");
					}
				}
				Console.WriteLine();
			}
			Console.WriteLine("------------------------------------------");
		}
	}
}
