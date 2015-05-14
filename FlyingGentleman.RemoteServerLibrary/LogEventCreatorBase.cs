using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyingGentleman.Library;
using System.ServiceModel;

namespace FlyingGentleman.RemoteServerLibrary
{
	/// <summary>
	/// The base class for operations which will use WCF messaging to process events.
	/// </summary>
	public class LogEventCreatorBase
	{
		/// <summary>
		/// Logs the fact that an event was created.
		/// </summary>
		/// <param name="e"></param>
		public void LogEventCreated(LogEvent e)
		{
			if (OperationContext.Current != null)
			{
				ILogEventCreator callback = OperationContext.Current.GetCallbackChannel<ILogEventCreator>();
				callback.LogEventCreated(e);
			}
		}
	}
}
