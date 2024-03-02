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

    /// <summary>
    /// Represents the death saving throws of a character.
    /// </summary>
    public sealed class DeathSavingThrows : ICopyable<DeathSavingThrows>
    {
        private const int MaxDeathSaves = 5;
        private const int SavesInARow = 3;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeathSavingThrows"/> class.
        /// </summary>
        public DeathSavingThrows()
        {
            this.DeathSaves = [];
            this.CurrentDeathSave = 0;
        }

        /// <summary>
        /// Gets or sets the number of death saves made.
        /// </summary>
        public int CurrentDeathSave { get; set; }

        /// <summary>
        /// Gets or sets the list of death saving throw results.
        /// </summary>
        public List<AbilitySave> DeathSaves { get; set; }

        /// <summary>
        /// Gets the current status of death saving throws.
        /// </summary>
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

        /// <summary>
        /// Ensures that the death saving throws list is initialized.
        /// </summary>
        public void LazyInitialize()
        {
            if (this.DeathSaves.IsEmpty())
            {
                this.ResetDeathSaves();
            }
        }

        /// <summary>
        /// Records a death saving throw.
        /// </summary>
        /// <param name="deathSave">The result of the death saving throw.</param>
        public void MakeDeathSave(AbilitySave deathSave)
        {
            this.LazyInitialize();
            if (this.CurrentDeathSave < MaxDeathSaves)
            {
                this.DeathSaves[this.CurrentDeathSave] = deathSave;

                this.CurrentDeathSave++;
            }
        }

        /// <summary>
        /// Resets the death saving throws to their default state.
        /// </summary>
        public void ResetDeathSaves()
        {
            this.DeathSaves.Clear();
            this.CurrentDeathSave = 0;

            for (int i = 0; i < MaxDeathSaves; i++)
            {
                this.DeathSaves.Add(AbilitySave.None);
            }
        }

        /// <summary>
        /// Creates a deep copy of the DeathSavingThrows instance.
        /// </summary>
        /// <returns>A deep copy of the DeathSavingThrows instance.</returns>
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
