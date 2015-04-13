using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetGain
{
	public class Relationship
	{
		public dynamic extensions { get; set; }

		public string start { get; set; }
		public string property { get; set; }
		public string self { get; set; }
		public virtual string properties { get; set; }
		public string type { get; set; }
		public string end { get; set; }

		public RelationshipMetadata metadata { get; set; }

		public virtual dynamic data { get; set; }		
		
		public long id
		{
			get
			{
				return metadata.id;
			}
		}
	}
}
