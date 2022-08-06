// <copyright file="CcsHashing.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Persistence
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    using Concierge.Character;
    using Concierge.Persistence;
    using Newtonsoft.Json;

    public static class CcsHashing
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0063:Use simple 'using' statement", Justification = "Cleaner code.")]
        public static string HashCharacter(ConciergeCharacter character)
        {
            var rawString = JsonConvert.SerializeObject(character, Formatting.Indented);

            using (HashAlgorithm algorithm = SHA256.Create())
            {
                var bytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(rawString));
                return BitConverter.ToString(bytes).Replace("-", string.Empty);
            }
        }

        public static bool CheckHash(CcsFile file)
        {
            var newHash = HashCharacter(file.Character);
            var existingHash = file.Hash;

            return newHash.Equals(existingHash);
        }
    }
}
