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
    using System.Linq;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Items;
    using Concierge.Character.Spellcasting;
    using Concierge.Character.Statuses;
    using Concierge.Console.Enums;
    using Concierge.Console.Scripts;
    using Concierge.Persistence;
    using Concierge.Persistence.ReadWriters;
    using Concierge.Tools;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    public sealed class ConciergeConsole : INotifyPropertyChanged
    {
        public const string Prompt = "CS> ";

        private readonly string consoleHistoryFile = Path.Combine(ConciergeFiles.AppDataDirectory, ConciergeFiles.ConsoleHistoryName);
        private readonly string[] listScripts = new string[]
        {
            "Inventory",
            "Weapons",
            "Ammunition",
            "Spells",
            "MagicClasses",
            "Ability",
            "Language",
            "ClassResource",
            "StatusEffect",
            "All",
        };

        private string consoleInput = Prompt;
        private ObservableCollection<ConsoleResult> consoleOutput = new ();

        public ConciergeConsole()
        {
            this.GenerateHeader();

            this.History = new History(HistoryReadWriter.Read(this.consoleHistoryFile), Prompt);
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

        public void Run()
        {
            var ignoreResult = false;
            var command = this.ConsoleInput;
            var result = new ConsoleResult($"Error: '{command.Strip(Prompt)}' does not contain a valid command.", ResultType.Error);

            if (command.IsNullOrWhiteSpace())
            {
                return;
            }

            this.ConsoleOutput.Add(new ConsoleResult(command, ResultType.Information));
            var script = GetScript(command);

            if (this.listScripts.Contains(script))
            {
                result = RunListScript(command, script);
            }

            if (script.Equals("Clear", StringComparison.InvariantCultureIgnoreCase))
            {
                this.ConsoleOutput.Clear();
                this.GenerateHeader();
                ignoreResult = true;
            }

            HistoryReadWriter.Write(this.consoleHistoryFile, command);
            this.History.Add(command);
            this.ConsoleInput = Prompt;

            if (!ignoreResult)
            {
                this.ConsoleOutput.Add(result);
            }
        }

        private static string GetScript(string command)
        {
            var token = command.Split(' ').LastOrDefault(string.Empty).Split('.').FirstOrDefault(string.Empty);
            return token.FirstLetterToUpperCase();
        }

        private static ConsoleResult RunListScript(string command, string name)
        {
            return name switch
            {
                "Inventory" => new ListScript<Inventory>(Constants.Inventories.ToList(), Program.CcsFile.Character.Inventories, name).Evaluate(command),
                "Weapons" => new ListScript<Weapon>(Constants.Weapons.ToList(), Program.CcsFile.Character.Weapons, name).Evaluate(command),
                "Ammunition" => new ListScript<Ammunition>(Constants.Ammunitions.ToList(), Program.CcsFile.Character.Ammunitions, name).Evaluate(command),
                "Spells" => new ListScript<Spell>(Constants.Spells.ToList(), Program.CcsFile.Character.Spells, name).Evaluate(command),
                "MagicClasses" => new ListScript<MagicClass>(new List<MagicClass>(), Program.CcsFile.Character.MagicClasses, name).Evaluate(command),
                "Ability" => new ListScript<Ability>(Constants.Abilities.ToList(), Program.CcsFile.Character.Abilities, name).Evaluate(command),
                "Language" => new ListScript<Language>(Constants.Languages.ToList(), Program.CcsFile.Character.Languages, name).Evaluate(command),
                "ClassResource" => new ListScript<ClassResource>(new List<ClassResource>(), Program.CcsFile.Character.ClassResources, name).Evaluate(command),
                "StatusEffect" => new ListScript<StatusEffect>(new List<StatusEffect>(), Program.CcsFile.Character.StatusEffects, name).Evaluate(command),
                _ => new ConsoleResult($"Error: '{command}' does not contain a valid command.", ResultType.Error),
            };
        }

        private void GenerateHeader()
        {
            this.ConsoleOutput.Add(new ConsoleResult($"Concierge [Version {Program.AssemblyVersion}]", ResultType.Information));
            this.ConsoleOutput.Add(new ConsoleResult("Starting console...", ResultType.Information));
            this.ConsoleOutput.Add(new ConsoleResult(string.Empty, ResultType.Information));
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
