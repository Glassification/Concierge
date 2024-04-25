// <copyright file="ConciergeConsole.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;

    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Console.Enums;
    using Concierge.Console.Services;
    using Concierge.Persistence.ReadWriters;
    using Concierge.Tools;

    /// <summary>
    /// Represents a console for executing commands in the Concierge application.
    /// </summary>
    public sealed class ConciergeConsole : INotifyPropertyChanged
    {
        private readonly ConsoleReadWriter consoleReadWriter;
        private readonly HistoryReadWriter historyReadWriter;

        private readonly string consoleHistoryFile = Path.Combine(ConciergeFiles.HistoryDirectory, ConciergeFiles.ConsoleHistoryName);
        private readonly string consoleOutputFile = Path.Combine(ConciergeFiles.AppDataDirectory, ConciergeFiles.ConsoleOutput);

        private string consoleInput = Constants.ConsolePrompt;
        private ObservableCollection<ConsoleResult> consoleOutput = [];
        private bool writeOutput;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConciergeConsole"/> class.
        /// </summary>
        public ConciergeConsole()
        {
            this.consoleReadWriter = new ConsoleReadWriter(Program.ErrorService);
            this.historyReadWriter = new HistoryReadWriter(Program.ErrorService);

            this.GenerateHeader();
            this.LoadConsoleOutput();

            this.History = new History(this.historyReadWriter.ReadList<string>(this.consoleHistoryFile), Constants.ConsolePrompt);
            this.writeOutput = true;
        }

        /// <summary>
        /// Represents the method that will handle the event raised when the console exits.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains no event data.</param>
        public delegate void ExitedEventHandler(object sender, EventArgs e);

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Occurs when the console exits.
        /// </summary>
        public event ExitedEventHandler? Exited;

        /// <summary>
        /// Gets the command history associated with the console.
        /// </summary>
        public History History { get; private set; }

        /// <summary>
        /// Gets or sets the input entered into the console.
        /// </summary>
        public string ConsoleInput
        {
            get => this.consoleInput;
            set
            {
                this.consoleInput = value;
                this.OnPropertyChanged(nameof(this.ConsoleInput));
            }
        }

        /// <summary>
        /// Gets or sets the output displayed in the console.
        /// </summary>
        public ObservableCollection<ConsoleResult> ConsoleOutput
        {
            get => this.consoleOutput;
            set
            {
                this.consoleOutput = value;
                this.OnPropertyChanged(nameof(this.ConsoleOutput));
            }
        }

        /// <summary>
        /// Executes the command entered into the console.
        /// </summary>
        /// <returns>The result type of the executed command.</returns>
        public ResultType Execute()
        {
            if (IsEmpty(this.ConsoleInput))
            {
                this.AddConsoleOutput(ConsoleResult.Empty);
                return ResultType.NotImplemented;
            }

            this.AddConsoleOutput(new ConsoleResult(this.ConsoleInput, ResultType.Information));
            this.writeOutput = true;

            var command = new ConsoleCommand(this.ConsoleInput);
            Program.Logger.Info($"Executing command: {command}");
            if (!command.IsValid)
            {
                this.WriteResult(ConsoleResult.DefaultError(this.ConsoleInput));
                return ResultType.Error;
            }

            var scripts = this.GetScriptService(command);
            var result = RunAllScripts(command, scripts);
            Program.Logger.Info($"Command result: {result}");

            this.WriteResult(result);

            return result.Type;
        }

        private static ConsoleResult RunAllScripts(ConsoleCommand command, List<ScriptService> services)
        {
            var result = ConsoleResult.Empty;

            foreach (var service in services)
            {
                result = service.Run(command);
                if (result.Type != ResultType.NotImplemented)
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
            var ccsCompressionScriptService = new CcsCompressionScriptService();

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

            if (ccsCompressionScriptService.Contains(command.Name))
            {
                matchingScripts.Add(ccsCompressionScriptService);
            }

            if (command.Name.Equals("Help", StringComparison.InvariantCultureIgnoreCase))
            {
                matchingScripts.Add(new HelpScriptService());
            }

            if (command.Name.Equals("Clear", StringComparison.InvariantCultureIgnoreCase))
            {
                this.ClearConsoleOutput();
            }

            if (command.Name.Equals("Exit", StringComparison.InvariantCultureIgnoreCase))
            {
                this.writeOutput = false;
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
            this.historyReadWriter.Append(this.consoleHistoryFile, this.ConsoleInput);
            this.History.Add(this.ConsoleInput);
            this.ConsoleInput = Constants.ConsolePrompt;

            if (this.writeOutput)
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
            this.consoleReadWriter.Append(this.consoleOutputFile, result);
        }

        private void LoadConsoleOutput()
        {
            var list = this.consoleReadWriter.ReadList<ConsoleResult>(this.consoleOutputFile);
            foreach (var item in list)
            {
                this.ConsoleOutput.Add(item);
            }
        }

        private void ClearConsoleOutput()
        {
            this.consoleReadWriter.Clear(this.consoleOutputFile);

            this.ConsoleOutput.Clear();
            this.GenerateHeader();
            this.writeOutput = false;
        }
    }
}
