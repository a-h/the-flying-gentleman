using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyingGentleman.Library.ExtensionMethods;
using System.ServiceModel;

namespace FlyingGentleman.Library
{
	/// <summary>
	/// Creates log events.
	/// </summary>
	public static class LogEventHelper
	{
		/// <summary>
		/// Fires a log event when appropriate.
		/// </summary>
		/// <param name="source">The source of the event.</param>
		/// <param name="action">The action of the event.</param>
		/// <param name="message">The text of the event message.</param>
		public static void CreateEvent(object source, string action, string message)
		{
			CreateEvent(null, source, action, message);
		}

		/// <summary>
		/// Fires a log event when appropriate.
		/// </summary>
		/// <param name="actionLogEvent">An action to take with the new log event.</param>
		/// <param name="source">The source of the event.</param>
		/// <param name="action">The action of the event.</param>
		/// <param name="message">The text of the event message.</param>
		public static void CreateEvent(Action<LogEvent> actionLogEvent, object source, string action, string message)
		{
			if (source != null)
			{
				var logEvent = new LogEvent(source.GetType().Name, action, message);

				if (actionLogEvent != null)
				{
					actionLogEvent.BeginInvoke(logEvent, null, null);
				}

				if (OperationContext.Current != null)
				{
					ILogEventCreator creator = OperationContext.Current.GetCallbackChannel<ILogEventCreator>();
					creator.LogEventCreated(logEvent);
				}
			}
		}

		/// <summary>
		/// Formats a log event for display in the Console.
		/// </summary>
		/// <param name="le">The event to format.</param>
		/// <returns>A console-formatted string.</returns>
		public static string FormatEvent(LogEvent le)
		{
			return le.ToJson();
		}
	}
}
