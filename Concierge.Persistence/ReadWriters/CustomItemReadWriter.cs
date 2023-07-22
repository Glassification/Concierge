// <copyright file="CustomItemReadWriter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence.ReadWriters
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Newtonsoft.Json;

    public sealed class CustomItemReadWriter : IReadWriters
    {
        private readonly IErrorService errorService;

        public CustomItemReadWriter(IErrorService errorService)
        {
            this.errorService = errorService;
        }

        public void Append<T>(string filePath, T value)
        {
            try
            {
                var rawBlob = JsonConvert.SerializeObject(value, Formatting.None);
                if (!rawBlob.IsNullOrWhiteSpace())
                {
                    File.AppendAllText(filePath, $"{rawBlob}{Environment.NewLine}");
                }
            }
            catch (Exception ex)
            {
                this.errorService.LogError(ex);
            }
        }

        public void Clear(string filePath)
        {
            throw new NotImplementedException();
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
                var lines = File.ReadAllLines(filePath);

                foreach (var line in lines)
                {
                    var blob = JsonConvert.DeserializeObject<T>(line);
                    if (blob is not null)
                    {
                        list.Add(blob);
                    }
                }
            }
            catch (Exception ex)
            {
                this.errorService.LogError(ex);
            }

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
            try
            {
                var lines = new List<string>();
                foreach (var item in value)
                {
                    var rawBlob = JsonConvert.SerializeObject(item, Formatting.None);
                    if (!rawBlob.IsNullOrWhiteSpace())
                    {
                        lines.Add(rawBlob);
                    }
                }

                File.WriteAllLines(filePath, lines);
            }
            catch (Exception ex)
            {
                this.errorService.LogError(ex);
            }
        }
    }
}
