// <copyright file="DefaultListReadWriter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence.ReadWriters
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using Concierge.Common;
    using Concierge.Common.Exceptions;
    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Concierge.Logging;
    using Newtonsoft.Json;

    public sealed class DefaultListReadWriter : IReadWriters
    {
        private readonly IErrorService errorService;
        private readonly Logger logger;

        public DefaultListReadWriter(IErrorService errorService, Logger logger)
        {
            this.errorService = errorService;
            this.logger = logger;
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
                var bytes = File.ReadAllBytes(filePath);
                return this.ReadJson<T>(bytes);
            }
            catch (Exception ex)
            {
                this.errorService.LogError(ex);
                return new T();
            }
        }

        public T ReadJson<T>(byte[] file)
            where T : new()
        {
            try
            {
                var rawJson = Encoding.Default.GetString(file);
                var defaultList = JsonConvert.DeserializeObject<T>(rawJson);

                if (defaultList is null)
                {
                    throw new NullValueException(nameof(defaultList));
                }

                this.logger.Info($"{typeof(T).Name} file loaded successfully.");

                return defaultList;
            }
            catch (Exception ex)
            {
                ex = ex.TryConvertToReadWriterException(typeof(T).Name);
                this.errorService.LogError(ex);

                return new T();
            }
        }

        public List<T> ReadList<T>(string filePath)
        {
            var defaultList = new List<T>();

            try
            {
                var items = filePath.Split("\r\n");
                foreach (var item in items)
                {
                    defaultList.Add(ObjectUtility.ConvertToType<T>(item));
                }

                this.logger.Info($"{typeof(T).Name} list file loaded successfully.");
            }
            catch (Exception ex)
            {
                this.errorService.LogError(ex);
            }

            return defaultList;
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
            throw new NotImplementedException();
        }

        public void WriteList<T>(string filePath, List<T> value)
        {
            throw new NotImplementedException();
        }
    }
}
