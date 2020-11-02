using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Atc.AutoFormatter.Wrappers
{
    public interface IRunningDocumentTable
    {
        IRunningDocumentInfo GetDocumentInfo(uint docCookie);
        uint Advise(IVsRunningDocTableEvents sink);
    }

    public interface IRunningDocumentInfo
    {
        string Moniker { get; }
    }

    public class RunningDocumentInfoWrapper : IRunningDocumentInfo
    {
        private readonly RunningDocumentInfo info;

        public RunningDocumentInfoWrapper(RunningDocumentInfo info)
        {
            this.info = info;
        }

        public string Moniker => info.Moniker;
    }
}