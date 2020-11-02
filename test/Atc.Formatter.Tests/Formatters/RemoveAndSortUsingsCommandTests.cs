using Atc.AutoFormatter.Formatters;
using Atc.Formatter.Tests.TestInfrastructure;
using AutoFixture.Xunit2;
using EnvDTE80;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using NSubstitute;
using Xunit;

namespace Atc.Formatter.Tests.Formatters
{
    public class RemoveAndSortUsingsCommandTests
    {
        [Theory, AutoNSubstituteData]
        public void Will_Execute_EditRemoveAndSort_Command_On_DTE(
            [Frozen] DTE2 dte,
            RemoveAndSortUsingsCommand sut,
            string filePath,
            ITextView textView)
        {
            sut.Execute(
                filePath + ".cs",
                textView);

            dte
                .Received(1)
                .ExecuteCommand(
                    "Edit.RemoveAndSort",
                    string.Empty);
        }

        [Theory, AutoNSubstituteData]
        public void Will_Not_Execute_EditRemoveAndSort_Command_If_Not_CS_File(
            [Frozen] DTE2 dte,
            RemoveAndSortUsingsCommand sut,
            string filePath,
            ITextView textView)
        {
            sut.Execute(
                filePath + ".txt",
                textView);

            dte
                .DidNotReceive()
                .ExecuteCommand(
                    Arg.Any<string>(),
                    Arg.Any<string>());
        }

        [Theory, AutoNSubstituteData]
        public void Will_Not_Execute_EditRemoveAndSort_Command_If_TextView_Contains_Compiler_IfDirective(
            [Frozen] DTE2 dte,
            RemoveAndSortUsingsCommand sut,
            string filePath,
            ITextView textView,
            ITextSnapshot snapshot)
        {
            textView.TextSnapshot.Returns(snapshot);
            snapshot.GetText().Returns("#if");

            sut.Execute(
                filePath + ".cs",
                textView);

            dte
                .DidNotReceive()
                .ExecuteCommand(
                    Arg.Any<string>(),
                    Arg.Any<string>());
        }
    }
}