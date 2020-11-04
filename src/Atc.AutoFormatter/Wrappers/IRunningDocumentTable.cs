using Microsoft.VisualStudio.Shell.Interop;

namespace Atc.AutoFormatter.Wrappers
{
    public interface IRunningDocumentTable
    {
        IRunningDocumentInfo GetDocumentInfo(uint docCookie);
        uint Advise(IVsRunningDocTableEvents sink);
    }
}