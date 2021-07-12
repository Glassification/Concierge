// <copyright file="Logger.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;

    using Concierge.Logging.Enums;
    using Concierge.Utility.Extensions;
    using Newtonsoft.Json;

    public abstract class Logger : IDisposable
    {
        private readonly LogVerbosity logVerbosity;
        private readonly Queue<Action> queue = new Queue<Action>();
        private readonly ManualResetEvent hasNewItems = new ManualResetEvent(false);
        private readonly ManualResetEvent terminate = new ManualResetEvent(false);
        private readonly ManualResetEvent waiting = new ManualResetEvent(false);
        private readonly Thread loggingThread;

        protected Logger()
        {
            this.logVerbosity = LogVerbosity.Full;

            this.loggingThread = new Thread(new ThreadStart(this.ProcessQueue)) { IsBackground = true };
            this.loggingThread.Start();
        }

        /// =========================================
        /// Info()
        /// =========================================
        public void Info(string message)
        {
            this.Log(message, LogType.INF);
        }

        /// =========================================
        /// Info()
        /// =========================================
        public void Info(object obj)
        {
            try
            {
                this.Log($"{obj.GetType()} - {JsonConvert.SerializeObject(obj)}", LogType.INF);
            }
            catch (Exception ex)
            {
                // Swallow the error
                this.Log(ex.Message, LogType.ERR);
            }
        }

        /// =========================================
        /// Debug()
        /// =========================================
        public void Debug(string message)
        {
            this.Log(message, LogType.DBG);
        }

        /// =========================================
        /// Warning()
        /// =========================================
        public void Warning(string message)
        {
            this.Log(message, LogType.WRN);
        }

        /// =========================================
        /// Error()
        /// =========================================
        public void Error(string message)
        {
            this.Log(message, LogType.ERR);
        }

        /// =========================================
        /// Error()
        /// =========================================
        public void Error(Exception e)
        {
            if (this.logVerbosity != LogVerbosity.None)
            {
                this.Log(this.UnwrapExceptionMessages(e), LogType.ERR);
            }
        }

        /// =========================================
        /// NewLine()
        /// =========================================
        public void NewLine()
        {
            this.Log();
        }

        /// =========================================
        /// ToString()
        /// =========================================
        public override string ToString()
        {
            return $"Logger settings: [Type: {this.GetType().Name}, Verbosity: {this.logVerbosity}, ";
        }

        /// =========================================
        /// Flush()
        /// =========================================
        public void Flush()
        {
            this.waiting.WaitOne();
        }

        /// =========================================
        /// Dispose()
        /// =========================================
        public void Dispose()
        {
            this.terminate.Set();
            this.loggingThread.Join();
        }

        /// =========================================
        /// CreateLog()
        /// =========================================
        protected abstract void CreateLog(string message);

        /// =========================================
        /// ComposeLogRow()
        /// =========================================
        protected virtual string ComposeLogRow(string message, LogType logType)
        {
            return $"[{DateTime.Now.ToString(CultureInfo.InvariantCulture)} {logType}] - {message}";
        }

        /// =========================================
        /// UnwrapExceptionMessages()
        /// =========================================
        protected virtual string UnwrapExceptionMessages(Exception ex)
        {
            if (ex == null)
            {
                return string.Empty;
            }

            return $"{ex}, Inner exception: {this.UnwrapExceptionMessages(ex.InnerException)} ";
        }

        /// =========================================
        /// ProcessQueue()
        /// =========================================
        private void ProcessQueue()
        {
            while (true)
            {
                this.waiting.Set();

                var i = WaitHandle.WaitAny(new WaitHandle[] { this.hasNewItems, this.terminate });

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

        /// =========================================
        /// Log()
        /// =========================================
        private void Log(string message, LogType logType)
        {
            if (message.IsNullOrWhiteSpace())
            {
                return;
            }

            var logRow = this.ComposeLogRow(message, logType);

            if (this.logVerbosity == LogVerbosity.Full)
            {
                lock (this.queue)
                {
                    this.queue.Enqueue(() => this.CreateLog(logRow));
                }

                this.hasNewItems.Set();
            }
        }

        /// =========================================
        /// Log()
        /// =========================================
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
