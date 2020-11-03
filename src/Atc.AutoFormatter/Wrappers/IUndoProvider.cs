#nullable enable
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;

namespace Atc.AutoFormatter.Wrappers
{
    public interface IUndoProvider
    {
        ITextUndoTransaction? StartTransaction(ITextView textView);
    }
}