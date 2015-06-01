using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetGain.Extensions;
using System.IO;

namespace NetGain.Streaming
{
	public class DatabaseProvider : ProviderBase 
	{
		public DatabaseProvider()
		{
			UrlEndpoint = string.Empty;
			UseStreaming = true;
		}

		public Stream Labels()
		{
			HttpWebResponse response = ExecuteRequest(UrlRoot.FixTrailingSlash() + "labels", "GET");
			return response.GetResponseStream();
		}

		public Stream RelationshipTypes()
		{
			HttpWebResponse response = ExecuteRequest(UrlRoot.FixTrailingSlash() + "relationship/types", "GET");
			return response.GetResponseStream();
		}

		public Stream PropertyKeys()
		{
			HttpWebResponse response = ExecuteRequest(UrlRoot.FixTrailingSlash() + "propertykeys", "GET");
			return response.GetResponseStream();
		}
	}
}
