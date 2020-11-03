using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Atc.AutoFormatter.Wrappers
{
    public static class VsTextViewExtensions
    {
        public static ITextView? GetTextView(this IVsTextView textView)
        {
            var userData = textView as IVsUserData;
            if (userData == null)
            {
                return null;
            }

            var guidViewHost = DefGuidList.guidIWpfTextViewHost;
            userData.GetData(ref guidViewHost, out var holder);
            var viewHost = (IWpfTextViewHost)holder;

            return viewHost.TextView;
        }
    }
}