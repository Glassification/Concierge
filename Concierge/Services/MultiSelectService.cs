// <copyright file="MultiSelectService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System.Windows.Controls;

    using Concierge.Character.Equipable;
    using Concierge.Common.Utilities;
    using Concierge.Display.Components;

    public sealed class MultiSelectService
    {
        private readonly ConciergeDesignButton upButton;
        private readonly ConciergeDesignButton downButton;
        private readonly ConciergeDesignButton recoverButton;
        private readonly ConciergeDesignButton editButton;
        private readonly ConciergeDesignButton deleteButton;

        private readonly ConciergeDesignToggleButton multiSelectButton;

        private readonly ConciergeDataGrid dataGrid;

        public MultiSelectService(
            ConciergeDesignButton upButton,
            ConciergeDesignButton downButton,
            ConciergeDesignButton recoverButton,
            ConciergeDesignButton editButton,
            ConciergeDesignButton deleteButton,
            ConciergeDesignToggleButton multiSelectButton,
            ConciergeDataGrid dataGrid)
        {
            this.upButton = upButton;
            this.downButton = downButton;
            this.recoverButton = recoverButton;
            this.editButton = editButton;
            this.deleteButton = deleteButton;
            this.multiSelectButton = multiSelectButton;
            this.dataGrid = dataGrid;
        }

        public void SetControlState()
        {
            var selectedItem = this.dataGrid.SelectedItem as Augment;
            var state = selectedItem is not null && (!this.multiSelectButton.IsChecked ?? false);

            this.SetAllButtonsState(state);
            if (selectedItem is not null)
            {
                DisplayUtility.SetControlEnableState(this.recoverButton, selectedItem.Recoverable && selectedItem.Total < selectedItem.Quantity);
            }
        }

        public void SetAllButtonsState(bool state)
        {
            DisplayUtility.SetControlEnableState(this.upButton, state);
            DisplayUtility.SetControlEnableState(this.downButton, state);
            DisplayUtility.SetControlEnableState(this.recoverButton, state);
            DisplayUtility.SetControlEnableState(this.editButton, state);
            DisplayUtility.SetControlEnableState(this.deleteButton, state);
        }

        public void Check()
        {
            this.SetAllButtonsState(false);
            this.dataGrid.SelectionMode = DataGridSelectionMode.Extended;
        }

        public void Uncheck()
        {
            this.dataGrid.UnselectAll();
            this.dataGrid.SelectionMode = DataGridSelectionMode.Single;
        }
    }
}
