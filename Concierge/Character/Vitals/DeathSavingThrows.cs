﻿// <copyright file="DeathSavingThrows.cs" company="Thomas Beckett">
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
            this.DeathSaves = new List<DeathSave>();
            this.CurrentDeathSave = 0;
        }

        [JsonIgnore]
        public DeathSave DeathSaveStatus => this.HasRequiredInARow(DeathSave.Failure) ? DeathSave.Failure : this.HasRequiredInARow(DeathSave.Success) ? DeathSave.Success : DeathSave.None;

        public int CurrentDeathSave { get; set; }

        public List<DeathSave> DeathSaves { get; set; }

        public void LazyInitialize()
        {
            if (this.DeathSaves.IsEmpty())
            {
                this.ResetDeathSaves();
            }
        }

        public void MakeDeathSave(DeathSave deathSave)
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
                this.DeathSaves.Add(DeathSave.None);
            }
        }

        public DeathSavingThrows DeepCopy()
        {
            return new DeathSavingThrows()
            {
                CurrentDeathSave = this.CurrentDeathSave,
                DeathSaves = new List<DeathSave>(this.DeathSaves),
            };
        }

        private bool HasRequiredInARow(DeathSave deathSave)
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