namespace Intcrementor
{
    [Command(PackageIds.DecrementAll)]
    internal class DecrementAllCommand : IntcrementCommandBase<DecrementAllCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await base.ExecuteAsync(e);
            await Manager.GetSelectionsAndAdjustAsync(Manager.GetIntegersInDocViewAsMatchList(), () => Manager.SelectionBroker.PerformActionOnAllSelections(x => Manager.AdjustSelection(x.Selection, -1)), "Decrementing all numbers");
        }
    }
}
