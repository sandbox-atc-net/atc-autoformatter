using Microsoft.VisualStudio.TextManager.Interop;

namespace Atc.AutoFormatter.Services
{
    public interface IVsTextViewProvider
    {
        IVsTextView GetVsTextView(string filePath);
    }
}