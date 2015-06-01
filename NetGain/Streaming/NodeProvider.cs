using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetGain.Streaming
{
	public class NodeProvider : NetGain.NodeProvider
	{
		public NodeProvider() : base()
		{
			UrlEndpoint = "node";
			UseStreaming = true;
		}

		public new Stream Get(long id)
		{
			var url = string.Format("{0}/{1}", UrlEndpoint, id);
			HttpWebResponse response = (HttpWebResponse)ExecuteRequest(url, "GET");
			return response.GetResponseStream();
		}

		public new Stream Get(long id, string property)
		{
			var url = string.Format("{0}/{1}/properties/{2}", UrlEndpoint, id, property);
			HttpWebResponse response = (HttpWebResponse)ExecuteRequest(url, "GET");
			return response.GetResponseStream();
		}

	}
}
