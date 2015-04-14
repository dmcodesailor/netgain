using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetGain.Extensions;

namespace NetGain.Transaction
{
	public class TransactionManager : ProviderBase
	{
		public TransactionManager() : base()
		{
			UrlEndpoint = "transaction";
		}

		/// <summary>
		/// If a "Location" header is available in the provided response
		/// object this method extracts the location URL and then strips
		/// everything to the right of the last "/".  This is cast to a
		/// long as this is the transaction ID.
		/// </summary>
		/// <param name="response">HttpWebResponse object containing the
		/// headers to process</param>
		/// <returns>a nullable long containing the transaction ID
		/// or null if no "Location" header is available</returns>
		private long? TransactionIdFromResponseLocation(HttpWebResponse response)
		{
			long? result = null;
			var locationValues = response.Headers.GetValues("Location");
			if (locationValues != null && locationValues.Length > 0)
			{
				var location = response.Headers["Location"];
				var idPortion = location.Substring(location.LastIndexOf("/") + 1);
				result = long.Parse(idPortion);
			}
			return result;
		}

		/// <summary>
		/// The response to a transaction call can include a "transaction" JSON
		/// object.  This is stored in the response as type "dynamic".  This 
		/// method extracts the "expires" property of the transaction object
		/// and attempts to convert it to a DateTime.
		/// </summary>
		/// <param name="transaction">the dynamic (JSON) object from which
		/// the expires property is read</param>
		/// <returns>a nullable DateTime containing the expires stamp of
		/// the transaction or null if it cannot be parsed</returns>
		private DateTime? ExpiresStampFromDynamic (dynamic transaction)
		{
			DateTime? result = null;
			if (transaction != null)
			{
				DateTime tryResult;
				if (DateTime.TryParse(transaction.expires, out tryResult))
				{
					result = tryResult;
				}
			}
			return result;
		}

		private Transaction InvokeTransactionPostRequest(IEnumerable<Statement> statements, string endpointExtensions)
		{
			// Change the endpoint for the specified operation - execute statements and commit.
			UrlEndpoint = String.Concat(UrlEndpoint.FixTrailingSlash(), endpointExtensions);

			// Modify the JSON string to wrap the individual statements in a "statement" property of a JSON object.
			var jsonEntity = JsonConvert.SerializeObject(statements);
			jsonEntity = String.Concat("{ \"statements\" : ", jsonEntity, " } ");

			// NOTE: Parameters are automatically handled during serialization.
			// However, a more robust approach may be beneficial.  Consider implementing this in the Generic namespace.

			// Proceed with executing the transaction.
			byte[] postBody = Encoding.GetEncoding(DefaultEncoding).GetBytes(jsonEntity);
			HttpWebResponse response = (HttpWebResponse)ExecuteRequest(UrlEndpoint, "POST", DefaultContentType, postBody);
			Transaction result = ConvertResponseStream<Transaction>(response);

			// Parse the response headers (Location header) to extract the transaction ID.
			var id = TransactionIdFromResponseLocation(response);
			result.id = id.HasValue ? id.Value : 0;
			// Parse the transaction dynamic object in the response 
			DateTime? expiresStamp = ExpiresStampFromDynamic(result.transaction);
			result.expires = expiresStamp.HasValue ? expiresStamp.Value : DateTime.MinValue;

			return result;
		}

		/// <summary>
		/// Open a transaction.
		/// </summary>
		/// <returns>TransactionResponse</returns>
		public Transaction Begin()
		{
			return Begin(new Statement[] { });
		}

		public Transaction Begin(string statement)
		{
			return Begin(new Statement[] { new Statement(){ statement = statement} });
		}

		/// <summary>
		/// Open a transaction and execute one or more statements immediately.
		/// </summary>
		/// <param name="statements"></param>
		/// <returns>TransactionResponse</returns>
		public Transaction Begin(IEnumerable<Statement> statements)
		{
			return InvokeTransactionPostRequest(statements, string.Empty);
		}

		/// <summary>
		/// Commit an existing transaction.
		/// </summary>
		/// <param name="transaction"></param>
		/// <returns>TransactionResponse</returns>
		public Transaction Commit(Transaction transaction)
		{
			return Commit(transaction, new Statement[]{});
		}

		/// <summary>
		/// Commit an existing transaction after executing the specified statements.
		/// </summary>
		/// <param name="transaction"></param>
		/// <param name="statements"></param>
		/// <returns>TransactionResponse</returns>
		public Transaction Commit(Transaction transaction, IEnumerable<Statement> statements)
		{
			if (transaction == null)
				throw new ArgumentNullException("transaction");

			return InvokeTransactionPostRequest(statements, transaction.id.ToString() + "/commit");
		}

		/// <summary>
		/// Execute one or more specified statements using the specified transaction.
		/// </summary>
		/// <param name="transaction"></param>
		/// <param name="statements"></param>
		/// <returns></returns>
		public Transaction Execute(Transaction transaction, IEnumerable<Statement> statements)
		{
			if (transaction == null)
				throw new ArgumentNullException("transaction");

			return InvokeTransactionPostRequest(statements, transaction.id.ToString());
		}

		/// <summary>
		/// Create a new transaction, execute one or more statements against it, and immediately commit the transaction.
		/// </summary>
		/// <param name="statements"></param>
		/// <returns></returns>
		public Transaction ExecuteCommit(IEnumerable<Statement> statements)
		{
			return InvokeTransactionPostRequest(statements, "commit");
		}

		/// <summary>
		/// Execute one or more specified statements using the specified transaction
		/// and return the results in REST format.
		/// </summary>
		/// <param name="transaction"></param>
		/// <param name="statements"></param>
		/// <returns></returns>
		public Transaction ExecuteRestReturn(Transaction transaction, IEnumerable<Statement> statements)
		{
			foreach (var stmt in statements)
			{
				stmt.resultDataContents = new string[] { "REST" };
			}
			Transaction result = InvokeTransactionPostRequest(statements, transaction.id.ToString());
			if (result.id <= 0)
			{
				result.id = transaction.id;
			}
			return result;
		}

		/// <summary>
		/// Execute one or more specified statements using the specified transaction
		/// and return the results in graph format.
		/// </summary>
		/// <param name="statements"></param>
		/// <returns></returns>
		public Transaction ExecuteGraph(IEnumerable<Statement> statements)
		{
			foreach (var stmt in statements)
			{
				stmt.resultDataContents = new string[] { "row", "graph" };
			}
			return ExecuteCommit(statements);
		}

		/// <summary>
		/// Rollback the specified transaction.  Any further statements trying to run in this transaction will fail immediately.
		/// </summary>
		/// <param name="transaction"></param>
		/// <returns></returns>
		public void Rollback(Transaction transaction)
		{
			if (transaction == null)
				throw new ArgumentNullException("transaction");
			
			// Change the endpoint for the specified operation - execute statements and commit.
			UrlEndpoint = String.Concat("transaction/", transaction.id);
			HttpWebResponse response = (HttpWebResponse)ExecuteRequest(UrlEndpoint, "DELETE");
		}

		/// <summary>
		/// Resets the timeout on the specified transaction.
		/// </summary>
		/// <param name="transaction"></param>
		/// <returns></returns>
		public Transaction ResetTimeout(Transaction transaction)
		{
			if (transaction == null)
				throw new ArgumentNullException("transaction");

			return InvokeTransactionPostRequest(new Statement[]{}, transaction.id.ToString());
		}
	}
}
