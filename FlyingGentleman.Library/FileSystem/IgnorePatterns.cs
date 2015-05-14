using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;

namespace FlyingGentleman.Library.FileSystem
{
	/// <summary>
	/// Regular expressions for common ignore patterns.
	/// </summary>
	public static class IgnorePatterns
	{
		/// <summary>
		/// Ignore the ASP.Net Web.Config file.
		/// </summary>
		public static IgnoreExpression WebConfigFile = new IgnoreExpression(@"^.+\\[Ww]eb\.[Cc]onfig$", false);

		/// <summary>
		/// Ignore the App_Offline.htm file.
		/// </summary>
		public static IgnoreExpression AppOfflineFile = new IgnoreExpression(@"^.+\\[Aa]pp_[Oo]ffline\\.htm$", false);

		/// <summary>
		/// Ignore directories which end in ".svn";
		/// </summary>
		public static IgnoreExpression SvnDirectory = new IgnoreExpression(@"^.+\\.svn$", false);
	}
}
