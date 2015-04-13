using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetGain.interfaces
{
	public interface IDatabase
	{
		long NodesCount { get; }
		long RelationshipsCount { get; }
		string[] Labels { get; }
	}
}
