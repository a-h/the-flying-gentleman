using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingGentleman.Library
{
	/// <summary>
	/// A handler which is used by WCF to route events back to WCF clients over a duplex communication
	/// channel such as TCP bindings.
	/// </summary>
	public class AddToCollectionCallbackHandler : ILogEventCreator
	{
		/// <summary>
		/// The lock object prevents multiple threads from accessing the event collection concurrently.
		/// </summary>
		static object LockObject = new Object();

		/// <summary>
		/// Initializes a new instance of the <see cref="AddToCollectionCallbackHandler"/> class.
		/// </summary>
		/// <param name="events">The event collection to update.</param>
		/// <param name="action">The action to call when an event is added to the collection.</param>
		public AddToCollectionCallbackHandler(LogEventCollection events, Action<LogEvent> action)
		{
			this.Events = events;
			this.Action = action;
		}

		/// <summary>
		/// Fired when an Event is logged by a WCF service.  See
		/// http://www.switchonthecode.com/tutorials/wcf-tutorial-events-and-callbacks
		/// </summary>
		/// <param name="e"></param>
		public void LogEventCreated(LogEvent e)
		{
			lock (AddToCollectionCallbackHandler.LockObject)
			{
				this.Events.Add(e);
			}

			if (this.Action != null)
			{
				this.Action.BeginInvoke(e, null, null);
			}
		}

		/// <summary>
		/// A collection of Events which have been processed by this handler.
		/// </summary>
		/// <value>
		/// The events.
		/// </value>
		public LogEventCollection Events { get; set; }

		/// <summary>
		/// Gets or sets the action which is executed when an Event is created, e.g. 
		/// write to the console.
		/// </summary>
		/// <value>
		/// The action.
		/// </value>
		public Action<LogEvent> Action { get; set; }
	}
}
