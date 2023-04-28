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
    using Concierge.Display.Enums;
    using Concierge.Logging;
    using Concierge.Tools.Display;

    public sealed class ErrorService
    {
        public ErrorService(Logger logger)
        {
            this.Logger = logger;
        }

        private Logger Logger { get; set; }

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
