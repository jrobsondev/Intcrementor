using Microsoft.VisualStudio.Text;

namespace Intcrementor.Helpers
{
    internal static class NumberHelper
    {
        internal static void AdjustSelection(SnapshotSpan selection, ITextBuffer textBuffer, bool isIncrement)
        {
            if (selection != default && int.TryParse(selection.GetText(), out int selectedNumber))
            {
                //This will need to use the value in the options or maybe just pass a value in so it can be used by other update methods
                if (isIncrement)
                    selectedNumber++;
                else
                    selectedNumber--;
                textBuffer.Replace(selection, selectedNumber.ToString());
            }
        }
    }
}
