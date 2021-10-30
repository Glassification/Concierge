// <copyright file="DataGridDictionary.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.Dictionary
{
    using System.Windows.Controls;
    using System.Windows.Input;

    public partial class DataGridDictionary
    {
        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is not DataGridRow)
            {
                return;
            }

            var dataGridRow = sender as DataGridRow;
            var page = dataGridRow.Tag as IConciergePage;

            page.Edit(dataGridRow.DataContext);
        }
    }
}
