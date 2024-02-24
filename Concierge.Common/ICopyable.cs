// <copyright file="ICopyable.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common
{
    /// <summary>
    /// Represents an interface for creating deep copies of objects.
    /// </summary>
    /// <typeparam name="T">The type of object to be copied.</typeparam>
    public interface ICopyable<T>
    {
        T DeepCopy();
    }
}
