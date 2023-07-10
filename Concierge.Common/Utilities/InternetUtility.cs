// <copyright file="InternetUtility.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Utilities
{
    using System;
    using System.Net.NetworkInformation;

    public static class InternetUtility
    {
        public const int Timeout = 1000;
        public const string Host = "google.com";

        public static bool IsConnected
        {
            get
            {
                try
                {
                    return new Ping().Send(Host, Timeout, new byte[32], new PingOptions()).Status == IPStatus.Success;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
