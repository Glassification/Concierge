// <copyright file="SpellDetailsWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows.Utility
{
    using System.Windows;

    using Concierge.Common.Extensions;
    using Concierge.Display.Components;

    /// <summary>
    /// Interaction logic for SpellDetailsWindow.xaml.
    /// </summary>
    public partial class SpellDetailsWindow : ConciergeWindow
    {
        public SpellDetailsWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();
        }

        public override string HeaderText => "Spell Details";

        public override string WindowName => nameof(SpellDetailsWindow);

        public override object? ShowWindow()
        {
            this.Draw();
            this.ShowConciergeWindow();

            return null;
        }

        public void Draw()
        {
            var spellcasting = Program.CcsFile.Character.SpellCasting;
            if (spellcasting.Spells.IsEmpty())
            {
                return;
            }

            this.SpellDetailsDataGrid.ItemsSource = spellcasting.GetSpellDetails([.. Defaults.MagicClasses]);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseConciergeWindow();
        }
    }
}
