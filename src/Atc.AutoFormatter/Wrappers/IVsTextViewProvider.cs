#nullable enable
using Microsoft.VisualStudio.TextManager.Interop;

namespace Atc.AutoFormatter.Wrappers
{
    public interface IVsTextViewProvider
    {
        IVsTextView? GetVsTextView(string filePath);
    }
}