using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetGain.Transaction
{
	public class Statement
	{
		public Statement()
		{
			//parameters = new List<Parameter>();
		}
		public string statement { get; set; }
		public Parameter parameters { get; set; }
		public string[] resultDataContents { get; set; }
	}
}
