using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;

namespace Atc.AutoFormatter.Services
{
    public class UndoProvider : IUndoProvider
    {
        public ITextUndoTransaction StartTransaction(ITextView textView)
        {
            var componentModel = (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));
            var undoHistoryRegistry = componentModel.DefaultExportProvider
                .GetExportedValue<ITextUndoHistoryRegistry>();

            undoHistoryRegistry.TryGetHistory(
                textView.TextBuffer,
                out var history);

            return history?.CreateTransaction("Auto Formatting");
        }
    }
}
