// <copyright file="HealthControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Character;
    using Concierge.Character.Enums;
    using Concierge.Character.Vitals;
    using Concierge.Commands;
    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Concierge.Display.Enums;
    using Concierge.Tools.DiceRoller;
    using MaterialDesignThemes.Wpf;

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
                this.HpLabel.Margin = new Thickness(5, 40, 0, 0);
            }
        }

        public void Draw(Vitality vitality)
        {
            this.CurrentHealth = vitality.CurrentHealth;
            this.TotalHealth = vitality.Health.MaxHealth;

            int third = vitality.Health.MaxHealth / 3;
            int hp = vitality.CurrentHealth;

            var brush = hp < third && hp > 0
                ? Brushes.IndianRed
                : hp >= third * 2 && hp > 0 ? Brushes.DarkGreen : hp <= 0 ? Brushes.DarkGray : Brushes.DarkOrange;

            this.HpBackground.Background = brush;
            this.HpBorder.BorderBrush = brush;

            this.HealDamageButton.SetEnableState(!vitality.Health.IsFull);
            this.TakeDamageButton.SetEnableState(!vitality.Health.IsEmpty);
        }

        public void Draw(Health health)
        {
            this.CurrentHealth = health.BaseHealth;
            this.TotalHealth = health.MaxHealth;

            int third = health.MaxHealth / 3;
            int hp = health.BaseHealth;

            var brush = hp < third && hp > 0
                ? Brushes.IndianRed
                : hp >= third * 2 && hp > 0 ? Brushes.DarkGreen : hp <= 0 ? Brushes.DarkGray : Brushes.DarkOrange;

            this.HpBackground.Background = brush;
            this.HpBorder.BorderBrush = brush;

            this.HealDamageButton.SetEnableState(!health.IsFull);
            this.TakeDamageButton.SetEnableState(!health.IsEmpty);
        }

        public void SetDeathSaveStyle(DeathSavingThrows deathSavingThrows)
        {
            SetDeathSaveStyleHelper(this.DeathSave1, deathSavingThrows.DeathSaves[0]);
            SetDeathSaveStyleHelper(this.DeathSave2, deathSavingThrows.DeathSaves[1]);
            SetDeathSaveStyleHelper(this.DeathSave3, deathSavingThrows.DeathSaves[2]);
            SetDeathSaveStyleHelper(this.DeathSave4, deathSavingThrows.DeathSaves[3]);
            SetDeathSaveStyleHelper(this.DeathSave5, deathSavingThrows.DeathSaves[4]);
        }

        private static void SetDeathSaveStyleHelper(PackIcon packIcon, AbilitySave deathSave)
        {
            packIcon.Foreground = deathSave switch
            {
                AbilitySave.Failure => Brushes.IndianRed,
                AbilitySave.Success => ConciergeBrushes.Mint,
                _ => Brushes.SlateGray,
            };
        }

        private static void SetStatusText(DiceRoll diceRoll, CharacterSheet character)
        {
            var deathSave = character.Vitality.DeathSavingThrows;
            var name = character.Disposition.Name;
            var bulder = new StringBuilder($"Rolled {diceRoll}");
            if (deathSave.DeathSaveStatus == AbilitySave.Success)
            {
                bulder.Append(StringUtility.CreateCharacters(" ", 6));
                bulder.Append(name.IsNullOrWhiteSpace() ? "Succeeded 3 saves and stabilized!" : $"{name} succeeded 3 saves and is stabilized!");
            }
            else if (deathSave.DeathSaveStatus == AbilitySave.Failure)
            {
                bulder.Append(StringUtility.CreateCharacters(" ", 6));
                bulder.Append(name.IsNullOrWhiteSpace() ? "Failed 3 saves and died!" : $"{name} failed 3 saves and has died!");
            }

            Program.MainWindow?.DisplayStatusText(bulder.ToString());
        }

        private void SetButtonEnableState(AbilitySave abilitySave)
        {
            var isEnabled = abilitySave == AbilitySave.None;
            this.RollSave.SetEnableState(isEnabled);
        }

        private void RollSave_Click(object sender, RoutedEventArgs e)
        {
            var character = Program.CcsFile.Character;
            if (character.Vitality.DeathSavingThrows.DeathSaveStatus != AbilitySave.None)
            {
                return;
            }

            var oldItem = character.Vitality.DeathSavingThrows.DeepCopy();
            var diceRoll = character.Vitality.DeathSavingThrows.RollDeathSave();
            Program.UndoRedoService.AddCommand(new EditCommand<DeathSavingThrows>(character.Vitality.DeathSavingThrows, oldItem, ConciergePages.Overview));

            SetStatusText(diceRoll, character);
            this.SetButtonEnableState(character.Vitality.DeathSavingThrows.DeathSaveStatus);
            this.RaiseEvent(new RoutedEventArgs(SaveClickedEvent));
        }

        private void ResetSaves_Click(object sender, RoutedEventArgs e)
        {
            var character = Program.CcsFile.Character;
            var oldItem = character.Vitality.DeathSavingThrows.DeepCopy();
            character.Vitality.DeathSavingThrows.ResetDeathSaves();
            Program.UndoRedoService.AddCommand(new EditCommand<DeathSavingThrows>(character.Vitality.DeathSavingThrows, oldItem, ConciergePages.Overview));

            this.SetButtonEnableState(character.Vitality.DeathSavingThrows.DeathSaveStatus);
            this.RaiseEvent(new RoutedEventArgs(SaveClickedEvent));
        }

        private void HealDamageButton_Click(object sender, RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(HealClickedEvent));
        }

        private void TakeDamageButton_Click(object sender, RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(DamageClickedEvent));
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(EditClickedEvent));
        }
    }
}
