using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebInstallation
{
	/// <summary>
	/// It is important that the settings only inherits from a single interface.
	/// </summary>
	public class DevSettings : ISettings
	{
		public string WebsiteName
		{
			get
			{
				return "Development Website";
			}
		}
	}
}
