using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using VsNerdX.Core;
using VsNerdX.Dispatcher;
using VsNerdX.Util;

namespace VsNerdX
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(VsNerdXPackage.PackageGuidString)]
    [ProvideAutoLoad(UIContextGuids80.NoSolution)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class VsNerdXPackage : Package
    {
        /// <summary>
        /// VsNerdXPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "c8973938-38ba-4db7-9798-11c7f5b4bc1f";

        private CommandProcessor commandProcessor;
        private ConditionalKeyDispatcher _keyDispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="VsNerdXPackage"/> class.
        /// </summary>
        public VsNerdXPackage()
        {
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            var logger = new DebugLogger();
            var dte = GetService(typeof(DTE)) as DTE2;
            var solutionExplorerControl = new HierarchyControl(this, logger);

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
