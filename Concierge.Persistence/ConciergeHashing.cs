// <copyright file="ConciergeHashing.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    using Newtonsoft.Json;

    public static class ConciergeHashing
    {
        public static string HashData<T>(T item)
        {
            var rawString = JsonConvert.SerializeObject(item, Formatting.Indented);
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawString));
            return BitConverter.ToString(bytes).Replace("-", string.Empty);
        }

        public static bool CheckHash<T>(T item, string existingHash)
        {
            return HashData(item).Equals(existingHash);
        }
    }
}
