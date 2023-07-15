// <copyright file="InternetInfo.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Dtos
{
    using Concierge.Common.Enums;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Represents information about the Internet connection.
    /// </summary>
    public sealed class InternetInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InternetInfo"/> class with default values.
        /// </summary>
        public InternetInfo()
            : this(string.Empty, InternetStatus.Disconnected)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InternetInfo"/> class with the specified name and status.
        /// </summary>
        /// <param name="name">The name of the Internet connection.</param>
        /// <param name="status">The status of the Internet connection.</param>
        public InternetInfo(string name, InternetStatus status)
        {
            this.Name = name;
            this.Status = status;
        }

        /// <summary>
        /// Gets the icon associated with the Internet connection status.
        /// </summary>
        public PackIconKind Icon
        {
            get
            {
                return this.Status switch
                {
                    InternetStatus.Disconnected => PackIconKind.WifiOff,
                    InternetStatus.Wired => PackIconKind.Lan,
                    InternetStatus.Wireless => PackIconKind.Wifi,
                    _ => PackIconKind.None,
                };
            }
        }

        /// <summary>
        /// Gets or sets the status of the Internet connection (Disconnected, Wired, or Wireless).
        /// </summary>
        public InternetStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the name of the Internet connection.
        /// </summary>
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{this.Status} {this.Name}";
        }
    }
}
