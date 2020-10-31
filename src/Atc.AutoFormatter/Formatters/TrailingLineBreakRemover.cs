using Atc.AutoFormatter.Formatters;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace Atc.AutoFormatter.Formatters
{
    public class TrailingLineBreakRemover : ITextFormatter
    {
        public void Execute(string documentPath, ITextView textView)
        {
            var snapshot = textView.TextSnapshot;
            using (var edit = snapshot.TextBuffer.CreateEdit())
            {
                var lineNumber = snapshot.LineCount - 1;
                while (lineNumber >= 0 && IsEmptyLine(snapshot, lineNumber))
                    lineNumber--;

                var startEmptyLineNumber = lineNumber + 1;
                if (startEmptyLineNumber > snapshot.LineCount - 1)
                    return;

                var lastTextLine = snapshot
                    .GetLineFromLineNumber(startEmptyLineNumber - 1);
                var startPosition = lastTextLine
                    .End
                    .Position;

                var endPosition = snapshot
                    .GetLineFromLineNumber(snapshot.LineCount - 1)
                    .EndIncludingLineBreak
                    .Position;

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
