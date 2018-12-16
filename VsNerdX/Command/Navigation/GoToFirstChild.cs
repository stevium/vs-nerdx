using System.Windows.Forms;
using VsNerdX.Core;

namespace VsNerdX.Command.Navigation
{
    public class GoToLastChild : ICommand
    {
        private IHierarchyControl _hierarchyControl;

        public GoToLastChild(IHierarchyControl hierarchyControl)
        {
            this._hierarchyControl = hierarchyControl;
        }

        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            this._hierarchyControl.GoToLastChild();
            return new ExecutionResult(executionContext.Clear(), CommandState.Handled);
        }
    }
}