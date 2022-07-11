using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Intcrementor
{
    internal class IntcrementCommandBase<TCommand> : BaseCommand<TCommand>
        where TCommand : IntcrementCommandBase<TCommand>, new()
    {
        protected const string PATTERN = @"\b\d{1,2}\b";
        protected DocumentView DocView { get; private set; }
        protected IWpfTextView TextView { get; private set; }
        protected string DocumentText { get; private set; }
        protected MatchCollection IntMatches { get; private set; }
        protected IMultiSelectionBroker SelectionBroker { get; private set; }

        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await Package.JoinableTaskFactory.SwitchToMainThreadAsync();
            DocView = await VS.Documents.GetActiveDocumentViewAsync();
            if (DocView?.TextView == null) return;

            TextView = DocView.TextView;
            SelectionBroker = TextView.GetMultiSelectionBroker();
            DocumentText = TextView.TextSnapshot.GetText();
            IntMatches = Regex.Matches(DocumentText, PATTERN);
        }

        internal async Task GetSelectionsAndAdjustAsync(Action action, string undoContextDescription)
        {
            var undoContext = await DocView.TextBuffer.OpenUndoContextAsync(undoContextDescription);
            var selections = new List<Microsoft.VisualStudio.Text.Selection>();
            if (IntMatches.Count > 0)
            {
                foreach (Match match in IntMatches)
                {
                    SnapshotPoint point = new(TextView.TextSnapshot, match.Index);
                    TextView.Caret.MoveTo(point);
                    SnapshotSpan span = new(point, match.Length);
                    TextView.Selection.Select(span, false);
                    SnapshotSpan snapshotSpan = TextView.Selection.SelectedSpans.FirstOrDefault();
                    Microsoft.VisualStudio.Text.Selection selection = new(snapshotSpan);
                    selections.Add(selection);
                }
                SelectionBroker.AddSelectionRange(selections);
                try
                {
                    action.Invoke();
                    undoContext.Complete();
                }
                catch
                {
                    undoContext.Dispose();
                }
            }
        }
    }
}
