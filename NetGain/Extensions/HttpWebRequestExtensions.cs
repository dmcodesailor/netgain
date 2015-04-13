using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetGain.Extensions
{
	public static class HttpWebRequestExtensions
	{
		public static void AddBasicAuthorizationHeader (this HttpWebRequest request, string username, string password, string encoding = "ISO-8859-1")
		{
			String encodedUsernamePassword = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding(encoding).GetBytes(username + ":" + password));
			request.Headers.Add("Authorization", "Basic " + encodedUsernamePassword);
		}

		public static void AddStreamingHeader (this HttpWebRequest request)
		{
			request.Headers.Add("X-Stream", "true");
		}

	}
}
