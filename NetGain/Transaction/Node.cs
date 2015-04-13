using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetGain.Transaction
{
	public class Node : NetGain.Node
	{
		public new string[] labels { get; set;}
		public new dynamic properties { get; set; }
	}
}
