using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace NetGain.Test
{
	[TestClass]
	public class RelationshipTests
	{
		private NodeProvider _nodeProvider = new NodeProvider();
		private RelationshipProvider _relationshipProvider = new RelationshipProvider();

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
		}

		private Person TestPerson
		{
			get
			{
				return new Person
				{
					born = 1964
					, name = "Keanu Reeves"
				};
			}
		}

		private Movie TestMovie
		{
			get
			{
				return new Movie
				{
					released = 1999
					, tagline = "Welcome to the Real World"
					, title = "The Matrix"
				};
			}
		}

		[TestMethod]
		public void CreateRelationship_ActorMovieNoProperties_ExpectSuccess()
		{
			Node personNode = _nodeProvider.Create(TestPerson);
			Node movieNode = _nodeProvider.Create(TestMovie);
			Relationship relationship = _relationshipProvider.Create(personNode, "ACTED_IN", movieNode);
			Relationship getRel = _relationshipProvider.Get(relationship.id);
			Assert.AreEqual(relationship.id, getRel.id);
			Assert.AreEqual(relationship.type, getRel.type);
			Assert.AreEqual(getRel.type, "ACTED_IN");
		}

		[TestMethod]
		public void CreateRelationship_WithProperties_ExpectSuccess()
		{
			Node personNode = _nodeProvider.Create(TestPerson);
			Node movieNode = _nodeProvider.Create(TestMovie);
			dynamic props = new { createdBy = "B-Dogg", createStamp = DateTime.UtcNow.ToString() };
			Relationship relationship = _relationshipProvider.Create(personNode, "ACTED_IN", movieNode, props);
			Relationship getRel = _relationshipProvider.Get(relationship.id);
			Assert.AreEqual(relationship.id, getRel.id);
			Assert.AreEqual(relationship.type, getRel.type);
			Assert.AreEqual(getRel.type, "ACTED_IN");
		}

		[TestMethod]
		public void GetRelationship_FromNodeNoTypeSpecified_ExepctOneItem()
		{
			Node personNode = _nodeProvider.Create(TestPerson);
			Node movieNode = _nodeProvider.Create(TestMovie);
			Relationship relationship = _relationshipProvider.Create(personNode, "ACTED_IN", movieNode);
			var result = _relationshipProvider.Get(personNode, new string[] { });
			Assert.AreEqual(1, result.Count());
		}

		[TestMethod]
		public void GetRelationship_FromNodeTypeSpecified_ExepctOneItem()
		{
			Node personNode = _nodeProvider.Create(TestPerson);
			Node movieNode = _nodeProvider.Create(TestMovie);
			Relationship relationship = _relationshipProvider.Create(personNode, "ACTED_IN", movieNode);
			var result = _relationshipProvider.Get(personNode, new string[] { "ACTED_IN" });
			Assert.AreEqual(1, result.Count());
		}

		[TestMethod]
		public void GetProperty_PropertyExists_ExpectSuccess()
		{
			Node personNode = _nodeProvider.Create(TestPerson);
			Node movieNode = _nodeProvider.Create(TestMovie);
			dynamic props = new { createdBy = "B-Dogg", createStamp = DateTime.UtcNow.ToString() };
			Relationship relationship = _relationshipProvider.Create(personNode, "ACTED_IN", movieNode, props);
			var result = _relationshipProvider.GetProperty(relationship.id, "createdBy");
			Assert.AreEqual("B-Dogg", result);
		}

		[TestMethod]
		public void GetProperties_AllProperties_ExpectSuccess()
		{
			Node personNode = _nodeProvider.Create(TestPerson);
			Node movieNode = _nodeProvider.Create(TestMovie);
			string timestamp = DateTime.UtcNow.ToString();
			dynamic props = new { createdBy = "B-Dogg", createStamp = timestamp };
			Relationship relationship = _relationshipProvider.Create(personNode, "ACTED_IN", movieNode, props);
			dynamic result = _relationshipProvider.GetProperties(relationship.id);
			Assert.AreEqual("B-Dogg", result.createdBy.ToString());
			Assert.AreEqual(timestamp, result.createStamp.ToString());
		}

		[TestMethod]
		public void SetProperty_CreatedBy_ExpectSuccess()
		{
			Node personNode = _nodeProvider.Create(TestPerson);
			Node movieNode = _nodeProvider.Create(TestMovie);
			string timestamp = DateTime.UtcNow.ToString();
			dynamic props = new { createdBy = "B-Dogg", createStamp = timestamp };
			Relationship relationship = _relationshipProvider.Create(personNode, "ACTED_IN", movieNode, props);
			_relationshipProvider.SetProperty(relationship.id, "createdBy", "Brian");
			var result = _relationshipProvider.GetProperty(relationship.id, "createdBy");
			Assert.AreEqual("Brian", result);
		}

		[TestMethod]
		public void SetProperties_AllProperties_ExpectSuccess()
		{
			Node personNode = _nodeProvider.Create(TestPerson);
			Node movieNode = _nodeProvider.Create(TestMovie);
			string timestamp = DateTime.UtcNow.ToString();
			dynamic props = new { createdBy = "B-Dogg", createStamp = timestamp };
			Relationship relationship = _relationshipProvider.Create(personNode, "ACTED_IN", movieNode, props);
			dynamic newprops = new { createdBy = "Brian", createStamp = DateTime.UtcNow.AddSeconds(1).ToString() };
			_relationshipProvider.SetProperties(relationship.id, newprops);
			dynamic result = _relationshipProvider.GetProperties(relationship.id);
			Assert.AreNotEqual(props.createdBy, result.createdBy.ToString());
			Assert.AreNotEqual(props.createStamp, result.createStamp.ToString());
			Assert.AreEqual(newprops.createdBy, result.createdBy.ToString());
			Assert.AreEqual(newprops.createStamp, result.createStamp.ToString());
		}

		[TestMethod]
		[ExpectedException(typeof(System.Net.WebException))]
		public void DeleteRelationship_ActorMovie_ExpectSuccess()
		{
			Node personNode = _nodeProvider.Create(TestPerson);
			Node movieNode = _nodeProvider.Create(TestMovie);
			Relationship relationship = _relationshipProvider.Create(personNode, "ACTED_IN", movieNode);
			_relationshipProvider.Delete(relationship.id);
			relationship = _relationshipProvider.Get(relationship.id);
		}
	}
}
