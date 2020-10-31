using Atc.AutoFormatter.Services;
using Microsoft.VisualStudio;

namespace Atc.AutoFormatter.Services
{
    public class DocumentEventHandler : VsRunningDocTableEvents3
    {
        private readonly IDocumentLocator locator;
        private readonly IDocumentFormatter formatter;

        public DocumentEventHandler(
            IDocumentLocator locator,
            IDocumentFormatter formatter)
        {
            this.locator = locator;
            this.formatter = formatter;
        }

        public override int OnBeforeSave(uint docCookie)
        {
            var document = locator.FindDocument(docCookie);
            if (document != null)
                formatter.Format(document);

            return VSConstants.S_OK;
        }
    }
}