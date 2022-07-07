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

            //======== TEST AREA ========//

            var lines = docView.TextView.TextViewLines.ToList();
            string PATTERN = @"\b\d{1,2}\b";
            foreach (var line in lines)
            {
                var match = Regex.Match(line.Snapshot.GetText(), PATTERN);
                if (match.Success)
                {
                    var point = new SnapshotPoint(line.Snapshot, match.Index);
                    docView.TextView.Caret.MoveTo(point);
                    SnapshotSpan span = new SnapshotSpan(point, match.Length);
                    docView.TextView.Selection.Select(span, false);
                    Increment(docView);
                    //Need to figure out how to select the position + length of integer value then can increment!
                }
            }

            //======== TEST AREA ========//
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
