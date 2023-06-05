// <copyright file="IReadWriters.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence.ReadWriters
{
    using System.Collections.Generic;

    public interface IReadWriters
    {
        public List<T> ReadList<T>(string filePath);

        public List<T> ReadList<T>(byte[] file);

        public T ReadJson<T>(string filePath)
            where T : new();

        public T ReadJson<T>(byte[] file)
            where T : new();

        public void WriteList<T>(string filePath, List<T> value);

        public bool WriteJson<T>(string filePath, T value);

        public void Append<T>(string filePath, T value);

        public void Clear(string filePath);
    }
}
