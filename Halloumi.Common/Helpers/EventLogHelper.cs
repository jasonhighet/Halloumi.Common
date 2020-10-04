using System;
using System.Diagnostics;

namespace Halloumi.Common.Helpers
{
    /// <summary>
    /// Helper methods for writing to the event log
    /// </summary>
    public static class EventLogHelper
    {
        #region Public Methods

        /// <summary>
        /// Logs an error to the event log.
        /// </summary>
        /// <param name="text">The error message to log.</param>
        public static void LogError(string text)
        {
            WriteToEventLog(text, EventLogEntryType.Error);
        }

        /// <summary>
        /// Logs an error to the event log.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        public static void LogError(Exception exception)
        {
            var text = exception.ToString()
                + Environment.NewLine
                + Environment.NewLine
                + exception.StackTrace;

            WriteToEventLog(text, EventLogEntryType.Error);
        }

        /// <summary>
        /// Logs an error to the event log.
        /// </summary>
        /// <param name="text">The text to log.</param>
        /// <param name="exception">The exception to log.</param>
        public static void LogError(string text, Exception exception)
        {
            text += Environment.NewLine
                + Environment.NewLine
                + exception.ToString()
                + Environment.NewLine
                + Environment.NewLine
                + exception.StackTrace;

            WriteToEventLog(text, EventLogEntryType.Error);
        }

        /// <summary>
        /// Logs text to the event log.
        /// </summary>
        /// <param name="text">The text to log.</param>
        public static void LogInfo(string text)
        {
            WriteToEventLog(text, EventLogEntryType.Information);
        }

        /// <summary>
        /// Logs a warning to the event log.
        /// </summary>
        /// <param name="text">The warning text to log.</param>
        public static void LogWarning(string text)
        {
            WriteToEventLog(text, EventLogEntryType.Warning);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Writes an entry to the event log.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="entryType">Type of entry to log.</param>
        private static void WriteToEventLog(string text, EventLogEntryType entryType)
        {

            const string logName = "Application";
            var logSource = ApplicationHelper.GetTitle();            
            
            try
            {
                if (!EventLog.SourceExists(logSource)) EventLog.CreateEventSource(logSource, logName);
                EventLog.WriteEntry(logSource, text, entryType);     
            }
            catch
            { }
        }

        #endregion
    }
}
