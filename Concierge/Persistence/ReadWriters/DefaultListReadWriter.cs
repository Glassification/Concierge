// <copyright file="DefaultListReadWriter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence.ReadWriters
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Newtonsoft.Json;

    public static class DefaultListReadWriter
    {
        public static List<T> ReadJson<T>(byte[] resource)
        {
            var defaultList = new List<T>();

            try
            {
                var rawJson = Encoding.Default.GetString(resource);
                defaultList = JsonConvert.DeserializeObject<List<T>>(rawJson);

                Program.Logger.Info($"{typeof(T)} loaded successfully.");
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex);
            }
            finally
            {
                if (defaultList is null)
                {
                    defaultList = new List<T>();
                }
            }

            return defaultList;
        }

        public static List<string> ReadStrings(string resource)
        {
            var defaultList = new List<string>();

            try
            {
                defaultList.AddRange(resource.Split('\n'));

                Program.Logger.Info($"String file loaded successfully.");
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex);
            }

            return defaultList;
        }
    }
}
