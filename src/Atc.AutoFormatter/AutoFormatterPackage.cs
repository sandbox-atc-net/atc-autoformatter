using System;
using System.Runtime.InteropServices;
using System.Threading;
using Atc.AutoFormatter.Formatters;
using Atc.AutoFormatter.Services;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace Atc.AutoFormatter
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(AutoFormatterPackage.PackageGuidString)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists, PackageAutoLoadFlags.BackgroundLoad)]
    public sealed class AutoFormatterPackage : AsyncPackage
    {
        public const string PackageGuidString = "034df847-828d-4e1f-8b1d-f80d91edf4a0";

        protected override async Task InitializeAsync(
            CancellationToken cancellationToken,
            IProgress<ServiceProgressData> progress)
        {
            var dte = await GetServiceAsync(typeof(SDTE)) as DTE2;
            var runningDocumentTable = new RunningDocumentTable(this);

            var locator = new DocumentLocator(dte, runningDocumentTable);
            var formatter = new DocumentFormatter(
                dte,
                new VsTextViewProvider(this),
                new UndoProvider(),
                new ITextFormatter[]
                {
                    new RemoveAndSortUsingsCommand(dte),
                    new TrailingWhiteSpaceRemover(),
                    new TrailingLineBreakRemover(),
                });


            var eventHandler = new DocumentEventHandler(
                locator,
                formatter);

            runningDocumentTable.Advise(eventHandler);
        }
    }
}