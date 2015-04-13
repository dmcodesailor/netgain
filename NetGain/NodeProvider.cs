using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Configuration;

namespace NetGain
{
	public class NodeProvider : ProviderBase
	{
		public NodeProvider() : base()
		{
			UrlEndpoint = "node";
		}

		public Node Create()
		{
			HttpWebResponse response = (HttpWebResponse)ExecuteRequest(UrlEndpoint, "POST");
			Node result = ConvertResponseStream<Node>(response);
			return result;
		}

		public Node Create(object entity)
		{
			var jsonEntity = JsonConvert.SerializeObject(entity);
			byte[] postBody = Encoding.GetEncoding(DefaultEncoding).GetBytes(jsonEntity);
			HttpWebResponse response = (HttpWebResponse)ExecuteRequest(UrlEndpoint, "POST", DefaultContentType, postBody);
			Node result = ConvertResponseStream<Node>(response);
			return result;
		}

		public IEnumerable<Node> Create(IEnumerable<Node> entities)
		{
			List<Node> result = new List<Node>();
			if (entities != null)
			{
				result.AddRange(from entity in entities.Where(e => e != null)
								select Create(entity));
			}
			return result.AsEnumerable<Node>();
		}

		public Node Get(long id)
		{
			var url = string.Format("{0}/{1}", UrlEndpoint, id);
			HttpWebResponse response = (HttpWebResponse)ExecuteRequest(url, "GET");
			Node result = ConvertResponseStream<Node>(response);
			return result;
		}

		public object Get (long id, string property)
		{
			var url = string.Format("{0}/{1}/properties/{2}", UrlEndpoint, id, property);
			HttpWebResponse response = (HttpWebResponse)ExecuteRequest(url, "GET");
			object result = ConvertResponseStream<object>(response);
			return result;
		}

		public void Set(long id, string property, object value)
		{
			var jsonEntity = JsonConvert.SerializeObject(value);
			byte[] postBody = Encoding.GetEncoding(DefaultEncoding).GetBytes(jsonEntity);
			var url = string.Format("{0}/{1}/properties/{2}", UrlEndpoint, id, property);
			HttpWebResponse response = (HttpWebResponse)ExecuteRequest(url, "PUT", DefaultContentType, postBody);
			if (response.StatusCode != HttpStatusCode.NoContent)
				throw new InvalidOperationException();
		}

		public void Update(long id, object entity)
		{
			var url = string.Format("{0}/{1}/properties", UrlEndpoint, id);
			var jsonEntity = JsonConvert.SerializeObject(entity);
			byte[] postBody = Encoding.GetEncoding(DefaultEncoding).GetBytes(jsonEntity);
			HttpWebResponse response = (HttpWebResponse)ExecuteRequest(url, "PUT", DefaultContentType, postBody);
			if (response.StatusCode != HttpStatusCode.NoContent)
				throw new InvalidOperationException();
		}

		public void Delete(long id)
		{
			var url = string.Format("{0}/{1}", UrlEndpoint, id);
			HttpWebResponse response = (HttpWebResponse)ExecuteRequest(url, "DELETE");
			if (response.StatusCode != HttpStatusCode.NoContent)
				throw new InvalidOperationException();
		}

		public void DeleteProperty (long id, string property)
		{
			var url = string.Format("{0}/{1}/properties/{2}", UrlEndpoint, id, property);
			HttpWebResponse response = ExecuteRequest(url, "DELETE");
			if (response.StatusCode != HttpStatusCode.NoContent)
				throw new InvalidOperationException();

		}

		public void DeleteProperties (long id)
		{
			var url = string.Format("{0}/{1}/properties", UrlEndpoint, id);
			HttpWebResponse response = ExecuteRequest(url, "DELETE");
			if (response.StatusCode != HttpStatusCode.NoContent)
				throw new InvalidOperationException();
		}

	}
}
