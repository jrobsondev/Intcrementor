﻿using Intcrementor.Helpers;

namespace Intcrementor
{
    [Command(PackageIds.IncrementAll)]
    internal class IncrementAllCommand : IntcrementCommandBase<IncrementAllCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await base.ExecuteAsync(e);
            await GetSelectionsAndAdjustAsync(() => SelectionBroker.PerformActionOnAllSelections(x => NumberHelper.AdjustSelection(x.Selection, DocView.TextBuffer, true)), "Incrementing all numbers");
        }
    }
}
