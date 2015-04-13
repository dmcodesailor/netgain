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
	public class DatabaseProvider : ProviderBase
	{
		public DatabaseProvider()
		{
			UrlEndpoint = string.Empty;
		}

		public IEnumerable<string> Labels()
		{
			HttpWebResponse response = ExecuteRequest(UrlRoot.FixTrailingSlash() + "labels", "GET");
			IEnumerable<string> result = ConvertResponseStream<IEnumerable<string>>(response);
			return result;
		}

		public IEnumerable<string> RelationshipTypes()
		{
			HttpWebResponse response = ExecuteRequest(UrlRoot.FixTrailingSlash() + "relationship/types", "GET");
			IEnumerable<string> result = ConvertResponseStream<IEnumerable<string>>(response);
			return result;
		}

		public IEnumerable<string> PropertyKeys()
		{
			HttpWebResponse response = ExecuteRequest(UrlRoot.FixTrailingSlash() + "propertykeys", "GET");
			IEnumerable<string> result = ConvertResponseStream<IEnumerable<string>>(response);
			return result;
		}
	}
}
