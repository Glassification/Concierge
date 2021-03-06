﻿// <copyright file="Guard.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility
{
    using System;

    using Concierge.Utility.Extensions;

    public static class Guard
    {
        public static void IsNull(object value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void IsNullOrEmpty(string value, string name)
        {
            if (value.IsNullOrEmpty())
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void IsNullOrWhiteSpace(string value, string name)
        {
            if (value.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}
