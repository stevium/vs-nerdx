using EnvDTE;
using Microsoft.VisualStudio.CommandBars;
using System;
using System.Windows.Forms;
using VsNerdX.Core;

namespace VsNerdX.Command.Navigation
{
    public class ShowAllFiles: ICommand
    {
        private readonly IHierarchyControl _hierarchyControl;

        public ShowAllFiles(IHierarchyControl hierarchyControl)
        {
            this._hierarchyControl = hierarchyControl;
        }

        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            var dte = (this._hierarchyControl as HierarchyControl).dte;
            try
            {
                dte.ExecuteCommand("Project.ShowAllFiles");
            }
            catch (Exception e)
            {
                dte.ExecuteCommand("SolutionExplorer.Folder.ShowAllFiles");
            }
            executionContext = executionContext.Clear().With(mode: InputMode.Normal);
            return new ExecutionResult(executionContext, CommandState.Handled);
        }
    }
}
