using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetGain.Extensions
{
	public static class StringExtensions
	{
		public static string FixTrailingSlash(this string url)
		{
			if (url.Substring(url.Length - 1, 1) != "/")
				url = String.Concat(url, "/");
			return url;
		}

		public static string UnfixTrailingSlash(this string url)
		{
			if (url.Substring(url.Length - 1, 1) == "/")
				url = url.Substring(0, url.Length - 1);
			return url;
		}

	}
}
