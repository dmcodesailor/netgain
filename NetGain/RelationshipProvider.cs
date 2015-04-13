using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetGain.Extensions;

namespace NetGain
{
	public class RelationshipProvider : ProviderBase
	{
		private string _urlEndpointPlural = "relationships";

		public RelationshipProvider() : base()
		{
			UrlEndpoint = "relationship";
		}

		public Relationship Create(Node fromNode, string type, Node toNode, dynamic properties = null)
		{
			var url = string.Format("node/{0}/{1}", fromNode.id, _urlEndpointPlural);
			dynamic entity = new { to = string.Format("{0}/node/{1}", UrlRoot.UnfixTrailingSlash(), toNode.id), type = type, data = properties };
			string jsonEntity = JsonConvert.SerializeObject(entity);
			byte[] postBody = Encoding.GetEncoding(DefaultEncoding).GetBytes(jsonEntity);
			HttpWebResponse response = ExecuteRequest(url, "POST", DefaultContentType, postBody);
			Relationship result = ConvertResponseStream<Relationship>(response);
			return result;
		}

		public Relationship Get(long id)
		{
			var url = string.Format ("{0}/{1}", UrlEndpoint, id);
			HttpWebResponse response = ExecuteRequest(url, "GET");
			Relationship result = ConvertResponseStream<Relationship>(response);
			return result;
		}

		public IEnumerable<Relationship> Get(Node node, string[] types)
		{
			var url = string.Format("node/{0}/{1}/all/", node.id, _urlEndpointPlural);
			// Append the type names to the URL.
			for (int i = 0; i < types.Length; i++ )
			{
				if (i > 0)
					url += "%26"; // Not much advantage to using '&' then encoding it.
				url += types[i];
			}
			HttpWebResponse response = ExecuteRequest(url, "GET");
			IEnumerable<Relationship> result = ConvertResponseStream<IEnumerable<Relationship>>(response);
			return result;
		}

		public IEnumerable<Relationship> GetIncoming(Node node)
		{
			return GetRelationshipsByDirection(node, "in");
		}

		public IEnumerable<Relationship> GetOutgoing(Node node)
		{
			return GetRelationshipsByDirection(node, "out");
		}

		private IEnumerable<Relationship> GetRelationshipsByDirection(Node node, string direction)
		{
			var url = string.Format("node/{0}/{1}/{2}", node.id, _urlEndpointPlural, direction);
			HttpWebResponse response = ExecuteRequest(url, "GET");
			IEnumerable<Relationship> result = ConvertResponseStream<IEnumerable<Relationship>>(response);
			return result;
		}

		public object GetProperty (long id, string property)
		{
			var url = string.Format("{0}/{1}/properties/{2}", UrlEndpoint, id, property);
			HttpWebResponse response = ExecuteRequest(url, "GET");
			object result = ConvertResponseStream<object>(response);
			return result;
		}

		public dynamic GetProperties (long id)
		{
			var url = string.Format("{0}/{1}/properties", UrlEndpoint, id);
			HttpWebResponse response = ExecuteRequest(url, "GET");
			dynamic result = ConvertResponseStream<dynamic>(response);
			return result;
		}

		public void SetProperty (long id, string property, object value)
		{
			var url = string.Format("{0}/{1}/properties/{2}", UrlEndpoint, id, property);
			byte[] postBody = Encoding.GetEncoding(DefaultEncoding).GetBytes(JsonConvert.SerializeObject(value));
			HttpWebResponse response = ExecuteRequest(url, "PUT", DefaultContentType, postBody);
			if (response.StatusCode != HttpStatusCode.NoContent)
				throw new InvalidOperationException();
		}

		public void SetProperties(long id, dynamic properties)
		{
			var url = string.Format("{0}/{1}/properties", UrlEndpoint, id);
			byte[] postBody = Encoding.GetEncoding(DefaultEncoding).GetBytes(JsonConvert.SerializeObject(properties));
			HttpWebResponse response = ExecuteRequest(url, "PUT", DefaultContentType, postBody);
			if (response.StatusCode != HttpStatusCode.NoContent)
				throw new InvalidOperationException();
		}

		public void Delete (long id)
		{
			var url = string.Format("{0}/{1}", UrlEndpoint, id);
			HttpWebResponse response = ExecuteRequest(url, "DELETE");
			if (response.StatusCode != HttpStatusCode.NoContent)
				throw new InvalidOperationException();
		}


	}
}
