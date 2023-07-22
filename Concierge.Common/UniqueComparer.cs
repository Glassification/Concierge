// <copyright file="UniqueComparer.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common
{
    using System.Collections.Generic;

    public sealed class UniqueComparer<T> : IComparer<T>
        where T : IUnique
    {
        public UniqueComparer()
        {
        }

        public int Compare(T? x, T? y)
        {
            if (x?.Name.Equals(y?.Name) ?? false)
            {
                return 0;
            }
            else if (x is null)
            {
                return -1;
            }
            else if (y is null)
            {
                return 1;
            }
            else
            {
                return string.Compare(x.Name, y.Name);
            }
        }
    }
}
