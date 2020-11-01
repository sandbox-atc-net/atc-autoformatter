using Microsoft.VisualStudio.Text.Editor;

namespace Atc.AutoFormatter.Formatters
{
    public interface ITextFormatter
    {
        void Execute(string filePath, ITextView textView);
    }
}