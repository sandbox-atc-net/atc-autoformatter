using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Atc.AutoFormatter.Formatters;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;

namespace Atc.AutoFormatter.Services
{
    public class DocumentFormatter : IDocumentFormatter
    {
        private readonly DTE2 dte;
        private readonly IVsTextViewProvider textViewProvider;
        private readonly IUndoProvider undoProvider;
        private readonly IEnumerable<ITextFormatter> formatters;

        public DocumentFormatter(
            DTE2 dte,
            IVsTextViewProvider textViewProvider,
            IUndoProvider undoProvider,
            IEnumerable<ITextFormatter> formatters)
        {
            this.dte = dte;
            this.textViewProvider = textViewProvider;
            this.undoProvider = undoProvider;
            this.formatters = formatters;
        }

        public void Format(Document document)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (!IsTextDocument(document))
            {
                return;
            }

            var oldActiveDocument = dte.ActiveDocument;
            document.Activate();

            try
            {
                var vsTextView = textViewProvider.GetVsTextView(document.FullName);
                if (vsTextView == null)
                {
                    return;
                }

                var textView = vsTextView.GetTextView();
                if (textView == null)
                {
                    return;
                }

                using (var undo = undoProvider.StartTransaction(textView))
                {
                    vsTextView.GetCaretPos(out var oldCaretLine, out var oldCaretColumn);
                    vsTextView.SetCaretPos(oldCaretLine, 0);

                    foreach (var formatter in formatters)
                    {
                        formatter.Execute(document.FullName, textView);
                    }

                    vsTextView.GetCaretPos(out var newCaretLine, out var newCaretColumn);
                    vsTextView.SetCaretPos(newCaretLine, oldCaretColumn);

                    undo?.Complete();
                }
            }
            finally
            {
                oldActiveDocument?.Activate();
            }
        }

        [SuppressMessage(
            "Usage",
            "VSTHRD010:Invoke single-threaded types on Main thread",
            Justification = "Only called from main thread")]
        private bool IsTextDocument(Document document)
            => document?.Type == "Text"
            && document?.Language != null
            && document?.Language != "Plain Text";
    }
}