using System;
using System.Linq;
using System.Windows.Forms;
using VsNerdX.Core;

namespace VsNerdX.Command.Navigation
{
    public class CopyFile : ICommand
    {
        private readonly IHierarchyControl _hierarchyControl;

        public CopyFile(IHierarchyControl hierarchyControl)
        {
            this._hierarchyControl = hierarchyControl;
        }

        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            var dte = (this._hierarchyControl as HierarchyControl).dte;
            try
            {
                dte.ExecuteCommand("Edit.Copy");
            }
            catch (Exception e) { }
            executionContext = executionContext.Clear().With(mode: InputMode.Normal);
            return new ExecutionResult(executionContext, CommandState.Handled);
        }
    }
}
