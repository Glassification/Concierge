// <copyright file="ConciergeConsole.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console
{
    using System;
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

            var result = this.GetScriptService(command).Run(command);
            Program.Logger.Info($"Command result: {result}");

            this.WriteResult(result);
        }

        private static bool IsEmpty(string command)
        {
            return command.Strip(Constants.ConsolePrompt).IsNullOrWhiteSpace();
        }

        private ScriptService GetScriptService(ConsoleCommand command)
        {
            var name = command.Name;
            var listScriptService = new ListScriptService();
            var wealthScriptService = new WealthScriptService();
            var readWriterScriptService = new ReadWriterScriptService();

            if (listScriptService.Contains(name))
            {
                return listScriptService;
            }
            else if (wealthScriptService.Contains(name))
            {
                return wealthScriptService;
            }
            else if (readWriterScriptService.Contains(name))
            {
                return readWriterScriptService;
            }
            else if (name.Equals("List", StringComparison.InvariantCultureIgnoreCase))
            {
                return new ListCommandsScriptService();
            }
            else if (name.Equals("Clear", StringComparison.InvariantCultureIgnoreCase))
            {
                this.ClearConsoleOutput();
            }
            else if (name.Equals("Exit", StringComparison.InvariantCultureIgnoreCase))
            {
                this.WriteOutput = false;
                this.Exited?.Invoke(command, new EventArgs());
            }

            return new UnknownScriptService();
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
