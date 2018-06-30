using System.Windows.Forms;
using VsNerdX.Core;

namespace VsNerdX.Command.Navigation
{
    public class GoToRoot : ICommand
    {
        private IHierarchyControl _hierarchyControl;

        public GoToRoot(IHierarchyControl hierarchyControl)
        {
            this._hierarchyControl = hierarchyControl;
        }

        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            this._hierarchyControl.GoToTop();
            return new ExecutionResult(executionContext.Clear(), CommandState.Handled);
        }
    }
}