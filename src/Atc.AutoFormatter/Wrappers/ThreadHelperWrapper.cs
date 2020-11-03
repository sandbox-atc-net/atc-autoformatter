using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.Shell;

namespace Atc.AutoFormatter.Wrappers
{
    [ExcludeFromCodeCoverage]
    public class ThreadHelperWrapper : IThreadHelper
    {
        public void ThrowIfNotOnUIThread()
            => ThreadHelper.ThrowIfNotOnUIThread();
    }
}