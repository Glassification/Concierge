// <copyright file="CustomColorReadWriter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence.ReadWriters
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Concierge.Common;
    using Concierge.Common.Exceptions;
    using Concierge.Common.Extensions;
    using Newtonsoft.Json;

    public sealed class CustomColorReadWriter : IReadWriters
    {
        private readonly IErrorService errorService;

        public CustomColorReadWriter(IErrorService errorService)
        {
            this.errorService = errorService;
        }

        public void Append<T>(string filePath, T value)
        {
            throw new NotImplementedException();
        }

        public void Clear(string filePath)
        {
            throw new NotImplementedException();
        }

        public bool Read(string filePath)
        {
            throw new NotImplementedException();
        }

        public T ReadJson<T>(string filePath)
            where T : new()
        {
            try
            {
                var rawJson = File.ReadAllText(filePath);
                var customColorService = JsonConvert.DeserializeObject<T>(rawJson);

                if (customColorService is null)
                {
                    throw new NullValueException(nameof(customColorService));
                }

                return customColorService;
            }
            catch (Exception ex)
            {
                ex = ex.TryConvertToReadWriterException(filePath);
                this.errorService.LogError(ex);
                return new T();
            }
        }

        public T ReadJson<T>(byte[] file)
            where T : new()
        {
            throw new NotImplementedException();
        }

        public List<T> ReadList<T>(string filePath)
        {
            throw new NotImplementedException();
        }

        public List<T> ReadList<T>(byte[] file)
        {
            throw new NotImplementedException();
        }

        public bool Write(string filePath)
        {
            throw new NotImplementedException();
        }

        public bool WriteJson<T>(string filePath, T value)
        {
            try
            {
                var rawJson = JsonConvert.SerializeObject(value, Formatting.Indented);
                File.WriteAllText(filePath, rawJson);

                return true;
            }
            catch (Exception ex)
            {
                this.errorService.LogError(ex);
                return false;
            }
        }

        public void WriteList<T>(string filePath, List<T> value)
        {
            throw new NotImplementedException();
        }
    }
}
