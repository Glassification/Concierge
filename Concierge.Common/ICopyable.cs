// <copyright file="ICopyable.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common
{
    public interface ICopyable<T>
    {
        T DeepCopy();
    }
}
