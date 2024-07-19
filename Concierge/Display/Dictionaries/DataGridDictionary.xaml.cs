// <copyright file="DataGridDictionary.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Dictionaries
{
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Display.Components;
    using Concierge.Services;

    public partial class DataGridDictionary
    {
        private static void PageEdit(DataGridRow dataGridRow)
        {
            var page = dataGridRow.Tag as ConciergePage;
            if (page?.HasEditableDataGrid ?? false)
            {
                SoundService.PlayUpdateValue();
            }

            page?.Edit(dataGridRow.DataContext);
        }

        private static void WindowEdit(DataGridRow dataGridRow)
        {
            var window = dataGridRow.Tag as ConciergeWindow;
            if (window is not null)
            {
                SoundService.PlayUpdateValue();
                window.ShowEdit(dataGridRow.DataContext);
            }
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is not DataGridRow dataGridRow)
            {
                return;
            }

            PageEdit(dataGridRow);
            WindowEdit(dataGridRow);
        }
    }
}
