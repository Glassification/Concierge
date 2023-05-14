// <copyright file="CharacterProperties.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character
{
    using System.Text;

    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Newtonsoft.Json;

    public sealed class CharacterProperties : ICopyable<CharacterProperties>
    {
        public CharacterProperties()
        {
            this.Name = string.Empty;
            this.Race = new Race();
            this.Background = string.Empty;
            this.Alignment = string.Empty;
            this.Experience = string.Empty;
            this.Class1 = new CharacterClass(1);
            this.Class2 = new CharacterClass(2);
            this.Class3 = new CharacterClass(3);
            this.CharacterIcon = new CharacterImage();
        }

        public string Name { get; set; }

        public Race Race { get; set; }

        public string Background { get; set; }

        public string Alignment { get; set; }

        public string Experience { get; set; }

        public CharacterClass Class1 { get; set; }

        public CharacterClass Class2 { get; set; }

        public CharacterClass Class3 { get; set; }

        public CharacterImage CharacterIcon { get; set; }

        [JsonIgnore]
        public int Level => this.Class1.Level + this.Class2.Level + this.Class3.Level;

        [JsonIgnore]
        public string GetClasses
        {
            get
            {
                var classes = new StringBuilder();

                classes.Append(this.Class1.Name.IsNullOrWhiteSpace() ? string.Empty : $"{this.Class1}, ");
                classes.Append(this.Class2.Name.IsNullOrWhiteSpace() ? string.Empty : $"{this.Class2}, ");
                classes.Append(this.Class3.Name.IsNullOrWhiteSpace() ? string.Empty : this.Class3);

                var classString = classes.ToString().Trim(new char[] { ' ', ',' });

                return classString;
            }
        }

        public CharacterProperties DeepCopy()
        {
            return new CharacterProperties()
            {
                Name = this.Name,
                Race = this.Race.DeepCopy(),
                Background = this.Background,
                Alignment = this.Alignment,
                Experience = this.Experience,
                Class1 = this.Class1.DeepCopy(),
                Class2 = this.Class2.DeepCopy(),
                Class3 = this.Class3.DeepCopy(),
                CharacterIcon = this.CharacterIcon.DeepCopy(),
            };
        }

        public CharacterClass GetClassByNumber(int num)
        {
            return num switch
            {
                2 => this.Class2,
                3 => this.Class3,
                _ => this.Class1,
            };
        }
    }
}
