// <copyright file="DefaultListReadWriter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence.ReadWriters
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Newtonsoft.Json;

    public static class DefaultListReadWriter
    {
        public static List<T> ReadJson<T>(string fileName)
        {
            try
            {
                var bytes = File.ReadAllBytes(fileName);
                return ReadJson<T>(bytes);
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex);
                return new List<T>();
            }
        }

        public static List<T> ReadJson<T>(byte[] resource)
        {
            var defaultList = new List<T>();

            try
            {
                var rawJson = Encoding.Default.GetString(resource);
                defaultList = JsonConvert.DeserializeObject<List<T>>(rawJson);

                Program.Logger.Info($"{typeof(T).Name} file loaded successfully.");
            }
            catch (Exception ex)
            {
                ex = ex.TryConvertToReadWriterException(typeof(T).Name);
                Program.ErrorService.LogError(ex);
            }
            finally
            {
                defaultList ??= new List<T>();
            }

            return defaultList;
        }

        public static List<T> ReadGenericList<T>(string resource)
        {
            var defaultList = new List<T>();

            try
            {
                var items = resource.Split("\r\n");
                foreach (var item in items)
                {
                    defaultList.Add(ObjectUtility.ConvertToType<T>(item));
                }

                Program.Logger.Info($"{typeof(T).Name} list file loaded successfully.");
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex);
            }

            return defaultList;
        }
    }
}
