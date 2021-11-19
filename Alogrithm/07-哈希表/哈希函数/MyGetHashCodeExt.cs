using System;

namespace DataStructure.哈希表.哈希函数 {
	public static class MyGetHashCodeExt {
		public static int GetHashCodeFunc(this int value) {
			return value;
		}

		public static int GetHashCodeFunc(this float value) {
			return BitConverter.SingleToInt32Bits(value);
		}

		public static int GetHashCodeFunc(this long value) {
			return (int)(value ^ ((value) >> 32));//java的是无符号右移，C#需自行实现
		}

		public static int GetHashCodeFunc(this double value) {
			long bits = BitConverter.DoubleToInt64Bits(value);
			return (int)(bits ^ ((bits) >> 32));
		}

		public static int GetHashCodeFunc(this string str) {
			int hashCode = 0;
			for (int i = 0; i < str.Length; i++) {
				char c = str[i];
				hashCode = (hashCode << 5) - hashCode + c;
			}
			return hashCode;
		}
	}
}
