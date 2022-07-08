using Intcrementor.Helpers;
using Microsoft.VisualStudio.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace Intcrementor
{
    [Command(PackageIds.IncrementAll)]
    internal class IncrementAllCommand : IntcrementCommandBase<IncrementAllCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await base.ExecuteAsync(e);
            if (IntMatches.Count > 0)
            {
                foreach (Match match in IntMatches)
                {
                    SnapshotPoint point = new (TextView.TextSnapshot, match.Index);
                    TextView.Caret.MoveTo(point);
                    SnapshotSpan span = new(point, match.Length);
                    TextView.Selection.Select(span, false);
                    SnapshotSpan selection = TextView.Selection.SelectedSpans.FirstOrDefault();
                    //NumberHelper.AdjustSelection(selection, DocView.TextBuffer, true);
                }
            }
        }
    }
}
