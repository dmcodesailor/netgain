using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetGain.interfaces
{
	public interface INodeProvider //: IProvider
	{
		INode<T> Create<T>(T entity);
		INode<T> Get<T>(long id);
		IEnumerable<INode<T>> Get<T>();
		IEnumerable<INode<T>> Get<T>(string[] labels);
		INode<T> Update<T>(T entity);
		INode<T> Delete<T>(long id);
		INode<T> Delete<T>(T entity);
	}
}
