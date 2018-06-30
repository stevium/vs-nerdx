using System.Windows.Forms;
using VsNerdX.Core;

namespace VsNerdX.Command.Navigation
{
    public class GoDown : ICommand
    {
        private readonly IHierarchyControl _hierarchyControl;

        public GoDown(IHierarchyControl hierarchyControl)
        {
            this._hierarchyControl = hierarchyControl;
        }

        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            this._hierarchyControl.GoDown();
            return new ExecutionResult(executionContext.Clear(), CommandState.Handled);
        }
    }
}