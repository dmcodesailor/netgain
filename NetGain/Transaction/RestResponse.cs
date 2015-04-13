using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetGain.Transaction
{
	public class RestResponse
	{
		public string outgoing_relationships { get; set; }
		public string labels { get; set; }
		public string traverse { get; set; }
		public string all_typed_relationships { get; set; }
		public string self { get; set; }
		public string property { get; set; }
		public string properties { get; set; }
		public string outgoing_typed_relationships { get; set; }
		public string incoming_relationships { get; set; }
		public string create_relationship { get; set; }
		public string paged_traverse { get; set; }
		public string all_relationships { get; set; }
		public string incoming_typed_relationships { get; set; }

		public RestResponseMetadata metadata { get; set; }

		public virtual dynamic data { get; set; }
	}
}
