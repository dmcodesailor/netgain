using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetGain.Transaction;


namespace NetGain.Test
{
    [TestClass]
    public class TransactionManagerTests
    {
        [TestMethod]
        public void TestTransactionManager_Delete()
        {
            // Arrange
            CreateMoveNode(new Movie { released = 2015, tagline = "Ultron lives!", title = "Avengers: Age of Ultron" });
            CreateMoveNode(new Movie { released = 2013, tagline = "Avengers", title = "Marvel's The Avengers" });

            // Act
            DeleteAllRelationshipsAndNodes();

            // Assert
            var txMgr = new TransactionManager();
            var stmt = new Statement {statement = "Match (n:Movie) return n"};
            var tx = txMgr.Begin(new[] {stmt});
            var data = (dynamic)tx.results[0].data;
            
            Assert.AreEqual(0, data.Count);
        }

        [TestMethod]
        public void TestTransactionManager_MatchNReturnN_ShouldReturnAllNodes()
        {
            // Arrange
            DeleteAllRelationshipsAndNodes();

            CreateMoveNode(new Movie { released = 2015, tagline = "Ultron lives!", title = "Avengers: Age of Ultron" });
            CreateMoveNode(new Movie { released = 2013, tagline = "Avengers", title = "Marvel's The Avengers" });
            var txMgr = new TransactionManager();
            var stmt = new Statement { statement = "Match (n:Movie) return n" };

            // Act
            var tx = txMgr.Begin(new[] {stmt});

            // Assert
            var data = (dynamic) tx.results[0].data;
            Assert.AreEqual(2, data.Count);
            Assert.AreEqual(2015, data[0].row[0].released.Value);
            Assert.AreEqual(2013, data[1].row[0].released.Value);
        }

        [TestMethod]
        public void TestTransactionManager_StatementWithParams_ShouldExecuteSuccessfully()
        {
            // Arrange
            DeleteAllRelationshipsAndNodes();

            var txMgr = new TransactionManager();
            var parm = new Parameter {props = new List<dynamic> {new {title = "The Matrix", released = 1999}}};
            var stmt = new Statement {statement = "CREATE (n:Movie {props}) return n", parameters = parm};

            // Act
            var tx = txMgr.Begin(new[] { stmt });
            txMgr.Commit(tx);

            // Assert
            var data = ReadAllMovieNodes();

            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1999, data[0].row[0].released.Value);
            Assert.AreEqual("The Matrix", data[0].row[0].title.Value);
        }

        private static dynamic ReadAllMovieNodes()
        {
            var txMgr = new TransactionManager();
            var stmt = new Statement {statement = "Match (n:Movie) return n"};
            var tx = txMgr.Begin(new[] {stmt});

            dynamic data = tx.results[0].data;
            return data;
        }

        private static void DeleteAllRelationshipsAndNodes()
        {
            var txMgr = new TransactionManager();
            var deleteRelationshipsStatement = new Statement {statement = "MATCH ()-[r]-() DELETE r"};
            var deleteNodesStatement = new Statement {statement = "MATCH (n) DELETE n"};
            var deleteTx = txMgr.Begin(new[] {deleteRelationshipsStatement, deleteNodesStatement});
            txMgr.Commit(deleteTx);
        }

        private static void CreateMoveNode(Movie movie)
        {
            var nodeProvider1 = new NodeProvider();
            var labelProvider1 = new LabelProvider();
            var movie1 = movie;
            var node1 = nodeProvider1.Create(movie1);
            labelProvider1.Add(node1, "Movie");
        }
    }
}

