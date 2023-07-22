// <copyright file="CustomBlob.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Data
{
    using Concierge.Common;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a custom data blob that can be used to store serialized objects with their associated names.
    /// </summary>
    public sealed class CustomBlob
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomBlob"/> class with empty name and blob.
        /// </summary>
        public CustomBlob()
        {
            this.Name = string.Empty;
            this.Blob = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomBlob"/> class with the given item.
        /// The item's type name will be used as the name, and the object will be serialized into the blob.
        /// </summary>
        /// <param name="item">The object to be serialized and stored in the blob.</param>
        public CustomBlob(IUnique item)
        {
            this.Name = item.GetType().Name;
            this.Blob = JsonConvert.SerializeObject(item, Formatting.None);
        }

        /// <summary>
        /// Gets or sets the name associated with the custom blob.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the serialized representation of the object stored in the custom blob.
        /// </summary>
        public string Blob { get; set; }
    }
}
