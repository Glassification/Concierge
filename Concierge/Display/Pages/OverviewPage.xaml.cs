// <copyright file="OverviewPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Pages
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Character.Aspects;
    using Concierge.Character.Enums;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Configuration;
    using Concierge.Display;
    using Concierge.Display.Enums;
    using Concierge.Display.Windows;
    using Concierge.Services;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Interaction logic for OverviewPage.xaml.
    /// </summary>
    public partial class OverviewPage : Page, IConciergePage
    {
        public OverviewPage()
        {
            this.InitializeComponent();
            this.InspirationLabel.IsIcon = true;
        }

        public bool HasEditableDataGrid => false;

        public ConciergePage ConciergePage => ConciergePage.Overview;

        public void Draw(bool isNewCharacterSheet = false)
        {
            this.DrawDetails();
            this.DrawAttributes();
            this.DrawSavingThrows();
            this.DrawSkills();
            this.DrawHealth();
            this.DrawArmorClass();
            this.DrawHitDice();
            this.DrawWealth();
            this.DrawWeight();
        }

        public void DrawDetails()
        {
            var character = Program.CcsFile.Character;

            this.InitiativeLabel.Value = character.Initiative.ToString();
            this.PerceptionLabel.Value = character.PassivePerception.ToString();
            this.VisionLabel.Value = character.Detail.Senses.Vision.ToString().FormatFromPascalCase();
            this.MovementLabel.Value = character.GetMovement().ToString();
            this.InspirationLabel.IconKind = character.Detail.Senses.Inspiration ? PackIconKind.WeatherSunset : PackIconKind.None;
        }

        public void DrawAttributes()
        {
            var character = Program.CcsFile.Character;

            this.StrengthAttributeDisplay.Draw(character.Attributes.Strength);
            this.DexterityAttributeDisplay.Draw(character.Attributes.Dexterity);
            this.ConstitutionAttributeDisplay.Draw(character.Attributes.Constitution);
            this.IntelligenceAttributeDisplay.Draw(character.Attributes.Intelligence);
            this.WisdomAttributeDisplay.Draw(character.Attributes.Wisdom);
            this.CharismaAttributeDisplay.Draw(character.Attributes.Charisma);
        }

        public void DrawSavingThrows()
        {
            var character = Program.CcsFile.Character;

            this.StrengthSavingThrow.Draw(character.Attributes.Strength);
            this.DexteritySavingThrow.Draw(character.Attributes.Dexterity);
            this.ConstitutionSavingThrow.Draw(character.Attributes.Constitution);
            this.IntelligenceSavingThrow.Draw(character.Attributes.Intelligence);
            this.WisdomSavingThrow.Draw(character.Attributes.Wisdom);
            this.CharismaSavingThrow.Draw(character.Attributes.Charisma);
        }

        public void DrawSkills()
        {
            var character = Program.CcsFile.Character;

            this.AthleticsSkill.Draw(character.Attributes, character.Attributes.Athletics);
            this.AcrobaticsSkill.Draw(character.Attributes, character.Attributes.Acrobatics);
            this.SleightOfHandSkill.Draw(character.Attributes, character.Attributes.SleightOfHand);
            this.StealthSkill.Draw(character.Attributes, character.Attributes.Stealth);
            this.ArcanaSkill.Draw(character.Attributes, character.Attributes.Arcana);
            this.HistorySkill.Draw(character.Attributes, character.Attributes.History);
            this.InvestigationSkill.Draw(character.Attributes, character.Attributes.Investigation);
            this.NatureSkill.Draw(character.Attributes, character.Attributes.Nature);
            this.ReligionSkill.Draw(character.Attributes, character.Attributes.Religion);
            this.AnimalHandlingSkill.Draw(character.Attributes, character.Attributes.AnimalHandling);
            this.InsightSkill.Draw(character.Attributes, character.Attributes.Insight);
            this.MedicineSkill.Draw(character.Attributes, character.Attributes.Medicine);
            this.PerceptionSkill.Draw(character.Attributes, character.Attributes.Perception);
            this.SurvivalSkill.Draw(character.Attributes, character.Attributes.Survival);
            this.DeceptionSkill.Draw(character.Attributes, character.Attributes.Deception);
            this.IntimidationSkill.Draw(character.Attributes, character.Attributes.Intimidation);
            this.PerformanceSkill.Draw(character.Attributes, character.Attributes.Performance);
            this.PersuasionSkill.Draw(character.Attributes, character.Attributes.Persuasion);
        }

        public void DrawHealth()
        {
            this.HealthDisplay.Draw(Program.CcsFile.Character.Vitality);
            this.DrawDeathSavingThrows();
        }

        public void DrawArmorClass()
        {
            var character = Program.CcsFile.Character;

            this.ArmorClassField.Text = character.Equipment.Defense.GetTotalArmorClass(character.Attributes.Dexterity).ToString();
            this.ArmorStatusIcon.Kind = character.Equipment.Defense.ArmorStatusIcon;
            this.ArmorStatusIcon.ToolTip = $"Armor is {character.Equipment.Defense.ArmorStatus}";
            this.ArmorStatusIcon.Foreground = character.Equipment.Defense.ArmorStatusBrush;
        }

        public void DrawHitDice()
        {
            this.HitDiceDisplay.DrawHitDice(Program.CcsFile.Character.Vitality.HitDice);
        }

        public void DrawWealth()
        {
            this.WealthDisplay.SetWealth(Program.CcsFile.Character.Wealth, Program.CcsFile.Character.Equipment);
        }

        public void DrawWeight()
        {
            this.WeightDisplay.SetWeightValues(Program.CcsFile.Character, AppSettingsManager.UserSettings.UnitOfMeasurement);
            this.WeightDisplay.FormatCarryWeight(Program.CcsFile.Character);
        }

        public void Edit(object itemToEdit)
        {
            throw new NotImplementedException();
        }

        private void DrawDeathSavingThrows()
        {
            var deathSaves = Program.CcsFile.Character.Vitality.DeathSavingThrows;
            deathSaves.LazyInitialize();

            this.HealthDisplay.SetDeathSaveStyle(deathSaves);
        }

        private void TakeDamageButton_Click(object sender, RoutedEventArgs e)
        {
            var result = ConciergeWindowService.ShowDamage(
                Program.CcsFile.Character.Vitality,
                typeof(HpWindow),
                this.Window_ApplyChanges,
                ConciergePage.Overview);
            this.DrawHealth();
        }

        private void HealDamageButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowHeal(
                Program.CcsFile.Character.Vitality,
                typeof(HpWindow),
                this.Window_ApplyChanges,
                ConciergePage.Overview);
            this.DrawHealth();
        }

        private void SavingThrow_ToggleClicked(object sender, RoutedEventArgs e)
        {
            this.DrawSavingThrows();
        }

        private void Skill_ToggleClicked(object sender, RoutedEventArgs e)
        {
            this.DrawDetails();
            this.DrawSkills();
        }

        private void HitDiceDisplay_ValueChanged(object sender, RoutedEventArgs e)
        {
            this.DrawHitDice();
            this.DrawHealth();
        }

        private void HealthDisplay_SaveClicked(object sender, RoutedEventArgs e)
        {
            var deathSave = Program.CcsFile.Character.Vitality.DeathSavingThrows;
            var name = Program.CcsFile.Character.Disposition.Name;
            if (deathSave.DeathSaveStatus == AbilitySave.Success)
            {
                var succeed = name.IsNullOrWhiteSpace() ? "Succeeded 3 saves and stabilized!" : $"{name} succeeded 3 saves and is stabilized!";
                Program.MainWindow?.DisplayStatusText(succeed);
            }
            else if (deathSave.DeathSaveStatus == AbilitySave.Failure)
            {
                var fail = name.IsNullOrWhiteSpace() ? "Failed 3 saves and died!" : $"{name} failed 3 saves and has died!";
                Program.MainWindow?.DisplayStatusText(fail);
            }

            this.DrawDeathSavingThrows();
        }

        private void EditHealth_Click(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();

            ConciergeWindowService.ShowEdit(
                Program.CcsFile.Character.Vitality.Health,
                typeof(HealthWindow),
                this.Window_ApplyChanges,
                ConciergePage.Overview);
            this.DrawHealth();
        }

        private void HitDiceDisplay_EditClicked(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();

            ConciergeWindowService.ShowEdit(
                Program.CcsFile.Character.Vitality.HitDice,
                typeof(HitDiceWindow),
                this.Window_ApplyChanges,
                ConciergePage.Overview);
            this.DrawHitDice();
        }

        private void WealthDisplay_EditClicked(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();

            ConciergeWindowService.ShowEdit(
                Program.CcsFile.Character.Wealth,
                sender,
                typeof(WealthWindow),
                this.Window_ApplyChanges,
                ConciergePage.Overview);
            this.DrawWealth();
        }

        private void Grid_SavingThrowMouseUp(object sender, MouseButtonEventArgs e)
        {
            SoundService.PlayNavigation();

            ConciergeWindowService.ShowEdit(
                Program.CcsFile.Character.Attributes,
                typeof(SavingThrowWindow),
                this.Window_ApplyChanges,
                ConciergePage.Overview);
            this.DrawSavingThrows();
        }

        private void Grid_SkillMenuMouseUp(object sender, MouseButtonEventArgs e)
        {
            SoundService.PlayNavigation();

            ConciergeWindowService.ShowEdit(
                Program.CcsFile.Character.Attributes,
                typeof(SkillWindow),
                this.Window_ApplyChanges,
                ConciergePage.Overview);
            this.DrawSkills();
        }

        private void Label_EditClicked(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();

            ConciergeWindowService.ShowEdit(
                Program.CcsFile.Character.Detail.Senses,
                typeof(SensesWindow),
                this.Window_ApplyChanges,
                ConciergePage.Overview);
            this.DrawDetails();
        }

        private void AttributeDisplay_EditClicked(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();

            ConciergeWindowService.ShowEdit(
                Program.CcsFile.Character.Attributes,
                typeof(AttributesWindow),
                this.Window_ApplyChanges,
                ConciergePage.Overview);
            this.Draw();
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case nameof(ArmorWindow):
                    this.DrawArmorClass();
                    break;
                case nameof(AttributesWindow):
                    this.Draw();
                    break;
                case nameof(SensesWindow):
                    this.DrawDetails();
                    break;
                case nameof(HealthWindow):
                    this.DrawHealth();
                    break;
                case nameof(HitDiceWindow):
                    this.DrawHitDice();
                    break;
                case nameof(SkillWindow):
                    this.DrawSkills();
                    break;
                case nameof(SavingThrowWindow):
                    this.DrawSavingThrows();
                    break;
                case nameof(WealthWindow):
                    this.DrawWealth();
                    break;
            }
        }

        private void ArmorClass_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SoundService.PlayNavigation();

            ConciergeWindowService.ShowEdit(
                Program.CcsFile.Character.Equipment.Defense,
                typeof(ArmorWindow),
                this.Window_ApplyChanges,
                ConciergePage.Overview);
            this.DrawArmorClass();
        }

        private void AllProficiency_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SoundService.PlayUpdateValue();

            var character = Program.CcsFile.Character;
            var attributeCopy = character.Attributes.DeepCopy();

            var state = character.Attributes.GetProficiencyState();
            character.Attributes.SetProficiencyState(state);
            this.DrawSkills();

            Program.UndoRedoService.AddCommand(new EditCommand<Attributes>(character.Attributes, attributeCopy, this.ConciergePage));
        }

        private void AllExpertise_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SoundService.PlayUpdateValue();

            var character = Program.CcsFile.Character;
            var attributeCopy = character.Attributes.DeepCopy();

            var state = character.Attributes.GetExpertiseState();
            character.Attributes.SetExpertiseState(state);
            this.DrawSkills();

            Program.UndoRedoService.AddCommand(new EditCommand<Attributes>(character.Attributes, attributeCopy, this.ConciergePage));
        }

        private void SaveProficiency_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SoundService.PlayUpdateValue();

            var character = Program.CcsFile.Character;
            var attributeCopy = character.Attributes.DeepCopy();

            var state = character.Attributes.GetState();
            character.Attributes.Strength.Proficiency = state;
            character.Attributes.Dexterity.Proficiency = state;
            character.Attributes.Constitution.Proficiency = state;
            character.Attributes.Intelligence.Proficiency = state;
            character.Attributes.Wisdom.Proficiency = state;
            character.Attributes.Charisma.Proficiency = state;
            this.DrawSavingThrows();

            Program.UndoRedoService.AddCommand(new EditCommand<Attributes>(character.Attributes, attributeCopy, this.ConciergePage));
        }
    }
}
