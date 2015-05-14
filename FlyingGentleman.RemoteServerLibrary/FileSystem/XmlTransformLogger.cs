using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.XmlTransform;

namespace FlyingGentleman.RemoteServerLibrary
{
    /// <summary>
    /// logs XML transformation messages
    /// </summary>
    public class XmlTransformLogger : IXmlTransformationLogger
    {
        private Action<string> _log;

        /// <summary>
        /// Creates an XML Transform Logger with the provided log action
        /// </summary>
        public XmlTransformLogger(Action<string> Log)
        {
            _log = Log;
        }

        /// <summary>
        /// Logs a message
        /// </summary>
        public void EndSection(MessageType type, string message, params object[] messageArgs)
        {
            
        }

        /// <summary>
        /// Logs a message
        /// </summary>
        public void EndSection(string message, params object[] messageArgs)
        {

        }

        /// <summary>
        /// Logs a message
        /// </summary>
        public void LogError(string file, int lineNumber, int linePosition, string message, params object[] messageArgs)
        {
            Exception ex = new Exception(string.Format("{0} had an error on line {1} at position {2} with error {3}.", file, lineNumber, linePosition, message));
            _log.Invoke(message);
            throw ex;
        }

        /// <summary>
        /// Logs a message
        /// </summary>
        public void LogError(string file, string message, params object[] messageArgs)
        {
            Exception ex = new Exception(string.Format("{0} had an error {1}.", file, string.Format(message, messageArgs)));
            _log.Invoke(message);
            throw ex;
        }

        /// <summary>
        /// Logs a message
        /// </summary>
        public void LogError(string message, params object[] messageArgs)
        {
            Exception ex = new Exception(string.Format(message, messageArgs));
            _log.Invoke(message);
            throw ex;
        }

        /// <summary>
        /// Logs a message
        /// </summary>
        public void LogErrorFromException(Exception ex, string file, int lineNumber, int linePosition)
        {
            _log.Invoke(string.Format("{0} {1}", ex.Message, ex.StackTrace));
            throw ex;
        }

        /// <summary>
        /// Logs a message
        /// </summary>
        public void LogErrorFromException(Exception ex, string file)
        {
            _log.Invoke(string.Format("{0} {1}", ex.Message, ex.StackTrace));
            throw ex;
        }

        /// <summary>
        /// Logs a message
        /// </summary>
        public void LogErrorFromException(Exception ex)
        {
            _log.Invoke(string.Format("{0} {1}", ex.Message, ex.StackTrace));
            throw ex;
        }

        /// <summary>
        /// Logs a message
        /// </summary>
        public void LogMessage(MessageType type, string message, params object[] messageArgs)
        {

        }

        /// <summary>
        /// Logs a message
        /// </summary>
        public void LogMessage(string message, params object[] messageArgs)
        {

        }

        /// <summary>
        /// Logs a message
        /// </summary>
        public void LogWarning(string file, int lineNumber, int linePosition, string message, params object[] messageArgs)
        {

        }

        /// <summary>
        /// Logs a message
        /// </summary>
        public void LogWarning(string file, string message, params object[] messageArgs)
        {

        }

        /// <summary>
        /// Logs a message
        /// </summary>
        public void LogWarning(string message, params object[] messageArgs)
        {

        }

        /// <summary>
        /// Logs a message
        /// </summary>
        public void StartSection(MessageType type, string message, params object[] messageArgs)
        {

        }

        /// <summary>
        /// Logs a message
        /// </summary>
        public void StartSection(string message, params object[] messageArgs)
        {

        }
    }
}
