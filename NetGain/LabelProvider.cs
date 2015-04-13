using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetGain
{
	public class LabelProvider : ProviderBase
	{
		public LabelProvider () : base()
		{
			UrlEndpoint = "labels";
		}

		public void Add(Node node, string label)
		{
			Add(node, new string[] { label });
		}

		public void Add(Node node, string[] labels)
		{
			// Build the URL.
			string url = string.Format("node/{0}/{1}", node.id, UrlEndpoint);

			// The only content in the body is the label of the node to be created which is
			// the name of the Type (NOT the fully-qualified name).
			byte[] postBody = Encoding.GetEncoding(DefaultEncoding).GetBytes(JsonConvert.SerializeObject(labels));

			// Execute the request.  Expect a response status code of 204.
			HttpWebResponse response = ExecuteRequest(url, "POST", DefaultContentType, postBody);
			if (response.StatusCode != HttpStatusCode.NoContent)
				throw new InvalidOperationException();
		}

		public string[] Get()
		{
			HttpWebResponse response = ExecuteRequest(UrlEndpoint, "GET");
			string[] result = ConvertResponseStream<string[]>(response);
			return result;
		}

		public string[] Get(Node node)
		{
			var url = string.Format("node/{0}/{1}", node.id, UrlEndpoint);
			HttpWebResponse response = ExecuteRequest(url, "GET");
			string[] result = ConvertResponseStream<string[]>(response);
			return result;
		}

		public IEnumerable<Node> Get(string label)
		{
			var url = string.Format("label/{0}/nodes", label);
			HttpWebResponse response = ExecuteRequest(url, "GET", string.Empty, null);
			IEnumerable<Node> result = ConvertResponseStream<IEnumerable<Node>>(response);
			return result;
		}

		public IEnumerable<Node> Get(string label, string propertyName, object propertyValue)
		{
			var url = string.Format("label/{0}/nodes?{1}={2}", label, propertyName, Uri.EscapeUriString(JsonConvert.SerializeObject(propertyValue)));
			HttpWebResponse response = ExecuteRequest(url, "GET", string.Empty, null);
			IEnumerable<Node> result = ConvertResponseStream<IEnumerable<Node>>(response);
			return result;
		}

		public void Set(Node node, string label)
		{
			Set(node, new string[] { label });
		}

		public void Set(Node node, string[] labels)
		{
			// Build the request object.
			string url = string.Format("node/{0}/{1}", node.id, UrlEndpoint);

			// The only content in the body is the label of the node to be created which is
			// the name of the Type (NOT the fully-qualified name).
			byte[] postBody = Encoding.GetEncoding(DefaultEncoding).GetBytes(JsonConvert.SerializeObject(labels));

			HttpWebResponse response = ExecuteRequest(url, "PUT", DefaultContentType, postBody);
			if (response.StatusCode != HttpStatusCode.NoContent)
				throw new InvalidOperationException();
		}

		public void Remove(Node node, string label)
		{
			Remove(node, new string[] { label });
		}

		public void Remove(Node node, string[] labels)
		{
			foreach (var label in labels)
			{
				string url = string.Format("node/{0}/{1}/{2}", node.id, UrlEndpoint, label);
				HttpWebResponse response = ExecuteRequest(url, "DELETE", string.Empty, null);
				if (response.StatusCode != HttpStatusCode.NoContent)
					throw new InvalidOperationException();
			}
		}

	}
}
