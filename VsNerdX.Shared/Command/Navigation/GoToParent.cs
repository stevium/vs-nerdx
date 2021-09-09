using System.Windows.Forms;
using VsNerdX.Core;

namespace VsNerdX.Command.Navigation
{
    public class GoToParent : ICommand
    {
        private IHierarchyControl _hierarchyControl;

        public GoToParent(IHierarchyControl hierarchyControl)
        {
            this._hierarchyControl = hierarchyControl;
        }

        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            this._hierarchyControl.GoToParent();
            return new ExecutionResult(executionContext.Clear(), CommandState.Handled);
        }
    }
}