// <copyright file="ConciergeHashing.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    using Newtonsoft.Json;

    /// <summary>
    /// Provides methods for hashing and checking hash values of data.
    /// </summary>
    public static class ConciergeHashing
    {
        /// <summary>
        /// Computes the hash value of an item using the SHA256 algorithm.
        /// </summary>
        /// <typeparam name="T">The type of the item to hash.</typeparam>
        /// <param name="item">The item to hash.</param>
        /// <returns>A string representing the hashed value of the item.</returns>
        public static string HashData<T>(T item)
        {
            var rawString = JsonConvert.SerializeObject(item, Formatting.Indented);
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawString));
            return BitConverter.ToString(bytes).Replace("-", string.Empty);
        }

        /// <summary>
        /// Checks whether the hash value of an item matches a given hash.
        /// </summary>
        /// <typeparam name="T">The type of the item to check.</typeparam>
        /// <param name="item">The item to check.</param>
        /// <param name="existingHash">The existing hash to compare against.</param>
        /// <returns><c>true</c> if the computed hash of the item matches the existing hash; otherwise, <c>false</c>.</returns>
        public static bool CheckHash<T>(T item, string existingHash)
        {
            return HashData(item).Equals(existingHash);
        }
    }
}
