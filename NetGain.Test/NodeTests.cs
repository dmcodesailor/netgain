using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace NetGain.Test
{
	[TestClass]
	public class NodeTests
	{
		private NodeProvider _nodeProvider = new NodeProvider();

		[TestInitialize]
		public void Init()
		{
			_nodeProvider.Credential = new System.Net.NetworkCredential("neo4j", "armadabob");
			_nodeProvider.DefaultContentType = "application/json";
			_nodeProvider.DefaultEncoding = "UTF-8";
			_nodeProvider.UrlRoot = "http://localhost:7474/db/data/";
		}

		[TestMethod]
		public void CreateNode_NoLabelNoProperties_ExpectSuccess()
		{
			Node newNode = _nodeProvider.Create();
			Node getNode = _nodeProvider.Get(newNode.id);
			Assert.AreEqual(newNode.id, getNode.id);
		}

		[TestMethod]
		public void CreateNode_MovieNode_ExpectSuccess()
		{
			Movie newMovie = new Movie { released = 2015, tagline = "Ultron lives!", title = "Avengers: Age of Ultron" };
			Node newNode = _nodeProvider.Create(newMovie);
			Node getNode = _nodeProvider.Get(newNode.id);
			string jsonString = getNode.data.ToString();
			Movie getMovie = (Movie)JsonConvert.DeserializeObject<Movie>(jsonString);
			Assert.AreEqual(newMovie.released, getMovie.released);
			Assert.AreEqual(newMovie.tagline, getMovie.tagline);
			Assert.AreEqual(newMovie.title, getMovie.title);
		}

		[TestMethod]
		public void GetNodeProperty_MovieReleased_ExpectSuccess()
		{
			Movie newMovie = new Movie { released = 2015, tagline = "Ultron lives!", title = "Avengers: Age of Ultron" };
			Node newNode = _nodeProvider.Create(newMovie);
			int released = Convert.ToInt32(_nodeProvider.Get(newNode.id, "released").ToString());
			Assert.AreEqual(newMovie.released, released);
		}

		[TestMethod]
		public void SetNodeProperty_MovieReleased_ExpectSuccess()
		{
			Movie newMovie = new Movie { released = 2014, tagline = "Ultron lives!", title = "Avengers: Age of Ultron" };
			Node newNode = _nodeProvider.Create(newMovie);
			_nodeProvider.Set(newNode.id, "released", 2015);
			int released = Convert.ToInt32(_nodeProvider.Get(newNode.id, "released").ToString());
			Assert.AreNotEqual(newMovie.released, released);
		}

		[TestMethod]
		public void UpdateNode_MovieNode_ExpectSuccess()
		{
			Movie newMovie = new Movie { released = 2014, tagline = "Ultron lives!", title = "Avengers: Age of Ultron" };
			Node newNode = _nodeProvider.Create(newMovie);
			Movie updateMovie = new Movie { released = 2015, tagline = "I'm going to show you something wonderful.", title = "Age of Ultron" };
			_nodeProvider.Update(newNode.id, updateMovie);
			Node getNode = _nodeProvider.Get(newNode.id);
			Movie getMovie = JsonConvert.DeserializeObject<Movie>(getNode.data.ToString());
			Assert.AreEqual(updateMovie.released, getMovie.released);
			Assert.AreEqual(updateMovie.tagline, getMovie.tagline);
			Assert.AreEqual(updateMovie.title, getMovie.title);
			Assert.AreNotEqual(newMovie.released, getMovie.released);
			Assert.AreNotEqual(newMovie.tagline, getMovie.tagline);
			Assert.AreNotEqual(newMovie.title, getMovie.title);
		}

		[TestMethod]
		[ExpectedException(typeof(System.Net.WebException))]
		public void DeleteNode_MovieNode_ExpectSuccess()
		{
			Movie newMovie = new Movie { released = 2015, tagline = "Ultron lives!", title = "Avengers: Age of Ultron" };
			Node newNode = _nodeProvider.Create(newMovie);
			_nodeProvider.Delete(newNode.id);
			Node getNode = _nodeProvider.Get(newNode.id);
		}
	}
}
