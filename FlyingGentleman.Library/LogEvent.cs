using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FlyingGentleman.Library
{
	/// <summary>
	/// A single log event.
	/// </summary>
	[DataContract]
	[Serializable]
	public class LogEvent : MarshalByRefObject
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LogEvent"/> class.
		/// </summary>
		public LogEvent()
		{
		}

		/// <summary>
		/// Creates a log event.
		/// </summary>
		/// <param name="category">The category of the Log Event.  (The name of the class which is being executed.)</param>
		/// <param name="action">The name of the action being executed.</param>
		/// <param name="message">The message from the Action.</param>
		public LogEvent(string category, string action, string message)
		{
			this.DateTime = DateTime.Now;
			this.Category = category;
			this.Action = action;
			this.Message = message;
		}

		/// <summary>
		/// The name of the class which is being executed.
		/// </summary>
		[DataMember]
		public string Category { get; set; }

		/// <summary>
		/// The name of the action being executed.
		/// </summary>
		[DataMember]
		public string Action { get; set; }

		/// <summary>
		/// The message from the Action.
		/// </summary>
		[DataMember]
		public string Message;

		/// <summary>
		/// The Date and Time of the Event.
		/// </summary>
		[DataMember]
		public DateTime DateTime { get; set; }
	}
}
