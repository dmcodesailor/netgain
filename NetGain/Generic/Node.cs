using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetGain.Generic
{
	public class Node<T> : Node
	{
		public new T Data { get; set; }
	}
}
