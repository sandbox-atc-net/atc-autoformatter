using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using NSubstitute;

namespace Atc.Formatter.Tests.Formatters
{
    public static class TextViewFactory
    {
        public const string LineBreak = "\r\n";

        public static ITextView CreateTextView(IEnumerable<string> lines)
        {
            var textLines = lines.ToArray();

            var edit = Substitute.For<ITextEdit>();

            var textBuffer = Substitute.For<ITextBuffer>();
            textBuffer.CreateEdit().Returns(edit);

            var snapshot = Substitute.For<ITextSnapshot>();
            snapshot.TextBuffer.Returns(textBuffer);
            snapshot.Length.Returns(lines.Sum(l => l.Length + LineBreak.Length));
            snapshot.LineCount.Returns(textLines.Length);
            snapshot
                .GetLineFromLineNumber(default)
                .ReturnsForAnyArgs(c =>
                {
                    var index = c.Arg<int>();
                    var line = textLines[index];

                    var lineEndWithLineBreak = textLines.Where((s, i) => i <= index).Sum(s => s.Length + LineBreak.Length);
                    var lineEnd = lineEndWithLineBreak - LineBreak.Length;

                    var textSnapshotLine = Substitute.For<ITextSnapshotLine>();
                    textSnapshotLine.GetText().Returns(line);
                    textSnapshotLine.End.Returns(c => new SnapshotPoint(snapshot, lineEnd));
                    textSnapshotLine.EndIncludingLineBreak.Returns(c => new SnapshotPoint(snapshot, lineEndWithLineBreak));
                    return textSnapshotLine;
                });

            var textView = Substitute.For<ITextView>();
            textView.TextSnapshot.Returns(snapshot);

            return textView;
        }
    }
}