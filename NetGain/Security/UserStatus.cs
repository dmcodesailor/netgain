using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetGain.Security
{
	public class UserStatus
	{
		public string username { get; set; }
		public string password_change { get; set; }
		public bool password_change_required { get; set; }
	}
}
