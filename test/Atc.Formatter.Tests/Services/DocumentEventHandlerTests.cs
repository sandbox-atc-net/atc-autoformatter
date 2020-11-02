using Atc.AutoFormatter.Services;
using Atc.AutoFormatter.Tests.TestInfrastructure;
using AutoFixture.Xunit2;
using EnvDTE;
using NSubstitute;
using Xunit;

namespace Atc.Formatter.Tests.Services
{
    public class DocumentEventHandlerTests
    {
        [Theory, AutoNSubstituteData]
        public void OnBeforeSave_Calls_DocumentLocator(
            [Frozen] IDocumentLocator locator,
            DocumentEventHandler sut,
            uint docCookie)
        {
            sut.OnBeforeSave(docCookie);

            locator
                .Received(1)
                .FindDocument(docCookie);
        }

        [Theory, AutoNSubstituteData]
        public void OnBeforeSave_Calls_DocumentFormatter(
            [Frozen] IDocumentLocator locator,
            [Frozen] IDocumentFormatter formatter,
            DocumentEventHandler sut,
            uint docCookie,
            Document document)
        {
            locator
                .FindDocument(docCookie)
                .Returns(document);

            sut.OnBeforeSave(docCookie);

            formatter
                .Received(1)
                .Format(document);
        }
    }
}