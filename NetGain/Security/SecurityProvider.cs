using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetGain.Security
{
	public class SecurityProvider : ProviderBase
	{
		public UserStatus UserStatus()
		{
			UserStatus result = new UserStatus();
			return result;
		}

		public void ChangePassword (string username, string password)
		{
			var url = String.Concat("user/{0}", username);
			var jsonPassword = String.Format("{\"password\":\"{0}\"}", password);
			string jsonEntity = JsonConvert.SerializeObject(jsonPassword);
			byte[] postBody = Encoding.GetEncoding(DefaultEncoding).GetBytes(jsonEntity);
			HttpWebResponse response = ExecuteRequest(url, "POST", DefaultContentType, postBody);
			if (response.StatusCode != HttpStatusCode.OK)
			{
				throw new InvalidOperationException();
			}
		}
	}
}
