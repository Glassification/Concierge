// <copyright file="SearchIgnore.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Attributes
{
    using System;

    /// <summary>
    /// Specifies that a class, struct, or property should be ignored during searching operations.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property)]
    public sealed class SearchIgnore : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchIgnore"/> class.
        /// </summary>
        public SearchIgnore()
        {
        }
    }
}
