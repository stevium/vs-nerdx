using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell.Interop;
using VsNerdX.Core;
using VsNerdX.Dispatcher;
using VsNerdX.Util;
using PackageAutoLoadFlags = VsNerdX.Util.PackageAutoLoadFlags;

namespace VsNerdX
{
    [AsyncPackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Util.ProvideAutoLoad(UIContextGuids80.NoSolution, PackageAutoLoadFlags.BackgroundLoad)]
    [Guid(PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class VsNerdXPackage : AsyncPackage, IAsyncLoadablePackageInitialize
    {
        public const string PackageGuidString = "c8973938-38ba-4db7-9798-11c7f5b4bc1f";

        private CommandProcessor _commandProcessor;
        private ConditionalKeyDispatcher _keyDispatcher;
        private bool _isAsyncLoadSupported;
        private DebugLogger _logger;
        private DTE2 _dte;

        public IVsTask Initialize(Microsoft.VisualStudio.Shell.Interop.IAsyncServiceProvider asyncServiceProvider,
            IProfferAsyncService pProfferService, IAsyncProgressCallback pProgressCallback)
        {
            return ThreadHelper.JoinableTaskFactory.RunAsync<object>(async () =>
            {
                BackgroundThreadInitialization();
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                MainThreadInitialization();
                return null;
            }).AsVsTask();
        }

        private void BackgroundThreadInitialization()
        {
            _logger = new DebugLogger();
        }
        
        private async void MainThreadInitialization()
        {
            _dte = await GetServiceAsync(typeof(DTE)) as DTE2;
            var solutionExplorerControl = new HierarchyControl(this, _logger, _dte);

            _commandProcessor = new CommandProcessor(solutionExplorerControl, _logger);

            _keyDispatcher = new ConditionalKeyDispatcher(
                new SolutionExplorerDispatchCondition(solutionExplorerControl, _logger),
                new KeyDispatcher(_commandProcessor),
                _logger);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _keyDispatcher.Dispose();
        }
    }
}