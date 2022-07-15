using Intcrementor.DialogWindows;

namespace Intcrementor
{
    [Command(PackageIds.RangeDialog)]
    internal class RangeDialogCommand : IntcrementCommandBase<RangeDialogCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await base.ExecuteAsync(e);
            RangeDialogWindow dialog = new(this);
            dialog.ShowModal();
        }
    }
}
