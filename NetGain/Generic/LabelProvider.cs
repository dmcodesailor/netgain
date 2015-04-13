using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetGain.Generic
{
	public class LabelProvider : ProviderBase
	{
		public LabelProvider() : base()
		{
			UrlEndpoint = "labels";
		}

		public void Add<T> (Node<T> node, string label)
		{
			Add<T>(node, new string[] { label });
		}

		public void Add<T> (Node<T> node, string[] labels)
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
			// Execute the request using the default endpoint for this provider.  Nothing is POSTed with this request.
			HttpWebResponse response = (HttpWebResponse)ExecuteRequest(UrlEndpoint, "GET", string.Empty, null);
			string[] result = null;
			using (var responseStream = response.GetResponseStream())
			{
				StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding(DefaultEncoding));
				var jsonString = streamReader.ReadToEnd();
				result = JsonConvert.DeserializeObject<string[]>(jsonString);
			}
			return result;
		}

		public string[] Get<T>(Node<T> node)
		{
			var url = string.Format("node/{0}/{1}", node.id, UrlEndpoint);
			HttpWebResponse response = (HttpWebResponse)ExecuteRequest(url, "GET", string.Empty, null);
			string[] result = null;
			using (var responseStream = response.GetResponseStream())
			{
				StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding(DefaultEncoding));
				var jsonString = streamReader.ReadToEnd();
				result = JsonConvert.DeserializeObject<string[]>(jsonString);
			}
			return result;
		}

		public IEnumerable<Node<object>> Get(string label)
		{
			var url = string.Format("label/{0}/nodes", label);
			HttpWebResponse response = ExecuteRequest(url, "GET", string.Empty, null);
			IEnumerable<Node<object>> result = null;
			using (var responseStream = response.GetResponseStream())
			{
				StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding(DefaultEncoding));
				var jsonString = streamReader.ReadToEnd();
				result = JsonConvert.DeserializeObject<IEnumerable<Node<object>>>(jsonString);
			}
			return result;
		}

		public IEnumerable<Node<object>> Get(string label, string propertyName, object propertyValue)
		{
			var url = string.Format("label/{0}/nodes?{1}={2}", label, propertyName, Uri.EscapeUriString(JsonConvert.SerializeObject(propertyName)));
			HttpWebResponse response = ExecuteRequest(url, "GET", string.Empty, null);
			IEnumerable<Node<object>> result = null;
			using (var responseStream = response.GetResponseStream())
			{
				StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding(DefaultEncoding));
				var jsonString = streamReader.ReadToEnd();
				result = JsonConvert.DeserializeObject<IEnumerable<Node<object>>>(jsonString);
			}
			return result;
		}

		public void Set<T>(Node<T> node, string label)
		{
			Set<T>(node, new string[] { label });
		}

		public void Set<T>(Node<T> node, string[] labels)
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

		public void Remove<T>(Node<T> node, string label)
		{
			Remove<T>(node, new string[] { label });
		}

		public void Remove<T>(Node<T> node, string[] labels)
		{
			// Build the request object.
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
