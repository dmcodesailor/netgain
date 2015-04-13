using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetGain.Transaction
{
	public class Transaction
	{
		public long id { get; set; }
		public string commit { get; set; }
		public TransactionResults[] results { get; set; }
		public DateTime expires { get; set; }
		public IEnumerable<Error> errors { get; set; }
		public dynamic transaction { get; set; }
	}
}
