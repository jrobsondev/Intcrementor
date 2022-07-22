using Intcrementor.Options;
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
        private const string PATTERN_FORMAT_STRING = @"\b\d{{1,{0}}}\b";
        private readonly DocumentView _DocView;
        private readonly IWpfTextView _TextView;
        private readonly string _DocumentText;
        private readonly string _IntegerMatchPattern;

        public IntcrementorManager(DocumentView docView, General options)
        {
            _DocView = docView;
            _TextView = _DocView.TextView;
            _DocumentText = _TextView.TextSnapshot.GetText();
            _IntegerMatchPattern = string.Format(PATTERN_FORMAT_STRING, options.MaxIntegerLength);
            SelectionBroker = _TextView.GetMultiSelectionBroker();
            Options = options;
        }

        public IMultiSelectionBroker SelectionBroker { get; }
        public General Options { get; }

        public List<Match> GetIntegersInDocViewAsMatchList() => Regex.Matches(_DocumentText, _IntegerMatchPattern).Cast<Match>().ToList();

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
                    await VS.StatusBar.ShowMessageAsync($"Successfully updated: {matches.Count} numbers");
                }
                catch (Exception ex)
                {
                    undoContext.Dispose();
                    await ErrorHandler.ShowErrorNotificationAsync(ex);
                }
            }
        }
    }
}
