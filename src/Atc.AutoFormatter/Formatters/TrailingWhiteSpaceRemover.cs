using Microsoft.VisualStudio.Text.Editor;

namespace Atc.AutoFormatter.Formatters
{
    public class TrailingWhiteSpaceRemover : ITextFormatter
    {
        public void Execute(string filePath, ITextView textView)
        {
            var snapshot = textView.TextSnapshot;
            using (var edit = snapshot.TextBuffer.CreateEdit())
            {
                var hasModified = false;
                for (var i = 0; i < snapshot.LineCount; i++)
                {
                    var line = snapshot.GetLineFromLineNumber(i);
                    var lineText = line.GetText();

                    var trimmedLength = lineText.TrimEnd().Length;
                    if (trimmedLength == lineText.Length)
                    {
                        continue;
                    }

                    var spaceLength = lineText.Length - trimmedLength;
                    var endPosition = line.End.Position;
                    edit.Delete(endPosition - spaceLength, spaceLength);
                    hasModified = true;
                }

                if (hasModified)
                {
                    _ = edit.Apply();
                }
            }
        }
    }
}