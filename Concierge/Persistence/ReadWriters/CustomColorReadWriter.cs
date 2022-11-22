// <copyright file="CustomColorReadWriter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence.ReadWriters
{
    using System;
    using System.IO;

    using Concierge.Exceptions;
    using Concierge.Services;
    using Newtonsoft.Json;

    public static class CustomColorReadWriter
    {
        public static CustomColorService Read(string fileName)
        {
            try
            {
                var rawJson = File.ReadAllText(fileName);
                var customColorService = JsonConvert.DeserializeObject<CustomColorService>(rawJson);

                if (customColorService is null)
                {
                    throw new NullValueException(nameof(customColorService));
                }

                Program.Logger.Info($"Successfully loaded {fileName}");

                return customColorService;
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex);
                return new CustomColorService();
            }
        }

        public static void Write(string fileName, CustomColorService customColorService)
        {
            try
            {
                var rawJson = JsonConvert.SerializeObject(customColorService, Formatting.Indented);
                File.WriteAllText(fileName, rawJson);
                Program.Logger.Info($"Successfully updated {fileName}");
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex);
            }
        }
    }
}
