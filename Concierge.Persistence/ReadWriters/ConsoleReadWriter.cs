// <copyright file="ConsoleReadWriter.cs" company="Thomas Beckett">
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

    public class ConsoleReadWriter : IReadWriters
    {
        private readonly IErrorService errorService;

        public ConsoleReadWriter(IErrorService errorService)
        {
            this.errorService = errorService;
        }

        public void Append<T>(string filePath, T value)
        {
            try
            {
                var json = JsonConvert.SerializeObject(value);
                File.AppendAllText(filePath, json);
                File.AppendAllText(filePath, "\n");
            }
            catch (Exception ex)
            {
                this.errorService.LogError(ex);
            }
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
                var rawJson = File.ReadAllLines(filePath);
                foreach (var line in rawJson)
                {
                    var json = JsonConvert.DeserializeObject<T>(line);
                    if (json is not null)
                    {
                        list.Add(json);
                    }
                }
            }
            catch (Exception ex)
            {
                ex = ex.TryConvertToReadWriterException(filePath);
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
            throw new NotImplementedException();
        }
    }
}
