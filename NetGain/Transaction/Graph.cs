using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetGain.Transaction
{
	public class Graph
	{
		public IEnumerable<Node> nodes { get; set; }
		public IEnumerable<Relationship> relationships { get; set; }
	}
}
