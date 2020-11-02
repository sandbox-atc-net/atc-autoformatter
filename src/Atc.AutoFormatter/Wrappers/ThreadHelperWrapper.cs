using Microsoft.VisualStudio.Shell;

namespace Atc.AutoFormatter.Wrappers
{
    public class ThreadHelperWrapper : IThreadHelper
    {
        public void ThrowIfNotOnUIThread()
            => ThreadHelper.ThrowIfNotOnUIThread();
    }
}