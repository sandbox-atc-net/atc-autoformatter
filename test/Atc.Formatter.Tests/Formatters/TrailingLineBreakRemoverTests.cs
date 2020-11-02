using System.Collections.Generic;
using System.Linq;
using Atc.AutoFormatter.Formatters;
using Atc.Formatter.Tests.TestInfrastructure;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using NSubstitute;
using Xunit;

namespace Atc.Formatter.Tests.Formatters
{
    public class TrailingLineBreakRemoverTests
    {
        private const string LineBreak = "\r\n";

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

        [Theory, AutoNSubstituteData]
        public void Execute_Calls_CreateEdit_On_TextBuffer(
            TrailingLineBreakRemover sut,
            string filePath,
            string[] lines)
        {
            var emptyLines = Enumerable.Repeat(string.Empty, 2);
            var textView = CreateTextView(lines.Union(emptyLines));

            sut.Execute(filePath, textView);

            textView.TextSnapshot.TextBuffer
                .Received(1)
                .CreateEdit();
        }

        [Theory, AutoNSubstituteData]
        public void Execute_Calls_Delete_On_TextEdit(
            TrailingLineBreakRemover sut,
            string filePath,
            string[] lines)
        {
            var emptyLines = Enumerable.Repeat(string.Empty, 2);
            var allLines = lines.Union(emptyLines);
            var allText = string.Join(LineBreak, allLines);
            var textView = CreateTextView(allLines);

            sut.Execute(filePath, textView);

            var edit = textView.TextSnapshot.TextBuffer.CreateEdit();
            edit
                .Received(1)
                .Delete(
                    allText.TrimEnd().Length,
                    emptyLines.Count() * LineBreak.Length);
        }

        [Theory, AutoNSubstituteData]
        public void Execute_Calls_Apply_On_TextEdit(
            TrailingLineBreakRemover sut,
            string filePath,
            string[] lines)
        {
            var emptyLines = Enumerable.Repeat(string.Empty, 2);
            var textView = CreateTextView(lines.Union(emptyLines));

            sut.Execute(filePath, textView);

            var edit = textView.TextSnapshot.TextBuffer.CreateEdit();
            edit
                .Received(1)
                .Apply();
        }

        [Theory, AutoNSubstituteData]
        public void Execute_Does_Not_Call_CreateEdit_If_No_Trailing_LineBreaks(
            TrailingLineBreakRemover sut,
            string filePath,
            string[] lines)
        {
            var textView = CreateTextView(lines);

            sut.Execute(filePath, textView);

            textView.TextSnapshot.TextBuffer
                .DidNotReceive()
                .CreateEdit();
        }
    }
}