using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Atc.AutoFormatter.Services;
using Atc.AutoFormatter.Tests.TestInfrastructure;
using Atc.AutoFormatter.Wrappers;
using AutoFixture.Xunit2;
using EnvDTE;
using EnvDTE80;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Atc.Formatter.Tests.Services
{
    [SuppressMessage(
        "Usage",
        "VSTHRD010:Invoke single-threaded types on Main thread",
        Justification = "Unit test")]
    public class DocumentLocatorTests
    {
        [Theory, AutoNSubstituteData]
        public void FindDocuemnt_Will_ThrowIfNotOnUIThread(
            [Frozen] IThreadHelper threadHelper,
            DocumentLocator sut,
            uint docCookie)
        {
            sut.FindDocument(docCookie);

            threadHelper
                .Received(1)
                .ThrowIfNotOnUIThread();
        }

        [Theory, AutoNSubstituteData]
        public void FindDocument_Gets_DocumentInfo(
            [Frozen] IRunningDocumentTable runningDocumentTable,
            DocumentLocator sut,
            uint docCookie)
        {
            sut.FindDocument(docCookie);

            runningDocumentTable
                .Received(1)
                .GetDocumentInfo(docCookie);
        }

        [Theory, AutoNSubstituteData]
        public void FindDocument_Returns_Matching_Document_In_DTE_Documents_Collection(
            [Frozen] DTE2 dte,
            [Frozen] IRunningDocumentTable runningDocumentTable,
            DocumentLocator sut,
            string documentPath,
            IRunningDocumentInfo info,
            Document[] documents,
            Documents documentCollection,
            uint docCookie)
        {
            runningDocumentTable
                .GetDocumentInfo(docCookie)
                .Returns(info);
            info.Moniker
                .Returns(documentPath);
            dte.Documents
                .Returns(documentCollection);
            documentCollection.GetEnumerator()
                .Returns(documents.GetEnumerator());
            documents.Last().FullName
                .Returns(documentPath);

            var result = sut.FindDocument(docCookie);
            result
                .Should()
                .Be(documents.Last());
        }

        [Theory, AutoNSubstituteData]
        public void FindDocument_Returns_Null_If_No_Matching_Document_In_DTE_Documents_Collection(
            [Frozen] DTE2 dte,
            [Frozen] IRunningDocumentTable runningDocumentTable,
            DocumentLocator sut,
            string documentPath,
            IRunningDocumentInfo info,
            Document[] documents,
            Documents documentCollection,
            uint docCookie)
        {
            runningDocumentTable
                .GetDocumentInfo(docCookie)
                .Returns(info);
            info.Moniker
                .Returns(documentPath);
            dte.Documents
                .Returns(documentCollection);
            documentCollection.GetEnumerator()
                .Returns(documents.GetEnumerator());

            var result = sut.FindDocument(docCookie);
            result
                .Should()
                .BeNull();
        }
    }
}