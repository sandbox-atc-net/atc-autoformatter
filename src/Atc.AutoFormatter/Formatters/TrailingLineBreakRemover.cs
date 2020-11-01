using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace Atc.AutoFormatter.Formatters
{
    public class TrailingLineBreakRemover : ITextFormatter
    {
        public void Execute(string filePath, ITextView textView)
        {
            var snapshot = textView.TextSnapshot;
            var lastLine = snapshot.LineCount - 1;

            var lastTextLine = lastLine;
            while (lastTextLine >= 0 && IsEmptyLine(snapshot, lastTextLine))
                lastTextLine--;

            if (lastTextLine >= lastLine)
                return;

            var startPosition = snapshot
                .GetLineFromLineNumber(lastTextLine)
                .End
                .Position;

            var endPosition = snapshot
                .GetLineFromLineNumber(lastLine)
                .EndIncludingLineBreak
                .Position;

            using (var edit = snapshot.TextBuffer.CreateEdit())
            {
                edit.Delete(startPosition, endPosition - startPosition);
                edit.Apply();
            }
        }

        private static bool IsEmptyLine(ITextSnapshot snapshot, int lineNumber)
            => snapshot
                .GetLineFromLineNumber(lineNumber)
                .GetText()
                .Trim()
            == string.Empty;
    }
}