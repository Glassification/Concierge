// <copyright file="HistoryReadWriter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence.ReadWriters
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Concierge.Common;
    using Concierge.Common.Extensions;

    public sealed class HistoryReadWriter : IReadWriters
    {
        private readonly IErrorService errorService;

        public HistoryReadWriter(IErrorService errorService)
        {
            this.errorService = errorService;
        }

        public void Clear(string filePath)
        {
            try
            {
                File.WriteAllText(filePath, string.Empty);
            }
            catch (Exception ex)
            {
                this.errorService.LogError(ex);
            }
        }

        public T ReadJson<T>(string filePath)
            where T : new()
        {
            throw new NotImplementedException();
        }

        public T ReadJson<T>(byte[] file)
            where T : new()
        {
            throw new NotImplementedException();
        }

        public List<T> ReadList<T>(string filePath)
        {
            var list = new List<T>();

            try
            {
                CreateFileIfMissing(filePath);

                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var history = line.Split(']')[1].Trim();
                    list.Add((T)(object)history);
                }
            }
            catch (Exception ex)
            {
                ex = ex.TryConvertToReadWriterException(filePath);
                this.errorService.LogError(ex);
            }

            list.Reverse();
            return list;
        }

        public List<T> ReadList<T>(byte[] file)
        {
            throw new NotImplementedException();
        }

        public bool WriteJson<T>(string filePath, T value)
        {
            throw new NotImplementedException();
        }

        public void WriteList<T>(string filePath, List<T> value)
        {
            throw new NotImplementedException();
        }

        public void Append<T>(string filePath, T value)
        {
            try
            {
                File.AppendAllText(filePath, $"[{ConciergeDateTime.LoggingNow}] {value}\n");
            }
            catch (Exception ex)
            {
                this.errorService.LogError(ex);
            }
        }

        private static void CreateFileIfMissing(string filePath)
        {
            if (!File.Exists(filePath))
            {
                using var createStream = File.Create(filePath);
            }
        }

        public bool Read(string filePath)
        {
            throw new NotImplementedException();
        }

        public bool Write(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
