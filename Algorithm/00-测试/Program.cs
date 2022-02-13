using Algorithm.Util;
using Algorithm.串匹配;
using System.IO;
using System.Text;

namespace Algorithm.测试
{
	static class Program
	{
		static void Main(string[] args)
		{
			string path = "D:\\Demo\\C#\\Alogrithm\\Algorithm\\13-串匹配\\text.txt";
			WriteText(path, 10);

			string text = GetText(path);
			string pattern = "56";
			TimeTestUtil.TimeTest(null);
			TimeTestUtil.TimeTest(Sequence.BF, text, pattern);
			TimeTestUtil.TimeTest(Sequence.BF_Better, text, pattern);
			TimeTestUtil.TimeTest(Sequence.KMP, text, pattern);
			TimeTestUtil.TimeTest(Sequence.KMP_Better, text, pattern);

		}

		private static string GetText(string path)
		{
			StreamReader sr = new StreamReader(path, Encoding.Default);
			return sr.ReadToEnd();
		}

		static void WriteText(string path, long count)
		{
			FileStream file = new FileStream(path, FileMode.Create);
			StringBuilder sb = new StringBuilder();
			for (long i = 0; i < count; i++)
			{
				sb.Append(i);
			}
			byte[] data = Encoding.Default.GetBytes(sb.ToString());
			file.Write(data, 0, data.Length);
			file.Close();
		}

	}
}
