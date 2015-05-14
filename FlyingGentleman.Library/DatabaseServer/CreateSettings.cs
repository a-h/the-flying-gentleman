using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingGentleman.Library.DatabaseServer
{
	/// <summary>
	/// Settings for the database installation.
	/// </summary>

	public class CreateSettings : ModifySettings
    {
        /// <summary>
        /// The name of the entities method we're going to call;
        /// </summary>
        public override string MethodName { get { return "DropAndCreateDatabase"; } }
	}
}
