using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace NetGain.Test
{
	[TestClass]
	public class LabelTests
	{
		private NodeProvider _nodeProvider = new NodeProvider();
		private RelationshipProvider _relationshipProvider = new RelationshipProvider();
		private LabelProvider _labelProvider = new LabelProvider();

		[TestInitialize]
		public void Init()
		{
			_relationshipProvider.Credential = new System.Net.NetworkCredential("neo4j", "armadabob");
			_relationshipProvider.DefaultContentType = "application/json";
			_relationshipProvider.DefaultEncoding = "UTF-8";
			_relationshipProvider.UrlRoot = "http://localhost:7474/db/data/";

			_nodeProvider.Credential = new System.Net.NetworkCredential("neo4j", "armadabob");
			_nodeProvider.DefaultContentType = "application/json";
			_nodeProvider.DefaultEncoding = "UTF-8";
			_nodeProvider.UrlRoot = "http://localhost:7474/db/data/";

			_labelProvider.Credential = new System.Net.NetworkCredential("neo4j", "armadabob");
			_labelProvider.DefaultContentType = "application/json";
			_labelProvider.DefaultEncoding = "UTF-8";
			_labelProvider.UrlRoot = "http://localhost:7474/db/data/";
		}

		[TestMethod]
		public void AddLabel_SingleLabel_ExpectSuccess()
		{
			Node node = _nodeProvider.Create();
			string[] beforeLabels = _labelProvider.Get(node);
			Assert.IsTrue(beforeLabels.Length == 0);
			_labelProvider.Add(node, "UnitTest");
			string[] afterLabels = _labelProvider.Get(node);
			Assert.IsTrue(afterLabels.Length == 1);
			Assert.AreEqual("UnitTest", afterLabels[0]);
		}

		[TestMethod]
		public void AddLabel_MultipleLabels_ExpectSuccess()
		{
			Node node = _nodeProvider.Create();
			string[] beforeLabels = _labelProvider.Get(node);
			Assert.IsTrue(beforeLabels.Length == 0);
			_labelProvider.Add(node, new string[] {"UnitTest", "ArbitraryLabel"});
			string[] afterLabels = _labelProvider.Get(node);
			Assert.IsTrue(afterLabels.Length == 2);
			Assert.AreEqual("UnitTest", afterLabels[0]);
			Assert.AreEqual("ArbitraryLabel", afterLabels[1]);
		}

		[TestMethod]
		public void GetNodes_MovieDbLabel_ExpectSuccess()
		{
			IEnumerable<Node> nodes = _labelProvider.Get("Person");
			Assert.IsTrue(nodes.Count() > 0);
		}

		[TestMethod]
		public void GetNodes_MovieDbLabelKeanuReevesProperty_ExpectSuccess()
		{
			IEnumerable<Node> nodes = _labelProvider.Get("Person", "name", "Keanu Reeves");
			Assert.IsTrue(nodes.Count() > 0);
		}

		[TestMethod]
		public void SetLabels_ArbitraryNode_ExpectSuccess()
		{
			Node node = _nodeProvider.Create();
			_labelProvider.Add(node, new string[] { "LabelA", "Label2", "LabelIII" });
			string[] beforeLabels = _labelProvider.Get(node);
			_labelProvider.Set(node, new string[] { "A", "2", "III" });
			string[] afterLabels = _labelProvider.Get(node);
			for (int i = 0; i < afterLabels.Length; i++)
			{
				Assert.IsFalse(beforeLabels.Contains(afterLabels[i]));
			}
		}

		[TestMethod]
		public void RemoveLabel_ArbitraryNode_ExpectSuccess()
		{
			Node node = _nodeProvider.Create();
			_labelProvider.Add(node, new string[] { "LabelA", "Label2", "LabelIII" });
			string[] beforeLabels = _labelProvider.Get(node);
			_labelProvider.Remove(node, "Label2");
			string[] afterLabels = _labelProvider.Get(node);
			Assert.IsFalse(afterLabels.Contains("Label2"));
		}
	}
}
