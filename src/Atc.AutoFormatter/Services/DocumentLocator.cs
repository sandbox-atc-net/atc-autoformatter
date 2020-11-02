#nullable enable
using System.Diagnostics.CodeAnalysis;
using Atc.AutoFormatter.Wrappers;
using EnvDTE;
using EnvDTE80;

namespace Atc.AutoFormatter.Services
{
    [SuppressMessage(
        "Usage",
        "VSTHRD010:Invoke single-threaded types on Main thread",
        Justification = "Handled through IThreadHelper")]
    public class DocumentLocator : IDocumentLocator
    {
        private readonly DTE2 dte;
        private readonly IRunningDocumentTable documentTable;
        private readonly IThreadHelper threadHelper;

        public DocumentLocator(
            DTE2 dte,
            IRunningDocumentTable documentTable,
            IThreadHelper threadHelper)
        {
            this.dte = dte;
            this.documentTable = documentTable;
            this.threadHelper = threadHelper;
        }

        public Document? FindDocument(uint docCookie)
        {
            threadHelper.ThrowIfNotOnUIThread();

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