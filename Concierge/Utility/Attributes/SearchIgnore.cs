// <copyright file="SearchIgnore.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property)]
    public sealed class SearchIgnore : Attribute
    {
        public SearchIgnore()
        {
        }
    }
}
