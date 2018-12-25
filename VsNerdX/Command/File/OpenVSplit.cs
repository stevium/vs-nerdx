using EnvDTE;
using Microsoft.VisualStudio.CommandBars;
using System;
using System.Windows.Forms;
using VsNerdX.Core;

namespace VsNerdX.Command.Navigation
{
    public class OpenVSplit : ICommand
    {
        private readonly IHierarchyControl _hierarchyControl;

        public OpenVSplit(IHierarchyControl hierarchyControl)
        {
            this._hierarchyControl = hierarchyControl;
        }

        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            var dte = (this._hierarchyControl as HierarchyControl).dte;
            try
            {
                dte.ExecuteCommand("View.Open");
                dte.ExecuteCommand("Window.NewVerticalTabGroup");
            }
            catch (Exception e) { }
            executionContext = executionContext.Clear().With(mode: InputMode.Normal);
            return new ExecutionResult(executionContext, CommandState.Handled);
        }
    }
}
