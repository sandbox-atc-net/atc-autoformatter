using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Atc.AutoFormatter.Wrappers
{
    public class RunningDocumentTableWrapper : IRunningDocumentTable
    {
        private readonly RunningDocumentTable runningDocumentTable;

        public RunningDocumentTableWrapper(RunningDocumentTable runningDocumentTable)
        {
            this.runningDocumentTable = runningDocumentTable;
        }

        public IRunningDocumentInfo GetDocumentInfo(uint docCookie)
            => new RunningDocumentInfoWrapper(
                runningDocumentTable.GetDocumentInfo(docCookie));

        public uint Advise(IVsRunningDocTableEvents sink)
            => runningDocumentTable.Advise(sink);
    }
}