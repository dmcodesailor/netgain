using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Text;

namespace NetGain.Generic
{
	public class NodeProvider : ProviderBase
	{
		public NodeProvider() : base()
		{
			UrlEndpoint = "node";
		}

		/// <summary>
		/// Create a node with no properties.  NOTE:  The label is the type name only
		/// and is _NOT_ the fully-qualified name.
		/// </summary>
		/// <typeparam name="T">the Type of the node to create becomes the label</typeparam>
		/// <returns>a Node of T containing the ID of the created node</returns>
		public Node<T> Create<T>()
		{
			// Build the request object for node manipulation.  
			var request = RequestFactory(UrlEndpoint, Credential.UserName, Credential.Password);
			request.Method = "POST";

			//// The only content in the body is the label of the node to be created which is
			//// the name of the Type (NOT the fully-qualified name).
			//byte[] postBody = Encoding.ASCII.GetBytes(typeof(T).Name);

			//// Write the body as a stream.  Since the body consists only of the Type name
			//// it is appropriate to write the entire body in a single stream operation.
			//Stream requestStream = request.GetRequestStream();
			//requestStream.Write(postBody, 0, postBody.Length);

			// Execute the request and process the response.
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			Node<T> result = new Node<T>();
			using (var responseStream = response.GetResponseStream())
			{
				StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding(DefaultEncoding));
				var jsonString = streamReader.ReadToEnd();
				result = JsonConvert.DeserializeObject<Node<T>>(jsonString);
			}

			LabelProvider labelProvider = new LabelProvider();
			labelProvider.Add<T>(result, typeof(T).Name);
			
			return result;
		}

		/// <summary>
		/// Create a node with no properties.  NOTE:  The label is the type name only
		/// and is _NOT_ the fully-qualified name.
		/// </summary>
		/// <typeparam name="T">the Type of the node to create becomes the label</typeparam>
		/// <param name="entity">the entity containing the properties to for the node</param>
		/// <returns>a Node of T containing the ID of the created node</returns>
		public Node<T> Create<T>(T entity)
		{
			// Verify parameters.
			if (entity == null)
				throw new ArgumentNullException("entity", "The entity parameter is required.");

			// Prepare the data for transmission via HTTP.
			var jsonEntity = JsonConvert.SerializeObject(entity);
			byte[] postBody = Encoding.GetEncoding(DefaultEncoding).GetBytes(jsonEntity);

			// Execute REST calls.
			HttpWebResponse response = ExecuteRequest(UrlEndpoint, "POST", "application/json", postBody);
			Node<T> result = new Node<T>();
			using (var responseStream = response.GetResponseStream())
			{
				StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding(DefaultEncoding));
				var jsonString = streamReader.ReadToEnd();
				result = JsonConvert.DeserializeObject<Node<T>>(jsonString);
			}

			// Organize and return result.
			return result;
		}

		/// <summary>
		/// Create a batch of nodes of a given type T.  Note that null entities are
		/// omitted from the results.
		/// </summary>
		/// <typeparam name="T">the Type of the nodes to create becomes the label</typeparam>
		/// <param name="entities">the batch of entities from which nodes are created</param>
		/// <returns>an IEnumerable of Node of T representing the non-null entities for which
		/// nodes were created</returns>
		public IEnumerable<Node<T>> Create<T>(IEnumerable<T> entities)
		{
			List<Node<T>> result = new List<Node<T>>();
			if (entities != null)
			{
				result.AddRange(from entity in entities.Where(e => e != null)
								select Create(entity));
			}
			return result.AsEnumerable<Node<T>>();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="id"></param>
		/// <returns></returns>
		public Node<T> Get<T>(long id)
		{
			var url = string.Format ("{0}/{1}", UrlEndpoint, id);
			HttpWebResponse response = (HttpWebResponse)ExecuteRequest(url, "GET", string.Empty, null);
			Node<T> result = new Node<T>();
			using (var responseStream = response.GetResponseStream())
			{
				StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding(DefaultEncoding));
				var jsonString = streamReader.ReadToEnd();
				result = JsonConvert.DeserializeObject<Node<T>>(jsonString);
			}
			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public IEnumerable<Node<T>> Get<T>()
		{
			var url = string.Format("label/{0}/nodes", typeof(T).Name);
			HttpWebResponse response = ExecuteRequest(url, "GET", string.Empty, null);
			IEnumerable<Node<T>> result = null;
			using (var responseStream = response.GetResponseStream())
			{
				StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding(DefaultEncoding));
				var jsonString = streamReader.ReadToEnd();
				result = JsonConvert.DeserializeObject<IEnumerable<Node<T>>>(jsonString);
			}
			return result;
		}

		/// <summary>
		/// COPIED from LabelProvider
		/// </summary>
		/// <param name="label"></param>
		/// <returns></returns>
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

		public void Update<T> (T entity)
		{

		}

	}
}
