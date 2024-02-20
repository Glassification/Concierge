// <copyright file="Disposition.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Dispositions
{
    using Concierge.Common;
    using Concierge.Common.Exceptions;
    using Newtonsoft.Json;

    public sealed class Disposition : ICopyable<Disposition>
    {
        public Disposition()
        {
            this.Name = string.Empty;
            this.Race = new Race();
            this.Background = string.Empty;
            this.Alignment = string.Empty;
            this.Experience = string.Empty;
            this.Class1 = new Class(1);
            this.Class2 = new Class(2);
            this.Class3 = new Class(3);
        }

        public string Alignment { get; set; }

        public string Background { get; set; }

        public Class Class1 { get; set; }

        public Class Class2 { get; set; }

        public Class Class3 { get; set; }

        public string Experience { get; set; }

        [JsonIgnore]
        public int Level => this.Class1.Level + this.Class2.Level + this.Class3.Level;

        public string Name { get; set; }

        public Race Race { get; set; }

        public Disposition DeepCopy()
        {
            return new Disposition()
            {
                Name = this.Name,
                Race = this.Race.DeepCopy(),
                Background = this.Background,
                Alignment = this.Alignment,
                Experience = this.Experience,
                Class1 = this.Class1.DeepCopy(),
                Class2 = this.Class2.DeepCopy(),
                Class3 = this.Class3.DeepCopy(),
            };
        }

        public Class GetClass(int num)
        {
            return num switch
            {
                1 => this.Class1,
                2 => this.Class2,
                3 => this.Class3,
                _ => throw new InvalidValueException(num.ToString()),
            };
        }
    }
}
