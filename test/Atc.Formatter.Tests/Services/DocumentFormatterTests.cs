using System.Diagnostics.CodeAnalysis;
using Atc.AutoFormatter.Formatters;
using Atc.AutoFormatter.Services;
using Atc.AutoFormatter.Wrappers;
using Atc.Formatter.Tests.TestInfrastructure;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using EnvDTE;
using EnvDTE80;
using FluentAssertions;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.TextManager.Interop;
using NSubstitute;
using Xunit;

namespace Atc.Formatter.Tests.Services
{
    [SuppressMessage(
        "Usage",
        "VSTHRD010:Invoke single-threaded types on Main thread",
        Justification = "Unit test")]
    public class DocumentFormatterTests
    {
        private readonly DTE2 dte;
        private readonly IVsTextViewProvider textViewProvider;
        private readonly IUndoProvider undoProvider;
        private readonly IThreadHelper threadHelper;
        private readonly ITextFormatter[] formatters;
        private readonly DocumentFormatter sut;

        private readonly IVsTextView vsTextView;
        private readonly ITextView textView;
        private readonly Document document;

        public DocumentFormatterTests()
        {
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            document = Substitute.For<Document>();
            document.Type.Returns("Text");
            document.Language.Returns(fixture.Create<string>());
            document.FullName.Returns(fixture.Create<string>());

            textView = Substitute.For<IWpfTextView, ITextView>();

            vsTextView = Substitute.For<IVsTextView, IVsUserData>();
            vsTextView
                .As<IVsUserData>()
                .WhenForAnyArgs(u => u.GetData(default, out _))
                .Do(c =>
                {
                    var holder = Substitute.For<IWpfTextViewHost>();
                    holder.TextView.Returns(textView);
                    c[1] = holder;
                });

            dte = Substitute.For<DTE2>();
            textViewProvider = Substitute.For<IVsTextViewProvider>();
            textViewProvider
                .GetVsTextView(default)
                .ReturnsForAnyArgs(vsTextView);
            undoProvider = Substitute.For<IUndoProvider>();
            threadHelper = Substitute.For<IThreadHelper>();
            formatters = fixture.Create<ITextFormatter[]>();

            sut = new DocumentFormatter(dte, textViewProvider, undoProvider, threadHelper, formatters);
        }

        [Fact]
        public void Format_Will_ThrowIfNotOnUIThread()
        {
            sut.Format(document);

            threadHelper
                .Received(1)
                .ThrowIfNotOnUIThread();
        }

        [Fact]
        public void Format_Will_Activate_Document()
        {
            sut.Format(document);

            document
                .Received(1)
                .Activate();
        }

        [Theory, AutoNSubstituteData]
        public void Format_Will_Reactivate_ActiveDocument(
            Document activeDocument)
        {
            dte.ActiveDocument.Returns(activeDocument);

            sut.Format(document);

            activeDocument
                .Received(1)
                .Activate();
        }

        [Theory, AutoNSubstituteData]
        public void Format_Will_Do_Nothing_If_Not_Text_Document(
            Document invalidDocument)
        {
            sut.Format(invalidDocument);

            invalidDocument
                .DidNotReceive()
                .Activate();
            _ = dte
                .DidNotReceive()
                .ActiveDocument;
        }

        [Fact]
        public void Format_Will_GetsVsTextView_From_Provider()
        {
            sut.Format(document);

            textViewProvider
                .Received(1)
                .GetVsTextView(document.FullName);
        }

        [Fact]
        public void Format_Will_StartTransaction_On_UndoProvider()
        {
            sut.Format(document);

            undoProvider
                .Received(1)
                .StartTransaction(textView);
        }

        [Theory, AutoNSubstituteData]
        public void Format_Will_Complete_UndoTransaction(
            ITextUndoTransaction undoTransaction)
        {
            undoProvider
                .StartTransaction(default)
                .ReturnsForAnyArgs(undoTransaction);

            sut.Format(document);

            undoTransaction
                .Received(1)
                .Complete();
        }

        [Theory, AutoNSubstituteData]
        public void Format_Will_Dispose_UndoTransaction(
           ITextUndoTransaction undoTransaction)
        {
            undoProvider
                .StartTransaction(default)
                .ReturnsForAnyArgs(undoTransaction);

            sut.Format(document);

            undoTransaction
                .Received(1)
                .Dispose();
        }

        [Theory, AutoNSubstituteData]
        public void Format_Will_SetCaretPos_To_Beginning_Of_Line(
            int caretLine,
            int caretColumn)
        {
            vsTextView
                .WhenForAnyArgs(v => v.GetCaretPos(out _, out _))
                .Do(c =>
                {
                    c[0] = caretLine;
                    c[1] = caretColumn;
                });

            sut.Format(document);

            vsTextView
                .Received()
                .SetCaretPos(caretLine, 0);
            vsTextView
                .Received()
                .SetCaretPos(caretLine, caretColumn);
        }

        [Fact]
        public void Format_Will_Call_Execute_On_Formatters()
        {
            sut.Format(document);

            foreach (var formatter in formatters)
            {
                formatter
                    .Received(1)
                    .Execute(document.FullName, textView);
            }
        }
    }
}