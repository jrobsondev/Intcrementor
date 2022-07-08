using Intcrementor.Helpers;
using Microsoft.VisualStudio.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Intcrementor
{
    [Command(PackageIds.DecrementAll)]
    internal class DecrementAllCommand : IntcrementCommandBase<DecrementAllCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await base.ExecuteAsync(e);
            var undoContext = await DocView.TextBuffer.OpenUndoContextAsync("Decrementing all numbers");
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
                    SelectionBroker.PerformActionOnAllSelections(x => NumberHelper.AdjustSelection(x.Selection, DocView.TextBuffer, false));
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
