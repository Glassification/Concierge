// <copyright file="CcsFile.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence
{
    public class CcsFile
    {
        public CcsFile()
        {
        }

        public string Name { get; set; }

        public string Path { get; set; }

        public string AbsolutePath => this.Path + this.Name;
    }
}
