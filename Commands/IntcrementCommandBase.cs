using Intcrementor.Options;

namespace Intcrementor
{
    internal class IntcrementCommandBase<TCommand> : BaseCommand<TCommand>
        where TCommand : IntcrementCommandBase<TCommand>, new()
    {
        public IntcrementorManager Manager { get; private set; }

        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await Package.JoinableTaskFactory.SwitchToMainThreadAsync();
            var docView = await VS.Documents.GetActiveDocumentViewAsync();
            if (docView?.TextView == null) return;
            var options = await General.GetLiveInstanceAsync();

            Manager = new(docView, options);
        }
    }
}
