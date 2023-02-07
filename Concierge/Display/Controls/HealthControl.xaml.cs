// <copyright file="HealthControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;

    using Concierge.Character.Enums;
    using Concierge.Character.Statuses;
    using Concierge.Commands;
    using Concierge.Display.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for HealthControl.xaml.
    /// </summary>
    public partial class HealthControl : UserControl
    {
        public static readonly DependencyProperty CurrentHealthProperty =
            DependencyProperty.Register(
                "CurrentHealth",
                typeof(int),
                typeof(HealthControl),
                new UIPropertyMetadata(0));

        public static readonly DependencyProperty TotalHealthProperty =
            DependencyProperty.Register(
                "TotalHealth",
                typeof(int),
                typeof(HealthControl),
                new UIPropertyMetadata(0));

        public static readonly DependencyProperty DeathSavesEnabledProperty =
            DependencyProperty.Register(
                "DeathSavesEnabled",
                typeof(bool),
                typeof(HealthControl),
                new UIPropertyMetadata(true));

        public static readonly RoutedEvent HealClickedEvent =
            EventManager.RegisterRoutedEvent(
                "HealClicked",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(HealthControl));

        public static readonly RoutedEvent DamageClickedEvent =
            EventManager.RegisterRoutedEvent(
                "DamageClicked",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(HealthControl));

        public static readonly RoutedEvent EditClickedEvent =
            EventManager.RegisterRoutedEvent(
                "EditClicked",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(HealthControl));

        public static readonly RoutedEvent SaveClickedEvent =
            EventManager.RegisterRoutedEvent(
                "SaveClicked",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(HealthControl));

        public HealthControl()
        {
            this.InitializeComponent();
        }

        public event RoutedEventHandler HealClicked
        {
            add { this.AddHandler(HealClickedEvent, value); }
            remove { this.RemoveHandler(HealClickedEvent, value); }
        }

        public event RoutedEventHandler DamageClicked
        {
            add { this.AddHandler(DamageClickedEvent, value); }
            remove { this.RemoveHandler(DamageClickedEvent, value); }
        }

        public event RoutedEventHandler EditClicked
        {
            add { this.AddHandler(EditClickedEvent, value); }
            remove { this.RemoveHandler(EditClickedEvent, value); }
        }

        public event RoutedEventHandler SaveClicked
        {
            add { this.AddHandler(SaveClickedEvent, value); }
            remove { this.RemoveHandler(SaveClickedEvent, value); }
        }

        public int CurrentHealth
        {
            get
            {
                return (int)this.GetValue(CurrentHealthProperty);
            }

            set
            {
                this.SetValue(CurrentHealthProperty, value);
                this.CurrentHpField.Text = value.ToString();
            }
        }

        public int TotalHealth
        {
            get
            {
                return (int)this.GetValue(TotalHealthProperty);
            }

            set
            {
                this.SetValue(TotalHealthProperty, value);
                this.TotalHpField.Text = $"/{value}";
            }
        }

        public bool DeathSavesEnabled
        {
            get { return (bool)this.GetValue(DeathSavesEnabledProperty); }
            set { this.SetValue(DeathSavesEnabledProperty, value); }
        }

        public void InitializeDisplay()
        {
            if (!this.DeathSavesEnabled)
            {
                this.MainHealthGrid.RowDefinitions.RemoveAt(2);
                this.DeathSaveGrid.Visibility = Visibility.Collapsed;
                this.DeathSaveBorder.Visibility = Visibility.Collapsed;
            }
        }

        public void SetHealthStyle(Vitality vitality)
        {
            int third = vitality.Health.MaxHealth / 3;
            int hp = vitality.CurrentHealth;

            var brush = hp < third && hp > 0
                ? Brushes.IndianRed
                : hp >= third * 2 ? Brushes.DarkGreen : hp <= 0 ? Brushes.DarkGray : Brushes.DarkOrange;

            this.HpBackground.Foreground = brush;
            this.TotalHpField.Foreground = brush;
        }

        public void SetDeathSaveStyle(DeathSavingThrows deathSavingThrows)
        {
            SetDeathSaveStyleHelper(this.DeathSave1, deathSavingThrows.DeathSaves[0]);
            SetDeathSaveStyleHelper(this.DeathSave2, deathSavingThrows.DeathSaves[1]);
            SetDeathSaveStyleHelper(this.DeathSave3, deathSavingThrows.DeathSaves[2]);
            SetDeathSaveStyleHelper(this.DeathSave4, deathSavingThrows.DeathSaves[3]);
            SetDeathSaveStyleHelper(this.DeathSave5, deathSavingThrows.DeathSaves[4]);
        }

        private static void SetDeathSaveStyleHelper(Ellipse ellipse, DeathSave deathSave)
        {
            switch (deathSave)
            {
                case DeathSave.None:
                    ellipse.Fill = ConciergeColors.ControlBackGray;
                    break;
                case DeathSave.Failure:
                    ellipse.Fill = ConciergeColors.FailedSaveBrush;
                    break;
                case DeathSave.Success:
                    ellipse.Fill = ConciergeColors.SucceededSaveBrush;
                    break;
            }
        }

        private void PassSave_Click(object sender, RoutedEventArgs e)
        {
            var character = Program.CcsFile.Character;

            if (character.Vitality.DeathSavingThrows.DeathSaveStatus != DeathSave.None)
            {
                return;
            }

            var oldItem = character.Vitality.DeathSavingThrows.DeepCopy();
            character.Vitality.DeathSavingThrows.MakeDeathSave(DeathSave.Success);
            Program.UndoRedoService.AddCommand(new EditCommand<DeathSavingThrows>(character.Vitality.DeathSavingThrows, oldItem, ConciergePage.Overview));

            this.RaiseEvent(new RoutedEventArgs(SaveClickedEvent));

            Program.Modify();
        }

        private void FailSave_Click(object sender, RoutedEventArgs e)
        {
            var character = Program.CcsFile.Character;

            if (character.Vitality.DeathSavingThrows.DeathSaveStatus != DeathSave.None)
            {
                return;
            }

            var oldItem = character.Vitality.DeathSavingThrows.DeepCopy();
            character.Vitality.DeathSavingThrows.MakeDeathSave(DeathSave.Failure);
            Program.UndoRedoService.AddCommand(new EditCommand<DeathSavingThrows>(character.Vitality.DeathSavingThrows, oldItem, ConciergePage.Overview));

            this.RaiseEvent(new RoutedEventArgs(SaveClickedEvent));
            Program.Modify();
        }

        private void ResetSaves_Click(object sender, RoutedEventArgs e)
        {
            var oldItem = Program.CcsFile.Character.Vitality.DeathSavingThrows.DeepCopy();
            Program.CcsFile.Character.Vitality.DeathSavingThrows.ResetDeathSaves();
            Program.UndoRedoService.AddCommand(new EditCommand<DeathSavingThrows>(Program.CcsFile.Character.Vitality.DeathSavingThrows, oldItem, ConciergePage.Overview));

            this.RaiseEvent(new RoutedEventArgs(SaveClickedEvent));

            Program.Modify();
        }

        private void HealDamageButton_Click(object sender, RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(HealClickedEvent));
        }

        private void TakeDamageButton_Click(object sender, RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(DamageClickedEvent));
        }

        private void Grid_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(EditClickedEvent));
        }
    }
}
