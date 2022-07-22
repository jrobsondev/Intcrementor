namespace Intcrementor
{
    [Command(PackageIds.IncrementAll)]
    internal class IncrementAllCommand : IntcrementCommandBase<IncrementAllCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await base.ExecuteAsync(e);
            await Manager.GetSelectionsAndAdjustAsync(Manager.GetIntegersInDocViewAsMatchList(), () => Manager.SelectionBroker.PerformActionOnAllSelections(x => Manager.AdjustSelection(x.Selection, Manager.Options.DefaultStepValue)), "Incrementing all numbers");
        }
    }
}
