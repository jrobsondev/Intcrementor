using Microsoft.VisualStudio.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace Intcrementor
{
    [Command(PackageIds.MyCommand)]
    internal sealed class MyCommand : BaseCommand<MyCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await Package.JoinableTaskFactory.SwitchToMainThreadAsync();
            DocumentView docView = await VS.Documents.GetActiveDocumentViewAsync();
            if (docView?.TextView == null) return;

            const string PATTERN = @"\b\d{1,2}\b";
            var text = docView.TextView.TextSnapshot.GetText();
            var matches = Regex.Matches(text, PATTERN);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    var point = new SnapshotPoint(docView.TextView.TextSnapshot, match.Index);
                    docView.TextView.Caret.MoveTo(point);
                    SnapshotSpan span = new SnapshotSpan(point, match.Length);
                    docView.TextView.Selection.Select(span, false);
                    Increment(docView);
                }
            }
        }

        private void Increment(DocumentView docView)
        {
            var selection = docView.TextView.Selection.SelectedSpans.FirstOrDefault();
            if (selection != default && int.TryParse(selection.GetText(), out int selectedNumber))
            {
                selectedNumber++;
                docView.TextBuffer.Replace(selection, selectedNumber.ToString());
            }
        }
    }
}
