using Intcrementor.Helpers;

namespace Intcrementor
{
    [Command(PackageIds.DecrementAll)]
    internal class DecrementAllCommand : IntcrementCommandBase<DecrementAllCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await base.ExecuteAsync(e);
            await GetSelectionsAndAdjustAsync(() => SelectionBroker.PerformActionOnAllSelections(x => NumberHelper.AdjustSelection(x.Selection, DocView.TextBuffer, false)), "Decrementing all numbers");
        }
    }
}
