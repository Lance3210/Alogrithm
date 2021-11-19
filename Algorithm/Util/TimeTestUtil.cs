using System;

namespace Algorithm.Util {
	/// <summary>
	/// 使用前建议先执行一次TimeTest(null)，否则结果不会准确
	/// </summary>
	static class TimeTestUtil {
		//无参
		public static void TimeTest(Action action) {
			DateTime dateTime = DateTime.Now;
			if (action == null) {
				return;
			}
			action();
			PrintResult(action.Method.Name, DateTime.Now - dateTime);
		}

		//T
		public static void TimeTest<T>(Action<T> action, T arg) {
			DateTime dateTime = DateTime.Now;
			if (action == null) {
				return;
			}
			action(arg);
			PrintResult(action.Method.Name, DateTime.Now - dateTime);
		}

		//T T
		public static void TimeTest<T>(Func<T, T> func, T arg) {
			T result;
			DateTime dateTime = DateTime.Now;
			if (func == null) {
				return;
			}
			result = func(arg);
			PrintResult(func.Method.Name, result, DateTime.Now - dateTime);
		}

		//T T T
		public static void TimeTest<T>(Func<T, T, T> func, T arg1, T arg2) {
			T result;
			DateTime dateTime = DateTime.Now;
			if (func == null) {
				return;
			}
			result = func(arg1, arg2);
			PrintResult(func.Method.Name, result, DateTime.Now - dateTime);
		}

		//T T Q
		public static void TimeTest<T, Q>(Func<T, T, Q> func, T arg1, T arg2) {
			Q result;
			DateTime dateTime = DateTime.Now;
			if (func == null) {
				return;
			}
			result = func(arg1, arg2);
			PrintResult(func.Method.Name, result, DateTime.Now - dateTime);
		}

		//T[] T
		public static void TimeTest<T>(Func<T[], T> func, T[] array) {
			T result;
			DateTime dateTime = DateTime.Now;
			if (func == null) {
				return;
			}
			result = func(array);
			PrintResult(func.Method.Name, result, DateTime.Now - dateTime);
		}

		//T[] T T
		public static void TimeTest<T>(Func<T[], T, T> func, T[] array, T arg) {
			T result;
			DateTime dateTime = DateTime.Now;
			if (func == null) {
				return;
			}
			result = func(array, arg);
			PrintResult(func.Method.Name, result, DateTime.Now - dateTime);
		}

		//T[] T[] T T
		public static void TimeTest<T>(Func<T[], T[], T, T> func, T[] array1, T[] array2, T arg) {
			T result;
			DateTime dateTime = DateTime.Now;
			if (func == null) {
				return;
			}
			result = func(array1, array2, arg);
			PrintResult(func.Method.Name, result, DateTime.Now - dateTime);
		}

		private static void PrintResult<T>(string methodName, T result, TimeSpan timeSpan) {
			Console.WriteLine($"{methodName}\n{result}\t时间：{timeSpan.TotalMilliseconds} ms\n");
		}
		private static void PrintResult(string methodName, TimeSpan timeSpan) {
			Console.WriteLine($"{methodName}\n时间：{timeSpan.TotalMilliseconds} ms\n");
		}
	}
}
