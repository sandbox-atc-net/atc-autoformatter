#nullable enable
using System;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Atc.AutoFormatter.Services
{
    public class VsTextViewProvider : IVsTextViewProvider
    {
        private readonly IServiceProvider serviceProvider;

        public VsTextViewProvider(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IVsTextView? GetVsTextView(string filePath)
            => VsShellUtilities.IsDocumentOpen(
                       serviceProvider,
                       filePath,
                       Guid.Empty,
                       out _,
                       out _,
                       out var windowFrame)
            ? VsShellUtilities.GetTextView(windowFrame)
            : null;
    }
}