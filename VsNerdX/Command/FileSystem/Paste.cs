using System;
using System.Linq;
using System.Windows.Forms;
using VsNerdX.Core;

namespace VsNerdX.Command.Navigation
{
    public class Paste : ICommand
    {
        private readonly IHierarchyControl _hierarchyControl;

        public Paste(IHierarchyControl hierarchyControl)
        {
            this._hierarchyControl = hierarchyControl;
        }

        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            var dte = (this._hierarchyControl as HierarchyControl).dte;
            try
            {
                dte.ExecuteCommand("Edit.Paste");
            } catch (Exception e) { }

            return new ExecutionResult(executionContext.Clear(), CommandState.Handled);
        }
    }
}