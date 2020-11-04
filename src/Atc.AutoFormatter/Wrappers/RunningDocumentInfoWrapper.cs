using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.Shell;

namespace Atc.AutoFormatter.Wrappers
{
    [ExcludeFromCodeCoverage]
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