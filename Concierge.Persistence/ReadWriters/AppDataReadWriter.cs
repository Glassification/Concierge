// <copyright file="AppDataReadWriter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence.ReadWriters
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;

    using Concierge.Common;

    public sealed class AppDataReadWriter : IReadWriters
    {
        private readonly IErrorService errorService;

        public AppDataReadWriter(IErrorService errorService)
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
            try
            {
                ZipFile.ExtractToDirectory(filePath, ConciergeFiles.AppDataDirectory, true);
                return true;
            }
            catch (Exception ex)
            {
                this.errorService.LogError(ex);
                return false;
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
            throw new NotImplementedException();
        }

        public List<T> ReadList<T>(byte[] file)
        {
            throw new NotImplementedException();
        }

        public bool Write(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                ZipFile.CreateFromDirectory(ConciergeFiles.AppDataDirectory, filePath);
                return true;
            }
            catch (Exception ex)
            {
                this.errorService.LogError(ex);
                return false;
            }
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
