using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetGain.Transaction
{
	public class TransactionResults
	{
		public string[] columns { get; set; }
		public IEnumerable<Row> data { get; set; }
	}
}
