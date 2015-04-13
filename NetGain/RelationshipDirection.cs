using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetGain
{
	public abstract class RelationshipDirection
	{
		public string Name
		{
			get
			{
				return this.GetType().Name.Replace("RelationshipDirection", "");
			}
		}
	}

	public class IncomingRelationshipDirection : RelationshipDirection
	{

	}

	public class OutgoingRelationshipDirection : RelationshipDirection
	{

	}

	public class BidirectionalRelationshipDirection : RelationshipDirection
	{

	}
}
