using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetGain.Transaction
{
	public class Row
	{
		public IEnumerable<dynamic> row { get; set; }
		public Graph graph { get; set; }
		public IEnumerable<RestResponse> rest { get; set; }
	}
}
