// <copyright file="ErrorService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System;
    using System.Windows;

    using Concierge.Exceptions;
    using Concierge.Exceptions.Enums;
    using Concierge.Interfaces.Enums;
    using Concierge.Logging;
    using Concierge.Tools.Interface;
    using Concierge.Utility;

    public class ErrorService
    {
        public ErrorService(Logger logger)
        {
            Guard.IsNull(logger, nameof(logger));

            this.Logger = logger;
        }

        private Logger Logger { get; set; }

        public void LogError(Exception ex)
        {
            Guard.IsNull(ex, nameof(ex));

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

            this.Logger.Error(conciergeException);
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
                ConciergeWindowButtons.Ok,
                ConciergeWindowIcons.Error);
        }
    }
}
