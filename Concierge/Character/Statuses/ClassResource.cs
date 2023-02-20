// <copyright file="ClassResource.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Statuses
{
    using System;
    using System.Windows.Media;

    using Concierge.Character.Enums;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;
    using MaterialDesignThemes.Wpf;
    using Newtonsoft.Json;

    public sealed class ClassResource : ICopyable<ClassResource>, IUnique
    {
        private int spent;

        public ClassResource()
        {
            this.Type = string.Empty;
            this.Recovery = Recovery.None;
            this.Note = string.Empty;
            this.Id = Guid.NewGuid();
        }

        [JsonIgnore]
        public string Name
        {
            get => this.Type;
            set => this.Type = value;
        }

        public string Type { get; set; }

        public int Total { get; set; }

        public int Spent
        {
            get
            {
                return this.spent;
            }

            set
            {
                if (value <= this.Total)
                {
                    this.spent = value;
                }
            }
        }

        public string Note { get; set; }

        public Recovery Recovery { get; set; }

        [JsonIgnore]
        public string Description => $"{this.Type} - {this.Spent}/{this.Total} Used.{(this.Recovery == Recovery.None ? string.Empty : $" Recovers after {this.Recovery.GetDescription()}.")} {this.Note}";

        public Guid Id { get; set; }

        public override string ToString()
        {
            return this.Type;
        }

        public ClassResource DeepCopy()
        {
            return new ClassResource()
            {
                Type = this.Type,
                Total = this.Total,
                Spent = this.Spent,
                Note = this.Note,
                Recovery = this.Recovery,
                Id = this.Id,
            };
        }

        public (PackIconKind IconKind, Brush Brush, string Name) GetCategory()
        {
            throw new NotImplementedException();
        }
    }
}
