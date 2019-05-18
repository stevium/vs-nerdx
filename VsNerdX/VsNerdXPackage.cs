using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio;
using VsNerdX.Core;
using VsNerdX.Dispatcher;
using VsNerdX.Util;
using Task = System.Threading.Tasks.Task;

namespace VsNerdX
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.NoSolution_string)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionHasMultipleProjects_string)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionHasSingleProject_string)]
    [Guid(VsNerdXPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class VsNerdXPackage : AsyncPackage
    {
        public const string PackageGuidString = "c8973938-38ba-4db7-9798-11c7f5b4bc1f";

        private CommandProcessor commandProcessor;
        private ConditionalKeyDispatcher _keyDispatcher;

        #region Package Members

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            var logger = new DebugLogger();

            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            var dte = await GetServiceAsync(typeof(DTE)) as DTE2;
            var solutionExplorerControl = new HierarchyControl(this, logger, dte);

            this.commandProcessor = new CommandProcessor(solutionExplorerControl, logger);

            this._keyDispatcher = new ConditionalKeyDispatcher(
                new SolutionExplorerDispatchCondition(solutionExplorerControl, logger),
                new KeyDispatcher(this.commandProcessor),
                logger);

            logger.Log("Initialized...");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this._keyDispatcher.Dispose();
        }

        #endregion
    }
}
