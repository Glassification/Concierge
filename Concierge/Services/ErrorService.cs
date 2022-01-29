// <copyright file="ErrorService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System;

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

            ex = IsConciergeException(ex) ? ex : new GenericException(ex);
            var severity = GetSeverity(ex);

            switch (severity)
            {
                case Severity.Debug:
                    if (Program.IsDebug)
                    {
                        ShowMessage(ex.Message);
                    }

                    break;
                case Severity.Release:
                    ShowMessage(ex.Message);
                    break;
            }

            this.Logger.Error(ex);
        }

        private static bool IsConciergeException(Exception ex)
        {
            return ex is ConciergeException;
        }

        private static Severity GetSeverity(Exception ex)
        {
            if (ex is ConciergeException conciergeException)
            {
                return conciergeException.Severity;
            }

            return Severity.Release;
        }

        private static void ShowMessage(string message)
        {
            ConciergeMessageBox.Show(
                message,
                "Error",
                ConciergeWindowButtons.Ok,
                ConciergeWindowIcons.Error);
        }
    }
}
