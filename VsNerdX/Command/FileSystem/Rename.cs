using System;
using System.Windows.Forms;
using VsNerdX.Core;

namespace VsNerdX.Command.Navigation
{
    public class Rename : ICommand
    {
        private readonly IHierarchyControl _hierarchyControl;

        public Rename(IHierarchyControl hierarchyControl)
        {
            this._hierarchyControl = hierarchyControl;
        }

        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            var dte = (this._hierarchyControl as HierarchyControl).dte;
            try
            {
                dte.ExecuteCommand("File.Rename");
            }
            catch (Exception e) { }

            return new ExecutionResult(executionContext.Clear(), CommandState.Handled);
        }
    }
}