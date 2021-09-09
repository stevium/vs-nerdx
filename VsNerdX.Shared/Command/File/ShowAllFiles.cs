using EnvDTE;
using Microsoft.VisualStudio.CommandBars;
using System;
using System.Windows.Forms;
using VsNerdX.Core;
using static VsNerdX.VsNerdXPackage;

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
            try
            {
                var selectedItem = Dte.SelectedItems.Item(1);
                if (selectedItem.Project != null)
                {
                    Dte.ExecuteCommand("Project.ShowAllFiles");
                }
                else
                {
                    Dte.ExecuteCommand("SolutionExplorer.Folder.ShowAllFiles");
                }
            }
            catch (Exception e) { }
            executionContext = executionContext.Clear().With(mode: InputMode.Normal);
            return new ExecutionResult(executionContext, CommandState.Handled);
        }
    }
}
