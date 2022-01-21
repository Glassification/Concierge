// <copyright file="DataGridDictionary.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.Dictionary
{
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Utility;

    public partial class DataGridDictionary
    {
        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is not DataGridRow dataGridRow)
            {
                return;
            }

            var page = dataGridRow.Tag as IConciergePage;
            ConciergeSound.UpdateValue();
            page?.Edit(dataGridRow.DataContext);
        }
    }
}
