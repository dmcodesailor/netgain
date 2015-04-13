using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetGain
{
	public class Node
	{
		public dynamic extensions {get; set;}
		
		public string outgoing_relationships {get; set;}
		public virtual string labels {get; set;}
		public string traverse { get; set; }
		public string all_typed_relationships { get; set; }
		public string self { get; set; }
		public string property { get; set; }
		public virtual string properties { get; set; }
		public string outgoing_typed_relationships { get; set; }
		public string incoming_relationships { get; set; }
		public string create_relationship { get; set; }
		public string paged_traverse { get; set; }
		public string all_relationships { get; set; }
		public string incoming_typed_relationships { get; set; }

		public NodeMetadata metadata { get; set; }

		public virtual dynamic data { get; set; }

		public string TraverseUrl(string returnType)
		{
			return traverse.Replace("{returnType}", returnType);
		}

		public string AllTypedRelationshipsUrl(string[] types)
		{
			string result = all_typed_relationships;
			result = result.Substring(0, result.LastIndexOf("/") - 1);
			result += String.Join("&", types);
			return result;
		}

		public string PropertyUrl(string key)
		{
			return property.Replace("{key}", key);
		}

		public string OutgoingTypedRelationshipsUrl(string[] types)
		{
			string result = outgoing_typed_relationships;
			result = result.Substring(0, result.LastIndexOf("/") - 1);
			result += String.Join("&", types);
			return result;
		}

		public string PagedTraverseUrl (string returnType, int pageSize, int leaseTime)
		{
			return paged_traverse;
		}

		public string IncomeTypedRelationshipsUrl (string[] types)
		{
			string result = incoming_typed_relationships;
			result = result.Substring(0, result.LastIndexOf("/") - 1);
			result += String.Join("&", types);
			return result;
		}

  //"traverse" : "http://localhost:7474/db/data/node/8/traverse/{returnType}",
  //"all_typed_relationships" : "http://localhost:7474/db/data/node/8/relationships/all/{-list|&|types}",
  //"property" : "http://localhost:7474/db/data/node/8/properties/{key}",
  //"outgoing_typed_relationships" : "http://localhost:7474/db/data/node/8/relationships/out/{-list|&|types}",
  //"paged_traverse" : "http://localhost:7474/db/data/node/8/paged/traverse/{returnType}{?pageSize,leaseTime}",
  //"incoming_typed_relationships" : "http://localhost:7474/db/data/node/8/relationships/in/{-list|&|types}",
  
		public long id
		{
			get
			{
				return metadata.id;
			}
		}
	}
}
