﻿// <copyright file="ErrorService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System;
    using System.Diagnostics;

    using Concierge.Exceptions;
    using Concierge.Exceptions.Enums;
    using Concierge.Interface.Enums;
    using Concierge.Interface.HelperUi;
    using Concierge.Logging;
    using Concierge.Utility;

    public class ErrorService
    {
        private readonly ConciergeMessageWindow conciergeMessageWindow = new ConciergeMessageWindow();

        public ErrorService(Logger logger)
        {
            Guard.IsNull(logger, nameof(logger));

            this.Logger = logger;
        }

        private Logger Logger { get; set; }

        public void LogError(Exception ex, Severity severity)
        {
            Guard.IsNull(ex, nameof(ex));

            string message = IsHandledException(ex) ? ex.Message : $"A generic error occured: {ex.Message}";

            switch (severity)
            {
                case Severity.Debug:
#if DEBUG
                    Debug.WriteLine($"{ex.Message}\n{ex.StackTrace}");
                    this.ShowMessage(message);
#endif
                    break;
                case Severity.Release:
                    this.ShowMessage(message);
                    break;
            }

            this.Logger.Error(message);
        }

        private static bool IsHandledException(Exception ex)
        {
            return ex is ConciergeException;
        }

        private void ShowMessage(string message)
        {
            this.conciergeMessageWindow.ShowWindow(
                message,
                "Error",
                MessageWindowButtons.Ok,
                MessageWindowIcons.Error);
        }
    }
}