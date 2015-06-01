using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetGain;
using NetGain.Generic;
using NetGain.Transaction;
using System.IO;
using NetGain.Streaming;

namespace NetGainConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			Authenticate();
			//CreateNode();
			//RetrieveAllNodesOfType();
			//Graph();
			Streaming();
			Console.ReadLine();
		}

		static void Authenticate()
		{
			//NetGain.Neo4jDriver driver = new Neo4jDriver();
			//driver.Authenticate("neo4j", "armadabob");
			//driver.GetLabels("http://localhost:7474/db/data", "neo4j", "armadabob");
		}

		#region " --- NODES --- "
		
		static void CreateNode ()
		{
			NetGain.Generic.NodeProvider provider = new NetGain.Generic.NodeProvider();
			Node<Person> personNode = provider.Create<Person>();
			Console.WriteLine("NODE: Created node with id = {0}", personNode.id);


			NetGain.Generic.LabelProvider labelProvider = new NetGain.Generic.LabelProvider();
			labelProvider.Add<Person>(personNode, new string[] { "Actor", "Director" });

			foreach (var lbl in labelProvider.Get())
			{
				Console.WriteLine(lbl);
			}

		}

		static void RetrieveNode ()
		{
			NetGain.Generic.NodeProvider provider = new NetGain.Generic.NodeProvider();
			Node<Person> personNode = provider.Get<Person>(2524);
			Person person = personNode.Data;
			Console.WriteLine("{0} born {1}", person.name, person.born);
		}

		static void RetrieveAllNodesOfType ()
		{
			NetGain.Generic.NodeProvider nodeProvider = new NetGain.Generic.NodeProvider();
			foreach (var node in nodeProvider.Get<Person>())
			{
				Person person = node.Data;
				Console.WriteLine("{0} born {1}", person.name, person.born);
			}
		}

		static void UpdateNode ()
		{

		}

		static void DeleteNode ()
		{

		}
		
		#endregion

		#region " --- TRANSACTIONS --- "
		
		static void Statement()
		{
			TransactionManager txMgr = new TransactionManager();
			Statement stmt = new NetGain.Transaction.Statement();
			stmt.statement = "Match (n) return n";
			Transaction tx = txMgr.Begin(new Statement[] { stmt });
			foreach (dynamic obj in tx.results[0].data)
			{
				try
				{
					Console.WriteLine("{0} born {1}", obj.row[0].name, obj.row[0].born);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
		}

		static void StatementWithParams()
		{
			TransactionManager txMgr = new TransactionManager();
			Statement stmt = new NetGain.Transaction.Statement();
			stmt.statement = "CREATE (n {props}) return n";
			Parameter parm = new Parameter();
			parm.props = new List<dynamic>() { new { name = "B the Dogg", born = 1971}};
			stmt.parameters = parm;
			Transaction tx = txMgr.Begin(new Statement[] { stmt });
			foreach (dynamic obj in tx.results[0].data)
			{
				try
				{
					Console.WriteLine("{0} born {1}", obj.row[0].name, obj.row[0].born);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
		}

		static void Commit()
		{
			TransactionManager txMgr = new TransactionManager();
			Transaction tx = txMgr.Begin();
			tx = txMgr.Commit(tx);
			Console.WriteLine(tx.expires);
		}

		static void Graph()
		{
			TransactionManager txMgr = new TransactionManager();
			Statement stmt = new NetGain.Transaction.Statement();
			stmt.statement = "CREATE ( bike:Bike { weight: 10 } ) CREATE ( frontWheel:Wheel { spokes: 3 } ) CREATE ( backWheel:Wheel { spokes: 32 } ) CREATE p1 = (bike)-[:HAS { position: 1 } ]->(frontWheel) CREATE p2 = (bike)-[:HAS { position: 2 } ]->(backWheel) RETURN bike, p1, p2";
			stmt.statement = "MATCH (n)-[r]-() return n, r";
			Transaction tx = txMgr.ExecuteGraph(new Statement[] { stmt });
		}

		static void Rest()
		{
			TransactionManager txMgr = new TransactionManager();
			Transaction tx = txMgr.Begin();
			Statement stmt = new NetGain.Transaction.Statement();
			stmt.statement = "CREATE (n) RETURN n";
			tx = txMgr.ExecuteRestReturn(tx, new Statement[] { stmt });
			Console.WriteLine(tx.results[0].data.ElementAt(0).rest.ElementAt(0).labels);
		}

		static void Errors()
		{
			TransactionManager txMgr = new TransactionManager();
			Transaction tx = txMgr.Begin();
			Statement stmt = new NetGain.Transaction.Statement();
			stmt.statement = "invalid statement";
			tx = txMgr.ExecuteRestReturn(tx, new Statement[] { stmt });
			Console.WriteLine(tx.errors.Count());
		}

		#endregion

		#region " --- STREAMING --- "

		static void Streaming ()
		{
			NetGain.Streaming.LabelProvider lblProvider = new NetGain.Streaming.LabelProvider();
			
			char[] buffer = null;

			StreamReader sr = new StreamReader(lblProvider.Get("Movie"));
			do
			{
				buffer = new char[16];
				sr.Read(buffer, 0, buffer.Length);
				Console.WriteLine(buffer);
			} while (!sr.EndOfStream);
	
		}

		static void StreamingGraph ()
		{
			TransactionManager txMgr = new TransactionManager() { }; // UseStreaming = true };
			
			System.IO.Stream stream = new System.IO.MemoryStream();

			Transaction tx = txMgr.ExecuteGraph(new Statement[] { new Statement() { statement = "MATCH (n)-[r]-() RETURN n, r" } });
			Console.WriteLine("{0} nodes and {1} relationships", tx.results[0].data.ElementAt(0).graph.nodes.Count(), tx.results[0].data.ElementAt(0).graph.relationships.Count());
		}

		#endregion

	}
}
