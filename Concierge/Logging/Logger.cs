// <copyright file="Logger.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using Concierge.Logging.Enums;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;
    using Newtonsoft.Json;

    public abstract class Logger : IDisposable
    {
        private readonly LogVerbosity logVerbosity;
        private readonly Queue<Action> queue = new ();
        private readonly ManualResetEvent hasNewItems = new (false);
        private readonly ManualResetEvent terminate = new (false);
        private readonly ManualResetEvent waiting = new (false);
        private readonly Thread loggingThread;

        protected Logger(bool isDebug)
        {
            this.logVerbosity = LogVerbosity.Full;

            this.loggingThread = new Thread(new ThreadStart(this.ProcessQueue)) { IsBackground = true };
            this.loggingThread.Start();

            this.IsDebug = isDebug;
        }

        private bool IsDebug { get; init; }

        public void Info(string message)
        {
            this.Log(message, LogType.INF);
        }

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

        public void Debug(string message)
        {
            this.Log(message, LogType.DBG);
        }

        public void Warning(string message)
        {
            this.Log(message, LogType.WRN);
        }

        public void Error(string message)
        {
            this.Log(message, LogType.ERR);
        }

        public void Error(Exception e)
        {
            if (this.logVerbosity != LogVerbosity.None)
            {
                this.Log(this.UnwrapExceptionMessages(e), LogType.ERR);
            }
        }

        public void NewLine()
        {
            this.Log();
        }

        public override string ToString()
        {
            return $"Logger settings: [Type: {this.GetType().Name}, Verbosity: {this.logVerbosity}, ";
        }

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

        protected abstract void CreateLog(string message);

        protected virtual string ComposeLogRow(string message, LogType logType)
        {
            return $"[{ConciergeDateTime.LoggingNow} {logType}] - {message}";
        }

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

        private void Log(string message, LogType logType)
        {
            if (message.IsNullOrWhiteSpace())
            {
                return;
            }

            var logRow = this.ComposeLogRow(message, logType);

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
