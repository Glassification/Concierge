// <copyright file="GitUtility.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Utilities
{
    using System.Diagnostics;

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

            set
            {
                _branchName = value;
            }
        }

        private static bool IsInitialized { get; set; }

        public static void Initialize()
        {
            if (IsInitialized)
            {
                return;
            }

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
            IsInitialized = true;
        }
    }
}
