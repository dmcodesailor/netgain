using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetGain.interfaces
{
	public interface IProvider
	{
		INeo4jEntity Create<T>(T entity);
		INeo4jEntity Get<T>(long id);
		IEnumerable<INeo4jEntity> Get<T>();
		IEnumerable<INeo4jEntity> Get<T>(string[] labels);
		INeo4jEntity Update<T>(T entity);
		INeo4jEntity Delete(long id);
		INeo4jEntity Delete<T>(T entity);
	}
}
