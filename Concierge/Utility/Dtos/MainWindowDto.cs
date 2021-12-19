// <copyright file="MainWindowDto.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Dtos
{
    using System.Windows;

    public class MainWindowDto
    {
        public MainWindowDto()
        {
        }

        public Point Center { get; set; }

        public Point Location { get; set; }

        public int ActualWidth { get; set; }

        public WindowState WindowState { get; set; }
    }
}
