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
	public class LabelProvider : NetGain.LabelProvider
	{
		public LabelProvider () : base()
		{
			UrlEndpoint = "labels";
			UseStreaming =  true;
		}

		public new Stream Get()
		{
			HttpWebResponse response = ExecuteRequest(UrlEndpoint, "GET");
			return response.GetResponseStream();
		}

		public new Stream Get(Node node)
		{
			var url = string.Format("node/{0}/{1}", node.id, UrlEndpoint);
			HttpWebResponse response = ExecuteRequest(url, "GET");
			return response.GetResponseStream();
		}

		public new Stream Get(string label)
		{
			var url = string.Format("label/{0}/nodes", label);
			HttpWebResponse response = ExecuteRequest(url, "GET", string.Empty, null);
			return response.GetResponseStream();
		}

		public new Stream Get(string label, string propertyName, object propertyValue)
		{
			var url = string.Format("label/{0}/nodes?{1}={2}", label, propertyName, Uri.EscapeUriString(JsonConvert.SerializeObject(propertyValue)));
			HttpWebResponse response = ExecuteRequest(url, "GET", string.Empty, null);
			return response.GetResponseStream();
		}
	}
}
