﻿// <copyright file="ConciergeConsole.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;

    using Concierge.Console.Enums;
    using Concierge.Console.Services;
    using Concierge.Persistence;
    using Concierge.Persistence.ReadWriters;
    using Concierge.Tools;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    public sealed class ConciergeConsole : INotifyPropertyChanged
    {
        private readonly string consoleHistoryFile = Path.Combine(ConciergeFiles.HistoryDirectory, ConciergeFiles.ConsoleHistoryName);
        private readonly string consoleOutputFile = Path.Combine(ConciergeFiles.AppDataDirectory, ConciergeFiles.ConsoleOutput);

        private string consoleInput = Constants.ConsolePrompt;
        private ObservableCollection<ConsoleResult> consoleOutput = new ();

        public ConciergeConsole()
        {
            this.GenerateHeader();
            this.LoadConsoleOutput();

            this.History = new History(HistoryReadWriter.Read(this.consoleHistoryFile), Constants.ConsolePrompt);
            this.WriteOutput = true;
        }

        public delegate void ExitedEventHandler(object sender, EventArgs e);

        public event PropertyChangedEventHandler? PropertyChanged;

        public event ExitedEventHandler? Exited;

        public History History { get; private set; }

        public string ConsoleInput
        {
            get
            {
                return this.consoleInput;
            }

            set
            {
                this.consoleInput = value;
                this.OnPropertyChanged(nameof(this.ConsoleInput));
            }
        }

        public ObservableCollection<ConsoleResult> ConsoleOutput
        {
            get
            {
                return this.consoleOutput;
            }

            set
            {
                this.consoleOutput = value;
                this.OnPropertyChanged(nameof(this.ConsoleOutput));
            }
        }

        private bool WriteOutput { get; set; }

        public void Execute()
        {
            if (IsEmpty(this.ConsoleInput))
            {
                this.AddConsoleOutput(ConsoleResult.Empty);
                return;
            }

            this.AddConsoleOutput(new ConsoleResult(this.ConsoleInput, ResultType.Information));
            this.WriteOutput = true;

            var command = new ConsoleCommand(this.ConsoleInput);
            Program.Logger.Info($"Executing command: {command}");
            if (!command.IsValid)
            {
                this.WriteResult(ConsoleResult.DefaultError(this.ConsoleInput));
                return;
            }

            var scripts = this.GetScriptService(command);
            var result = RunAllScripts(command, scripts);
            Program.Logger.Info($"Command result: {result}");

            this.WriteResult(result);
        }

        private static ConsoleResult RunAllScripts(ConsoleCommand command, List<ScriptService> services)
        {
            var result = ConsoleResult.Empty;

            foreach (var service in services)
            {
                result = service.Run(command);
                if (result.Type != ResultType.Error)
                {
                    break;
                }
            }

            return result;
        }

        private static bool IsEmpty(string command)
        {
            return command.Strip(Constants.ConsolePrompt).IsNullOrWhiteSpace();
        }

        private List<ScriptService> GetScriptService(ConsoleCommand command)
        {
            var matchingScripts = new List<ScriptService>();

            var listScriptService = new ListScriptService();
            var wealthScriptService = new WealthScriptService();
            var readWriterScriptService = new ReadWriterScriptService();
            var listCommandScriptService = new ListCommandsScriptService();

            if (listScriptService.Contains(command.Name))
            {
                matchingScripts.Add(listScriptService);
            }

            if (wealthScriptService.Contains(command.Name))
            {
                matchingScripts.Add(wealthScriptService);
            }

            if (readWriterScriptService.Contains(command.Name))
            {
                matchingScripts.Add(readWriterScriptService);
            }

            if (listCommandScriptService.Contains(command.Name))
            {
                matchingScripts.Add(listCommandScriptService);
            }

            if (command.Name.Equals("Clear", StringComparison.InvariantCultureIgnoreCase))
            {
                this.ClearConsoleOutput();
            }

            if (command.Name.Equals("Exit", StringComparison.InvariantCultureIgnoreCase))
            {
                this.WriteOutput = false;
                this.Exited?.Invoke(command, new EventArgs());
            }

            if (matchingScripts.IsEmpty())
            {
                matchingScripts.Add(new UnknownScriptService());
            }

            return matchingScripts;
        }

        private void GenerateHeader()
        {
            this.ConsoleOutput.Add(new ConsoleResult($"Concierge [Version {Program.AssemblyVersion}]", ResultType.Information));
            this.ConsoleOutput.Add(new ConsoleResult("Starting console...", ResultType.Information));
            this.ConsoleOutput.Add(new ConsoleResult(string.Empty, ResultType.Information));
        }

        private void WriteResult(ConsoleResult result)
        {
            HistoryReadWriter.Write(this.consoleHistoryFile, this.ConsoleInput);
            this.History.Add(this.ConsoleInput);
            this.ConsoleInput = Constants.ConsolePrompt;

            if (this.WriteOutput)
            {
                this.AddConsoleOutput(result);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged is null)
            {
                return;
            }

            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AddConsoleOutput(ConsoleResult result)
        {
            this.ConsoleOutput.Add(result);
            ConsoleReadWriter.Write(this.consoleOutputFile, result);
        }

        private void LoadConsoleOutput()
        {
            var list = ConsoleReadWriter.Read(this.consoleOutputFile);
            foreach (var item in list)
            {
                this.ConsoleOutput.Add(item);
            }
        }

        private void ClearConsoleOutput()
        {
            ConsoleReadWriter.Clear(this.consoleOutputFile);

            this.ConsoleOutput.Clear();
            this.GenerateHeader();
            this.WriteOutput = false;
        }
    }
}