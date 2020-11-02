using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace Atc.AutoFormatter.Wrappers
{
    public abstract class VsRunningDocTableEvents3 : IVsRunningDocTableEvents3
    {
        public virtual int OnAfterFirstDocumentLock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
            => VSConstants.S_OK;
        public virtual int OnBeforeLastDocumentUnlock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
            => VSConstants.S_OK;
        public virtual int OnAfterSave(uint docCookie)
            => VSConstants.S_OK;
        public virtual int OnAfterAttributeChange(uint docCookie, uint grfAttribs)
            => VSConstants.S_OK;
        public virtual int OnBeforeDocumentWindowShow(uint docCookie, int fFirstShow, IVsWindowFrame pFrame)
            => VSConstants.S_OK;
        public virtual int OnAfterDocumentWindowHide(uint docCookie, IVsWindowFrame pFrame)
            => VSConstants.S_OK;
        public virtual int OnAfterAttributeChangeEx(uint docCookie, uint grfAttribs, IVsHierarchy pHierOld, uint itemidOld, string pszMkDocumentOld, IVsHierarchy pHierNew, uint itemidNew, string pszMkDocumentNew)
            => VSConstants.S_OK;
        public virtual int OnBeforeSave(uint docCookie)
            => VSConstants.S_OK;
    }
}