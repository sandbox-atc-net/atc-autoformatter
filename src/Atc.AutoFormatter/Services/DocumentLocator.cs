#nullable enable
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;

namespace Atc.AutoFormatter.Services
{
    public class DocumentLocator : IDocumentLocator
    {
        private readonly DTE2 dte;
        private readonly RunningDocumentTable documentTable;

        public DocumentLocator(
            DTE2 dte,
            RunningDocumentTable documentTable)
        {
            this.dte = dte;
            this.documentTable = documentTable;
        }

        public Document? FindDocument(uint docCookie)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var documentInfo = documentTable.GetDocumentInfo(docCookie);
            var documentPath = documentInfo.Moniker;

            foreach (var item in dte.Documents)
            {
                if (item is Document doc && doc.FullName == documentPath)
                {
                    return doc;
                }
            }

            return null;
        }
    }
}