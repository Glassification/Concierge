// <copyright file="MessageHistoryWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows.Utility
{
    using System.Windows;

    using Concierge.Common.Extensions;
    using Concierge.Data;
    using Concierge.Display.Components;

    /// <summary>
    /// Interaction logic for MessageHistoryWindow.xaml.
    /// </summary>
    public partial class MessageHistoryWindow : ConciergeWindow
    {
        public MessageHistoryWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();
        }

        public override string HeaderText => "Message History";

        public override string WindowName => nameof(MessageHistoryWindow);

        public override object? ShowWindow()
        {
            this.Draw();
            this.ShowConciergeWindow();

            return null;
        }

        public override void ShowEdit<T>(T item)
        {
            if (item is not MessageHistory messageHistory)
            {
                return;
            }

            Clipboard.SetText(messageHistory.Message);
        }

        public void Draw()
        {
            var messageHistory = Program.MessageService.Get();
            var count = messageHistory.Count;

            this.ItemTotalField.Text = $"({count} {"Item".Pluralize("s", count)})";
            messageHistory.ForEach(x => this.MessageHistoryDataGrid.Items.Add(x));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseConciergeWindow();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.MessageHistoryDataGrid.UnselectAll();
        }
    }
}
