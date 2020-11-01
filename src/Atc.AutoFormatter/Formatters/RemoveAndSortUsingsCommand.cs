using System;
using System.Runtime.InteropServices;
using Atc.AutoFormatter.Formatters;
using EnvDTE80;
using Microsoft.VisualStudio.Text.Editor;

namespace Atc.AutoFormatter.Formatters
{
    public class RemoveAndSortUsingsCommand : ITextFormatter
    {
        private readonly DTE2 dte;

        public RemoveAndSortUsingsCommand(DTE2 dte)
        {
            this.dte = dte;
        }

        public void Execute(string filePath, ITextView textView)
        {
            if (!IsCsFile(filePath))
                return;

            if (HasIfCompilerDirective(textView))
                return;

            ExecuteCommand("Edit.RemoveAndSort");
        }

        private void ExecuteCommand(string command)
        {
            try
            {
                dte.ExecuteCommand(command, string.Empty);
            }
            catch (COMException)
            {
            }
        }

        private static bool IsCsFile(string documentFullName)
            => documentFullName.EndsWith(
                ".cs",
                StringComparison.OrdinalIgnoreCase);

        private static bool HasIfCompilerDirective(ITextView textView)
            => textView.TextSnapshot.GetText().Contains("#if");
    }
}