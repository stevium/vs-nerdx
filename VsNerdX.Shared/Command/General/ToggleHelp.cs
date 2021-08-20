using System.Windows.Forms;
using VsNerdX.Core;

namespace VsNerdX.Command.General
{
    public class ToggleHelp : ICommand
    {
        private readonly IHierarchyControl _hierarchyControl;

        public ToggleHelp(IHierarchyControl hierarchyControl)
        {
            this._hierarchyControl = hierarchyControl;
        }

        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            this._hierarchyControl.ToggleHelp();
            return new ExecutionResult(executionContext.Clear(), CommandState.Handled);
        }
    }
}