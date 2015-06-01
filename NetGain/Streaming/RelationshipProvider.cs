using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetGain.Streaming
{
	public class RelationshipProvider : NetGain.RelationshipProvider
	{
		public RelationshipProvider() : base()
		{
			UrlEndpoint = "relationship";
			UseStreaming = true;
		}

		public new Stream Get(long id)
		{
			var url = string.Format("{0}/{1}", UrlEndpoint, id);
			HttpWebResponse response = ExecuteRequest(url, "GET");
			return response.GetResponseStream();
		}

		public new Stream Get(Node node, string[] types)
		{
			var url = string.Format("node/{0}/{1}/all/", node.id, _urlEndpointPlural);
			// Append the type names to the URL.
			for (int i = 0; i < types.Length; i++)
			{
				if (i > 0)
					url += "%26"; // Not much advantage to using '&' then encoding it.
				url += types[i];
			}
			HttpWebResponse response = ExecuteRequest(url, "GET");
			return response.GetResponseStream();
		}

		public new Stream GetIncoming(Node node)
		{
			return GetRelationshipsByDirection(node, "in");
		}

		public new Stream GetOutgoing(Node node)
		{
			return GetRelationshipsByDirection(node, "out");
		}

		private Stream GetRelationshipsByDirection(Node node, string direction)
		{
			var url = string.Format("node/{0}/{1}/{2}", node.id, _urlEndpointPlural, direction);
			HttpWebResponse response = ExecuteRequest(url, "GET");
			return response.GetResponseStream();
		}

		public new Stream GetProperty(long id, string property)
		{
			var url = string.Format("{0}/{1}/properties/{2}", UrlEndpoint, id, property);
			HttpWebResponse response = ExecuteRequest(url, "GET");
			return response.GetResponseStream();
		}

		public new Stream GetProperties(long id)
		{
			var url = string.Format("{0}/{1}/properties", UrlEndpoint, id);
			HttpWebResponse response = ExecuteRequest(url, "GET");
			return response.GetResponseStream();
		}
	}
}
