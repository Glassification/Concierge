﻿// <copyright file="MultiSelectService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System.Windows.Controls;

    using Concierge.Character.Equipable;
    using Concierge.Common.Extensions;
    using Concierge.Display.Components;

    /// <summary>
    /// Provides functionality for handling multi-selection in a DataGrid along with associated buttons.
    /// </summary>
    public sealed class MultiSelectService
    {
        private readonly ConciergeDesignButton upButton;
        private readonly ConciergeDesignButton downButton;
        private readonly ConciergeDesignButton recoverButton;
        private readonly ConciergeDesignButton editButton;
        private readonly ConciergeDesignButton deleteButton;

        private readonly ConciergeDesignToggleButton multiSelectButton;

        private readonly ConciergeDataGrid dataGrid;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiSelectService"/> class with the specified controls.
        /// </summary>
        /// <param name="upButton">The button to move the selected item up.</param>
        /// <param name="downButton">The button to move the selected item down.</param>
        /// <param name="recoverButton">The button to recover the selected item.</param>
        /// <param name="editButton">The button to edit the selected item.</param>
        /// <param name="deleteButton">The button to delete the selected item.</param>
        /// <param name="multiSelectButton">The toggle button to enable/disable multi-selection.</param>
        /// <param name="dataGrid">The DataGrid control.</param>
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

        /// <summary>
        /// Sets the control states based on the current selection and multi-select mode.
        /// </summary>
        public void SetControlState()
        {
            var selectedItem = this.dataGrid.SelectedItem as Augment;
            var state = selectedItem is not null && (!this.multiSelectButton.IsChecked ?? false);

            this.SetAllButtonsState(state);
            if (selectedItem is not null)
            {
                this.recoverButton.SetEnableState(selectedItem.Recoverable && selectedItem.Total < selectedItem.Quantity);
            }
        }

        /// <summary>
        /// Sets the enabled state of all associated buttons.
        /// </summary>
        /// <param name="state">The state to set.</param>
        public void SetAllButtonsState(bool state)
        {
            this.upButton.SetEnableState(state);
            this.downButton.SetEnableState(state);
            this.recoverButton.SetEnableState(state);
            this.editButton.SetEnableState(state);
            this.deleteButton.SetEnableState(state);
        }

        /// <summary>
        /// Enables multi-selection mode in the DataGrid.
        /// </summary>
        public void Check()
        {
            this.SetAllButtonsState(false);
            this.dataGrid.SelectionMode = DataGridSelectionMode.Extended;
        }

        /// <summary>
        /// Disables multi-selection mode in the DataGrid.
        /// </summary>
        public void Uncheck()
        {
            this.dataGrid.UnselectAll();
            this.dataGrid.SelectionMode = DataGridSelectionMode.Single;
        }
    }
}
