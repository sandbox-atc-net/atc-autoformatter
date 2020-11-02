#nullable enable
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;

namespace Atc.AutoFormatter.Services
{
    public interface IUndoProvider
    {
        ITextUndoTransaction? StartTransaction(ITextView textView);
    }
}