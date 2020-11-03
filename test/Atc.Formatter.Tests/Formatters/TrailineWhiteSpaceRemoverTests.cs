using System.Linq;
using Atc.AutoFormatter.Formatters;
using Atc.Formatter.Tests.TestInfrastructure;
using NSubstitute;
using Xunit;
using static Atc.Formatter.Tests.Formatters.TextViewFactory;

namespace Atc.Formatter.Tests.Formatters
{
    public class TrailineWhiteSpaceRemoverTests
    {
        [Theory, AutoNSubstituteData]
        public void Execute_Calls_CreateEdit_On_TextBuffer(
            TrailingWhiteSpaceRemover sut,
            string filePath,
            string[] lines)
        {
            var textView = CreateTextView(lines.Select(l => l + "  "));

            sut.Execute(filePath, textView);

            textView.TextSnapshot.TextBuffer
                .Received(1)
                .CreateEdit();
        }

        [Theory, AutoNSubstituteData]
        public void Execute_Calls_Delete_On_TextEdit(
            TrailingWhiteSpaceRemover sut,
            string filePath,
            string[] lines)
        {
            var linesWithSpaces = lines.Select(l => l + "  ").ToArray();
            var textView = CreateTextView(linesWithSpaces);

            sut.Execute(filePath, textView);

            var edit = textView.TextSnapshot.TextBuffer.CreateEdit();
            int pos = 0;
            foreach (var line in linesWithSpaces)
            {
                edit
                    .Received(1)
                    .Delete(
                        pos + line.TrimEnd().Length,
                        2);
                pos += line.Length + LineBreak.Length;
            }
        }

        [Theory, AutoNSubstituteData]
        public void Execute_Calls_Apply_On_TextEdit(
            TrailingWhiteSpaceRemover sut,
            string filePath,
            string[] lines)
        {
            var textView = CreateTextView(lines.Select(l => l + "  "));

            sut.Execute(filePath, textView);

            var edit = textView.TextSnapshot.TextBuffer.CreateEdit();
            edit
                .Received(1)
                .Apply();
        }

        [Theory, AutoNSubstituteData]
        public void Execute_Does_Not_Call_Apply_If_No_Trailing_LineBreaks(
            TrailingWhiteSpaceRemover sut,
            string filePath,
            string[] lines)
        {
            var textView = CreateTextView(lines);

            sut.Execute(filePath, textView);

            var edit = textView.TextSnapshot.TextBuffer.CreateEdit();
            edit
                .DidNotReceive()
                .Apply();
        }
    }
}