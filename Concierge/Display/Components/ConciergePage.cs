// <copyright file="ConciergePage.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Components
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Common.Extensions;
    using Concierge.Display.Enums;
    using Concierge.Persistence;

    public abstract partial class ConciergePage : Page
    {
        public ConciergePage()
        {
            var resourceDictionary = new ResourceDictionary
            {
                Source = new Uri("Display/Dictionaries/FontDictionary.xaml", UriKind.RelativeOrAbsolute),
            };

            this.AllowDrop = true;
            this.Style = resourceDictionary["PageFontStyle"] as Style;

            this.Drop += this.Page_Drop;
        }

        public ConciergePages ConciergePages { get; protected set; }

        public bool HasEditableDataGrid { get; protected set; }

        public abstract void Draw(bool isNewCharacterSheet = false);

        public abstract void Edit(object itemToEdit);

        private void Page_Drop(object sender, DragEventArgs e)
        {
            ConciergeDragDrop.Handle();

            var file = ConciergeDragDrop.Capture(e.Data, ".ccs");
            if (!file.IsValid)
            {
                var message = file.FilePath.IsNullOrWhiteSpace() ? string.Empty : $"Could not open '{file.FilePath}'\n";
                ConciergeMessageBox.ShowError($"{message}{file.ErrorMessage}");
                return;
            }

            Program.MainWindow?.OpenCharacterSheet(file.FilePath);
        }
    }
}
