// <copyright file="StackExtensions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides extension methods for working with stacks.
    /// </summary>
    public static class StackExtensions
    {
        /// <summary>
        /// Creates a new stack that is a clone of the original stack.
        /// </summary>
        /// <param name="stack">The original stack to be cloned.</param>
        /// <returns>A new stack containing the same elements as the original stack.</returns>
        public static Stack<object> Clone(this Stack<object> stack)
        {
            var list = stack.ToArray().Reverse();
            var newStack = new Stack<object>(list);

            return newStack;
        }
    }
}
