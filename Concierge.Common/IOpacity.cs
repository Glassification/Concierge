// <copyright file="IOpacity.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common
{
    /// <summary>
    /// Represents an interface for managing the enable state of an object with opacity settings.
    /// </summary>
    public interface IOpacity
    {
        /// <summary>
        /// Sets custom handling for the enable state of an object.
        /// </summary>
        /// <param name="state">
        /// A boolean value indicating the desired state.
        /// </param>
        void SetEnableState(bool state);
    }
}
