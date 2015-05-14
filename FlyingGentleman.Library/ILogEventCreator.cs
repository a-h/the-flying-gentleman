using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace FlyingGentleman.Library
{
	/// <summary>
	/// An interface which defines how events are fired within WCF services.
	/// </summary>
	public interface ILogEventCreator
	{
		/// <summary>
		/// Fired when an Event is logged by a WCF service.  See
		/// http://www.switchonthecode.com/tutorials/wcf-tutorial-events-and-callbacks
		/// </summary>
		[OperationContract(IsOneWay = true)]
		void LogEventCreated(LogEvent e);
	}
}
