// <copyright file="GitUtility.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Utilities
{
    using System;
    using System.Diagnostics;

    using Concierge.Exceptions;
    using Concierge.Persistence;

    public static class GitUtility
    {
        private static string? _branchName;

        public static string BranchName
        {
            get
            {
                Initialize();
                return _branchName ?? string.Empty;
            }

            private set
            {
                _branchName = value;
            }
        }

        public static bool IsInitialized { get; private set; }

        public static void Initialize()
        {
            if (IsInitialized || !Program.IsDebug)
            {
                return;
            }

            try
            {
                var startInfo = new ProcessStartInfo("git.exe")
                {
                    UseShellExecute = false,
                    WorkingDirectory = ConciergeFiles.ExecutingDirectory,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    Arguments = "rev-parse --abbrev-ref HEAD",
                };

                var process = new Process
                {
                    StartInfo = startInfo,
                };

                process.Start();
                BranchName = process.StandardOutput.ReadLine() ?? string.Empty;
                process.Dispose();
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(new GitException(ex));
            }

            IsInitialized = true;
        }
    }
}
