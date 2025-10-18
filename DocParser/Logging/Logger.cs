using DocParser.Enums;
using System.Text;

namespace DocParser.Logging
{
    public static class Logger
    {
        private static StringBuilder _log = new StringBuilder();

        /// <summary>
        /// Returns the last entry that was added to the log.
        /// </summary>
        /// <returns>Last log entry.</returns>
        public static string GetLastLogEntry()
        {
            var lines = _log.ToString().Split(["\r\n", "\n"], StringSplitOptions.None);
            return lines.Length > 0 ? lines[lines.Length - 1] : string.Empty;
        }

        /// <summary>
        /// Gets the contents of the log.
        /// </summary>
        /// <returns>Log as a string.</returns>
        public static string GetLog()
        {
            return _log.ToString();
        }

        /// <summary>
        /// Clears the log, outputting the last contents.
        /// </summary>
        /// <param name="logDump">Contents of the log prior to clearing.</param>
        public static void ClearLog(out string logDump)
        {
            logDump = GetLog();
            _log.Clear();
        }

        /// <summary>
        /// Logs a message (info).
        /// </summary>
        /// <param name="message">Message to log.</param>
        public static void Log(string message)
        {
            Log(message, LogSeverity.Info);
        }

        /// <summary>
        /// Logs a message with specified severity.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="severity">Message severity.</param>
        public static void Log(string message, LogSeverity severity)
        {
            _log.AppendLine($"{severity.ToString()}: {message}");
        }

        /// <summary>
        /// Logs a message with critical severity.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public static void LogCritical(string message)
        {
            Log(message, LogSeverity.Critical);
        }

        /// <summary>
        /// Logs a message with debug severity.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public static void LogDebug(string message)
        {
            Log(message, LogSeverity.Debug);
        }

        /// <summary>
        /// Logs a message with error severity, optionally including exception.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="exception">Optional exception (default null), from which the exception message will also be logged.</param>
        public static void LogError(string message, Exception? exception = null)
        {
            var errorMsg = exception != null ? $" ({exception.Message})" : string.Empty;
            Log($"{message}{errorMsg}", LogSeverity.Error);
        }

        /// <summary>
        /// Logs a message with warning severity.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public static void LogWarning(string message)
        {
            Log(message, LogSeverity.Warning);
        }
    }
}
