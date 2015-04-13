using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetGain.Extensions;
using System.Configuration;
using Newtonsoft.Json;

namespace NetGain
{
	public abstract class ProviderBase
	{
		public NetworkCredential Credential { get; set; }
		public string UrlRoot { get; set; }
		public string DefaultEncoding { get; set; }
		public string UrlEndpoint { get; set; }
		public string DefaultContentType { get; set; }
		public bool UseStreaming { get; set; }

		public ProviderBase()
		{
			Credential = new NetworkCredential(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["password"]);
			UrlRoot = ConfigurationManager.AppSettings["urlRoot"];
			DefaultEncoding = ConfigurationManager.AppSettings["defaultEncoding"];
			DefaultContentType = ConfigurationManager.AppSettings["defaultContentType"];
			UseStreaming = false;
		}

		protected HttpWebRequest RequestFactory (string urlEndpoint)
		{
			HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(UrlRoot.FixTrailingSlash() + urlEndpoint));
			webRequest.Accept = "application/json; charset=UTF-8";
			webRequest.ContentType = "application/json";
			return webRequest;
		}

		protected HttpWebRequest RequestFactory (string urlEndpoint, string username, string password)
		{
			HttpWebRequest webRequest = RequestFactory(urlEndpoint);
			webRequest.AddBasicAuthorizationHeader(username, password);
			return webRequest;
		}


		protected HttpWebResponse ExecuteRequest(string url, string method)
		{
			HttpWebRequest request = RequestFactory(url, Credential.UserName, Credential.Password);
			request.Method = method;
			return (HttpWebResponse)request.GetResponse();
		}
		
		protected HttpWebResponse ExecuteRequest (string url, string method, string contentType, byte[] postBody)
		{
			HttpWebRequest request = RequestFactory(url, Credential.UserName, Credential.Password);
			request.Method = method;

#if DEBUG
			if (UseStreaming)
			{
				request.AddStreamingHeader();
			}
#endif
			if (postBody != null)
			{
				// The only content in the body is the label of the node to be created which is
				// the name of the Type (NOT the fully-qualified name).
				request.ContentType = contentType;
				request.ContentLength = postBody.Length;

				// Write the body as a stream.  Since the body consists only of the Type name
				// it is appropriate to write the entire body in a single stream operation.
				Stream requestStream = request.GetRequestStream();
				requestStream.Write(postBody, 0, postBody.Length);
			}

			return (HttpWebResponse)request.GetResponse();

		}

		protected T ConvertResponseStream<T>(HttpWebResponse response)
		{
			T result;
			using (var responseStream = response.GetResponseStream())
			{
				using (StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding(DefaultEncoding)))
				{
					var jsonString = streamReader.ReadToEnd();
					result = JsonConvert.DeserializeObject<T>(jsonString);
				}
			}
			return result;
		}

	}
}
