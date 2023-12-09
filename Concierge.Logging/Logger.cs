// <copyright file="Logger.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Concierge.Logging.Enums;
    using Newtonsoft.Json;

    /// <summary>
    /// Abstract class representing a logger with different log levels and log output handling.
    /// </summary>
    public abstract class Logger : IDisposable
    {
        private readonly LogVerbosity logVerbosity;
        private readonly Queue<Action> queue = new ();
        private readonly ManualResetEvent hasNewItems = new (false);
        private readonly ManualResetEvent terminate = new (false);
        private readonly ManualResetEvent waiting = new (false);
        private readonly Thread loggingThread;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        /// <param name="isDebug">Indicates whether the logger is running in debug mode.</param>
        protected Logger(bool isDebug)
        {
            this.logVerbosity = LogVerbosity.Full;

            this.loggingThread = new Thread(new ThreadStart(this.ProcessQueue)) { IsBackground = true };
            this.loggingThread.Start();

            this.IsDebug = isDebug;
            this.IsStarted = false;
            this.SessionLog = [];
        }

        /// <summary>
        /// Gets the list of log entries collected during the logger session.
        /// </summary>
        public List<string> SessionLog { get; private set; }

        private bool IsDebug { get; init; }

        private bool IsStarted { get; set; }

        /// <summary>
        /// Logs an information message.
        /// </summary>
        /// <param name="message">The information message to log.</param>
        public void Info(string message)
        {
            this.Log(message, LogType.INF);
        }

        /// <summary>
        /// Logs an information message with the details of an object serialized as JSON.
        /// </summary>
        /// <param name="obj">The object to log.</param>
        public void Info(object obj)
        {
            try
            {
                this.Log($"{obj.GetType()} - {JsonConvert.SerializeObject(obj)}", LogType.INF);
            }
            catch (Exception ex)
            {
                // Swallow the error
                this.Error(ex);
            }
        }

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">The debug message to log.</param>
        public void Debug(string message)
        {
            this.Log(message, LogType.DBG);
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        public void Warning(string message)
        {
            this.Log(message, LogType.WRN);
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        public void Error(string message)
        {
            this.Log(message, LogType.ERR);
        }

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="e">The exception to log.</param>
        public void Error(Exception e)
        {
            if (this.logVerbosity != LogVerbosity.None)
            {
                this.Log(this.UnwrapExceptionMessages(e), LogType.ERR);
            }
        }

        /// <summary>
        /// Logs a new line.
        /// </summary>
        public void NewLine()
        {
            this.Log();
        }

        /// <summary>
        /// Starts the logger session.
        /// </summary>
        /// <param name="version">The version of the application.</param>
        public void Start(string version)
        {
            if (this.IsStarted)
            {
                return;
            }

            this.NewLine();
            this.Info($"Starting Concierge v{version}{(this.IsDebug ? " - Debug" : string.Empty)}");
            this.Info($"WiFi Connected: {SystemUtility.HasInternet}");
            this.Info($"Starting {SystemUtility.GetBatteryStatus()}");
            this.IsStarted = true;
        }

        /// <summary>
        /// Stops the logger session.
        /// </summary>
        public void Stop()
        {
            if (!this.IsStarted)
            {
                return;
            }

            this.Info($"Stopping Concierge{(this.IsDebug ? " - Debug" : string.Empty)}");
            this.Info($"Stopping {SystemUtility.GetBatteryStatus()}");
            this.IsStarted = false;
        }

        public override string ToString()
        {
            return $"Logger settings: [Type: {this.GetType().Name}, Verbosity: {this.logVerbosity}, ";
        }

        /// <summary>
        /// Flushes the logger, waiting for any remaining logs to be processed.
        /// </summary>
        public void Flush()
        {
            this.waiting.WaitOne();
        }

        public void Dispose()
        {
            this.terminate.Set();
            this.loggingThread.Join();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Creates a log entry from the specified message.
        /// </summary>
        /// <param name="message">The log message to be created.</param>
        protected abstract void CreateLog(string message);

        /// <summary>
        /// Composes a log row from the specified message and log type.
        /// </summary>
        /// <param name="message">The log message to be composed.</param>
        /// <param name="logType">The type of log entry.</param>
        /// <returns>The composed log row.</returns>
        protected virtual string ComposeLogRow(string message, LogType logType)
        {
            return $"[{ConciergeDateTime.LoggingNow} {logType}] - {message}";
        }

        /// <summary>
        /// Unwraps exception messages recursively, including inner exceptions.
        /// </summary>
        /// <param name="ex">The exception to unwrap.</param>
        /// <returns>The unwrapped exception messages.</returns>
        protected virtual string UnwrapExceptionMessages(Exception? ex)
        {
            if (ex == null)
            {
                return string.Empty;
            }

            return $"{ex}, Inner exception: {this.UnwrapExceptionMessages(ex.InnerException)} ";
        }

        private void ProcessQueue()
        {
            while (true)
            {
                this.waiting.Set();

                var i = WaitHandle.WaitAny([this.hasNewItems, this.terminate]);

                if (i == 1)
                {
                    return;
                }

                this.hasNewItems.Reset();
                this.waiting.Reset();

                Queue<Action> queueCopy;
                lock (this.queue)
                {
                    queueCopy = new Queue<Action>(this.queue);
                    this.queue.Clear();
                }

                foreach (var log in queueCopy)
                {
                    log();
                }
            }
        }

        private void Log(string message, LogType logType)
        {
            if (message.IsNullOrWhiteSpace())
            {
                return;
            }

            var logRow = this.ComposeLogRow(message, logType);
            this.SessionLog.Add(logRow);

            if (this.IsDebug)
            {
                System.Diagnostics.Debug.WriteLine(logRow);
            }

            if (this.logVerbosity == LogVerbosity.Full)
            {
                lock (this.queue)
                {
                    this.queue.Enqueue(() => this.CreateLog(logRow));
                }

                this.hasNewItems.Set();
            }
        }

        private void Log()
        {
            lock (this.queue)
            {
                this.queue.Enqueue(() => this.CreateLog(string.Empty));
            }

            this.hasNewItems.Set();
        }
    }
}
