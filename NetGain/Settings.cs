using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetGain
{
	public static class Settings
	{
		public static string UrlRoot
		{
			get
			{
				return ConfigurationManager.AppSettings["urlRoot"].ToString();
			}
		}

		public static string Username
		{
			get
			{
				return ConfigurationManager.AppSettings["username"].ToString();
			}
		}

		public static string Password
		{ 
			get
			{
				return ConfigurationManager.AppSettings["password"].ToString();
			}
		}

		public static string DefaultEncoding
		{
			get
			{
				return ConfigurationManager.AppSettings["defaultEncoding"].ToString();
			}
		}

		public static string DefaultContentType
		{
			get
			{
				return ConfigurationManager.AppSettings["defaultContentType"].ToString();
			}
		}
	}
}
