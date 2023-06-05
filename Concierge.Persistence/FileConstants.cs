// <copyright file="FileConstants.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence
{
    public static class FileConstants
    {
        public const string CcsOpenFilter = "*CCS (*.ccs)|*.ccs|JSON (*.json)|*.json|All files (*.*)|*.*";
        public const string ImageOpenFilter = "*BMP (*.bmp)|*.bmp|JPEG (*.jpeg;*.jpg)|*.jpeg;*.jpg|PNG (*.png)|*.png|TIFF (*.tiff)|*.tiff|All files (*.*)|*.*";
        public const string SaveFilter = "CCS (*.ccs)|*.ccs";

        public const string DefaultFileName = "New Character.ccs";
    }
}
