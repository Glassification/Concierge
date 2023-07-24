// <copyright file="CustomItemService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Equipable;
    using Concierge.Character.Spellcasting;
    using Concierge.Character.Vitals;
    using Concierge.Common;
    using Concierge.Data;
    using Concierge.Persistence.ReadWriters;
    using Newtonsoft.Json;

    public sealed class CustomItemService
    {
        private readonly IReadWriters readwriter;
        private readonly string filePath = Path.Combine(ConciergeFiles.GetCorrectCustomItemsPath(), ConciergeFiles.CustomItemsName);

        public CustomItemService()
        {
            this.readwriter = new CustomItemReadWriter(Program.ErrorService);
            this.CustomItems = new List<IUnique>();

            this.Initialize();
        }

        private List<IUnique> CustomItems { get; set; }

        public void AddCustomItem(IUnique item)
        {
            item.IsCustom = true;
            var blob = new CustomBlob(item);

            this.CustomItems.Add(item);
            this.readwriter.Append(this.filePath, blob);
        }

        public void UpdateCustomItem()
        {
            this.readwriter.WriteList(this.filePath, this.CustomItems.Select(x => new CustomBlob(x)).ToList());
        }

        public void RemoveCustomItem(IUnique item)
        {
            this.CustomItems.Remove(item);
            this.readwriter.WriteList(this.filePath, this.CustomItems.Select(x => new CustomBlob(x)).ToList());
        }

        public List<IUnique> GetCustomItems()
        {
            return this.CustomItems;
        }

        public List<T> GetCustomItems<T>()
        {
            var filteredList = new List<T>();
            foreach (var item in this.CustomItems)
            {
                if (item is T t)
                {
                    filteredList.Add(t);
                }
            }

            return filteredList;
        }

        private void Initialize()
        {
            var customBlobs = this.readwriter.ReadList<CustomBlob>(this.filePath);
            IUnique? item = null;

            foreach (var customBlob in customBlobs)
            {
                if (customBlob.Name.Equals(nameof(Ability)))
                {
                    item = JsonConvert.DeserializeObject<Ability>(customBlob.Blob);
                }
                else if (customBlob.Name.Equals(nameof(Ammunition)))
                {
                    item = JsonConvert.DeserializeObject<Ammunition>(customBlob.Blob);
                }
                else if (customBlob.Name.Equals(nameof(Armor)))
                {
                    item = JsonConvert.DeserializeObject<Armor>(customBlob.Blob);
                }
                else if (customBlob.Name.Equals(nameof(ClassResource)))
                {
                    item = JsonConvert.DeserializeObject<ClassResource>(customBlob.Blob);
                }
                else if (customBlob.Name.Equals(nameof(Inventory)))
                {
                    item = JsonConvert.DeserializeObject<Inventory>(customBlob.Blob);
                }
                else if (customBlob.Name.Equals(nameof(Language)))
                {
                    item = JsonConvert.DeserializeObject<Language>(customBlob.Blob);
                }
                else if (customBlob.Name.Equals(nameof(MagicClass)))
                {
                    item = JsonConvert.DeserializeObject<MagicClass>(customBlob.Blob);
                }
                else if (customBlob.Name.Equals(nameof(Proficiency)))
                {
                    item = JsonConvert.DeserializeObject<Proficiency>(customBlob.Blob);
                }
                else if (customBlob.Name.Equals(nameof(Spell)))
                {
                    item = JsonConvert.DeserializeObject<Spell>(customBlob.Blob);
                }
                else if (customBlob.Name.Equals(nameof(StatusEffect)))
                {
                    item = JsonConvert.DeserializeObject<StatusEffect>(customBlob.Blob);
                }
                else if (customBlob.Name.Equals(nameof(Weapon)))
                {
                    item = JsonConvert.DeserializeObject<Weapon>(customBlob.Blob);
                }

                if (item is not null)
                {
                    this.CustomItems.Add(item);
                }
            }
        }
    }
}
