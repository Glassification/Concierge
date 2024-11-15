// <copyright file="CustomItemService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Concierge.Character.Companions;
    using Concierge.Character.Details;
    using Concierge.Character.Equipable;
    using Concierge.Character.Magic;
    using Concierge.Character.Vitals;
    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Data;
    using Concierge.Persistence.ReadWriters;
    using Newtonsoft.Json;

    /// <summary>
    /// Provides methods for managing custom items, such as adding, updating, and removing them.
    /// </summary>
    public sealed class CustomItemService
    {
        private readonly CustomItemReadWriter readwriter = new CustomItemReadWriter(Program.ErrorService);
        private readonly string filePath = Path.Combine(ConciergeFiles.CustomItemsPath, ConciergeFiles.CustomItemsName);
        private readonly List<IUnique> customItems = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomItemService"/> class.
        /// </summary>
        public CustomItemService()
        {
            this.Initialize();
        }

        /// <summary>
        /// Adds a custom item to the list of custom items.
        /// </summary>
        /// <param name="item">The custom item to add.</param>
        public void AddItem(IUnique item)
        {
            item.IsCustom = true;
            var blob = new CustomBlob(item);

            this.customItems.Add(item);
            this.readwriter.Append(this.filePath, blob);

            Program.MainWindow?.DisplayStatusText($"Added custom {item.GetType().Name.ToPascalCase()} {item.Name}");
        }

        /// <summary>
        /// Updates the custom items in the file.
        /// </summary>
        public void UpdateItem()
        {
            this.readwriter.WriteList(this.filePath, this.customItems.Select(x => new CustomBlob(x)).ToList());
        }

        /// <summary>
        /// Removes a custom item from the list of custom items.
        /// </summary>
        /// <param name="item">The custom item to remove.</param>
        public void RemoveItem(IUnique item)
        {
            this.customItems.Remove(item);
            this.readwriter.WriteList(this.filePath, this.customItems.Select(x => new CustomBlob(x)).ToList());
        }

        /// <summary>
        /// Gets all custom items.
        /// </summary>
        /// <returns>The list of custom items.</returns>
        public List<IUnique> GetItems()
        {
            return this.customItems;
        }

        /// <summary>
        /// Gets custom items of a specific type.
        /// </summary>
        /// <typeparam name="T">The type of custom items to retrieve.</typeparam>
        /// <returns>The list of custom items of the specified type.</returns>
        public List<T> GetItems<T>()
        {
            var filteredList = new List<T>();
            foreach (var item in this.customItems)
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
                else if (customBlob.Name.Equals(nameof(Augment)))
                {
                    item = JsonConvert.DeserializeObject<Augment>(customBlob.Blob);
                }
                else if (customBlob.Name.Equals(nameof(Armor)))
                {
                    item = JsonConvert.DeserializeObject<Armor>(customBlob.Blob);
                }
                else if (customBlob.Name.Equals(nameof(ClassResource)))
                {
                    item = JsonConvert.DeserializeObject<ClassResource>(customBlob.Blob);
                }
                else if (customBlob.Name.Equals(nameof(CompanionWeapon)))
                {
                    item = JsonConvert.DeserializeObject<CompanionWeapon>(customBlob.Blob);
                }
                else if (customBlob.Name.Equals(nameof(Inventory)))
                {
                    item = JsonConvert.DeserializeObject<Inventory>(customBlob.Blob);
                }
                else if (customBlob.Name.Equals(nameof(Language)))
                {
                    item = JsonConvert.DeserializeObject<Language>(customBlob.Blob);
                }
                else if (customBlob.Name.Equals(nameof(MagicalClass)))
                {
                    item = JsonConvert.DeserializeObject<MagicalClass>(customBlob.Blob);
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
                    this.customItems.Add(item);
                }
            }
        }
    }
}
