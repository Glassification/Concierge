// <copyright file="NotifiableText.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows.Helpers
{
    using System.ComponentModel;

    using Concierge.Common.Extensions;

    public sealed class NotifiableText : INotifyPropertyChanged
    {
        private string text = string.Empty;

        public NotifiableText()
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                this.text = value;
                this.OnPropertyChanged(nameof(this.Text));
            }
        }

        public bool IsEmpty => this.Text.IsNullOrWhiteSpace();

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged is not null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
