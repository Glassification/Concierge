// <copyright file="ErrorService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System;
    using System.Windows;

    using Concierge.Common;
    using Concierge.Common.Enums;
    using Concierge.Common.Exceptions;
    using Concierge.Display;
    using Concierge.Display.Enums;
    using Concierge.Logging;

    /// <summary>
    /// Provides error logging and handling functionality for the application.
    /// </summary>
    public sealed class ErrorService : IErrorService
    {
        private Logger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorService"/> class with the specified logger.
        /// </summary>
        /// <param name="logger">The logger used for error logging.</param>
        public ErrorService(Logger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Logs an error and handles it according to its severity.
        /// </summary>
        /// <param name="ex">The exception to log and handle.</param>
        public void LogError(Exception ex)
        {
            var conciergeException = GetConciergeException(ex);
            switch (conciergeException.Severity)
            {
                case Severity.Debug:
                    if (Program.IsDebug)
                    {
                        ShowMessage(conciergeException);
                    }

                    break;
                case Severity.Release:
                    ShowMessage(conciergeException);
                    break;
            }

            this.logger.Error(conciergeException);
            IsFatalException(conciergeException);
        }

        private static ConciergeException GetConciergeException(Exception ex)
        {
            if (ex is ConciergeException conciergeException)
            {
                return conciergeException;
            }

            return new GenericException(ex);
        }

        private static void IsFatalException(ConciergeException ex)
        {
            if (ex.IsFatal)
            {
                Application.Current.Shutdown();
            }
        }

        private static void ShowMessage(ConciergeException ex)
        {
            ConciergeMessageBox.Show(
                ex.Message,
                ex.IsFatal ? "Fatal Error" : "Error",
                ConciergeButtons.Ok,
                ConciergeIcons.Error);
        }
    }
}
