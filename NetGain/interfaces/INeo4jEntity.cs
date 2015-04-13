using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetGain.interfaces
{
	public interface INeo4jEntity
	{
		long id { get; set; }
		dynamic extensions { get; set; }
	}
}
