// <copyright file="PlayerHandbookPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.PlayerHandbookPageInterface
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Windows.Controls;

    using Concierge.Interfaces.Enums;

    /// <summary>
    /// Interaction logic for PlayerHandbookPage.xaml.
    /// </summary>
    public partial class PlayerHandbookPage : Page, IConciergePage
    {
        public PlayerHandbookPage()
        {
            this.InitializeComponent();
            this.PdfViewer.Source = new Uri(@$"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Properties\Resources\Pdfs\DnD5ePlayersHandbook.pdf");
        }

        public ConciergePage ConciergePage => ConciergePage.PlayerHandbook;

        public bool HasEditableDataGrid => false;

        public void Draw()
        {
        }

        public void Edit(object itemToEdit)
        {
            throw new NotImplementedException();
        }
    }
}
