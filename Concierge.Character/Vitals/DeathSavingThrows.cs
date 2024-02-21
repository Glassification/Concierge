// <copyright file="DeathSavingThrows.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Vitals
{
    using System.Collections.Generic;

    using Concierge.Character.Enums;
    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Newtonsoft.Json;

    public sealed class DeathSavingThrows : ICopyable<DeathSavingThrows>
    {
        private const int MaxDeathSaves = 5;
        private const int SavesInARow = 3;

        public DeathSavingThrows()
        {
            this.DeathSaves = [];
            this.CurrentDeathSave = 0;
        }

        public int CurrentDeathSave { get; set; }

        public List<AbilitySave> DeathSaves { get; set; }

        [JsonIgnore]
        public AbilitySave DeathSaveStatus
        {
            get
            {
                if (this.HasRequiredInARow(AbilitySave.Failure))
                {
                    return AbilitySave.Failure;
                }

                if (this.HasRequiredInARow(AbilitySave.Success))
                {
                    return AbilitySave.Success;
                }

                return AbilitySave.None;
            }
        }

        public void LazyInitialize()
        {
            if (this.DeathSaves.IsEmpty())
            {
                this.ResetDeathSaves();
            }
        }

        public void MakeDeathSave(AbilitySave deathSave)
        {
            this.LazyInitialize();
            if (this.CurrentDeathSave < MaxDeathSaves)
            {
                this.DeathSaves[this.CurrentDeathSave] = deathSave;

                this.CurrentDeathSave++;
            }
        }

        public void ResetDeathSaves()
        {
            this.DeathSaves.Clear();
            this.CurrentDeathSave = 0;

            for (int i = 0; i < MaxDeathSaves; i++)
            {
                this.DeathSaves.Add(AbilitySave.None);
            }
        }

        public DeathSavingThrows DeepCopy()
        {
            return new DeathSavingThrows()
            {
                CurrentDeathSave = this.CurrentDeathSave,
                DeathSaves = new List<AbilitySave>(this.DeathSaves),
            };
        }

        private bool HasRequiredInARow(AbilitySave deathSave)
        {
            var count = 0;

            foreach (var save in this.DeathSaves)
            {
                if (save == deathSave)
                {
                    count++;
                }

                if (count == SavesInARow)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
