// <copyright file="ConciergeConsole.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;

    using Concierge.Console.Enums;
    using Concierge.Console.Services;
    using Concierge.Persistence;
    using Concierge.Persistence.ReadWriters;
    using Concierge.Tools;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    public sealed class ConciergeConsole : INotifyPropertyChanged
    {
        private readonly string consoleHistoryFile = Path.Combine(ConciergeFiles.AppDataDirectory, ConciergeFiles.ConsoleHistoryName);

        private string consoleInput = Constants.ConsolePrompt;
        private ObservableCollection<ConsoleResult> consoleOutput = new ();

        public ConciergeConsole()
        {
            this.GenerateHeader();

            this.History = new History(HistoryReadWriter.Read(this.consoleHistoryFile), Constants.ConsolePrompt);
            this.WriteOutput = false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

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
                this.ConsoleOutput.Add(ConsoleResult.Empty);
                return;
            }

            this.ConsoleOutput.Add(new ConsoleResult(this.ConsoleInput, ResultType.Information));

            var command = new ConsoleCommand(this.ConsoleInput);
            Program.Logger.Info($"Executing command: {command}");
            if (!command.IsValid)
            {
                this.WriteResult(ConsoleResult.Default(this.ConsoleInput));
                return;
            }

            var result = this.GetScriptService(command.Name).Run(command);
            Program.Logger.Info($"Command result: {result}");

            this.WriteResult(result);
        }

        private static bool IsEmpty(string command)
        {
            return command.Strip(Constants.ConsolePrompt).IsNullOrWhiteSpace();
        }

        private IScriptService GetScriptService(string name)
        {
            if (ListScriptService.ListScripts.Contains(name, StringComparer.InvariantCultureIgnoreCase))
            {
                return new ListScriptService();
            }
            else if (WealthScriptService.WealthScripts.Contains(name, StringComparer.InvariantCultureIgnoreCase))
            {
                return new WealthScriptService();
            }
            else if (ReadWriterScriptService.ReadWriteScripts.Contains(name, StringComparer.InvariantCultureIgnoreCase))
            {
                return new ReadWriterScriptService();
            }
            else if (name.Equals("Clear", StringComparison.InvariantCultureIgnoreCase))
            {
                this.ConsoleOutput.Clear();
                this.GenerateHeader();
                this.WriteOutput = true;
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

            if (!this.WriteOutput)
            {
                this.ConsoleOutput.Add(result);
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
    }
}
