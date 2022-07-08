using Microsoft.VisualStudio.Text;

namespace Intcrementor.Helpers
{
    internal static class NumberHelper
    {
        internal static void AdjustSelection(Microsoft.VisualStudio.Text.Selection selection, ITextBuffer textBuffer, bool isIncrement)
        {
            var span = selection.Extent.SnapshotSpan;
            if (selection != default && int.TryParse(span.GetText(), out int selectedNumber))
            {
                //This will need to use the value in the options or maybe just pass a value in so it can be used by other update methods
                if (isIncrement)
                    selectedNumber++;
                else
                    selectedNumber--;
                textBuffer.Replace(span, selectedNumber.ToString());
            }
        }
    }
}
