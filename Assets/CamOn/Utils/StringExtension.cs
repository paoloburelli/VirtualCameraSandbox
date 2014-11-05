using System;
namespace CamOn.Utils
{
	public static class StringExtension
	{
		public static int Count (this string s, char c)
		{
			int ret = 0;
			foreach (char ch in s)
				if (ch == c)
					ret++;
			return ret;
		}
	}
}

