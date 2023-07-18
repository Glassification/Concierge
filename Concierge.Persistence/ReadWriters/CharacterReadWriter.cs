// <copyright file="CharacterReadWriter.cs" company="Thomas Beckett">
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
    using Concierge.Configuration;
    using Concierge.Logging;
    using Newtonsoft.Json;

    public sealed class CharacterReadWriter : IReadWriters
    {
        private const string JsonSearchText = "\"Character\"";

        private readonly IErrorService errorService;
        private readonly Logger logger;

        public CharacterReadWriter(IErrorService errorService, Logger logger)
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

        public T ReadJson<T>(string filePath)
            where T : new()
        {
            try
            {
                T? ccsFile;
                var rawJson = File.ReadAllText(filePath);
                if (rawJson.Contains(JsonSearchText))
                {
                    this.logger.Info("No Decompressing needed.");
                    ccsFile = JsonConvert.DeserializeObject<T>(rawJson);
                }
                else
                {
                    this.logger.Info("Decompressing file.");
                    var compressedJson = File.ReadAllBytes(filePath);
                    ccsFile = JsonConvert.DeserializeObject<T>(CcsCompression.Unzip(compressedJson));
                }

                if (ccsFile is null)
                {
                    throw new NullValueException(nameof(ccsFile));
                }

                AppSettingsManager.RefreshUnits();
                this.logger.Info($"Successfully loaded {filePath}");

                return ccsFile;
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

        public bool WriteJson<T>(string filePath, T value)
        {
            try
            {
                var rawJson = JsonConvert.SerializeObject(value, Formatting.Indented);

                if (AppSettingsManager.StartUp.CompressCharacterSheet)
                {
                    this.logger.Info("Compressing file.");
                    File.WriteAllBytes(filePath, CcsCompression.Zip(rawJson));
                }
                else
                {
                    this.logger.Info("No Compressing needed.");
                    File.WriteAllText(filePath, rawJson);
                }

                this.logger.Info($"Successfully saved to {filePath}");

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
