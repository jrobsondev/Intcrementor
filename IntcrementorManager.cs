using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Intcrementor
{
    public class IntcrementorManager
    {
        private const string PATTERN = @"\b\d{1,2}\b";
        private readonly DocumentView _DocView;
        private readonly IWpfTextView _TextView;
        private readonly string _DocumentText;
        public IMultiSelectionBroker SelectionBroker { get; }

        public IntcrementorManager(DocumentView docView)
        {
            _DocView = docView;
            _TextView = _DocView.TextView;
            SelectionBroker = _TextView.GetMultiSelectionBroker();
            _DocumentText = _TextView.TextSnapshot.GetText();
        }

        public List<Match> GetIntegersInDocViewAsMatchList() => Regex.Matches(_DocumentText, PATTERN).Cast<Match>().ToList();

        internal void AdjustSelection(Microsoft.VisualStudio.Text.Selection selection, int adjustmentStep)
        {
            var span = selection.Extent.SnapshotSpan;
            if (selection != default && int.TryParse(span.GetText(), out int selectedNumber))
            {
                selectedNumber += adjustmentStep;
                _DocView.TextBuffer.Replace(span, selectedNumber.ToString());
            }
        }

        public async Task GetSelectionsAndAdjustAsync(List<Match> matches, Action action, string undoContextDescription)
        {
            ITextUndoTransaction undoContext = await _DocView.TextBuffer.OpenUndoContextAsync(undoContextDescription);
            List<Microsoft.VisualStudio.Text.Selection> selections = new();
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    SnapshotPoint point = new(_TextView.TextSnapshot, match.Index);
                    _TextView.Caret.MoveTo(point);
                    SnapshotSpan span = new(point, match.Length);
                    _TextView.Selection.Select(span, false);
                    SnapshotSpan snapshotSpan = _TextView.Selection.SelectedSpans.FirstOrDefault();
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
