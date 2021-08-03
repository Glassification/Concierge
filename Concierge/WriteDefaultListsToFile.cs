// <copyright file="WriteDefaultListsToFile.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge
{
    using System.Collections.Generic;
    using System.IO;

    using Newtonsoft.Json;

    public static class WriteDefaultListsToFile
    {
        public static void WriteList<T>(List<T> list)
        {
            var rawJson = JsonConvert.SerializeObject(list, Formatting.Indented);

            File.WriteAllText($"C:\\Users\\TomBe\\Documents\\{typeof(T).Name}.json", rawJson);
        }
    }
}
