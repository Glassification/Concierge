// <copyright file="CustomBlob.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence
{
    using Concierge.Common;
    using Newtonsoft.Json;

    public sealed class CustomBlob
    {
        public CustomBlob()
        {
            this.Name = string.Empty;
            this.Blob = string.Empty;
        }

        public CustomBlob(IUnique item)
        {
            this.Name = item.GetType().Name;
            this.Blob = JsonConvert.SerializeObject(item, Formatting.None);
        }

        public string Name { get; set; }

        public string Blob { get; set; }
    }
}
