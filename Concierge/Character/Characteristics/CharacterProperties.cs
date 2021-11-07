// <copyright file="CharacterProperties.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Characteristics
{
    using System.Text;

    using Concierge.Utility;
    using Concierge.Utility.Extensions;
    using Newtonsoft.Json;

    public class CharacterProperties : ICopyable
    {
        public CharacterProperties()
        {
            this.Name = string.Empty;
            this.Race = string.Empty;
            this.Background = string.Empty;
            this.Alignment = string.Empty;
            this.Experience = string.Empty;
            this.Class1 = new CharacterClass();
            this.Class2 = new CharacterClass();
            this.Class3 = new CharacterClass();
        }

        public string Name { get; set; }

        public string Race { get; set; }

        public string Background { get; set; }

        public string Alignment { get; set; }

        public string Experience { get; set; }

        public CharacterClass Class1 { get; set; }

        public CharacterClass Class2 { get; set; }

        public CharacterClass Class3 { get; set; }

        [JsonIgnore]
        public int Level => this.Class1.Level + this.Class2.Level + this.Class3.Level;

        [JsonIgnore]
        public string GetClasses
        {
            get
            {
                var classes = new StringBuilder();

                classes.Append(this.Class1.Name.IsNullOrWhiteSpace() ? string.Empty : $"{this.Class1.Name}, ");
                classes.Append(this.Class2.Name.IsNullOrWhiteSpace() ? string.Empty : $"{this.Class2.Name}, ");
                classes.Append(this.Class3.Name.IsNullOrWhiteSpace() ? string.Empty : this.Class3.Name);

                var classString = classes.ToString().Trim(new char[] { ' ', ',' });

                return classString;
            }
        }

        public ICopyable DeepCopy()
        {
            return new CharacterProperties()
            {
                Name = this.Name,
                Race = this.Race,
                Background = this.Background,
                Alignment = this.Alignment,
                Experience = this.Experience,
                Class1 = this.Class1.DeepCopy() as CharacterClass,
                Class2 = this.Class2.DeepCopy() as CharacterClass,
                Class3 = this.Class3.DeepCopy() as CharacterClass,
            };
        }
    }
}
