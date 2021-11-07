// <copyright file="ICopyable.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility
{
    public interface ICopyable
    {
        ICopyable DeepCopy();
    }
}
