// <copyright file="HpWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Windows;

    using Concierge.Character.Enums;
    using Concierge.Character.Vitals;
    using Concierge.Commands;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Display.Windows.Utility;
    using Concierge.Services;

    /// <summary>
    /// Interaction logic for HpWindow.xaml.
    /// </summary>
    public partial class HpWindow : ConciergeWindow
    {
        private AbilitySave abilitySave;

        public HpWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.PreviousHeal = 0;
            this.PreviousDamage = 0;
            this.ConciergePage = ConciergePages.None;
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.HpUpDown);
        }

        public override string HeaderText => this.IsHealing ? "Healing" : "Damage";

        public override string WindowName => nameof(HpWindow);

        private int PreviousHeal { get; set; }

        private int PreviousDamage { get; set; }

        private bool IsHealing { get; set; }

        public override ConciergeResult ShowHeal<T>(T vitality)
        {
            if (vitality is not Vitality castItem)
            {
                return ConciergeResult.NoResult;
            }

            Program.Drawing();
            this.IsHealing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.HpUpDown.Value = this.PreviousHeal;
            Program.NotDrawing();

            this.ShowConciergeWindow();
            this.SetPreviousValue();

            if (this.Result == ConciergeResult.OK)
            {
                var oldItem = castItem.Health.DeepCopy();
                castItem.Heal(this.HpUpDown.Value);
                Program.UndoRedoService.AddCommand(new EditCommand<Health>(castItem.Health, oldItem, this.ConciergePage));
            }

            return this.Result;
        }

        public override ConciergeResult ShowDamage<T>(T vitality)
        {
            if (vitality is not Vitality castItem)
            {
                return ConciergeResult.NoResult;
            }

            Program.Drawing();
            this.IsHealing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.HpUpDown.Value = this.PreviousDamage;
            Program.NotDrawing();

            this.ShowConciergeWindow();
            this.SetPreviousValue();

            if (this.Result == ConciergeResult.OK)
            {
                var oldItem = castItem.Health.DeepCopy();
                castItem.Damage(this.HpUpDown.Value);
                Program.UndoRedoService.AddCommand(
                    new DamageCommand(
                        castItem,
                        oldItem,
                        castItem.Health.DeepCopy(),
                        this.abilitySave != AbilitySave.Success ? Program.CcsFile.Character.SpellCasting.ConcentratedSpell : null,
                        this.ConciergePage));

                if (this.abilitySave == AbilitySave.Failure)
                {
                    Program.CcsFile.Character.SpellCasting.ClearConcentration();
                }
            }

            return this.Result;
        }

        private void SetPreviousValue()
        {
            Program.Drawing();
            if (this.IsHealing)
            {
                this.PreviousHeal = this.HpUpDown.Value;
            }
            else
            {
                this.PreviousDamage = this.HpUpDown.Value;
            }

            Program.NotDrawing();
        }

        private void ConcentrationCheck(int damage)
        {
            var concentratedSpell = Program.CcsFile.Character.SpellCasting.ConcentratedSpell;
            if (this.ConciergePage != ConciergePages.Overview || concentratedSpell is null)
            {
                return;
            }

            this.abilitySave = WindowService.ShowAbilityCheckWindow(
                typeof(ConcentrationCheckWindow),
                Program.CcsFile.Character.Attributes.Constitution,
                damage);
            var status = $"{(this.abilitySave == AbilitySave.Failure ? "Lost" : "Kept")} concentration on {concentratedSpell.Name}.";

            Program.Logger.Info(status);
            Program.MainWindow?.DisplayStatusText(status);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Exit;
            this.CloseConciergeWindow();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ConcentrationCheck(this.HpUpDown.Value);
            this.ReturnAndClose();
        }
    }
}
